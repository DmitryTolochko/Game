using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    interface LevelInterface
    {
        List<Button> buttons { get; }
        List<WindowElement> windowElements { get; }
        Player player { get; }
    }
}
