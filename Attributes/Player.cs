using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Player
    {
        public List<List<Image>> Animations = new List<List<Image>>();
        public Point Location;
        public Point Target;
    }
}
