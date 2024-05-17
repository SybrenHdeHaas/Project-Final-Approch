using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//physcis for possibly all gameobjects
public class ColliderObject : GameObject
{
    public static float bounciness = 0.0f;
    public static bool wordy = false; //if true, enable debug messages 
    public static bool wordy1 = false; //if true, enable debug messages 1
    public static bool wordy2 = false; //if true, enable debug messages 2 CheckCollisionTiles
    public static bool wordy4 = false; //if true, enable debug messages 4
    public static bool wordy5 = false; //if true, enable debug message 5 CheckCollisionHitbox
    public static bool wordy6 = false;
    public static bool wordy7 = false; //if true, enable debug message 7 CheckCollisionTiles extra once
    public static bool wordy8 = false; //if true, enable debug messages 8 CheckCollisionHitbox 
    public List<CollisionInfo> GetCollisionList() { return _collisionList; }
    protected Vec2 position;
    protected Vec2 _oldPosition;
    protected Vec2 velocity;

    protected bool moving; //if the ball is moving or not (only collision detect and resolve should be applied)
    
    protected bool firstTime;
    protected List<CollisionInfo> _collisionList = new List<CollisionInfo>();
    protected float _density = 1;
    protected float mass;

    
    public Vec2 Position
    {
        get { return position;}
        set { position = value;}
    }

    public Vec2 OldPosition
    {
        get { return _oldPosition; }
    }

    public float Mass
    { 
        get { return mass; }
    }

    public Vec2 Velocity
    {
        get {  return velocity; }

        set { velocity = value; }
    }

    public ColliderObject(Vec2 pPosition, Vec2 pVelocity, bool pMoving, float pDensity = 1)
    {
        position = pPosition;
        velocity = pVelocity;
        moving = pMoving;
        _density = pDensity;
    }


    public void Step() //first step in collison calculation
    {
        if (moving == false)
        {
            return;
        }

        firstTime = true;
        _collisionList.Clear();
        _oldPosition = position;

        MoveAndDetectAndResolveCollision();
    }

    //Move the object 
    protected void MoveAndDetectAndResolveCollision() //second step in collison calculation
    {
        position += velocity;

        CollisionInfo earilestCollision = FindEarliestCollision();

        if (earilestCollision != null)
        {
            ResolveCollision(earilestCollision);

            if (wordy)
            {
                Console.WriteLine("time: " + earilestCollision.timeOfImpact);
            }

            //for sliding behavior for gravity + multiple objects
            if (Math.Round(earilestCollision.timeOfImpact, 1) == 0 && firstTime)
            {
                firstTime = false;
                _collisionList.Clear();

                if (wordy)
                {
                    Console.WriteLine("doing calculation again");
                }

                MoveAndDetectAndResolveCollision(); //this would only be called once every frame as firstTime got set to false before this method call.
            }
        }
    }
    protected virtual CollisionInfo FindEarliestCollision() //third step in collison calculation
    {
        MyGame myGame = (MyGame)game;

        return FindLowestTOICollision();
    }



    //return the collision with smallest value of TOI in collision list
    protected CollisionInfo FindLowestTOICollision() //fourth step in collison calculation
    {
        float TOI = 0;
        CollisionInfo theCollsion = null;

        for (int i = 0; i < _collisionList.Count; i++)
        {
            if (_collisionList[i].other is null)
            {
                continue;
            }

            if (wordy)
            {
                //debug messages
                Console.WriteLine("TOI: " + _collisionList[i].timeOfImpact);
            }

            if (i == 0)
            {
                theCollsion = _collisionList[i];
                TOI = _collisionList[i].timeOfImpact;
                continue;
            }

            if (_collisionList[i].timeOfImpact < TOI)
            {
                theCollsion = _collisionList[i];
                TOI = _collisionList[i].timeOfImpact;
            }
        }
        return theCollsion; //return null if collision not found
    }

    protected virtual void ResolveCollision(CollisionInfo col)//fifth step in collison calculation
    {
    }

}