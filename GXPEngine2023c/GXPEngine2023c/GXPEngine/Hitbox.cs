using GXPEngine;
using System;
using System.CodeDom;

/* the actual hitbox of player */
public class Hitbox : Sprite
{
    public float mass;
    public int playerIndex;
    public Hitbox(float mass, int playerIndex) : base("Hitbox.png")
    {
        this.mass = mass;
        alpha = 0.5f;
        this.playerIndex = playerIndex;
    }

    public void ChangeOffSetAndSize(Vec2 playerPos, float[] theStats)
    {
        x = playerPos.x + theStats[0];
        y = playerPos.y + theStats[1];
        width =  (int)theStats[2];
        height =   (int)theStats[3];
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