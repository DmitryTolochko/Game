using System.Collections.Generic;
using System.Drawing;

namespace Game
{
    public class Player
    {
        public PlayerAnimator PlayerAnimator;
        public Point ActualLocation;
        public Size Size;
        public int Border;
        private Size windowSize;

        private static Point SpawnLocation;

        public Player(Size windowSize)
        {
            this.windowSize = windowSize;
            SpawnLocation = new Point(-120, windowSize.Height - windowSize.Height * 67 / 100);
            //2 * windowSize.Height / 5
            ActualLocation = SpawnLocation;
            Size = new Size(windowSize.Width * 36 / 100, windowSize.Height * 64 / 100);
            PlayerAnimator = new PlayerAnimator();
        }

        public enum TargetDirection
        {
            Up, 
            Left, 
            Right,
            Nowhere
        }

        public int dx = 1;
        public bool CanJump = true;

        public void MoveTo(List<TargetDirection> direction)
        {
            if (direction.Contains(TargetDirection.Up) && !(ActualLocation.Y < 0) && CanJump)
            {
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y - 20/dx);
                //dx += 3;
                //if (dx > 80)
                //{
                //    CanJump = false;
                //    dx = 1;
                //}
            }
            if (direction.Contains(TargetDirection.Left) && ActualLocation.X >= 0)
            {
                ActualLocation = new Point(ActualLocation.X-10, ActualLocation.Y);
            }
            if (direction.Contains(TargetDirection.Right) && ActualLocation.X + Size.Width <= windowSize.Width)
            {
                ActualLocation = new Point(ActualLocation.X+10, ActualLocation.Y);
            }
            if (!direction.Contains(TargetDirection.Up) && ActualLocation.Y <= SpawnLocation.Y)
            {
                //dx += 1;
                //if (System.Math.Abs(SpawnLocation.Y - ActualLocation.Y + dx) >= 0.2)
                //{
                //    ActualLocation = new Point(ActualLocation.X, SpawnLocation.Y);
                //    dx = 1;
                //    CanJump = true;
                //}
                //else
                ActualLocation = new Point(ActualLocation.X, ActualLocation.Y + 30);
            }
        }
    }
}
