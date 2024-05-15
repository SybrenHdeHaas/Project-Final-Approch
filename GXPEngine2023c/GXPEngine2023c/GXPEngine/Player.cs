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
    public int playerIndex; //renamed from index to playerIndex for better naming. 0 = player1, 1 = player2
    Detection detectionRange;
    public Hitbox playerHitBox;
    List<Vec2> fanVelocityList = new List<Vec2>();
    Vec2 fanVelocity;
    Vec2 position;
    float friction = 0.5f;
    ColliderRect playerCollision; //handles the player's collision

    public float mass;

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index");
        SetAnimationCycle(0, 1);
        mass = 4 * width * height;

        detectionRange = new Detection(-40, -30, mass); //the player's actual hit box.
        detectionRange.scale = 2.5f;
        AddChild(detectionRange);
        

        playerHitBox = new Hitbox(-60, -60, mass); //the player's actual hit box.
        playerHitBox.scale = 2.5f;
        AddChild(playerHitBox);

        playerCollision = new ColliderRect(playerHitBox, new Vec2(0, 0), new Vec2(0, 0), playerHitBox.width, playerHitBox.height, true);
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
            if (Input.GetKey(Key.A) == true && playerIndex == 0)
            {
                acceleration = new Vec2(-1, 0);
                acceleration += velocity * -friction;
            }

            if (Input.GetKey(Key.J) == true && playerIndex == 1)
            {
                acceleration = new Vec2(-1, 0);
                acceleration += velocity * -friction;
            }

            if (Input.GetKey(Key.D) == true && playerIndex == 0)
            {
                acceleration = new Vec2(1, 0);
                acceleration += velocity * -friction;
            }

            if (Input.GetKey(Key.L) == true && playerIndex == 1)
            {
                acceleration = new Vec2(1, 0);
                acceleration += velocity * -friction;
            }

            if (Input.GetKey(Key.W) == true && playerIndex == 0)
            {
                acceleration += new Vec2(0, -5);
                acceleration += velocity * -friction;
            }

            if (Input.GetKey(Key.I) == true && playerIndex == 1)
            {
                acceleration += new Vec2(0, -5);
                acceleration += velocity * -friction;
            }

            //no player move control so no acceleration
            if (Input.GetKey(Key.A) == false && Input.GetKey(Key.D) == false && Input.GetKey(Key.W) == false 
                && playerIndex == 0)
            {
                acceleration = new Vec2(0, 0);
                acceleration += velocity * -friction;
            }


            if (Input.GetKey(Key.J) == false && Input.GetKey(Key.L) == false && Input.GetKey(Key.I) == false
    && playerIndex == 1)
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
        if (inshell)
        {
            SetAnimationCycle(1, 1);

            if (Input.GetKey(Key.W) && playerIndex == 0) { inshell = false; }

            if (Input.GetKey(Key.I) && playerIndex == 1) { inshell = false; }
        }

        if (!inshell)
        {
            SetAnimationCycle(0, 1);

            if (Input.GetKey(Key.S) && playerIndex == 0) { inshell = true; }

            if (Input.GetKey(Key.K) && playerIndex == 1) { inshell = true; }
        }
    }

  
    void UpdateCollision()
    {
        playerCollision.width = playerHitBox.width;
        playerCollision.height = playerHitBox.height;
        playerCollision.Position = position + new Vec2(playerHitBox.x, playerHitBox.y);

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