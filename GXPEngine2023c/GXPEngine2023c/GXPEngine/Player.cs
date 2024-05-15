using GXPEngine;
using System;
using System.Collections.Generic;
using TiledMapParser;

public class Player : AnimationSpriteCustom
{

    public Vec2 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }

    public Vec2 Position
    {
        get { return position; }
        set { position = value; }
    }

    private Vec2 playerVelocity = new Vec2();
    private Vec2 velocity;
    private Vec2 acceleration;

    public int GetPlyaerIndex() { return playerIndex; }
    private int playerIndex; //renamed from index to playerIndex for better naming. 1 = player1, 2 = player2


    private Boolean inshell;

    public Boolean OnGround
    {
        get { return onGround; }
        set { onGround = value; }
    }

    public Boolean OnCeiling
    {
        get { return onCeiling; }
        set { onCeiling = value; }

    }


    private Boolean onGround;
    private Boolean onCeiling;


    private string collisionDirection;
    List<Vec2> fanVelocityList = new List<Vec2>();
    private Vec2 fanVelocity;
    private Vec2 position;
    private Boolean[] movementDirection = new Boolean[3];
    public Detection detectionRange;
    public Hitbox playerHitBox;

    private Vec2 frictionForce;
    private float friction;
    private float standUpFriction = 0.25f; //max speed is now determined by friction. can be overruled by external forces not from the player
    private float inShellFriction = 0.02f;


    private static Vec2 externalForces;
    private static Vec2 gravityForce;
    private static Vec2 antiGravity;
    private float gravity = 1f;
    private Vec2 dragForce;
    private float drag = 0.01f;

    ColliderRect playerCollision; //handles the player's collision

    public float mass;

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index");
        SetAnimationCycle(0, 1);
        mass = 4 * width * height;
        SetOrigin(width / 2, height / 2);

        detectionRange = new Detection(0, 0, mass); 
        detectionRange.scaleX = 1.75f;
        detectionRange.scaleY = 2.5f;
        AddChild(detectionRange);

        playerHitBox = new Hitbox(-32, -32, mass); //the player's actual hit box.
        playerHitBox.scaleX = 2f;
        playerHitBox.scaleY = 2f;
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




    private void PlayerInput()
    {
        movementDirection[0] = false;
        movementDirection[1] = false;
        movementDirection[2] = false;

        if (!inshell)
        {

            switch (playerIndex)
            {
                case 0:

                    //ARRAY for movement directions
                    if (Input.GetKey(Key.A))
                    {
                        movementDirection[0] = true;

                    }
                    else { movementDirection[0] = false; }
                    if (Input.GetKey(Key.D))
                    {
                        movementDirection[1] = true;

                    }
                    else { movementDirection[1] = false; }
                    if (Input.GetKeyDown(Key.W) && onGround && !onCeiling)
                    {
                        movementDirection[2] = true;

                    }
                    else { movementDirection[2] = false; }

                    break;
                case 1:

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
                    if (Input.GetKeyDown(Key.I) && onGround && !onCeiling)
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
        if (!inshell)
        {

            if (moveDir[0])
            {

                acceleration = new Vec2(-1, 0);

            }
            if (moveDir[1])
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
        else { acceleration = new Vec2(0, 0); }




        if (inshell)
        {
            friction = inShellFriction;
        }

        if (!inshell)
        {
            friction = standUpFriction;
        }

        CalcFanVeclotiy();

        if (inshell) { friction = inShellFriction; }
        if (!inshell) { friction = standUpFriction; }

        frictionForce = -friction * playerVelocity;

        if (!onGround) { gravity = 1f; }
        if (onGround) { gravity = 0f; }

        gravityForce = new Vec2(0, gravity);

        acceleration += frictionForce + gravityForce;

        playerVelocity += acceleration;

        velocity = playerVelocity + fanVelocity;
    }



    private void shellState()
    {
        if (playerIndex == 0)
        {
            if (inshell)
            {
                SetAnimationCycle(1, 1);
                detectionRange.scaleX = 2.5f;
                detectionRange.scaleY = 1.75f;
                
                if (Input.GetKey(Key.W)) { inshell = false; }

            }

            if (!inshell)
            {
                SetAnimationCycle(0, 1);
                detectionRange.scaleX = 1.75f;
                detectionRange.scaleY = 2.5f;
                
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
        playerCollision.Width = playerHitBox.width;
        playerCollision.Height = playerHitBox.height;
        playerCollision.Position = position + new Vec2(playerHitBox.x, playerHitBox.y);

        playerCollision.Velocity = velocity;
    }


    void Update()
    {
        PlayerInput();
        shellState();


        UpdateCollision();
        playerCollision.Step();

        velocity = playerCollision.Velocity;
        position += velocity;
        x = position.x;
        y = position.y;


        //movement information
        if (Input.GetKeyDown(Key.G))
        {
            Console.WriteLine("velocity  {0}, playerVelocity {1},  fanVelocty {2}, gravityForce {3}, frictionForce {4}", velocity, playerVelocity, fanVelocity, gravityForce, frictionForce);
            Console.WriteLine("onCeiling {0}, onGround {1}", onCeiling, onGround);
        }


    }
}