using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Player : AnimationSprite
    {

        private float maxSpeed = 2;
        private Vec2 velocity;
        private Vec2 newVelocity;
        private Vec2 acceleration;
        private int index;
        private Boolean inshell;
        private Boolean onGround;
        
        public Player(int index) : base("Player.png", 2, 1) 
        {
            Animate(0);
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
                
                if (Input.GetKey(Key.D)) { acceleration.x = 1;  }
                if (Input.GetKey(Key.A)) { acceleration.x = -1;  }
                
            }

            velocity += acceleration;

            if (velocity.x >= maxSpeed) { velocity.x = maxSpeed; }
            if (velocity.x <= -maxSpeed) { velocity.x = -maxSpeed; }

            if (velocity.x >= 0.25f) { acceleration.x -= 0.1f; }
            if (velocity.x <= -0.25f) { acceleration.x += 0.1f; }
            if (velocity.x > -0.25f && velocity.x < 0.25f) { velocity.x = 0; acceleration.x = 0; }

            Move(velocity.x, velocity.y);
        }

        private void shellState()
        {
            if (index == 0)
            {
                if (inshell) 
                {
                    SetCycle(1, 1);
                    
                    if (Input.GetKey(Key.W)) { inshell = false; } 

                }


                if (!inshell) 
                {
                    SetCycle(0, 1);

                    if (Input.GetKey(Key.S)) { inshell = true; } 

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
}
