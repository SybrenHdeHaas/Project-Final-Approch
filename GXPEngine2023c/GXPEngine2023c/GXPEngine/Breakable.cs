using GXPEngine;
using GXPEngine.Core;
using System;
using System.Runtime.InteropServices;
using TiledMapParser;

public class Breakable : AnimationSpriteCustom
{
    Vec2 thresholdVelocity; //if the player collides with this over this velocity amount, it's health will be damaged by 1
    int theHealth;

    public Breakable(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        thresholdVelocity.x = obj.GetFloatProperty("float_thresholdVelocityX");
        thresholdVelocity.y = obj.GetFloatProperty("float_thresholdVelocityy");
        theHealth = obj.GetIntProperty("int_theHealth");
    }

    public void tryDamage(Vec2 otherVelocity)
    {
        if (otherVelocity.Length() > thresholdVelocity.Length())
        {
            theHealth--;

            if (theHealth < 0)
            {
                //TODO: remove the wall
            }
        }
    }
}

