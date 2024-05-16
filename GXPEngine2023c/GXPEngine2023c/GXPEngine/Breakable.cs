using GXPEngine;
using GXPEngine.Core;
using System;
using System.Runtime.InteropServices;
using TiledMapParser;

public class Breakable : AnimationSpriteCustom
{
    float thresholdVelocityLength; //the threshold length
    int theHealth;

    public float Mass
    {
        get
        {
            return 4 * width / 2 * height / 2;
        }
    }

    public Breakable(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        thresholdVelocityLength = obj.GetFloatProperty("float_thresholdVelocityLength");
        theHealth = obj.GetIntProperty("int_theHealth");
    }

    public Boolean TryDamage(Vec2 otherVelocity)
    {
        //comparing player velocity length and the threshold length;
        if (otherVelocity.Length() > thresholdVelocityLength)
        {
            theHealth--;

            if (theHealth < 0)
            {
                return true;
            }
        }

        return false;
    }
}

