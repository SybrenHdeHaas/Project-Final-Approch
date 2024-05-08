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
    private Vec2 velocity;
    private Vec2 newVelocity;
    private Vec2 acceleration;
    private int index;
    private Boolean inshell;
    private Boolean onGround;


    List<Vec2> fanVelocityList = new List<Vec2>();
    Vec2 fanVelocity;

    Vec2 position;

    float friction = 0.5f;

    public Player(string filenName, int rows, int columns, TiledObject obj = null) : base(filenName, rows, columns, obj)
    {
        index = obj.GetIntProperty("int_index", 1);
        SetAnimationCycle(0, 1);
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

    void CalcFanVeclotiy()
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
        velocity += acceleration;
        CalcFanVeclotiy();
        velocity += fanVelocity;
        position += velocity;
        x = position.x;
        y = position.y;
    }

    private void shellState()
    {
        if (index == 0)
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

            //no player move control so no acceleration
            if (Input.GetKey(Key.A) == false && Input.GetKey(Key.D) == false)
            {
                acceleration = new Vec2(0, 0);
                acceleration += velocity * -friction;
            }

        }

        if (index == 1)
        {
            if (inshell && Input.GetKey(Key.I)) { inshell = false; }
            if (!inshell && Input.GetKey(Key.K)) { inshell = true; }
        }

    }
    void Update()
    {
        shellState();
        Moving();
    }
}