using GXPEngine;
using System;
using System.Collections.Generic;

internal class Detection : Sprite
{
    private string collisionDirection;
    public string getCollisionDirection() { return collisionDirection; }
    ColliderRect playerCollision;
    Player player;

    public Detection() : base("detector.png")
    {
        
        collider.isTrigger = true;
        SetOrigin(width / 2, height / 2);
        playerCollision = new ColliderRect(this, new Vec2(0, 0), new Vec2(0, 0), width, height, true);
        
    }

    void CastPlayer() { player = parent as Player; }

    Boolean FloorCheck() 
    {
        
        GameObject[] colls = GetCollisions();
        foreach (Tile coll in colls) 
        {
            if (coll.y > y) { return player.OnGround = true; }
        }

        return player.OnGround = false;
    }

    void ToggleVisable() 
    {

        if (Input.GetKeyDown(Key.G)) 
        {
            if (visible) { visible = false; } else visible = true;
        }
    
    }
    void UpdateCollision()
    {
        playerCollision.Velocity = player.Velocity;
        playerCollision.Position = player.Position;
    }

    void Update()
    {
        ToggleVisable();
        CastPlayer();
        FloorCheck();
        
/*        UpdateCollision();
        playerCollision.Step();


        player.Velocity = playerCollision.Velocity;
        player.Position += player.Velocity;

        player.x = player.Position.x;
        player.y = player.Position.y;*/


    }
}