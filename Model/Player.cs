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
        public Point ActualLocation;
        public Size Size;
        public int Border;
        private int timer = 0;
        public Rectangle WorkSpace;

        public bool IsJumping = false;

        public Point SpawnLocation;

        public Player(Size windowSize)
        {
            SpawnLocation = new Point(-120, windowSize.Height - windowSize.Height * 67 / 100);
            ActualLocation = SpawnLocation;
            Size = new Size(windowSize.Width * 36 / 100, windowSize.Height * 64 / 100);
            RunAnimation = new Animator(new ResourceManager(typeof(Resource2)));
            JumpAnimation = new Animator(new ResourceManager(typeof(Resource5)));
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width * 3 / 7, 
                ActualLocation.Y + Size.Height * 2 / 7, Size.Width / 4, Size.Height * 2 / 5);
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

            if (timer < 7 && direction.Contains(TargetDirection.Up) && !(ActualLocation.Y < 0))
            {
                if (!IsJumping)
                    JumpAnimation.RestartAnimation();
                IsJumping = true;
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y - 40);
                timer += 1;
            }
            else if (timer >= 7 && ActualLocation.Y <= Border)
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
            else if (!direction.Contains(TargetDirection.Up) && ActualLocation.Y <= Border)
            {
                timer--;
                if (ActualLocation.Y + 30 >= Border)
                {
                    ActualLocation = new Point(ActualLocation.X, Border);
                    IsJumping = false;
                }
                else
                    ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + 30);
                if (ActualLocation.Y == Border)
                    timer = 0;
            }
            else 
                timer += 1;
            if (direction.Contains(TargetDirection.Left) && ActualLocation.X >= 0)
            {
                ActualLocation = new Point(ActualLocation.X-15, ActualLocation.Y);
            }
            if (direction.Contains(TargetDirection.Right) && ActualLocation.X + Size.Width <= windowSize.Width)
            {
                ActualLocation = new Point(ActualLocation.X+10, ActualLocation.Y);
            }
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width * 3 / 7, 
                ActualLocation.Y + Size.Height * 2 / 7, Size.Width / 4, Size.Height * 2 / 5);
        }
    }
}
