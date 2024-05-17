using GXPEngine;
using GXPEngine.Core;
using System;
using System.Runtime.InteropServices;
using TiledMapParser;

public class Breakable : AnimationSpriteCustom
{
    float thresholdVelocityLength; //the threshold length
    int theHealth;


    public static Vec2 gravity = new Vec2(0, 1);
    
    public Vec2 velocity;
    public Vec2 position;
    ColliderBreakable playerCollision; //handles the player's collision


    public float Mass
    {
        get
        {
            return 4 * width / 2 * width / 2;
        }
    }

    public Breakable(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        thresholdVelocityLength = obj.GetFloatProperty("float_thresholdVelocityLength");
        theHealth = obj.GetIntProperty("int_theHealth");


        playerCollision = new ColliderBreakable(this, new Vec2(0, 0), new Vec2(0, 0), width, height, true);
    }

    public void UpdatePos()
    {
        position.x = x;
        position.y = y;
    }

    public Boolean TryDamage(Vec2 otherVelocity)
    {
        //comparing player velocity length and the threshold length;
        if (otherVelocity.Length() > thresholdVelocityLength)
        {
            theHealth--;

            if (theHealth < 0)
            {
                return true;
            }
        }

        return false;
    }


    void UpdateCollision()
    {
        playerCollision.Velocity = velocity;
        playerCollision.Position = position;
       
    }

    void Update()
    {
        velocity += gravity * 0.2f; 
        UpdateCollision();
        playerCollision.Step();


        velocity = playerCollision.Velocity;
        position += velocity;

        x = position.x;
        y = position.y;
    }
}
