using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//physcis for a ball object
public class ColliderRect : ColliderObject
{
    GameObject thisBallObject;
    public float getWidth() { return width; }
    public float getHeight() { return height; }
    private float width;
    private float height;
    public ColliderRect(GameObject pRectObject, Vec2 pPosition, Vec2 pVelocity, float pWidth, float pHeight, bool pMoving, float pDensity = 1) : base(pPosition, pVelocity, pMoving, pDensity)
    {

        thisBallObject = pRectObject;
        width = pWidth / 2;
        height = pHeight / 2;
        mass = 4 * width * height * _density;

    }

    protected override CollisionInfo FindEarliestCollision() //Overriden from third step in colliderObject?
    {
        MyGame myGame = (MyGame)game;
        CheckCollisionTiles(myGame);

        return FindLowestTOICollision();
    }


    //AABB collision detection with THIS and tile
    protected void CheckCollisionTiles(MyGame myGame) //possible fourth step
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
                        

                        if (wordy2)
                        {
                            Console.WriteLine("right:" + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), theTile, timeOfImpact, 4));
                        }


                    }
                    else
                    {

                        timeOfImpact = Math.Abs(_oldPosition.x - (theTile.x + theTile.width)) / Math.Abs(position.x - _oldPosition.x);
                        

                        if (wordy2)
                        {
                            Console.WriteLine("left: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), theTile, timeOfImpact, 3));
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

        }
    }

    protected override void ResolveCollision(CollisionInfo coll) //contineu from ColliderObject (fifth step)
    {

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

                else if (col.AABBDirection == 3)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving left");
                    }

                    position.x += position.x - POI.x;
                    velocity.x = momentum.x;
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

                
            }
        }
    }
}