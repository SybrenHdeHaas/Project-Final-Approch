using GXPEngine;
using System;
using System.CodeDom;

/* the actual hitbox of player */
public class Hitbox : Sprite
{
    public float mass;
    
    public Hitbox(float offSetX, float offSetY, float mass) : base("Hitbox.png")
    {
        x = offSetX;
        y = offSetY;
        this.mass = mass;
        alpha = 0.5f;
        SetOrigin(width / 2, height / 2);
        
    }

    public float GetX()
    {
        return parent.x + x;
        
    }

    public float GetY()
    {
        return parent.y + y;
    }
}