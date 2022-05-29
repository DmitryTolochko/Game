using System.Drawing;

namespace Game
{
    public class WindowElement
    {
        public readonly double yPosition;
        public readonly double xPosition;
        public readonly Bitmap Image;
        public readonly SizeF Size;
        public Rectangle rectangle;
        public WindowElement (double x, double y, Bitmap image, SizeF size)
        {
            yPosition = y;
            xPosition = x;
            Image = image;
            Size = size;
        }
    }
}
