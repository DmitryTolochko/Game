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
        
        public Size Size;
        public Rectangle WorkSpace;
        public int Border;

        public bool IsJumping = false;
        public bool IsCollised = false;

        public Point ActualLocation;
        public Point SpawnLocation;

        private int timer = 0;
        private TargetDirection targetDirection;
        public int Time;
        public int Acceleration = 10;
        public int JumpCount;
        private bool LastSpaceClick = false;

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
            Acceleration = Acceleration * level.windowSize.Height / 768;
            var windowSize = level.windowSize;
            SpawnLocation = new Point(-120 * windowSize.Width / 1366, windowSize.Height - windowSize.Height * 67 / 100);
            Size = new Size(windowSize.Width * 36 / 100, windowSize.Height * 64 / 100);
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
                default:
                    {
                        return new ResourceManager(typeof(Resource10));
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
                default:
                    {
                        return new ResourceManager(typeof(Resource11));
                    }
            }
        }

        private ResourceManager DriftSequence(int SkinNumber)
        {
            switch (SkinNumber)
            {
                case 1:
                    {
                        return new ResourceManager(typeof(Resource9));
                    }
                case 2:
                    {
                        return new ResourceManager(typeof(Resource8));
                    }
                default:
                    {
                        return new ResourceManager(typeof(Resource12));
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

        public void MoveTo(HashSet<TargetDirection> direction, GameModel gameModel)
        {
            if ((int)gameModel.Acceleration > Acceleration%10 + 1)
             Acceleration += 1;
            ChangeYPos(direction);

            if (direction.Contains(TargetDirection.Left) && ActualLocation.X >= 0)
                ActualLocation = new Point(ActualLocation.X - 15, ActualLocation.Y);
            if (direction.Contains(TargetDirection.Right) && ActualLocation.X + Size.Width <= gameModel.windowSize.Width)
                ActualLocation = new Point(ActualLocation.X + 10, ActualLocation.Y);
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width * 3 / 9,
                ActualLocation.Y + Size.Height * 2 / 9, Size.Width / 4, Size.Height * 2 / 5);
        }

        private void ChangeYPos(HashSet<TargetDirection> direction)
        {
            CountSpaceDoubleClicks(direction);
            if (timer < 7 && direction.Contains(TargetDirection.Up) && !(ActualLocation.Y < 0))
            {
                if (JumpCount > 2)
                    JumpCount++;
                CheckAndChangeYDirection(TargetDirection.Up);
                if (!IsJumping)
                {
                    JumpAnimation.RestartAnimation();
                    IsJumping = true;
                }
                if (timer == 0)
                    timer++;
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y - Acceleration * 12 / Math.Abs(Time));
                timer += 1;
            }
            else if (IsCollised && Border != -1)
            {
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y - Border);
                JumpCount = 1;
            }
            else if (timer >= 7 && ActualLocation.Y <= Border && !IsCollised)
            {
                CheckAndChangeYDirection(TargetDirection.Nowhere);
                FallDown();
                if (!direction.Contains(TargetDirection.Up))
                    timer = 0;
            }
            else if (!direction.Contains(TargetDirection.Up) && ActualLocation.Y <= Border && !IsCollised)
            {
                CheckAndChangeYDirection(TargetDirection.Nowhere);
                timer--;
                FallDown();
                if (ActualLocation.Y == Border)
                    timer = 0;
            }
            else
                timer += 1;
            if (IsCollised && !direction.Contains(TargetDirection.Up))
                timer = 0;
            if (ActualLocation.Y == Border && !direction.Contains(TargetDirection.Up) && !LastSpaceClick)
                JumpCount = 0;
        }

        private void FallDown()
        {
            var dy = Acceleration * Time;
            if (ActualLocation.Y + dy >= Border)
            {
                ActualLocation = new Point(ActualLocation.X, Border);
                IsJumping = false;
            }
            else
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + dy);
        }

        private void CountSpaceDoubleClicks(HashSet<TargetDirection> direction)
        {
            if (LastSpaceClick == false && direction.Contains(TargetDirection.Up))
                JumpCount++;
            if (direction.Contains(TargetDirection.Up))
                LastSpaceClick = true;
            else
                LastSpaceClick = false;
            if (JumpCount > 2)
                direction.Remove(TargetDirection.Up);
        }

        private void CheckAndChangeYDirection(TargetDirection direction)
        {
            if (targetDirection != direction)
            {
                targetDirection = direction;
                Time = 1;
            }
            else
                Time++;
        }
    }
}
