using GXPEngine;
using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

public class Player : AnimationSpriteCustom
{
    private float maxSpeed = 2;
    private Boolean onGround;

    private Boolean inshell;
    public static Vec2 gravity = new Vec2(0, 1);
    private Vec2 velocity;
    private Vec2 acceleration;
    private int playerIndex; //renamed from index to playerIndex for better naming. 1 = player1, 2 = player2
    public Detection detectionRange = new Detection(); //the player's actual hit box
    List<Vec2> fanVelocityList = new List<Vec2>();
    Vec2 fanVelocity;
    Vec2 position;
    float friction = 0.5f;
    ColliderRect playerCollision; //handles the player's collision

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index", 1);
        SetAnimationCycle(0, 1);
        AddChild(detectionRange);
        playerCollision = new ColliderRect(detectionRange, new Vec2(0, 0), new Vec2(0, 0), detectionRange.width, detectionRange.height, true);
        detectionRange.scale = 2.5f;
    }

    public void UpdatePos()
    {
        position.x = x;
        position.y = y;
    }

    public void ResetFanVelocityList()
    {
        fanVelocityList.Clear();
    }

    public void AddFanVelocity(Vec2 theFanVelocity)
    {
        fanVelocityList.Add(theFanVelocity);
    }

    private void CalcFanVeclotiy()
    {
        fanVelocity.x = 0;
        fanVelocity.y = 0;
        foreach (Vec2 theFanVelocity in fanVelocityList)
        {
            fanVelocity += theFanVelocity;
        }
    }

    private void Actions()
    {
    }

    private void Pickup()
    {
    }

    private void Kick()
    {
    }

    private void Moving()
    {
        if (!inshell) 
        {
            if (Input.GetKey(Key.A) == true)
            {
                acceleration = new Vec2(-1, 0);
                acceleration += velocity * -friction;
            }

            if (Input.GetKey(Key.D) == true)
            {
                acceleration = new Vec2(1, 0);
                acceleration += velocity * -friction;
            }

            if (Input.GetKey(Key.W) == true)
            {
                acceleration += new Vec2(0, -5);
                acceleration += velocity * -friction;
            }

            //no player move control so no acceleration
            if (Input.GetKey(Key.A) == false && Input.GetKey(Key.D) == false && Input.GetKey(Key.W) == false)
            {
                acceleration = new Vec2(0, 0);
                acceleration += velocity * -friction;
            }
        }

        if (inshell) 
        {
            acceleration = new Vec2(0, 0);
            velocity += velocity * -friction;
        }

        velocity += acceleration;
        CalcFanVeclotiy();
        velocity += fanVelocity;
        velocity += gravity;
    }

    private void shellState()
    {
        if (playerIndex == 0)
        {
            if (inshell)
            {
                SetAnimationCycle(1, 1);

                if (Input.GetKey(Key.W)) { inshell = false; }
            }

            if (!inshell)
            {
                SetAnimationCycle(0, 1);

                if (Input.GetKey(Key.S)) { inshell = true; }
            }
        }

        if (playerIndex == 1)
        {
            if (inshell && Input.GetKey(Key.I)) { inshell = false; }
            if (!inshell && Input.GetKey(Key.K)) { inshell = true; }
        }
    }

  
    void UpdateCollision()
    {
        playerCollision.width = detectionRange.width;
        playerCollision.height = detectionRange.height;
        playerCollision.Position = position;
        playerCollision.Velocity = velocity;
    }
  

    void Update()
    {
        shellState();
        Moving();
        UpdateCollision();
        playerCollision.Step();
        velocity = playerCollision.Velocity;
        position += velocity;

        x = position.x;
        y = position.y;
    }
}