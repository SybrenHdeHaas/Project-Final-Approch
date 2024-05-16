using GXPEngine;
using System;
using System.CodeDom;

/* the actual hitbox of player */
public class Hitbox : Sprite
{
    public float mass;
    public ColliderRect playerCollision;
    public Hitbox(float offSetX, float offSetY, float mass) : base("Hitbox.png")
    {

        x = offSetX;
        y = offSetY;
        this.mass = mass;
        playerCollision = new ColliderRect(this, new Vec2(0, 0), new Vec2(0, 0), width, height, true);

    }

    public float GetX()
    {
        return parent.x + x;
        
    }

    public float GetY()
    {
        return parent.y + y;
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
        if (Input.GetKeyDown(Key.APOSTROPHE)) 
        { 
           Console.WriteLine("HITBOX x {0}, y {1}", GetX(), GetY());
           Console.WriteLine(GetChildCount());
        }
        
    }

}