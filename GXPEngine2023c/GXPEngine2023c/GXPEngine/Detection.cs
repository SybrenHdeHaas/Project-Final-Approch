using GXPEngine;
using System;
using System.CodeDom;

/* the actual hitbox of player */
public class Detection : Sprite
{
    public Detection() : base("detector.png")
    {
        collider.isTrigger = true;
    }

    private void CollisionCheck() 
    { 
        GameObject[] colls = GetCollisions();

        foreach (GameObject coll in colls) { 
            //Console.WriteLine("detectionRange collision"); 
        }
    }

    void Update()
    {
        CollisionCheck();
    }
}