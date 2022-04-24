using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Diamond
    {
        public readonly Animator Animator;
        public Point ActualLocation;
        public readonly Size Size;
        public bool IsCollected = false;

        public Diamond(Size windowSize, Point position)
        {
            ActualLocation = position;
            Animator = new Animator(new ResourceManager(typeof(Resource4)));
            Size = new Size((int)(50 * 2.4 * windowSize.Width / 1920), (int)(50 * 2.4 * windowSize.Height / 1080));
        }

        public void Move(int speed)
        {
            ActualLocation = new Point(ActualLocation.X - speed, ActualLocation.Y);
        }
    }
}
