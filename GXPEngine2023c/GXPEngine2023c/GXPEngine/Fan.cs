using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Fan : AnimationSpriteCustom
{
    Vec2 pushVelocityDefault = new Vec2(0, 0);
    Vec2 pushVelocity; //any object impacted by fan would be applied with this velocity if the fan is on
    float theForce;
    int theDirection;
    public bool isOn;

    bool isOnCurrent;

    public SoundChannel fanSound;


    public Fan(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        theForce = obj.GetFloatProperty("float_theForce", 1);
        theDirection = obj.GetIntProperty("int_theDirection", 1);
        isOn = obj.GetBoolProperty("boolean_isOn");
    }

    public Fan(TiledObject obj = null) : base("Player.png", 1, 1, obj)
    {
        alpha = 0;
        theForce = obj.GetFloatProperty("float_theForce", 1);
        theDirection = obj.GetIntProperty("int_theDirection", 1);
        isOn = obj.GetBoolProperty("boolean_isOn");
    }

    void CalculateVelocity()
    {
        pushVelocity = pushVelocityDefault;
        switch (theDirection)
        {
            case 2:
                pushVelocity.x += theForce; //right
                break;
            case 3:
                pushVelocity.y += theForce; //up
                break;
            case 4:
                pushVelocity.y += -theForce; //down
                break;
            default:
                pushVelocity.x += -theForce; //left
                break;
        }
    }

    void Update()
    {
        if (isOn)
        {
            SetAnimationCycle(1, 1);
        }

        else
        {
            SetAnimationCycle(0, 1);
        }


        if (isOnCurrent != isOn)
        {
            isOnCurrent = isOn;
            if (isOn)
            {
                fanSound = new Sound("fan_loop.wav", true).Play();
                fanSound.Volume = 3f;
            }

            else
            {
                if (fanSound != null)
                {
                    fanSound.Stop();
                }
            }
        }

    }

    public Vec2 GetVelocity()
    {
        if (isOn)
        {

            CalculateVelocity();
            return pushVelocity;
        }

        else {
            return pushVelocityDefault;
        }
    }
}