using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Player : GameObject
    {

        private float maxSpeed;
        private Vec2 velocity;
        private Vec2 accelration;
        private int index;
        private Boolean inshell;
        
        public Player(int index) : base() 
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

        }

        private void shellState()
        {
            if (index == 0)
            {
                if (inshell && Input.GetKey(Key.W)) { inshell = false; }
                if (!inshell && Input.GetKey(Key.S)) { inshell = true; }
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



        }


    }
}
