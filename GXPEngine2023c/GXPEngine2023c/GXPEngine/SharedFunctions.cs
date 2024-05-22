using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/*
 * Providing functions that all classes can use
 */
public static class SharedFunctions
{
    
    //intersectionn with player red box and sprite
    public static bool CheckIntersectSpriteDetectionRange(Player thisObject, Sprite thatObject)
    {
        // Calculate half-width and half-height for each rectangle
        double rect1HalfWidth = thisObject.playerHitBox.width / 2;
        double rect1HalfHeight = thisObject.playerHitBox.height / 2;

        double rect2HalfWidth = thatObject.width / 2;
        double rect2HalfHeight = thatObject.height / 2;

        // Calculate the centers of the rectangles
        double rect1CenterX = thisObject.playerHitBox.GetX() + rect1HalfWidth;
        double rect1CenterY = thisObject.playerHitBox.GetY() + rect1HalfHeight;
        double rect2CenterX = thatObject.x;
        double rect2CenterY = thatObject.y;

        // Calculate the distance between the centers of the rectangles
        double distanceX = Math.Abs(rect1CenterX - rect2CenterX);
        double distanceY = Math.Abs(rect1CenterY - rect2CenterY);

        // Calculate the sum of half-widths and half-heights of the rectangles
        double sumHalfWidth = rect1HalfWidth + rect2HalfWidth;
        double sumHalfHeight = rect1HalfHeight + rect2HalfHeight;


        if (distanceX < (rect1HalfWidth + rect2HalfWidth) && distanceY < (rect1HalfHeight + rect2HalfHeight))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public static bool CheckIntersetSpriteBothTopLeftCorner(Sprite thisObject, Sprite thatObject)
    {
        float xOverlap = Math.Min(thisObject.x + thisObject.width, thatObject.x + thatObject.width) - Math.Max(thisObject.x, thatObject.x);
        float yOverlap = Math.Min(thisObject.y + thisObject.height, thatObject.y + thatObject.height) - Math.Max(thisObject.y, thatObject.y);
        
        if (xOverlap > 0 && yOverlap > 0)
        {
            return true;
        }

        return false;
    }


    public static bool CheckIntersectSpriteSprite(Sprite thisObject, Sprite thatObject)
    {
        // Calculate half-width and half-height for each rectangle
        double rect1HalfWidth = thisObject.width / 3;
        double rect1HalfHeight = thisObject.height / 3;

        double rect2HalfWidth = thatObject.width / 2;
        double rect2HalfHeight = thatObject.height / 2;

        // Calculate the centers of the rectangles
        double rect1CenterX = thisObject.x + rect1HalfWidth;
        double rect1CenterY = thisObject.y + rect1HalfHeight;
        double rect2CenterX = thatObject.x;
        double rect2CenterY = thatObject.y;

        // Calculate the distance between the centers of the rectangles
        double distanceX = Math.Abs(rect1CenterX - rect2CenterX);
        double distanceY = Math.Abs(rect1CenterY - rect2CenterY);

        // Calculate the sum of half-widths and half-heights of the rectangles
        double sumHalfWidth = rect1HalfWidth + rect2HalfWidth;
        double sumHalfHeight = rect1HalfHeight + rect2HalfHeight;


        if (distanceX < (rect1HalfWidth + rect2HalfWidth) && distanceY < (rect1HalfHeight + rect2HalfHeight))
        {
            Console.WriteLine("CheckIntersectSpriteSprite");
            return true;
        }
        else
        {
            return false;
        }
    }
}
