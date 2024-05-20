using GXPEngine;
using System;
using System.Collections.Generic;
using System.Threading;

//handles player-to-player detection
public class Detection : Sprite
{
    private string collisionDirection;
    public string getCollisionDirection() { return collisionDirection; }
    Player player;
    Boolean[] collisionSides = new Boolean[4]; //wich sides are colliding (up, down, left, right)
    
    private float[] startStats = new float[4];
    private float[] shellsStats = new float[4];
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

        startStats[0] = x; 
        startStats[1] = y;
        startStats[2] = width;
        startStats[3] = height;

        shellsStats[0] = x;
        shellsStats[1] = 16;
        shellsStats[2] = width;
        shellsStats[3] = height/3;

        this.mass = mass;
        visible = true;
    }

    void CastPlayer() { player = parent as Player; }

    public void inShellChanges() 
    {
        x = shellsStats[0];
        y = shellsStats[1];
        width = (int)shellsStats[2];
        height = (int)shellsStats[3];

    }

    public void outShellChanges() 
    {
        x = startStats[0];
        y = startStats[1];
        width = (int)startStats[2];
        height = (int)startStats[3];
    }

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