using System.Drawing;
using System.Resources;

namespace Game
{
    public class Diamond
    {
        public readonly Animator Animator;
        public Point ActualLocation;
        public Size Size;
        public bool IsCollected = false;
        private double speed;

        public Diamond(Size windowSize, Point position)
        {
            speed = windowSize.Width / 100;
            ActualLocation = position;
            Animator = new Animator(new ResourceManager(typeof(Resource4)));
            Size = new Size(
                (int)(50 * 2.4 * windowSize.Width / 1920),
                (int)(50 * 2.4 * windowSize.Height / 1080));
        }

        public void Move(GameModel level)
        {
            Size = new Size(
                (int)(50 * 2.4 * level.windowSize.Width / 1920),
                (int)(50 * 2.4 * level.windowSize.Height / 1080));
            ActualLocation = new Point(
                (int)(ActualLocation.X - speed * level.Acceleration),
                ActualLocation.Y);
        }

        public void OnResize(GameModel level)
        {
            speed = level.windowSize.Width / 100;
            Size = new Size(
                (int)(50 * 2.4 * level.windowSize.Width / 1920),
                (int)(50 * 2.4 * level.windowSize.Height / 1080));
            ActualLocation = new Point(
                ActualLocation.X * level.windowSize.Width / 1366,
                ActualLocation.Y * level.windowSize.Height / 768);
        }
    }
}
