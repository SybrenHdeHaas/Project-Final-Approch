﻿using GXPEngine;
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
    }

    public void ChangeOffSetAndSize(float[] theStats)
    {
        x = theStats[0];
        y = theStats[1];
        width = (int)theStats[2];
        height = (int)theStats[3];
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