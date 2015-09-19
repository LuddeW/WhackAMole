﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;

namespace WhackAMole
{
    class RabbitClass
    {
        public Vector2 RabbitPos;
        public float StopY;
        public int FrontHeight;
        public int RabbitHeight;
        public int RabbitWidth;
        public int Velocity;
        public Rectangle Hitbox;
        public bool Dead;
        public float SpawnTime;



        public RabbitClass(Vector2 RabbitPos, float StopY, int FrontHeight, int RabbitWidth, int RabbitHeight)
        {
            this.RabbitPos = RabbitPos;
            this.StopY = StopY;
            this.FrontHeight = FrontHeight;
            this.RabbitHeight = RabbitHeight;
            this.RabbitWidth = RabbitWidth;
            Dead = true;
            Velocity = 0;
            SpawnTime = 0;
            Hitbox = new Rectangle((int)RabbitPos.X, (int)RabbitPos.Y, RabbitWidth, RabbitHeight);

        }
        public void Movement(int NewVelocity)
        {

            int StopYLow = 450;
                
            StopY = FrontHeight + 0.25f * RabbitHeight;
            RabbitPos.Y = RabbitPos.Y + Velocity;
            Hitbox.Y = (int)RabbitPos.Y;

            if (Dead)
            {
                Velocity = NewVelocity;
                if (RabbitPos.Y >= StopYLow)
                {
                    Velocity = 0;
                }
            }
            if (!Dead)
            {
                
                if (RabbitPos.Y <= StopY)
                {
                    Velocity = NewVelocity;
                    
                    
                }
                else if (RabbitPos.Y >= StopYLow)
                {
                    Dead = true;
                    
                }
                
                

                
                
               
            }
            
           
        }

        
    }

}
