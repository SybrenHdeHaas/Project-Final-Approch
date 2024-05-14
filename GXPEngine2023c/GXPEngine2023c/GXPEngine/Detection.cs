using GXPEngine;
using System;
using System.Collections.Generic;

/* the actual hitbox of player */
public class Detection : Sprite
{
    private string collisionDirection;
    public string getCollisionDirection() { return collisionDirection; }
    ColliderRect playerCollision;
    Player player;

    public Detection() : base("detector.png")
    public float mass;
    public Detection(float offSetX, float offSetY, float mass) : base("detector.png")
    {
        
        x = offSetX;
        y = offSetY;
        collider.isTrigger = true;
        SetOrigin(width / 2, height / 2);
        playerCollision = new ColliderRect(this, new Vec2(0, 0), new Vec2(0, 0), width, height, true);
        
        this.mass = mass;

    }

    void CastPlayer() { player = parent as Player; }

    Boolean FloorCheck() 
    {
        
        GameObject[] colls = GetCollisions();
        foreach (Tile coll in colls) 
        {
            if (coll.y > player.y) { return player.OnGround = true; }
        }

        return player.OnGround = false;
    }

    Boolean CeilingCheck()
    {
        GameObject[] colls = GetCollisions();
        foreach (Tile coll in colls)
        {
            if (coll.y < player.y) { return player.OnCeiling = true; }

        }


        return player.OnCeiling = false;

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
    public float GetX()
    {
        return parent.x + x;
    }

    public float GetY() 
    {
        return parent.y + y;
    }

    void Update()
    {
        ToggleVisable();
        CastPlayer();
        FloorCheck();
        CeilingCheck();
/*      UpdateCollision();
        playerCollision.Step();


        player.Velocity = playerCollision.Velocity;
        player.Position += player.Velocity;

        player.x = player.Position.x;
        player.y = player.Position.y;*/


    }
}