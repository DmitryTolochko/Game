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
        public readonly Size Size;
        public byte[,] pixels;

        public Obstacle(Size windowSize, Bitmap image)
        {
            ActualLocation = new Point(windowSize.Width + image.Width, 8*windowSize.Height/12);
            Image = image;
            Size = new Size((int)(Image.Width * 2.4 * windowSize.Width / 1920), 
                (int)(Image.Height * 2.4 * windowSize.Height / 1080));
            pixels = LoadPixels(Image);
        }

        public void Move(int speed)
        {
            ActualLocation = new Point(ActualLocation.X - speed, ActualLocation.Y);
        }

        public static byte[,] LoadPixels(Bitmap bmp)
        {
            var pixels = new byte[bmp.Width, bmp.Height];
            for (var x = 0; x < bmp.Width; x++)
                for (var y = 0; y < bmp.Height; y++)
                    pixels[x, y] = bmp.GetPixel(x, y).A;
            return pixels;
        }
    }
}
