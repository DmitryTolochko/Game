using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public class Buttons
    {
        public CustomButton ExitButton;
        public CustomButton PlayButton;
        public CustomButton PauseButton;
        public CustomButton RecetButton;
    }

    public class CustomButton : Button
    {
        public CustomButton(Point point, Image bitmap)
        {
            Location = point;
            FlatAppearance.BorderSize = 0;
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.Transparent;
            Image = bitmap;
            Size = new Size(bitmap.Width, bitmap.Height);
        }
    }
}
