using GXPEngine;
using System;
using System.CodeDom;
using System.Runtime.InteropServices;

/* the actual hitbox of player */
public class Hitbox : Sprite
{
    public float mass;
    public ColliderRect playerCollision;

    public Hitbox(float offSetX, float offSetY, float objX, float objY, int objWidth, int objHeight, float mass) : base("Hitbox.png")
    {
        width = objWidth;
        height = objHeight;
        x = offSetX;
        y = offSetY;
        this.mass = mass;
        playerCollision = new ColliderRect(this, new Vec2(objX, objY), new Vec2(0, 0), width, height, true);
        visible = false;
    }

    public float GetX()
    {
        return parent.x;
        
    }

    public float GetY()
    {
        return parent.y;
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