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

    private Boolean inshell;

    private Vec2 playerVelocity; //velocity of player movement
    private Vec2 velocity; //overall velocity (playerVelocity + other impacts of velocity)
    private Vec2 acceleration;

    public int playerIndex; //renamed from index to playerIndex for better naming. 0 = player1, 1 = player2
    Detection detectionRange;
    public Hitbox playerHitBox;
    List<Vec2> fanVelocityList = new List<Vec2>();
    Vec2 fanVelocity;
    Vec2 position;

    ColliderPlayer playerCollision; //handles the player's collision

    public float mass;

    private Boolean[] movementDirection = new Boolean[3]; //stores player input (left, right, up). false means no input in that direction
    private Vec2 frictionForce;
    private Vec2 playerForce;

    private float friction; //speed loss X
    private float drag = 0.2f; //speed loss Y
    private float standUpFriction = 0.25f; //max speed is now determined by friction. can be overruled by external forces not from the player
    private float inShellFriction = 0.1f;
    private static Vec2 gravityForce;
    private float gravity = 1f;
    private Boolean movementLock = false; //if true, ignore inputs

    public Boolean onGround = false;
    public Boolean onCeiling = false;

    private float[] inShellStats; //posx, posy, width, height of player in shell
    private float[] outShellStats; //posx, posy, width, height of player not in shell

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



    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        playerIndex = obj.GetIntProperty("int_index");
        SetAnimationCycle(0, 1);
        mass = 4 * width * height;


        //we no longer use dection range, but leaving this here just in case
   //     detectionRange = new Detection(192, 192, mass);
   //     AddChild(detectionRange);

        playerHitBox = new Hitbox(0, 0, mass); //the player's actual hit box.
        AddChild(playerHitBox);

        float[] inShellStatsArray = { -64 / 2, (192 / 2) - 64, 64, 64 };
        inShellStats = inShellStatsArray;
        float[] outShellStatsArray = { -192 / 2, -192 / 2, 192, 192 };
        outShellStats = outShellStatsArray;

        playerHitBox.ChangeOffSetAndSize(outShellStats);
        playerCollision = new ColliderPlayer(playerHitBox, new Vec2(0, 0), new Vec2(0, 0), playerHitBox.width, playerHitBox.height, true);
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
        if (!onGround || onCeiling) { gravity = 1f; }
        if (onGround) { gravity = 0f; }

        gravityForce.y = gravity;
        acceleration += frictionForce + gravityForce;

        playerVelocity += acceleration + playerForce;
        playerForce = new Vec2(0f, 0f);

        velocity = playerVelocity + fanVelocity;
    }

    private void TryAction()
    {
    }


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
                    inshell = false;
                    playerHitBox.ChangeOffSetAndSize(outShellStats);
                    //      playerHitBox.playerCollision.outShellChanges();
                }
            }

            if (!inshell)
            {

                if (Input.GetKey(Key.K)) //player 2 inputs to go into inShell state
                {
                    inshell = true;
                    playerHitBox.ChangeOffSetAndSize(inShellStats);
                    //    playerHitBox.playerCollision.inShellChanges();
                }
            }
        }
    }

    void UpdateCollision()
    {
        playerCollision.width = playerHitBox.width;
        playerCollision.height = playerHitBox.height;
        playerCollision.Position = position + new Vec2(playerHitBox.x, playerHitBox.y);

        playerCollision.Velocity = velocity;
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

    void Update()
    {
        Move(movementDirection); //player movement accerlation
        ApplyForces(); //applying gravity, friction, and fan velocity
        TryAction(); //try perform kick and pickup if detection intersects
                     //       AnimationController(); //performs animation


        //if no movement lock, checks player input and shell state
        if (!movementLock)
        {
            PlayerInput(); //updates player input
            ShellState(); //player shell control
        }

        //We will let player collision decide if the player is on ground or on ceiliing or neither
        onCeiling = false;
        onGround = false;

        UpdateCollision(); //updates collision hitbox info
        playerCollision.Step(); //performs collision

        velocity = playerCollision.Velocity;
        position += velocity;
        x = position.x;
        y = position.y;

        VelocityFix(); //makes player at rest has extactly 0 velocity
    }
}