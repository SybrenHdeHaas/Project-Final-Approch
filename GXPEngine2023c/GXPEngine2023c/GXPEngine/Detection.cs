using GXPEngine;
using System;
using System.CodeDom;

internal class Detection : Sprite
{

    public Detection() : base("detector.png")
    {
        collider.isTrigger = true;
        SetOrigin(width / 2, height / 2);
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