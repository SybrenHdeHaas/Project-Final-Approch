using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

//physcis for a ball object
public class ColliderRect : ColliderObject
{
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

    private float[] startStats = new float[4];
    private float[] shellsStats = new float[4];

    Sprite thisObject;
    public ColliderRect(Sprite pRectObject, Vec2 pPosition, Vec2 pVelocity, float offsetX, float offsetY, float pWidth, float pHeight, bool pMoving, float pDensity = 1) : base(pPosition, pVelocity, pMoving, pDensity)
    {
        position = pPosition;
        thisObject = pRectObject;

        width = pWidth / 3;
        height = pHeight / 3;
        mass = 4 * width * height * _density;

        startStats[0] = position.x;
        startStats[1] = position.y;
        startStats[2] = width;
        startStats[3] = height;

        shellsStats[0] = position.x;
        shellsStats[1] = 0;
        shellsStats[2] = width;
        shellsStats[3] = height / 3;

    }

    public void inShellChanges()
    {
        Console.WriteLine("inshell colliderRect");
        position.x = shellsStats[0];
        position.y = shellsStats[1];
        width = (int)shellsStats[2];
        height = (int)shellsStats[3];

    }

    public void outShellChanges()
    {
        position.x = startStats[0];
        position.y = startStats[1];
        width = (int)startStats[2];
        height = (int)startStats[3];
    }



    protected override CollisionInfo FindEarliestCollision() //Overriden from third step in colliderObject?
    {
        MyGame myGame = (MyGame)game;
        CheckCollisionTiles(myGame);
        CheckCollisionHitbox(myGame);
        CheckCollisionDoor(myGame);
        CheckCollisionBreakable(myGame);

        return FindLowestTOICollision();
    }


    //AABB collision detection with THIS and tile
    protected void CheckCollisionTiles(MyGame myGame)
    {
        // Iterate through the tile list to check collisions
        foreach (var tile in GameData.tileList)
        {
            // Calculate overlap on both axes
            float xOverlap = Math.Min(position.x + width, tile.x + tile.width) - Math.Max(position.x, tile.x);
            float yOverlap = Math.Min(position.y + height, tile.y + tile.y) - Math.Max(position.y, tile.y);

/*            if (Input.GetKeyDown(Key.P))
            {
                Console.WriteLine();
                Console.WriteLine("pos X {0}, width {1}, tile.x {2}, tile.width {3}", position.x, width, tile.x, tile.width);
                Console.WriteLine("pos y {0}, height {1}, tile.y {2}, tile.y {3}", position.y, height, tile.y, tile.y);
                Console.WriteLine();
            }*/


            // Check if there is an overlap
            if (xOverlap > 0 && yOverlap > 0)
            {
                float timeOfImpact = -1;
                bool isHorizontalCollision = xOverlap < yOverlap;

                if (isHorizontalCollision)
                {
                    // Handle horizontal collisions
                    if (position.x <= tile.x)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.x + width) - tile.x) / Math.Abs(position.x - _oldPosition.x);
                        if (wordy2)
                        {
                            Console.WriteLine("right:" + timeOfImpact);
                            Console.WriteLine("oldPosition {0}, newPosition {1}, width{2}", _oldPosition, position.x, width);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), tile, timeOfImpact, 4)); // Right collision
                        }
                    }
                    else
                    {
                        timeOfImpact = Math.Abs(_oldPosition.x - (tile.x + tile.width)) / Math.Abs(position.x - _oldPosition.x);
                        if (wordy2)
                        {
                            Console.WriteLine("left: " + timeOfImpact);
                            Console.WriteLine("oldPosition {0}, newPosition {1}, width{2}", _oldPosition, position.x, width);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0f, 0f), tile, timeOfImpact, 3)); // Left collision
                        }
                    }
                }
                else
                {
                    // Handle vertical collisions
                    if (position.y < tile.y)
                    {
                        timeOfImpact = Math.Abs((_oldPosition.y + height) - tile.y) / Math.Abs(_oldPosition.y - position.y);
                        if (wordy2)
                        {
                            Console.WriteLine("down: " + timeOfImpact);
                        }

                        if (timeOfImpact <= 1 && timeOfImpact > 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), tile, timeOfImpact, 2)); // Down collision
                        }
                    }
                    else
                    {
                        timeOfImpact = Math.Abs(_oldPosition.y - (tile.y + tile.height)) / Math.Abs(_oldPosition.y - position.y);
                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            if (wordy2)
                            {
                                Console.WriteLine("up: " + timeOfImpact);
                            }

                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), tile, timeOfImpact, 1)); // Up collision
                        }
                    }
                }
            }
        }
    }


    //AABB collision detection with this and tile
    protected void CheckCollisionHitbox(MyGame myGame)
    {
        // Iterate through the player list to check collisions
        foreach (var player in GameData.playerList)
        {
            Hitbox theHitBox = player.playerHitBox;

            // Skip if thisObject is a hitbox and the same as the player's hitbox
            if (thisObject is Hitbox hitbox && thisObject == theHitBox)
            {
                continue;
            }

            // Calculate overlap on both axes
            float xOverlap = Math.Min(position.x + width, theHitBox.GetTrueX()+ theHitBox.trueWidth) - Math.Max(position.x, theHitBox.GetTrueX());
            float yOverlap = Math.Min(position.y + height + height, theHitBox.GetTrueY() + theHitBox.trueHeight) - Math.Max(position.y, theHitBox.GetTrueY());



            if (Input.GetKeyDown(Key.P))
            {
                Console.WriteLine();
                Console.WriteLine("xOverlap {0}, yOverlap {1}", xOverlap, yOverlap);
                Console.WriteLine("pos X {0}, width {1}, GetX {2}, boxWidth {3}", position.x, width, theHitBox.GetTrueX(), theHitBox.trueWidth);
                Console.WriteLine("pos y {0}, height {1}, GetY {2}, boxHeight {3}", position.y, height, theHitBox.GetTrueY(), theHitBox.trueHeight);
                Console.WriteLine();
            }

            // Check if there is an overlap
            if (xOverlap > 0 && yOverlap > 0)
            {
                float timeOfImpact = -1;
                bool isHorizontalCollision = xOverlap < yOverlap;

                if (isHorizontalCollision)
                {
                    // Handle horizontal collisions
                    if (position.x < theHitBox.GetTrueX())
                    {
                        timeOfImpact = Math.Abs((_oldPosition.x + width) - theHitBox.GetTrueX()) / Math.Abs(position.x - _oldPosition.x);
                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theHitBox, timeOfImpact, 4)); // Right collision
                            if (wordy5) Console.WriteLine("right:" + timeOfImpact);
                        }
                    }
                    else
                    {
                        timeOfImpact = Math.Abs(_oldPosition.x - (theHitBox.GetTrueX() + theHitBox.trueWidth)) / Math.Abs(position.x - _oldPosition.x);
                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theHitBox, timeOfImpact, 3)); // Left collision
                            if (wordy5) Console.WriteLine("left: " + timeOfImpact);
                        }
                    }
                }
                else
                {
                    // Handle vertical collisions
                    if (position.y < theHitBox.GetTrueY())
                    {
                        timeOfImpact = Math.Abs((_oldPosition.y + height) - theHitBox.GetTrueY()) / Math.Abs(_oldPosition.y - position.y);
                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theHitBox, timeOfImpact, 2)); // Down collision
                            if (wordy5) Console.WriteLine("down: " + timeOfImpact);
                        }
                    }
                    else
                    {
                        timeOfImpact = Math.Abs(_oldPosition.y - (theHitBox.GetTrueY() + theHitBox.trueHeight)) / Math.Abs(_oldPosition.y - position.y);
                        if (timeOfImpact <= 1 && timeOfImpact >= 0)
                        {
                            _collisionList.Add(new CollisionInfo(new Vec2(0, 0), theHitBox, timeOfImpact, 1)); // Up collision
                            if (wordy5) Console.WriteLine("up: " + timeOfImpact);
                        }
                    }
                }
            }
        }
    }


    //AABB collision detection with THIS and tile
    protected void CheckCollisionDoor(MyGame myGame) //possible fourth step
    {
        //checking the bricks
        foreach (KeyValuePair<string, Door> theDoorEntry in GameData.doorList)
        {
            Door theDoor = theDoorEntry.Value;

            double dx = Math.Abs((position.x + Width / 2) - (theDoor.x));
            double dy = Math.Abs((position.y + Height / 2) - (theDoor.y));

            double overlapX = (Width + theDoor.width) / 2 - dx;
            double overlapY = (Height + theDoor.height) / 2 - dy;

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

    //AABB collision detection with THIS and tile
    protected void CheckCollisionBreakable(MyGame myGame) //possible fourth step
    {
        //checking the bricks
        for (int i = 0; i < GameData.breakableList.Count(); i++)
        {
            Breakable theBreakable = GameData.breakableList[i];

            double dx = Math.Abs((position.x + Width / 2) - (theBreakable.x));
            double dy = Math.Abs((position.y + Height / 2) - (theBreakable.y));

            double overlapX = (Width + theBreakable.width) / 2 - dx;
            double overlapY = (Height + theBreakable.height) / 2 - dy;

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

        //earilstCollision is not used. Maybe for now.

        //solving all collisions
        foreach (CollisionInfo col in _collisionList)
        {
            //collision with detection
            if (col.other is Hitbox)
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

            if (col.other is Tile)
            {
                Tile theTile = (Tile)col.other;
                Vec2 centerOfMass = (Mass * velocity + theTile.Mass * new Vec2(0, 0)) / (Mass + theTile.Mass);
                Vec2 momentum = -bounciness * velocity;
                Vec2 POI = _oldPosition + (col.timeOfImpact * velocity);

                position.y -= POI.y - position.y;
                velocity.y = momentum.y;



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

            if (col.other is Door)
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

            if (col.other is Breakable)
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
                    Player thePlayer = (Player)thisObject.parent;

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
}
