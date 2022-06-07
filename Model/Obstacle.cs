using System.Drawing;

namespace Game
{
    public class Obstacle
    {
        public readonly Bitmap Image;
        public Point ActualLocation;
        public Size Size;
        public bool HadCrystal = false;
        public Rectangle WorkSpace;
        public readonly string Name;
        private double speed;

        public Obstacle(Size windowSize, Bitmap image, string name)
        {
            speed = windowSize.Width / 100;
            Image = image;
            Name = name;
            Size = new Size((int)(Image.Width * 2.4 * windowSize.Width / 1920),
                (int)(Image.Height * 2.4 * windowSize.Height / 1080));
            ActualLocation = new Point(windowSize.Width + image.Width, windowSize.Height - Size.Height);
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width / 7, ActualLocation.Y, Size.Width - Size.Width * 2 / 7, Size.Height);
        }

        public void Move(GameModel level)
        {
            Size = new Size((int)(Image.Width * 2.4 * level.windowSize.Width / 1920),
                (int)(Image.Height * 2.4 * level.windowSize.Height / 1080));
            ActualLocation = new Point(
                (int)(ActualLocation.X - speed * level.Acceleration),
                level.windowSize.Height - Size.Height);
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width / 7, ActualLocation.Y, Size.Width - Size.Width * 2 / 7, Size.Height);
        }

        public void OnResize(GameModel level)
        {
            speed = level.windowSize.Width / 100;
            Size = new Size((int)(Image.Width * 2.4 * level.windowSize.Width / 1920),
                (int)(Image.Height * 2.4 * level.windowSize.Height / 1080));
            ActualLocation = new Point(
                ActualLocation.X * level.windowSize.Width / 1366, 
                ActualLocation.Y * level.windowSize.Height / 768);
        }
    }
}
