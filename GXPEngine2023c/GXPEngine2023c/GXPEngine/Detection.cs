using GXPEngine;
using System;
using System.Collections.Generic;

/* the actual hitbox of player */
public class Detection : Sprite
{
    private string collisionDirection;
    public string getCollisionDirection() { return collisionDirection; }
    Player player;
    Boolean[] collisionSides = new Boolean[4]; //wich sides are colliding (up, down, left, right)
    

    private Detection dete;
    public Detection GetDete() { return dete; }
    public Player GetPlayer() { return player; }
    float mass;
    public Detection(int objWidth, int objHeight, float mass) : base("detector.png")
    {
        width = objWidth + 30;
        height = objHeight + 30;
        x = -width/2;
        y = -height/2;
        
        this.mass = mass;
        visible = false;
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
                dete = (Detection)coll;

                if (dete.player == null)
                {
                    return false;
                }

                if (dete.player.InShell == true) 
                {
                    
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