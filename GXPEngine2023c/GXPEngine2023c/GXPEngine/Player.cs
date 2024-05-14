using GXPEngine;
using System;
using System.Collections.Generic;
using TiledMapParser;

public class Player : AnimationSpriteCustom
{
    public static Vec2 gravity;
    private float gravForce = 0.5f;

    private float maxspeed = 2f;
    private Vec2 playerVelocity = new Vec2();
    private Vec2 velocity;
    private Vec2 acceleration;
    private int playerIndex; //renamed from index to playerIndex for better naming. 1 = player1, 2 = player2
    private Detection detectionRange = new Detection();
    private Boolean inshell;
    private Boolean onGround;
    private string collisionDirection;
    List<Vec2> fanVelocityList = new List<Vec2>();
    private Vec2 fanVelocity;
    private Vec2 position;
    private Boolean[] movementDirection = new Boolean[3];


    private float playerMoveVeloctity;
    private float standUpFriction = 0.5f;
    private float inShellFriction = 0.1f;
    private float friction;


    private float drag = 0.25f; //friction for up and down movement

    ColliderRect playerCollision; //handles the player's collision

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index", 1);
        SetAnimationCycle(0, 1);
        playerCollision = new ColliderRect(this, new Vec2(0, 0), new Vec2(0, 0), width, height, true);
        gravity = new Vec2(0, gravForce);


        AddChild(detectionRange);
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


    private void CollisionDirection()
    {
        List<CollisionInfo> coll = playerCollision.GetCollisionList();
        collisionDirection = "none";
        foreach (CollisionInfo theCollision in coll)
        {
            if (theCollision.AABBDirection == 1) { collisionDirection = "up"; }
            else if (theCollision.AABBDirection == 2) { collisionDirection = "down"; }
            else if (theCollision.AABBDirection == 3) { collisionDirection = "left"; }
            else if (theCollision.AABBDirection == 4) { collisionDirection = "right"; }
        }

    }

    private void groundCheck()
    {
        if (collisionDirection == "down")
        {
            onGround = true;
        }
        else onGround = false;

    }


    private void PlayerInput()
    {

        if (!inshell)
        {

            switch (playerIndex)
            {
                case 0:

                    //ARRAY for movement directions
                    movementDirection[0] = false;
                    movementDirection[1] = false;
                    movementDirection[2] = false;

                    if (Input.GetKey(Key.A))
                    {
                        movementDirection[0] = true;
                        
                    } else { movementDirection[0] = false; }
                    if (Input.GetKey(Key.D))
                    {
                        movementDirection[1] = true;
                    } else { movementDirection[1] = false; }
                    if (Input.GetKeyDown(Key.W) && onGround)
                    {
                        movementDirection[2] = true;
                    } else { movementDirection[2] = false; }

                    break;
                case 1:

                    movementDirection[0] = false;
                    movementDirection[1] = false;
                    movementDirection[2] = false;

                    if (Input.GetKey(Key.J))
                    {
                        movementDirection[0] = true;
                    }
                    else { movementDirection[0] = false; }
                    if (Input.GetKey(Key.L))
                    {
                        movementDirection[1] = true;
                    }
                    else { movementDirection[1] = false; }
                    if (Input.GetKeyDown(Key.I) && onGround)
                    {
                        movementDirection[2] = true;
                    }
                    else { movementDirection[2] = false; }

                    break;
            }   
        }

        Moving(movementDirection);

    }


    private void Moving(bool[] moveDir)
    {
        Boolean BlockMovementRight = false;
        Boolean BlockMovementLeft = false;
        

        if (!inshell)
        {

            if (moveDir[0] && !BlockMovementLeft) 
            {
                
                acceleration = new Vec2(-1, 0);
                
            }
            if (moveDir[1] && !BlockMovementRight)
            {
                acceleration = new Vec2(1, 0);
            }
            if (moveDir[2])
            {
                acceleration = new Vec2(0, -25);
                
            }

            if (!moveDir[0] && !moveDir[1] && !moveDir[2])
            {
                acceleration = new Vec2(0, 0);
            }
        }

        if (inshell)
        {
            
            friction = inShellFriction;
            
        }

        if (!inshell)
        {
            friction = standUpFriction;
        }

        CalcFanVeclotiy();


        playerVelocity += acceleration;

        if (playerVelocity.x <= -maxspeed)
        {
            playerVelocity.x = -maxspeed;
            BlockMovementLeft = true;

        }
        else { BlockMovementLeft = false; }

        if (playerVelocity.x >= maxspeed)
        {
            playerVelocity.x = maxspeed;
            BlockMovementRight = true;

        }
        else { BlockMovementRight = false; }

        velocity = playerVelocity;
        
        velocity += fanVelocity;
        velocity += gravity;

        if (Input.GetKeyDown(Key.G)) 
        { 
            Console.WriteLine("velocity  {0}, playerVelocity {1},  fanVelocty {2 }, gravity {3}", velocity, playerVelocity,  fanVelocity, gravity);
            Console.WriteLine("blockLeft {0}, blockRight {1}", BlockMovementLeft, BlockMovementRight);
        }
        
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
            if (inshell)
            {
                SetAnimationCycle(1, 1);
                if (Input.GetKey(Key.I)) { inshell = false; }

            }
            if (!inshell)
            {
                SetAnimationCycle(0, 1);
                if (Input.GetKey(Key.K)) { inshell = true; }

            }
        }

    }


    void UpdateCollision()
    {
        playerCollision.Velocity = velocity;
        playerCollision.Position = position;
    }


    void Update()
    {

        
        PlayerInput();
        shellState();
        CollisionDirection();
        groundCheck();

        UpdateCollision();
        playerCollision.Step();


        velocity = playerCollision.Velocity;
        position += velocity;

        x = position.x;
        y = position.y;


    }
}