using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using System.Resources;

namespace Game
{
    public class Player
    {
        public Animator RunAnimation;
        public Animator JumpAnimation;
        public Animator DriftAnimation;
        public Point ActualLocation;
        public Size Size;
        public int Border;
        private int timer = 0;
        public Rectangle WorkSpace;

        public bool IsJumping = false;
        public bool IsCollised = false;

        public Point SpawnLocation;

        public Player(Size windowSize, int SkinNumber)
        {
            SpawnLocation = new Point(-120, windowSize.Height - windowSize.Height * 67 / 100);
            ActualLocation = SpawnLocation;
            Size = new Size(windowSize.Width * 36 / 100, windowSize.Height * 64 / 100);
            RunAnimation = new Animator(RunSequence(SkinNumber));
            JumpAnimation = new Animator(JumpSequence(SkinNumber));
            DriftAnimation = new Animator(DriftSequence(SkinNumber));
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width * 3 / 9, 
                ActualLocation.Y + Size.Height * 2 / 9, Size.Width / 4, Size.Height * 2 / 5);
        }

        private ResourceManager RunSequence(int SkinNumber)
        {
            switch (SkinNumber)
            {
                case 1:
                    {
                        return new ResourceManager(typeof(Resource6));
                    }
                case 2:
                    {
                        return new ResourceManager(typeof(Resource2));
                    }
                case 3:
                    {
                        return new ResourceManager(typeof(Resource2));
                    }
                case 4:
                    {
                        return new ResourceManager(typeof(Resource2));
                    }
                default:
                    {
                        return new ResourceManager(typeof(Resource2));
                    }
            }
        }

        private ResourceManager JumpSequence(int SkinNumber)
        {
            switch (SkinNumber)
            {
                case 1:
                    {
                        return new ResourceManager(typeof(Resource7));
                    }
                case 2:
                    {
                        return new ResourceManager(typeof(Resource5));
                    }
                case 3:
                    {
                        return new ResourceManager(typeof(Resource5));
                    }
                case 4:
                    {
                        return new ResourceManager(typeof(Resource5));
                    }
                default:
                    {
                        return new ResourceManager(typeof(Resource5));
                    }
            }
        }

        private ResourceManager DriftSequence(int SkinNumber)
        {
            switch (SkinNumber)
            {
                case 1:
                    {
                        return new ResourceManager(typeof(Resource7));
                    }
                case 2:
                    {
                        return new ResourceManager(typeof(Resource8));
                    }
                case 3:
                    {
                        return new ResourceManager(typeof(Resource5));
                    }
                case 4:
                    {
                        return new ResourceManager(typeof(Resource5));
                    }
                default:
                    {
                        return new ResourceManager(typeof(Resource5));
                    }
            }
        }

        public enum TargetDirection
        {
            Up, 
            Left, 
            Right,
            Nowhere
        }

        public void MoveTo(List<TargetDirection> direction, Size windowSize)
        {
            SpawnLocation = new Point(-120, windowSize.Height - windowSize.Height * 67 / 100);
            Size = new Size(windowSize.Width * 36 / 100, windowSize.Height * 64 / 100);
            Border = SpawnLocation.Y;
            if (timer < 7 && direction.Contains(TargetDirection.Up) && !(ActualLocation.Y < 0))
            {
                if (!IsJumping)
                    JumpAnimation.RestartAnimation();
                IsJumping = true;
                if (timer == 0)
                    timer++;
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y - 10 - 80/Math.Abs(timer));
                timer += 1;
            }
            else if (timer >= 7 && ActualLocation.Y <= Border && !IsCollised)
            {
                if (ActualLocation.Y + 30 >= Border)
                {
                    ActualLocation = new Point(ActualLocation.X, Border);
                    IsJumping = false;
                }
                else
                      ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + 30);
                if (!direction.Contains(TargetDirection.Up))
                    timer = 0;
            }
            else if (!direction.Contains(TargetDirection.Up) && ActualLocation.Y <= Border && !IsCollised)
            {
                timer--;
                if (ActualLocation.Y + 30 >= Border)
                {
                    ActualLocation = new Point(ActualLocation.X, Border);
                    IsJumping = false;
                }
                //else if (timer != 0)
                //    ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + 30 + 50 / Math.Abs(timer));
                else
                    ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + 30);
                if (ActualLocation.Y == Border)
                    timer = 0;
            }
            else
                timer += 1;
            if (IsCollised && !direction.Contains(TargetDirection.Up))
                timer = 0;
            if (direction.Contains(TargetDirection.Left) && ActualLocation.X >= 0)
            {
                ActualLocation = new Point(ActualLocation.X-15, ActualLocation.Y);
            }
            if (direction.Contains(TargetDirection.Right) && ActualLocation.X + Size.Width <= windowSize.Width)
            {
                ActualLocation = new Point(ActualLocation.X+10, ActualLocation.Y);
            }
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width * 3 / 9, 
                ActualLocation.Y + Size.Height * 2 / 9, Size.Width / 4, Size.Height * 2 / 5);
        }
    }
}
