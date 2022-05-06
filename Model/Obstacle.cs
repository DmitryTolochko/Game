using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Obstacle(Size windowSize, Bitmap image, string name)
        {
            Image = image;
            Name = name;
            Size = new Size((int)(Image.Width * 2.4 * windowSize.Width / 1920), 
                (int)(Image.Height * 2.4 * windowSize.Height / 1080));
            ActualLocation = new Point(windowSize.Width + image.Width, windowSize.Height - Size.Height);
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width / 7, ActualLocation.Y, Size.Width - Size.Width * 2 / 7, Size.Height);
        }

        public void Move(int speed, GameModel level)
        {
            Size = new Size((int)(Image.Width * 2.4 * level.windowSize.Width / 1920),
                (int)(Image.Height * 2.4 * level.windowSize.Height / 1080));
            ActualLocation = new Point((int)(ActualLocation.X - (int)speed *level.Acceleration), level.windowSize.Height - Size.Height);
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width / 7, ActualLocation.Y, Size.Width - Size.Width * 2 / 7, Size.Height);
        }
    }
}
