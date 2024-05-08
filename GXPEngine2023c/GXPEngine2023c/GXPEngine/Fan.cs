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
    bool isOn = true;
    public Fan(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        theForce = obj.GetFloatProperty("float_theForce", 1);
        theDirection = obj.GetIntProperty("int_theDirection", 1);
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