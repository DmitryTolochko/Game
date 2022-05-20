using System;
using System.Collections.Generic;
using System.Drawing;
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
        private int timer = 0;
        public Rectangle WorkSpace;
        public int Border;

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
            Border = SpawnLocation.Y;
        }

        public void OnResize(GameModel level)
        {
            ActualLocation = new Point(ActualLocation.X* level.windowSize.Width / 1366, ActualLocation.Y * level.windowSize.Height/768);
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
            SpawnLocation = new Point(-120 * windowSize.Width / 1366, windowSize.Height - windowSize.Height * 67 / 100);
            Size = new Size(windowSize.Width * 36 / 100, windowSize.Height * 64 / 100);
            ChangeYPos(direction, windowSize);

            if (direction.Contains(TargetDirection.Left) && ActualLocation.X >= 0)
                ActualLocation = new Point(ActualLocation.X - 15, ActualLocation.Y);
            if (direction.Contains(TargetDirection.Right) && ActualLocation.X + Size.Width <= windowSize.Width)
                ActualLocation = new Point(ActualLocation.X + 10, ActualLocation.Y);
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width * 3 / 9,
                ActualLocation.Y + Size.Height * 2 / 9, Size.Width / 4, Size.Height * 2 / 5);
        }

        private int branchNum;
        public int Time;
        public int Acceleration = 10;

        private void ChangeYPos(List<TargetDirection> direction, Size windowSize)
        {
            if (timer < 7 && direction.Contains(TargetDirection.Up) && !(ActualLocation.Y < 0))
            {
                if (branchNum != 1)
                {
                    branchNum = 1;
                    Time = 1;
                }
                else
                    Time++;
                if (!IsJumping)
                    JumpAnimation.RestartAnimation();
                IsJumping = true;
                if (timer == 0)
                    timer++;
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y - 10 - 80 / Math.Abs(Time));
                timer += 1;
            }
            else if (IsCollised && Border != -1)
            {
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y - Border);
            }
            else if (timer >= 7 && ActualLocation.Y <= Border && !IsCollised)
            {
                if (branchNum != 2)
                {
                    branchNum = 2;
                    Time = 1;
                }
                else
                    Time++;
                var dy = Acceleration * Time;
                if (ActualLocation.Y + dy >= Border)
                {
                    ActualLocation = new Point(ActualLocation.X, Border);
                    IsJumping = false;
                }
                else
                    ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + dy);
                if (!direction.Contains(TargetDirection.Up))
                    timer = 0;
            }
            else if (!direction.Contains(TargetDirection.Up) && ActualLocation.Y <= Border && !IsCollised)
            {
                if (branchNum != 2)
                {
                    branchNum = 2;
                    Time = 1;
                }
                else
                    Time++;
                timer--;
                var dy = Acceleration * Time;
                if (ActualLocation.Y + dy >= Border)
                {
                    ActualLocation = new Point(ActualLocation.X, Border);
                    IsJumping = false;
                }
                else
                    ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + dy);
                if (ActualLocation.Y == Border)
                    timer = 0;
            }
            else
                timer += 1;
            if (IsCollised && !direction.Contains(TargetDirection.Up))
                timer = 0;
        }
    }
}
