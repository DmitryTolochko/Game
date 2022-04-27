using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Obstacle
    {
        public readonly Bitmap Image;
        public Point ActualLocation;
        public Size Size;
        public bool HadCrystal = false;
        public Rectangle WorkSpace;

        public Obstacle(Size windowSize, Bitmap image)
        {
            ActualLocation = new Point(windowSize.Width + image.Width, 8*windowSize.Height/12);
            Image = image;
            Size = new Size((int)(Image.Width * 2.4 * windowSize.Width / 1920), 
                (int)(Image.Height * 2.4 * windowSize.Height / 1080));
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width / 7, ActualLocation.Y, Size.Width - Size.Width * 2 / 7, Size.Height);
        }

        public void Move(int speed, Size windowSize)
        {
            Size = new Size((int)(Image.Width * 2.4 * windowSize.Width / 1920),
                (int)(Image.Height * 2.4 * windowSize.Height / 1080));
            ActualLocation = new Point(ActualLocation.X - speed, 8 * windowSize.Height / 12);
            WorkSpace = new Rectangle(ActualLocation.X + Size.Width / 7, ActualLocation.Y, Size.Width - Size.Width * 2 / 7, Size.Height);
        }
    }
}
