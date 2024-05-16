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

    private Vec2 playerVelocity;
    private Vec2 velocity;
    private Vec2 acceleration;

    public int GetPlyaerIndex() { return playerIndex; }
    
    public int playerIndex; //renamed from index to playerIndex for better naming. 0 = player1, 1 = player2

    public Boolean InShell {get { return inshell; }
                            set { inshell = value; } 
    }

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
    private Vec2 playerForce;


    private Vec2 kickForce;
    private Vec2 throwForce;
    float kickStrenghtX = -15;
    float kickStrengthY = 0;
    float throwStrenghtX = 15;
    float throwStrengthY = -15;

    private float friction;
    private float standUpFriction = 0.25f; //max speed is now determined by friction. can be overruled by external forces not from the player
    private float inShellFriction = 0.02f;


    private static Vec2 externalForces;
    private static Vec2 gravityForce;
    private static Vec2 antiGravity;
    private float gravity = 1f;
    private  Vec2 dragForce;
    private float drag = 0.01f;

    ColliderRect playerCollision; //handles the player's collision

    public float mass;

    private float standUpHitBoxCoordX = 0f;
    private float standUpHitBoxCoordY = 0f;
    private float inShellHitBoxCoordX = 0f;
    private float inShellHitBoxCoordY = 0f;

    private Boolean PassThrough = false;
    private Boolean movementLock = false;
    private Boolean inAction = false;


    public Vec2 spawnPoint; //the crood the player will move to if player dies

    int animationStartFrame = 0;
    int animationEndFrame = 1;
    int animtaionAmountFrame;


    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index");
        Console.WriteLine("int index {0}", obj.GetIntProperty("int_index"));
        Console.WriteLine("property Width {0}", obj.GetIntProperty("Width"));
        SetAnimationCycle(0, 1);
        mass = 4 * width * height;
        SetOrigin(width / 2, height / 2);
        
        width = obj.GetIntProperty("Width");
        height = obj.GetIntProperty("Height");


        detectionRange = new Detection(0, 0, mass);
        AddChild(detectionRange);
        detectionRange.scale = 7.5f;
        playerHitBox = new Hitbox(0, 0, mass); //the player's actual hit box.
        AddChild(playerHitBox);

        playerCollision = new ColliderRect(playerHitBox, new Vec2(0, 0), new Vec2(0, 0), playerHitBox.width, playerHitBox.height, true);
        Console.WriteLine("player x {0}, y {1}, width {2}, height {3}", x, y, playerHitBox.width, playerHitBox.height);
        Console.WriteLine("PlayerIndex {0}, width {1}, height {2}, scaleX {3}, scaleY {4}", playerIndex, width, height, scaleX, scaleY);

        Animate(0.085f);

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

    
    private Boolean ActionPossible() 
    {
        if (detectionRange.PlayerInteractCheck())
        {
            return true;
        }
                       
        return false;
    }

    
    private void Action()
    {
        if (playerIndex == 0)
        {
            if (Input.GetKeyDown(Key.Q) && ActionPossible())
            {
                Kick();
            }
        }

        if (playerIndex == 1)
        {
            if (ActionPossible())
            {
                if (Input.GetKey(Key.O)) { Pickup(true); }
                else { Pickup(false); }
            }
        }
    }






    private void Pickup(Boolean pickedUp) 
    {
        Player interactPlayer = detectionRange.GetDete().GetPlayer();

        if (pickedUp)
        {
            interactPlayer.movementLock = true;
            interactPlayer.position.x = x;
            interactPlayer.position.y = y + -height;
        } else { interactPlayer.movementLock = false; }


        if (Input.GetKeyDown(Key.U)) 
        {
            interactPlayer.movementLock = false;
            Throw();
            
            
        }

    }


    private void Kick() 
    {
        kickForce = new Vec2(kickStrenghtX, kickStrengthY);
        Console.WriteLine("kick");
        detectionRange.GetDete().GetPlayer().playerForce += kickForce;

    }
    private void Throw() 
    {
        throwForce = new Vec2(throwStrenghtX, throwStrengthY);
        Console.WriteLine("Throw");
        detectionRange.GetDete().GetPlayer().playerForce += throwForce;

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
        
        
    }

    private void Moving(bool[] moveDir)
    {
        
        float accelerationX = 0;
        float accelerationY = 0;
        if (!inshell)
        {

            if (moveDir[0])
            {
                accelerationX += -1;
            }
            if (moveDir[1])
            {
                accelerationX += 1;
            }


            if (moveDir[2])
            {
                accelerationY += -35;
            }

        }
        

        acceleration = new Vec2(accelerationX, accelerationY);
        animtaionAmountFrame = animationEndFrame - animationStartFrame +1;
        
        

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
        if (onGround || movementLock) { gravity = 0f; }

        gravityForce = new Vec2(0, gravity);

        acceleration += frictionForce + gravityForce;

        playerVelocity += acceleration + playerForce;

        playerForce = new Vec2(0f, 0f);

        velocity = playerVelocity + fanVelocity;
    }

    private void velocityFix()
    {
        if (playerVelocity.y >= -0.01f && playerVelocity.y <= 0.01f)
        { 
            playerVelocity.y = 0f;
        }

        if (playerVelocity.x >= -0.01f && playerVelocity.x <0.01f)
        {
            playerVelocity.x = 0f;
        }

    }


    private void shellState()
    {
        if (playerIndex == 0)
        {
            if (inshell)
            {
                playerHitBox.x = inShellHitBoxCoordX;
                playerHitBox.y = inShellHitBoxCoordY;
                if (Input.GetKey(Key.W)) { SetAnimationCycle(0, 1);  inshell = false; }

            }

            if (!inshell)
            {
                playerHitBox.x = standUpHitBoxCoordX;
                playerHitBox.y = standUpHitBoxCoordY;
                if (Input.GetKey(Key.S)) { SetAnimationCycle(25, 1); inshell = true; }

            }

        }



        if (playerIndex == 1)
        {

            if (inshell)
            {                
                playerHitBox.x = inShellHitBoxCoordX;
                playerHitBox.y = inShellHitBoxCoordY;
                if (Input.GetKey(Key.I)) { SetAnimationCycle(0, 1); inshell = false; }

            }
            if (!inshell)
            {
                
                playerHitBox.x = standUpHitBoxCoordX;
                playerHitBox.y = standUpHitBoxCoordY;
                if (Input.GetKey(Key.K)) { SetAnimationCycle(25, 1); inshell = true; }

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

        if (!movementLock) 
        {
            shellState();
            PlayerInput();
        }
        Moving(movementDirection);
        ActionPossible();
        Action();
        UpdateCollision();
        playerCollision.Step();

        velocity = playerCollision.Velocity;
        position += velocity;
        x = position.x;
        y = position.y;

        velocityFix();
        
        //movement information
        if (Input.GetKeyDown(Key.G))
        {
            Console.WriteLine("velocity  {0}, playerVelocity {1},  fanVelocty {2}, gravityForce {3}, frictionForce {4}", velocity, playerVelocity, fanVelocity, gravityForce, frictionForce);
            Console.WriteLine("onCeiling {0}, onGround {1}", onCeiling, onGround);
        }


    }
}