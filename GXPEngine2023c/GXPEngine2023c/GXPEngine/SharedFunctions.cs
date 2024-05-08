using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Providing functions that all classes can use
 */
public static class SharedFunctions
{
    //Check if two AnimationSpriteCustom objects intersect
    public static bool IntersectsAnimationSpriteCustom(AnimationSpriteCustom thisObject, AnimationSpriteCustom thatObject)
    {
        if (Math.Abs(thisObject.x - thatObject.x) <= (thisObject.width / 2) + (thatObject.width / 2) &&
            Math.Abs(thisObject.y - thatObject.y) <= (thisObject.height / 2) + (thatObject.width / 2))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
