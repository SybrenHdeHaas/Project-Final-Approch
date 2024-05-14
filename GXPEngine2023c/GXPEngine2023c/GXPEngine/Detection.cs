using GXPEngine;
using System;

internal class Detection : Sprite
{
    private string collisionDirection;
    public string getCollisionDirection() { return collisionDirection; }
    public Detection() : base("detector.png")
    {
        collider.isTrigger = true;
        SetOrigin(width / 2, height / 2);
    }



    private void CollisionCheck()
    {
        GameObject[] colls = GetCollisions();
        
        


    }


    void Update()
    {

        CollisionCheck();

    }
}