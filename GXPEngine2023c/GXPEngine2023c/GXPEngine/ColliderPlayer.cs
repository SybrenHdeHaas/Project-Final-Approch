using GXPEngine;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

//collision calculation and related for player (we use hitbox as the collider for player)
public class ColliderPlayer : ColliderObject
{
    Hitbox thisObject;
    public float width;
    public float height;
    public ColliderPlayer(Hitbox pRectObject, Vec2 pPosition, Vec2 pVelocity, float pWidth, float pHeight, bool pMoving, float pDensity = 1) : base(pPosition, pVelocity, pMoving, pDensity)
    {
        thisObject = pRectObject;
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
        }
    }

    //AABB collision check with other player
    protected void CheckCollisionHitBox(MyGame myGame)
    {
        //checking the bricks
        for (int i = 0; i < GameData.playerList.Count(); i++)
        {
            Hitbox theDetection = GameData.playerList[i].playerHitBox;

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


    //AABB collision check with door object
    protected void CheckCollisionDoor(MyGame myGame) //possible fourth step
    {
        //checking the bricks
        foreach (KeyValuePair<string, Door> theDoorEntry in GameData.doorList)
        {
            Door theDoor = theDoorEntry.Value;

            double dx = Math.Abs((position.x + width / 2) - (theDoor.x));
            double dy = Math.Abs((position.y + height / 2) - (theDoor.y));

            double overlapX = (width + theDoor.width) / 2 - dx;
            double overlapY = (height + theDoor.height) / 2 - dy;

            double rect1HalfWidth = width / 2;
            double rect1HalfHeight = height / 2;

            double rect2HalfWidth = theDoor.width / 2;
            double rect2HalfHeight = theDoor.height / 2;

            // Calculate the centers of the rectangles
            double rect1CenterX = position.x + rect1HalfWidth;
            double rect1CenterY = position.y + rect1HalfHeight;
            double rect2CenterX = theDoor.x;
            double rect2CenterY = theDoor.y;

            // Calculate the distance between the centers of the rectangles
            double distanceX = Math.Abs(rect1CenterX - rect2CenterX);
            double distanceY = Math.Abs(rect1CenterY - rect2CenterY);

            // Calculate the sum of half-widths and half-heights of the rectangles
            double sumHalfWidth = rect1HalfWidth + rect2HalfWidth;
            double sumHalfHeight = rect1HalfHeight + rect2HalfHeight;


            if (distanceX < (rect1HalfWidth + rect2HalfWidth) && distanceY < (rect1HalfHeight + rect2HalfHeight))
            {
                float timeOfImpact = -1;

                if (theDoor.isOpened)
                {
                    continue;
                }

                if (wordy6)
                {
                    Console.WriteLine("aaaaaa");
                }

                if (overlapX < overlapY)
                {
                    if (position.x < theDoor.x)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.x + width) - (theDoor.x - theDoor.width / 2)) / Math.Abs(position.x - _oldPosition.x);


                        if (wordy6)
                        {
                            Console.WriteLine("right:" + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), theDoor, timeOfImpact, 4));
                        }


                    }
                    else
                    {

                        timeOfImpact = Math.Abs(_oldPosition.x - (theDoor.x + theDoor.width / 2)) / Math.Abs(position.x - _oldPosition.x);


                        if (wordy6)
                        {
                            Console.WriteLine("left: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), theDoor, timeOfImpact, 3));
                        }
                    }
                }

                else
                {
                    if (position.y < theDoor.y - theDoor.height)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.y + height) - (theDoor.y - theDoor.height / 2)) / Math.Abs(_oldPosition.y - position.y);
                        if (wordy6)
                        {
                            Console.WriteLine("up: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theDoor, timeOfImpact, 2));
                        }
                    }

                    else
                    {

                        timeOfImpact = Math.Abs(_oldPosition.y - (theDoor.y + theDoor.height / 2)) / Math.Abs(_oldPosition.y - position.y);

                        if (wordy6)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theDoor, timeOfImpact, 1));
                        }
                    }
                }
            }
        }
    }


    //AABB collision check with breakable, if detected, breakable would be pushed
    protected void CheckCollisionBreakable(MyGame myGame) //possible fourth step
    {
        //checking the bricks
        for (int i = 0; i < GameData.breakableList.Count(); i++)
        {
            Breakable theBreakable = GameData.breakableList[i];

            double dx = Math.Abs((position.x + width / 2) - (theBreakable.x));
            double dy = Math.Abs((position.y + height / 2) - (theBreakable.y));

            double overlapX = (width + theBreakable.width) / 2 - dx;
            double overlapY = (height + theBreakable.height) / 2 - dy;

            double rect1HalfWidth = width / 2;
            double rect1HalfHeight = height / 2;

            double rect2HalfWidth = theBreakable.width / 2;
            double rect2HalfHeight = theBreakable.height / 2;

            // Calculate the centers of the rectangles
            double rect1CenterX = position.x + rect1HalfWidth;
            double rect1CenterY = position.y + rect1HalfHeight;
            double rect2CenterX = theBreakable.x;
            double rect2CenterY = theBreakable.y;

            // Calculate the distance between the centers of the rectangles
            double distanceX = Math.Abs(rect1CenterX - rect2CenterX);
            double distanceY = Math.Abs(rect1CenterY - rect2CenterY);

            // Calculate the sum of half-widths and half-heights of the rectangles
            double sumHalfWidth = rect1HalfWidth + rect2HalfWidth;
            double sumHalfHeight = rect1HalfHeight + rect2HalfHeight;


            if (distanceX < (rect1HalfWidth + rect2HalfWidth) && distanceY < (rect1HalfHeight + rect2HalfHeight))
            {
                float timeOfImpact = -1;


                if (wordy6)
                {
                    Console.WriteLine("aaaaaa");
                }

                if (overlapX < overlapY)
                {
                    if (position.x < theBreakable.x)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.x + width) - (theBreakable.x - theBreakable.width / 2)) / Math.Abs(position.x - _oldPosition.x);

                        if (wordy6)
                        {
                            Console.WriteLine("right:" + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), theBreakable, timeOfImpact, 4));
                        }


                    }
                    else
                    {

                        timeOfImpact = Math.Abs(_oldPosition.x - (theBreakable.x + theBreakable.width / 2)) / Math.Abs(position.x - _oldPosition.x);


                        if (wordy6)
                        {
                            Console.WriteLine("left: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), theBreakable, timeOfImpact, 3));
                        }
                    }
                }

                else
                {
                    if (position.y < theBreakable.y - theBreakable.height)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.y + height) - (theBreakable.y - theBreakable.height / 2)) / Math.Abs(_oldPosition.y - position.y);
                        if (wordy6)
                        {
                            Console.WriteLine("up: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theBreakable, timeOfImpact, 2));
                        }
                    }

                    else
                    {

                        timeOfImpact = Math.Abs(_oldPosition.y - (theBreakable.y + theBreakable.height / 2)) / Math.Abs(_oldPosition.y - position.y);

                        if (wordy6)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theBreakable, timeOfImpact, 1));
                        }
                    }
                }
            }
        }
    }
    protected override void ResolveCollision(CollisionInfo earilstCollision)
    {
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

                    foreach (Player thePlayer in GameData.playerList)
                    {
                        if (thePlayer.playerIndex == thisObject.playerIndex)
                        {
                            thePlayer.onCeiling = true;
                            
                        }
                    }

                    position.y -= POI.y - position.y;
                    velocity.y = momentum.y;
                }

                if (col.AABBDirection == 2)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving down");
                    }

                    foreach (Player thePlayer in GameData.playerList)
                    {
                        if (thePlayer.playerIndex == thisObject.playerIndex)
                        {
                            thePlayer.onGround = true;
                        }
                    }

                    position.y += POI.y - position.y;
                    velocity.y = momentum.y;
                }

                if (col.AABBDirection == 4)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving right");
                    }

                    position.x -= position.x - POI.x;
                    velocity.x = momentum.x;
                }

                if (col.AABBDirection == 3)
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
            else if (col.other is Hitbox)
            {
                Hitbox theHitbox = (Hitbox)col.other;
                Vec2 centerOfMass = (Mass * velocity + theHitbox.mass * new Vec2(0, 0)) / (Mass + theHitbox.mass);
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


            else if (col.other is Door)
            {
                Door theDoor = (Door)col.other;
                Vec2 centerOfMass = (Mass * velocity + theDoor.Mass * new Vec2(0, 0)) / (Mass + theDoor.Mass);
                Vec2 momentum = -bounciness * velocity;
                Vec2 POI = _oldPosition + (col.timeOfImpact * velocity);

                if (col.AABBDirection == 1)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving up");
                    }

                    foreach (Player thePlayer in GameData.playerList)
                    {
                        if (thePlayer.playerIndex == thisObject.playerIndex)
                        {
                            thePlayer.onCeiling = true;
                        }
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

                    foreach (Player thePlayer in GameData.playerList)
                    {
                        if (thePlayer.playerIndex == thisObject.playerIndex)
                        {
                            thePlayer.onDoor = true;

                        }
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


            else if (col.other is Breakable)
            {
                Breakable theBreakable = (Breakable)col.other;
                Vec2 centerOfMass = (Mass * velocity + theBreakable.Mass * theBreakable.velocity) / (Mass + theBreakable.Mass);
                Vec2 momentum = centerOfMass - bounciness * (velocity - centerOfMass);
                Vec2 POI = _oldPosition + (col.timeOfImpact * velocity);

                position.y -= POI.y - position.y;
                velocity.y = momentum.y;


                float breakAbleForceMultiply = 0.85f;

                if (col.AABBDirection == 1)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving up");
                    }

                    position.y -= POI.y - position.y;
                    velocity.y = momentum.y;
                    //     theBreakable.position.y += POI.y - position.y;
                    theBreakable.velocity.y = momentum.y * breakAbleForceMultiply;
                }

                else if (col.AABBDirection == 2)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving down");
                    }

                    position.y += POI.y - position.y;

                    velocity.y = momentum.y;
                    theBreakable.velocity.y = momentum.y * breakAbleForceMultiply;
                }

                else if (col.AABBDirection == 4)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving right");
                    }

                    position.x -= position.x - POI.x;

                    velocity.x = momentum.x;
                    theBreakable.velocity.x = momentum.x * breakAbleForceMultiply;
                }

                else if (col.AABBDirection == 3)
                {
                    if (wordy4)
                    {
                        Console.WriteLine("resolving left");
                    }

                    position.x += position.x - POI.x;

                    velocity.x = momentum.x;
                    theBreakable.velocity.x = momentum.x * breakAbleForceMultiply;
                }

                if (col.AABBDirection >= 0 && col.AABBDirection <= 4)
                {
                    Player thePlayer = null;
                    foreach (Player thePlayerr in GameData.playerList)
                    {
                        if (thePlayerr.playerIndex == thisObject.playerIndex)
                        {
                            thePlayer = thePlayerr;
                        }
                    }

                    if (thePlayer == null)
                    {
                        Console.WriteLine("null player should not happen");
                    }

                    if (theBreakable.TryDamage(thePlayer.Velocity) == true)
                    {
                        GameData.breakableList.Remove(theBreakable);
                        theBreakable.Destroy();
                        return;
                    }
                }
            }
        }
    }

    protected override CollisionInfo FindEarliestCollision()
    {
        MyGame myGame = (MyGame)game;
        CheckCollisionTiles(myGame);
        CheckCollisionHitBox(myGame);
        CheckCollisionDoor(myGame);
        CheckCollisionBreakable(myGame);

        return FindLowestTOICollision();
    }
}