using GXPEngine;
using System;
using System.CodeDom;
using System.Runtime.InteropServices;

//the collision hitbox of player
public class Hitbox : Sprite
{
    public float mass;
    public ColliderRect playerCollision;
    public Hitbox(float mass) : base("Hitbox.png")
    {
        this.mass = mass;
        alpha = 0.5f;
        playerCollision = new ColliderRect(this, new Vec2(0, 0), new Vec2(0, 0), width, height, true);
    }

    public void ChangeOffSetAndSize(float[] theStats)
    {
        x = theStats[0];
        y = theStats[1];
        width = (int)theStats[2];
        height = (int)theStats[3];
    }

    public void UpdateCollision(Vec2 pPosition)
    {
        playerCollision.Width = width;
        playerCollision.Height = height;
        playerCollision.Position = pPosition + new Vec2(x, y);
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
        if (Input.GetKeyDown(Key.BACKSLASH))
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