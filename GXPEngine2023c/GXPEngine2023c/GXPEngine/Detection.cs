using GXPEngine;

internal class Detection : Sprite
{

    public Detection() : base("detector.png")
    {
        collider.isTrigger = true;
        SetOrigin(width / 2, height / 2);
    }



    private void CollisionCheck() { GameObject[] colls = GetCollisions(); }


    void Update()
    {

        CollisionCheck();

    }
}