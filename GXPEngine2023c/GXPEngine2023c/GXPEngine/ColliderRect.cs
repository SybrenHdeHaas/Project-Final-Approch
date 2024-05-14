using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

//physcis for a ball object
public class ColliderRect : ColliderObject
{
    GameObject thisBallObject;
    public float Width 
    { 
        get { return width; }
        set { width = value; } 
    }
    public float Height
    {
        get { return height; }
        set { height = value; }
    }
    private float width;
    private float height;
    GameObject thisObject;
    public ColliderRect(GameObject pRectObject, Vec2 pPosition, Vec2 pVelocity, float pWidth, float pHeight, bool pMoving, float pDensity = 1) : base(pPosition, pVelocity, pMoving, pDensity)
    {
        thisObject = pRectObject;
        width = pWidth / 2;
        height = pHeight / 2;
        mass = 4 * width * height * _density;
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

    //AABB collision detection with this and tile
    protected void CheckCollisionDetection(MyGame myGame)
    {
        //checking the bricks
        for (int i = 0; i < GameData.playerList.Count(); i++)
        {
            Detection theDetection = GameData.playerList[i].detectionRange;

            if (thisObject == theDetection)
            {
                continue;
            }

            float xOverlap = Math.Min(position.x + width, theDetection.GetX() + theDetection.width) - Math.Max(position.x, theDetection.GetX());
            float yOverlap = Math.Min(position.y + height, theDetection.GetY() + theDetection.height) - Math.Max(position.y, theDetection.GetY());

            if (xOverlap > 0 && yOverlap > 0)
            {
                float timeOfImpact = -1;

                if (xOverlap < yOverlap)
                {
                    if (position.x < theDetection.GetX())
                    {
                        timeOfImpact = Math.Abs((_oldPosition.x + width) - theDetection.GetX()) / Math.Abs(position.x - _oldPosition.x);

                        if (wordy5)
                        {
                            Console.WriteLine("right:" + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theDetection, timeOfImpact, 4));
                        }
                    }
                    else
                    {
                        timeOfImpact = Math.Abs(_oldPosition.x - (theDetection.GetX() + theDetection.width)) / Math.Abs(position.x - _oldPosition.x);

                        if (wordy5)
                        {
                            Console.WriteLine("left: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theDetection, timeOfImpact, 3));
                        }
                    }
                }

                else
                {
                    if (position.y < theDetection.GetY())
                    {
                        timeOfImpact = Math.Abs((_oldPosition.y + height) - theDetection.GetY()) / Math.Abs(_oldPosition.y - position.y);
                        if (wordy5)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theDetection, timeOfImpact, 2));
                        }
                    }

                    else
                    {
                        timeOfImpact = Math.Abs(_oldPosition.y - (theDetection.GetY() + theDetection.height)) / Math.Abs(_oldPosition.y - position.y);

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            if (wordy5)
                            {
                                Console.WriteLine("up: " + timeOfImpact);
                            }

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theDetection, timeOfImpact, 1));
                        }
                    }
                }
            }
        }
    }

    /*
    //AABB collision detection with this and player
    protected void CheckCollisionPlayers(MyGame myGame)
    {
        for (int i = 0; i < GameData.playerList.Count(); i++)
        {
            Player theOtherPlayer = GameData.playerList[i];

            if (theOtherPlayer == thisObject)
            {
                continue;
            }

            float xOverlap = Math.Min(position.x + width, theOtherPlayer.x + theOtherPlayer.width) - Math.Max(position.x, theOtherPlayer.x);
            float yOverlap = Math.Min(position.y + height, theOtherPlayer.y + theOtherPlayer.height) - Math.Max(position.y, theOtherPlayer.y);

            if (xOverlap > 0 && yOverlap > 0)
            {

                float timeOfImpact = -1;

                Vec2 detectionPosThis = new Vec2(thisObject.detectionRange.x + position.x
                    , thisObject.detectionRange.y + position.y);
                Vec2 detectionPosOther = new Vec2(theOtherPlayer.detectionRange.GetX()
                    , theOtherPlayer.detectionRange.GetY());

                Vec2 oldPos = _oldPosition + new Vec2(thisObject.detectionRange.x
                    , thisObject.detectionRange.y);

                float widthDetectionThis = thisObject.detectionRange.width;
                float widthDetectionOther = theOtherPlayer.detectionRange.width;
                float heightDetectionThis = thisObject.detectionRange.height;
                float heightDetectionOther = theOtherPlayer.detectionRange.height;


                if (xOverlap < yOverlap)
                {
                    if (detectionPosThis.x < detectionPosOther.x)
                    {
                        timeOfImpact = Math.Abs((oldPos.x + widthDetectionThis) - theOtherPlayer.x) / Math.Abs(detectionPosThis.x - oldPos.x);

                        if (wordy5)
                        {
                            Console.WriteLine("right:" + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theOtherPlayer, timeOfImpact, 4));
                        }
                    }
                    else
                    {
                        timeOfImpact = Math.Abs(oldPos.x - (theOtherPlayer.x + widthDetectionOther)) / Math.Abs(detectionPosThis.x - oldPos.x);

                        if (wordy5)
                        {
                            Console.WriteLine("left: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theOtherPlayer, timeOfImpact, 3));
                        }
                    }
                }

                else
                {
                    if (detectionPosThis.y < detectionPosOther.y)
                    {
                        timeOfImpact = Math.Abs((oldPos.y + heightDetectionThis) - theOtherPlayer.y) / Math.Abs(oldPos.y - detectionPosThis.y);
                        if (wordy5)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theOtherPlayer, timeOfImpact, 2));
                        }
                    }

                    else
                    {
                        timeOfImpact = Math.Abs(oldPos.y - (detectionPosOther.y + heightDetectionOther)) / Math.Abs(oldPos.y - detectionPosThis.y);

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            if (wordy5)
                            {
                                Console.WriteLine("up: " + timeOfImpact);
                            }

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theOtherPlayer, timeOfImpact, 1));
                        }
                    }
                }
            }
        }
    }
    */

    protected override void ResolveCollision(CollisionInfo earilstCollision)
    {

        //earilstCollision is not used. Maybe for now.

        //solving all collisions
        foreach (CollisionInfo col in _collisionList)
        {
            //collision with a tile
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

            //collision with detection
            if (col.other is Detection)
            {
                Detection theDetection = (Detection)col.other;
                Vec2 centerOfMass = (Mass * velocity + theDetection.mass * new Vec2(0, 0)) / (Mass + theDetection.mass);
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
            }

            /*
            //collision with another player
            if (col.other is Player)
            {

                if (col.other == thisObject)
                {
                    continue;
                }

                Player thePlayer = (Player)col.other;

                Vec2 oldPos = _oldPosition + new Vec2(thisObject.detectionRange.x
    , thisObject.detectionRange.y);

                Vec2 centerOfMass = (Mass * velocity + thePlayer.mass * new Vec2(0, 0)) / (Mass + thePlayer.mass);
                Vec2 momentum = -bounciness * velocity;
                Vec2 POI = oldPos + (col.timeOfImpact * velocity);


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
            */
        }
    }

    protected override CollisionInfo FindEarliestCollision()
    {
        MyGame myGame = (MyGame)game;
        CheckCollisionTiles(myGame);
        CheckCollisionDetection(myGame);

        return FindLowestTOICollision();
    }
}