using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//physcis for a ball object
public class ColliderRect : ColliderObject
{
    GameObject thisBallObject;
    public float width;
    public float height;
    public ColliderRect(GameObject pRectObject, Vec2 pPosition, Vec2 pVelocity, float pWidth, float pHeight, bool pMoving, float pDensity = 1) : base(pPosition, pVelocity, pMoving, pDensity)
    {
        thisBallObject = pRectObject;
        width = pWidth / 2;
        height = pHeight / 2;
        mass = 4 * width * height * _density;
    }

    //AABB collision detection with this and tile
    protected void CheckCollisionTiles(MyGame myGame)
    {
        //checking the bricks
        for (int i = 0; i < GameData.tileList.Count(); i++)
        {
            Tile theTile = GameData.tileList[i];

            float xOverlap = Math.Min(position.x + width, theTile.x + theTile.width) - Math.Max(position.x, theTile.x);
            float yOverlap = Math.Min(position.y + height, theTile.y + theTile.height) - Math.Max(position.y, theTile.y);

            if (xOverlap > 0 && yOverlap > 0)
            {
                float timeOfImpact = -1;

                if (xOverlap < yOverlap)
                {
                    if (position.x < theTile.x)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.x + width) - theTile.x) / Math.Abs(position.x - _oldPosition.x);
                     //  timeOfImpact = Math.Abs(_oldPosition.x - (theTile.x + width)) / Math.Abs(position.x - _oldPosition.x);

                        if (wordy2)
                        {
                            Console.WriteLine("right:" + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 4));
                        }
                    }
                    else
                    {
                       //    timeOfImpact = Math.Abs((_oldPosition.x + width) - theTile.x) / Math.Abs(position.x - _oldPosition.x);
                       timeOfImpact = Math.Abs(_oldPosition.x - (theTile.x + theTile.width)) / Math.Abs(position.x - _oldPosition.x);

                        if (wordy2)
                        {
                            Console.WriteLine("left: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 3));
                        }
                    }
                }

                else
                {
                    if (position.y < theTile.y)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.y + height) - theTile.y) / Math.Abs(_oldPosition.y - position.y);
                        if (wordy2)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 2));
                        }
                    }

                    else
                    {
                        timeOfImpact = Math.Abs(_oldPosition.y - (theTile.y + theTile.height)) / Math.Abs(_oldPosition.y - position.y);

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            if (wordy2)
                            {
                                Console.WriteLine("up: " + timeOfImpact);
                            }

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 1));
                        }
                    }
                }
            }

            /*
            if (Math.Abs(position.x - theTile.x) <= theTile.width
                && Math.Abs(position.y - theTile.y) <= theTile.height)
            {
                float timeOfImpact = -1;

                Console.WriteLine("pos:" + position + "tile: " + theTile.x + ',' + theTile.y);

                if (position.y > theTile.y)
                {
                    timeOfImpact = Math.Abs(_oldPosition.y - (theTile.y + theTile.height)) / Math.Abs(_oldPosition.y - position.y);

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        if (wordy2)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 2));
                    }
                }

                else if (position.y < theTile.y)
                {
                    timeOfImpact = Math.Abs((_oldPosition.y + height) - theTile.y) / Math.Abs(_oldPosition.y - position.y);
                    if (wordy2)
                    {
                        Console.WriteLine("up: " + timeOfImpact);
                    }

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 1));
                    }
                }

                else if (position.x > theTile.x)
                {
                    timeOfImpact = Math.Abs((_oldPosition.x + width) - theTile.x) / Math.Abs(position.x - _oldPosition.x);
                    //  timeOfImpact = Math.Abs(_oldPosition.x - (theTile.x + width)) / Math.Abs(position.x - _oldPosition.x);

                    if (wordy2)
                    {
                        Console.WriteLine("right:" + timeOfImpact);
                    }

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {


                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 4));
                    }
                }

                else if (position.x < theTile.x)
                {
                    //    timeOfImpact = Math.Abs((_oldPosition.x + width) - theTile.x) / Math.Abs(position.x - _oldPosition.x);
                    timeOfImpact = Math.Abs(_oldPosition.x - (theTile.x + theTile.width)) / Math.Abs(position.x - _oldPosition.x);

                    if (wordy2)
                    {
                        Console.WriteLine("left: " + timeOfImpact);
                    }

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {

                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 3));
                    }
                }
            
            }
            */
        }
    }

    //AABB collision detection with this and tile
    /*protected void CheckCollisionTilesUpAndDown(MyGame myGame)
    {
        //checking the bricks
        for (int i = 0; i < GameData.tileList.Count(); i++)
        {
            Tile theTile = GameData.tileList[i];

            if (Math.Abs(position.x - theTile.x) <= theTile.width
                && Math.Abs(position.y - theTile.y) <= theTile.height)
            {
                float timeOfImpact = -1;

                if (position.y > theTile.y)
                {
                    timeOfImpact = Math.Abs(_oldPosition.y - (theTile.y + theTile.height)) / Math.Abs(_oldPosition.y - position.y);

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        if (wordy1)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 2));
                    }
                }

                else if (position.y < theTile.y)
                {
                    timeOfImpact = Math.Abs((_oldPosition.y + height) - theTile.y) / Math.Abs(_oldPosition.y - position.y);
                    if (wordy1)
                    {
                        Console.WriteLine("up: " + timeOfImpact);
                    }

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {
                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 1));
                    }
                }
            }
        }
    }*/

    //AABB collision detection with this and tile
    /*protected void CheckCollisionTilesLeftAndRight(MyGame myGame)
    {
        //checking the bricks
        for (int i = 0; i < GameData.tileList.Count(); i++)
        {
            Tile theTile = GameData.tileList[i];

            if (Math.Abs(position.x - theTile.x) <= theTile.width
                && Math.Abs(position.y - theTile.y) <= theTile.height)
            {
                float timeOfImpact = -1;
                
                if (position.x > theTile.x)
                {
                    timeOfImpact = Math.Abs((_oldPosition.x + width) - theTile.x) / Math.Abs(position.x - _oldPosition.x);
                    //  timeOfImpact = Math.Abs(_oldPosition.x - (theTile.x + width)) / Math.Abs(position.x - _oldPosition.x);

                    if (wordy4)
                    {
                        Console.WriteLine("right:" + timeOfImpact);
                    }

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {


                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 4));
                    }
                }

                else if (position.x < theTile.x)
                {
                    //    timeOfImpact = Math.Abs((_oldPosition.x + width) - theTile.x) / Math.Abs(position.x - _oldPosition.x);
                    timeOfImpact = Math.Abs(_oldPosition.x - (theTile.x + theTile.width)) / Math.Abs(position.x - _oldPosition.x);

                    if (wordy4)
                    {
                        Console.WriteLine("left: " + timeOfImpact);
                    }

                    if (timeOfImpact <= 1 && timeOfImpact >= 0)
                    {

                        _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theTile, timeOfImpact, 3));
                    }
                }

            }


        }
    }*/

    protected override void ResolveCollision(CollisionInfo coll)
    {
        /*
        if (col == null)
        {
            return;
        }
        */

        foreach (CollisionInfo col in _collisionList)
        {
            if (col.other is Tile)
            {

                Tile theTile = (Tile)col.other;

               
                Vec2 centerOfMass = (Mass * velocity + theTile.Mass * new Vec2(0, 0)) / (Mass + theTile.Mass);
                Vec2 momentum = -bounciness * velocity;
                Vec2 POI = _oldPosition + (col.timeOfImpact * velocity);
                

                if (col.AABBDirection == 1)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving up");
                    }

                    position.y -= POI.y - position.y;
                    velocity.y = momentum.y;
                }

                else if (col.AABBDirection == 2)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving down");
                    }

                    position.y += POI.y - position.y;
                    velocity.y = momentum.y;
                }

                else if (col.AABBDirection == 4)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving right");
                    }

                    position.x -= position.x - POI.x;
                    velocity.x = momentum.x;
                }

                else if (col.AABBDirection == 3)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving left");
                    }

                    position.x += position.x - POI.x;
                    velocity.x = momentum.x;
                }
            }
        }
        
        /*
        //brick resolve logic
        if (col.other is Tile)
        {

            Tile theTile = (Tile)col.other;

            float xOverlap = Math.Min(position.x + width, theTile.x + theTile.width) - Math.Max(position.x, theTile.x);
            float yOverlap = Math.Min(position.y + height, theTile.y + theTile.height) - Math.Max(position.y, theTile.y);

            Vec2 centerOfMass = (Mass * velocity + theTile.Mass * new Vec2(0, 0)) / (Mass + theTile.Mass);
            Vec2 momentum = centerOfMass - bounciness * (velocity - centerOfMass);
            Vec2 POI = _oldPosition + (col.timeOfImpact * velocity);

            if (xOverlap > 0 && yOverlap > 0)
            {
                if (xOverlap < yOverlap)
                {
                    if (position.x < theTile.x)
                    {
                        if (wordy2)
                        {
                            Console.WriteLine("resolving right");
                        }

                        position.x -= position.x - POI.x;
                        velocity.x = momentum.x;
                    }
                    else
                    {
                        if (wordy2)
                        {
                            Console.WriteLine("resolving 'left");
                        }

                        position.x -= position.x - POI.x;
                        velocity.x = momentum.x;
                    }
                }

                else
                {
                    if (position.y < theTile.y)
                    {
                        if (wordy2)
                        {
                            Console.WriteLine("resolving up");
                        }

                        position.y -= POI.y - position.y;
                        velocity.y = momentum.y;
                    }

                    else
                    {
                        if (wordy2)
                        {
                            Console.WriteLine("resolving down");
                        }

                        position.y += POI.y - position.y;
                        velocity.y = momentum.y;
                    }
                }
            }
        }
        */
       
    }

    protected override CollisionInfo FindEarliestCollision()
    {
        MyGame myGame = (MyGame)game;
        CheckCollisionTiles(myGame);

        return FindLowestTOICollision();
    }
}