using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game
{
    public class WindowElement
    {
        public readonly double yPosition;
        public readonly double xPosition;
        public readonly Image Image;
        public readonly Size Size;
        public WindowElement (double x, double y, Image image, Size size)
        {
            yPosition = y;
            xPosition = x;
            Image = image;
            Size = size;
        }
    }
}
