using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public class Level : LevelInterface
    {
        public Buttons buttons = new Buttons();
        public List<WindowElement> windowElements = new List<WindowElement>();

        public Player player => throw new NotImplementedException();

        List<Button> LevelInterface.buttons => throw new NotImplementedException();

        List<WindowElement> LevelInterface.windowElements => throw new NotImplementedException();
    }
}
