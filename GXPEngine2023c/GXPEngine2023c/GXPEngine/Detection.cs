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
    Boolean[] collisionSides = new Boolean[4]; //wich sides are colliding (up, down, left, right)
    


    float mass;
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
        
        foreach (GameObject coll in colls) 
        {
            if (coll is Tile) 
            {
                if (coll.y > player.y) { return player.OnGround = true; }
            }
            
        }

        return player.OnGround = false;
    }

    Boolean CeilingCheck()
    {
        GameObject[] colls = GetCollisions();
        foreach (GameObject coll in colls)
        {
            if (coll is Tile)
            {
                if (coll.y < player.y) { return player.OnCeiling = true;  }
            }
        }

        return player.OnCeiling = false;
    }

    public Boolean PlayerInteractCheck()
    {
        GameObject[] colls = GetCollisions();
        foreach (GameObject coll in colls)
        {
            if (coll is Detection)
            {
                Detection dete = (Detection)coll;
                if (dete.player.InShell == true) 
                {
                    Console.WriteLine("this player {0}, colliding Player {1}", this.player.PlayerIndex, dete.player.PlayerIndex);
                    return true;
                }

            }
        }



        return false;
    }

    void CollisionCheck() 
    {
        collisionSides[0] = false;
        collisionSides[1] = false;
        collisionSides[2] = false;
        collisionSides[3] = false;

        GameObject[] colls = GetCollisions();
        foreach (GameObject coll in colls)
        {
            if (coll is Tile)
            {
                if (coll.y < player.y) { collisionSides[0] = true; }
                if (coll.y > player.y) { collisionSides[1] = true; }
                if (coll.x < player.x) { collisionSides[2] = true; }
                if (coll.x > player.x) { collisionSides[3] = true; }

            } 
        }
    }



    void ToggleVisable() 
    {

        if (Input.GetKeyDown(Key.TAB)) 
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
        CollisionCheck();
        PlayerInteractCheck();

    }
}