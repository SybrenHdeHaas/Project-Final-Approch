using GXPEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    private Vec2 playerVelocity; //velocity of player movement
    private Vec2 velocity; //overall velocity (playerVelocity + other impacts of velocity)
    private Vec2 acceleration;
    public int GetPlayerIndex() { return playerIndex; }

    public int playerIndex; //0 = player1, 1 = player2

    public Boolean InShell
    {
        get { return inshell; }
        set { inshell = value; }
    }

    private Boolean inshell; //determines if player is inside shell or not

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

    private Boolean onGround = true;
    private Boolean onCeiling = false;

    private string collisionDirection;
    List<Vec2> fanVelocityList = new List<Vec2>();
    private Vec2 fanVelocity;
    private Vec2 position;
    private Boolean[] movementDirection = new Boolean[3]; //stores player input (left, right, up). false means no input in that direction
    public Detection detectionRange;
    public Hitbox playerHitBox;

    private Vec2 frictionForce;
    private Vec2 playerForce;

    private Vec2 kickForce;
    private Vec2 throwForce;
    float kickStrenghtX = 15;
    float kickStrengthY = 0;
    float throwStrenghtX = 15;
    float throwStrengthY = -15;

    private float friction; //speed loss X
    private float drag = 0.2f; //speed loss Y
    private float standUpFriction = 0.25f; //max speed is now determined by friction. can be overruled by external forces not from the player
    private float inShellFriction = 0.1f;

    private static Vec2 externalForces;
    private static Vec2 gravityForce;
    private float gravity = 1f;

    public float mass;

    private Boolean PassThrough = false;
    private Boolean movementLock = false; //if true, ignore inputs
    private Boolean inAction = false;

    public Vec2 spawnPoint; //the crood the player will move to if player dies

    int baseFrame;

    public float hitboxWorkingWidth;
    public float hitboxWorkingHeight;

    private Boolean animationTypeActive = false;

    private float[] inShellStats; //posx, posy, width, height of player in shell
    private float[] outShellStats; //posx, posy, width, height of player not in shell

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index");
        hitboxWorkingWidth = obj.GetIntProperty("Width");
        hitboxWorkingHeight = obj.GetIntProperty("Height");

        mass = 4 * width * height;

        //   playerHitBox = new Hitbox((int)(width / 3), (int)(height / 3), (int)hitboxWorkingWidth, (int)hitboxWorkingHeight, mass);
        playerHitBox = new Hitbox(mass); //the player's actual hit box.
        AddChild(playerHitBox);

        detectionRange = new Detection(width, height, mass);
        AddChild(detectionRange);

        float[] inShellStatsArray = {-64 / 2, (192 / 2) - 64, 64, 64};
        inShellStats = inShellStatsArray;
        float[] outShellStatsArray = {-192 / 2, -192 / 2, 192, 192};
        outShellStats = outShellStatsArray;

        playerHitBox.ChangeOffSetAndSize(outShellStats);

    }

    //performing player animation
    void AnimationController()
    {
        if (playerIndex == 0)
        {
            if (playerVelocity.x > 0)
            {
                
                if (!animationTypeActive)
                {
                    Console.WriteLine("activate animation left");
                    SetAnimationCycle(60, 8);
                    animationTypeActive = true;
                    _mirrorX = false;
                }
            }

            else if (playerVelocity.x < 0)
            {
                if (!animationTypeActive)
                {
                    Console.WriteLine("activate animation right");
                    SetAnimationCycle(60, 8);
                    animationTypeActive = true;
                    _mirrorX = true;
                }
            }

            if ((base.currentFrame - base._startFrame) == base.frameCount-1) 
            {
                if (!inshell) { baseFrame = 40; }
                if (inshell) { baseFrame = 59; }
                SetAnimationCycle(baseFrame, 1);
                animationTypeActive = false;
            }
        }

        if (playerIndex == 1)
        {
            if (playerVelocity.x > 0)
            {
                if (!animationTypeActive)
                {
                    Console.WriteLine("activate animation left");
                    SetAnimationCycle(30, 10);
                    animationTypeActive = true;
                    _mirrorX = false;
                }
            }

            else if (playerVelocity.x < 0)
            {
                if (!animationTypeActive)
                {
                    Console.WriteLine("activate animation right");
                    SetAnimationCycle(30, 10);
                    animationTypeActive = true;
                    _mirrorX = true;
                }
            }

            if ((base.currentFrame - base._startFrame) == base.frameCount - 1)
            {
                if (!inshell) { baseFrame = 0; }
                if (inshell) { baseFrame = 25; }
                SetAnimationCycle(baseFrame, 1);
                animationTypeActive = false;
            }
        }
        base.Update();
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

    //return true if player's detection intersects with other player's detection
    private Boolean ActionPossible()
    {
        if (detectionRange.PlayerInteractCheck())
        {
            return true;
        }
        return false;
    }
    private void TryAction()
    {
        bool actionPossble = ActionPossible(); //player can't perform kick and pickup if detection not intersecting

        if (playerIndex == 0)
        {
            if (Input.GetKeyDown(Key.Q) && actionPossble)
            {
                Kick();
            }
        }

        if (playerIndex == 1)
        {
            if (actionPossble)
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
            SoundChannel sound = new Sound("pick sound.mp3", false).Play();
            interactPlayer.movementLock = true;

            interactPlayer.position.x = x;
            interactPlayer.position.y = y + -height;

        }
        else { interactPlayer.movementLock = false; interactPlayer.velocity += velocity; }


        if (Input.GetKeyDown(Key.U))
        {
            interactPlayer.movementLock = false;
            Throw();
        }
    }
    private void Kick()
    {
        SoundChannel sound = new Sound("kick.wav", false).Play();
        sound.Volume = 1.3f;
        kickForce = new Vec2(kickStrenghtX, kickStrengthY);
        Console.WriteLine("kick");
        detectionRange.GetDete().GetPlayer().playerForce += kickForce;
    }
    private void Throw()
    {
        SoundChannel sound = new Sound("throw.wav", false).Play();
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
                        SoundChannel sound = new Sound("jump.wav", false).Play();
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
                        SoundChannel sound = new Sound("jump.wav", false).Play();
                        movementDirection[2] = true;
                    }
                    else { movementDirection[2] = false; }

                    break;
            }
        }
    }

    //updates movement acceleration based on the inputs
    private void Move(bool[] playerInput)
    {
        float accelerationX = 0;
        float accelerationY = 0;

        if (!inshell)
        {
            if (playerInput[0]) //left
            {
                _mirrorX = true;
                accelerationX += -1;
            }
            if (playerInput[1])//right
            {
                _mirrorX = false;
                accelerationX += 1;
            }
            if (playerInput[2])//up
            {
                accelerationY += -25;
            }
        }

        acceleration = new Vec2(accelerationX, accelerationY);
    }

    //applying gravity, friction, and fan velocity
    private void ApplyForces()
    {
        CalcFanVeclotiy();

        //different friction depending on shell state
        if (inshell) { friction = inShellFriction; }
        if (!inshell) { friction = standUpFriction; }

        frictionForce.x = -friction * playerVelocity.x;
        frictionForce.y = -drag * playerVelocity.y;

        //don't apply gravity if player is on ground
        if (!onGround) { gravity = 1f; }
        if (onGround) { gravity = 0f; }

        gravityForce.y = gravity;
        acceleration += frictionForce + gravityForce;

        playerVelocity += acceleration + playerForce;
        playerForce = new Vec2(0f, 0f);

        velocity = playerVelocity + fanVelocity;
    }

    private void VelocityFix()
    {
        if (playerVelocity.y >= -0.01f && playerVelocity.y <= 0.01f)
        {
            playerVelocity.y = 0f;
        }

        if (playerVelocity.x >= -0.01f && playerVelocity.x < 0.01f)
        {
            playerVelocity.x = 0f;
        }
    }

    //player shell control
    private void ShellState()
    {
        if (playerIndex == 0)
        {
            if (inshell)
            {
                if (Input.GetKey(Key.W)) //player 1 inputs to go out of inShell state
                { 
                    SetAnimationCycle(40, 1); 
                    inshell = false;
                    playerHitBox.ChangeOffSetAndSize(outShellStats);
             //       playerHitBox.playerCollision.outShellChanges();
                }
            }

            if (!inshell)
            {

                if (Input.GetKey(Key.S))  //player 1 inputs to go into inShell state
                { 
                    SetAnimationCycle(50, 8); 
                    inshell = true;
                    playerHitBox.ChangeOffSetAndSize(inShellStats);
              //      playerHitBox.playerCollision.inShellChanges();
                }
            }
        }

        if (playerIndex == 1)
        {
            if (inshell)
            {
                if (Input.GetKey(Key.I)) //player 2 inputs to go out of inShell state
                {
                    SetAnimationCycle(0, 1);
                    inshell = false;
                    playerHitBox.ChangeOffSetAndSize(outShellStats);
              //      playerHitBox.playerCollision.outShellChanges();
                }
            }

            if (!inshell)
            {

                if (Input.GetKey(Key.K)) //player 2 inputs to go into inShell state
                {
                    SetAnimationCycle(15, 11);
                    inshell = true;
                    playerHitBox.ChangeOffSetAndSize(inShellStats);
                //    playerHitBox.playerCollision.inShellChanges();
                }
            }
        }
    }

    void UpdateCollision()
    {
        playerHitBox.playerCollision.Velocity = velocity;
        playerHitBox.UpdateCollision(position);
    }

    void Update()
    {
        Move(movementDirection); //player movement accerlation
        ApplyForces(); //applying gravity, friction, and fan velocity
        TryAction(); //try perform kick and pickup if detection intersects
        AnimationController(); //performs animation

        //if no movement lock, checks player input and shell state
        if (!movementLock)
        {
            PlayerInput(); //updates player input
            ShellState(); //player shell control
        }

        UpdateCollision(); //updates collision hitbox info
        playerHitBox.playerCollision.Step(); //performs collision

        //update velocity and position from what the collision calculated
        velocity = playerHitBox.playerCollision.Velocity;
        position += velocity;
        x = position.x;
        y = position.y;

        VelocityFix(); //makes player at rest has extactly 0 velocity

        //movement information
        if (Input.GetKeyDown(Key.G))
        {
            Console.WriteLine();
            Console.WriteLine("Player:          x {0}, y {1}, width {2}, height {3}", x, y, width, height);
            Console.WriteLine("Hitbox:          x {0}, y {1}, width {2}, height {3}", playerHitBox.GetX(), playerHitBox.GetY(), playerHitBox.width, playerHitBox.height);
            Console.WriteLine("ColliderRect:    x {0}, y {1}, width {2}, height {3}", playerHitBox.playerCollision.Position.x, playerHitBox.playerCollision.Position.y, playerHitBox.playerCollision.Width, playerHitBox.playerCollision.Height);
            Console.WriteLine();
        }

        if (Input.GetKeyDown(Key.H))
        {
            Console.WriteLine();
            Console.WriteLine("velocity  {0}, playerVelocity {1},  fanVelocty {2}, gravityForce {3}, frictionForce {4}", velocity, playerVelocity, fanVelocity, gravityForce, frictionForce);
            Console.WriteLine("onCeiling {0}, onGround {1}", onCeiling, onGround);
            Console.WriteLine();
        }
    }
}