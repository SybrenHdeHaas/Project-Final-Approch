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

    private Boolean inshell; //true = player is in inshell state
    private Vec2 playerVelocity; //velocity of player movement
    private Vec2 velocity; //overall velocity (playerVelocity + other impacts of velocity)
    private Vec2 acceleration;

    public int playerIndex; //renamed from index to playerIndex for better naming. 0 = player1, 1 = player2
    public Detection detectionRange;
    public Hitbox playerHitBox;
    List<Vec2> fanVelocityList = new List<Vec2>();
    Vec2 fanVelocity;
    Vec2 position;

    ColliderPlayer playerCollision; //handles the player's collision

    public float mass;

    private Boolean[] movementDirection = new Boolean[3]; //stores player input (left, right, up). false means no input in that direction
    private Vec2 frictionForce; //overall velocity (playerVelocity + other impacts of velocity)
    private Vec2 playerForce; //velocity of player movement

    private float friction; //speed loss X
    private float drag = 0.2f; //speed loss Y
    private float standUpFriction = 0.25f; //max speed is now determined by friction. can be overruled by external forces not from the player
    private float inShellFriction = 0.1f;
    private static Vec2 gravityForce;
    private float gravity = 1f;
    private Boolean movementLock = false; //if true, ignore inputs

    public Boolean onGround = false;
    public Boolean onDoor = false;
    public Boolean onCeiling = false;
    public Boolean canDoAction = false; //if true, player can kick and pickup/throw aka detection intersected

    private float[] inShellStats; //posx, posy, width, height of player in shell
    private float[] outShellStats; //posx, posy, width, height of player not in shell

    public Vec2 spawnPoint; //the crood the player will move to if player dies

    private Boolean animationTypeActive = false;
    int baseFrame;

    private Vec2 kickForce;
    private Vec2 throwForce;
    float kickStrenghtX = 30;
    float kickStrengthY = 0;
    float throwStrenghtX = 30;
    float throwStrengthY = -30;

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

    public Boolean InShell
    {
        get { return inshell; }
        set { inshell = value; }
    }

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
    public int GetPlyaerIndex() { return playerIndex; }

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index");
        SetAnimationCycle(0, 1);
        mass = 4 * width * height;

        //get data about the hitbox (width, height, position) when player is not in shell from Tiled
        float hitboxWidth = obj.GetFloatProperty("float_hitBoxWidth", width);
        float hitboxHeight = obj.GetFloatProperty("float_hitBoxHeight", height);
        float hitBoxOffsetX = obj.GetFloatProperty("float_hitBoxOffsetX", 0);
        float hitBoxOffsetY = obj.GetFloatProperty("float_hitBoxOffsetY", 0);

        //get data about the hitbox (width, height, position) when player is in shell from Tiled
        float hitboxWidthShell = obj.GetFloatProperty("float_hitBoxShellWidth", 32);
        float hitboxHeightShell = obj.GetFloatProperty("float_hitBoxShellHeight", 32);
        float hitBoxShellOffsetX = obj.GetFloatProperty("float_hitBoxShellOffsetX", 0);
        float hiYtBoxShellOffset = obj.GetFloatProperty("float_hitBoxShellOffsetY", 0);

        //negative height is not allowed
        if (hitboxHeight < 0) 
        {
            hitboxHeight = height;
        }

        //negative width is not allowed
        if (hitboxWidth < 0)
        {
            hitboxWidth = width;
        }

        //negative height is not allowed
        if (hitboxWidthShell < 0)
        {
            hitboxWidthShell = 32;
        }

        //negative width is not allowed
        if (hitboxHeightShell < 0)
        {
            hitboxHeightShell = 32;
        }

        //creating hitbox
        if (playerIndex == 0)
        {
            playerHitBox = new Hitbox(mass, 0); //the player's actual hit box.
        }

        if (playerIndex == 1)
        {
            playerHitBox = new Hitbox(mass, 1); //the player's actual hit box.
        }

        //we no longer can make hitbox a child of player (would cause calculation errror if so), thus make it a child of myGame
        MyGame myGame = (MyGame)game;
        myGame.AddChild(playerHitBox);

        //creating derectionRange, works similar to hitbox but only deal with player intersection check instead
        detectionRange = new Detection(20,20);
        myGame.AddChild(detectionRange);

        //adding data
        float[] inShellStatsArray = { hitBoxShellOffsetX, hiYtBoxShellOffset, hitboxWidthShell, hitboxHeightShell};   //hitbox posx, posy, width, height of player in shell
        inShellStats = inShellStatsArray;
        float[] outShellStatsArray = { hitBoxOffsetX, hitBoxOffsetY, hitboxWidth, hitboxHeight}; //hitbox posx, posy, width, height of player not in shell
        outShellStats = outShellStatsArray;

        //adding collision calculator
        playerCollision = new ColliderPlayer(playerHitBox, new Vec2(0, 0), new Vec2(0, 0), playerHitBox.width, playerHitBox.height, true);
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
                    if (Input.GetKeyDown(Key.W) && ((onGround && !onCeiling) || onDoor))
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
                    if (Input.GetKeyDown(Key.I) && ((onGround && !onCeiling) || onDoor))
                    {
                        SoundChannel sound = new Sound("jump.wav", false).Play();
                        movementDirection[2] = true;
                    }
                    else { movementDirection[2] = false; }

                    break;
            }
        }
    }


    //player shell control
    private void ShellState()
    {
        if (playerIndex == 0)
        {
            if (inshell)
            {
                if (Input.GetKey(Key.W) && !OnCeiling) //player 1 inputs to go out of inShell state
                {
                    SetAnimationCycle(40, 1);
                    velocity += new Vec2(0, -20); //need to move the player a bit up to fix error which player falls off the map
                    inshell = false;
                }
            }

            if (!inshell)
            {

                if (Input.GetKey(Key.S))  //player 1 inputs to go into inShell state
                {
                    SetAnimationCycle(50, 8);
                    inshell = true;
                }
            }
        }

        if (playerIndex == 1)
        {
            if (inshell)
            {
                if (Input.GetKey(Key.I) && !OnCeiling) //player 2 inputs to go out of inShell state
                {
                    velocity += new Vec2(0, -20); //need to move the player a bit up to fix error which player falls off the map
                    inshell = false;
                }
            }

            if (!inshell)
            {

                if (Input.GetKey(Key.K)) //player 2 inputs to go into inShell state
                {
                    inshell = true;
                }
            }
        }
    }


    private void Kick()
    {
        SoundChannel sound = new Sound("kick.wav", false).Play();
        sound.Volume = 1.3f;

        //kick force change depending on player direction
        if (_mirrorX)
        {
            kickForce = new Vec2(-kickStrenghtX, kickStrengthY);
        }

        else
        {
            kickForce = new Vec2(kickStrenghtX, kickStrengthY);
        }

        //Get the player getting kicked
        Player theOtherPlayer = (Player)detectionRange.theDetectionInfo.theObject;

        //if that player is in shell, apply force
        if (theOtherPlayer.inshell)
        {
            theOtherPlayer.velocity = kickForce;
            theOtherPlayer.UpdateCollision(); //updates collision hitbox info
            theOtherPlayer.playerCollision.Step(); //performs collision
            theOtherPlayer.position += theOtherPlayer.playerCollision.Velocity;
            theOtherPlayer.x = theOtherPlayer.position.x;
            theOtherPlayer.y = theOtherPlayer.position.y;
        }
    }
    private void Throw()
    {
        SoundChannel sound = new Sound("throw.wav", false).Play();

        throwForce = new Vec2(throwStrenghtX, throwStrengthY);

        //get the player getting throwed
        Player theOtherPlayer = (Player)detectionRange.theDetectionInfo.theObject;

        //if that player is in shell, apply force
        if (theOtherPlayer.inshell)
        {
            theOtherPlayer.velocity = throwForce;
            theOtherPlayer.UpdateCollision(); //updates collision hitbox info
            theOtherPlayer.playerCollision.Step(); //performs collision
            theOtherPlayer.position += theOtherPlayer.playerCollision.Velocity;
            theOtherPlayer.x = theOtherPlayer.position.x;
            theOtherPlayer.y = theOtherPlayer.position.y;
        }
    }

    //determine if player can kick or pickup/throw
    private void Action()
    {
        detectionRange.PlayerInteractCheck();
        canDoAction = detectionRange.theDetectionInfo.intersected;

        if (Input.GetKeyDown(Key.Q) && canDoAction)
        {
            Kick();
        }

        if (canDoAction)
        {
            if (Input.GetKey(Key.O)) { Pickup(true); }
            else { Pickup(false); }
        }
    }

    private void Pickup(Boolean pickedUp)
    {
        Player interactPlayer = (Player)detectionRange.theDetectionInfo.theObject;


        if (pickedUp && interactPlayer.inshell)
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

            if ((base.currentFrame - base._startFrame) == base.frameCount - 1)
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

    void UpdateCollision()
    {
        if (!inshell)
        {
            playerHitBox.ChangeOffSetAndSize(position, outShellStats);
            detectionRange.ChangeOffSetAndSize(position, outShellStats);
        }

        else
        {
            playerHitBox.ChangeOffSetAndSize(position, inShellStats);
            detectionRange.ChangeOffSetAndSize(position, inShellStats);
        }
            

        playerCollision.width = playerHitBox.width;
        playerCollision.height = playerHitBox.height;
        playerCollision.Position = new Vec2(playerHitBox.GetX(), playerHitBox.GetY());

        playerCollision.Velocity = velocity;
    }

    void Update()
    {
        Move(movementDirection); //player movement accerlation
        ApplyForces(); //applying gravity, friction, and fan velocity

        canDoAction = false;

        if (!InShell)
        {
            Action();
        }
        

        AnimationController(); //performs animation

        //if no movement lock, checks player input and shell state
        if (!movementLock)
        {
            PlayerInput(); //updates player input
            ShellState(); //player shell control
        }

        //We will let player collision decide if the player is on ground or on ceiliing or neither
        onGround = false;
        onCeiling = false;
        onDoor = false;

        UpdateCollision(); //updates collision hitbox info
        playerCollision.Step(); //performs collision

        velocity = playerCollision.Velocity;
        position += velocity;
        x = position.x;
        y = position.y;

        VelocityFix(); //makes player at rest has extactly 0 velocity
    }
}