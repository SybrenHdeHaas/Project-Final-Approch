using GXPEngine;
using System;
using System.CodeDom;

/* the actual hitbox of player */
public class Detection : Sprite
{
    public float mass;
    public Detection(float offSetX, float offSetY, float mass) : base("detector.png")
    {
        x = offSetX;
        y = offSetY;
        collider.isTrigger = true;
        this.mass = mass;

    }

    private void CollisionCheck() 
    { 
        GameObject[] colls = GetCollisions();

        foreach (GameObject coll in colls) { 
            //Console.WriteLine("detectionRange collision"); 
        }
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
        CollisionCheck();
    }
}