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
        private Detection detectionRange = new Detection();
        
        public Player(int index) : base("Player.png", 2, 1) 
        {
            SetOrigin(width / 2, height / 2);
            Animate(0);
            AddChild(detectionRange);
            detectionRange.scale = 2.5f;
            
        }


        private void ShellUpdate() 
        {
            if (inshell) 
            {
                detectionRange.scaleY = 1.25f;
                detectionRange.y = 16f;
            }
            if (!inshell) 
            { 
                detectionRange.scaleY = 2.5f;
                detectionRange.y = 0;
            }
        
        
        
        }


        private void CollisionDetection() 
        { 
            
        
        
        
        
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

            if (velocity.x >= 0.15f) { acceleration.x -= 0.1f; }
            if (velocity.x <= -0.15f) { acceleration.x += 0.1f; }
            if (velocity.x > -0.15f && velocity.x < 0.15f) { velocity.x = 0; acceleration.x = 0; }

            MoveUntilCollision(velocity.x, velocity.y);
        }

        private void shellState()
        {
            if (index == 0)
            {
                if (inshell) 
                {
                    SetCycle(1, 1);
                    
                    if (Input.GetKey(Key.W)) { inshell = false; ShellUpdate(); } 

                }


                if (!inshell) 
                {
                    SetCycle(0, 1);

                    if (Input.GetKey(Key.S)) { inshell = true; ShellUpdate(); } 

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
