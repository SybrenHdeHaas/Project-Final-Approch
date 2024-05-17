using GXPEngine;
using GXPEngine.Core;
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
        double rect1HalfWidth = thisObject.playerHitBox.width / 3;
        double rect1HalfHeight = thisObject.playerHitBox.height / 3;

        double rect2HalfWidth = thatObject.width / 2;
        double rect2HalfHeight = thatObject.height / 2;

        // Calculate the centers of the rectanglesad
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
            if (Input.GetKeyDown(Key.X)) 
            {
                Console.WriteLine();
                Console.WriteLine("distanceX {0}, distanceY {1}", distanceX, distanceY);
                Console.WriteLine("rect1HalfWidth {0}, rect1HalfHeight {1}", rect1HalfWidth, rect1HalfHeight) ;
                Console.WriteLine("rect2HalfWidth {0}, rect2HalfHeight {1}", rect2HalfWidth, rect2HalfHeight) ;
                Console.WriteLine("rect1CenterX {0}, rect1CenterY {1}, rect2CenterX {2}, rect2CenterY {3}", rect1CenterX, rect1CenterY, rect2CenterX, rect2CenterY);
                Console.WriteLine("CheckIntersectSpriteDetectionRange");
                Console.WriteLine();
            }
            
            return true;
        }
        else
        {
            
            return false;
        }
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

    public static bool CheckIntersectSpriteCornerMiddle(Sprite thisObject, Sprite thatObject)
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
            Console.WriteLine("CheckIntersectSpriteCornerMiddle");
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool CheckPointWithRect(Vector2 boxCrood, int boxWidth, int boxHeight, Vector2 otherCrood)
    {
        if (boxCrood.x - boxWidth / 2 <= otherCrood.x && boxCrood.x + boxWidth / 2 >= otherCrood.x)
        {
            if (boxCrood.y - boxHeight / 2 <= otherCrood.y && boxCrood.y + boxHeight / 2 >= otherCrood.y)
            {
                Console.WriteLine("CheckPointWithRect");
                return true;
            }
        }
        return false;
    }
}
