using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Shell : AnimationSpriteCustom
{
    Vec2 pushVelocityDefault = new Vec2(0, 0);
    Vec2 pushVelocity; //any object impacted by fan would be applied with this velocity if the fan is on
    float theForce;
    int theDirection;
    bool isOn = true;
    public Shell(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        theForce = obj.GetFloatProperty("float_theForce", 1);
        theDirection = obj.GetIntProperty("int_theDirection", 1);
    }
}