using GXPEngine;
using System;
using System.CodeDom;
using System.Runtime.InteropServices;

/* the actual hitbox of player */
public class Hitbox : Sprite
{
    public float mass;
    public ColliderRect playerCollision;

    private float[] startStats = new float[4];
    private float[] shellsStats = new float[4];

    public Hitbox(float objX, float objY, int objWidth, int objHeight, float mass) : base("Hitbox.png")
    {
        width = objWidth;
        height = objHeight;
        x = -width/2;
        y = -height/2;

        startStats[0] = x;
        startStats[1] = y;
        startStats[2] = width;
        startStats[3] = height;

        shellsStats[0] = x;
        shellsStats[1] = 0;
        shellsStats[2] = width;
        shellsStats[3] = height / 2;

        this.mass = mass;
        playerCollision = new ColliderRect(this, new Vec2(objX, objY), new Vec2(0, 0), x, y, width, height, true);
        visible = false;
    }



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