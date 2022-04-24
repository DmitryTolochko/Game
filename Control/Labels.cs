using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public class Labels
    {
        public CustomLabel ScoreLabel;
        public CustomLabel BestScoreLabel;
        public CustomLabel CrystalCountLabel;

        public Labels(GameModel level)
        {
            ScoreLabel = new CustomLabel(level.Score.ToString(), new Point(level.windowSize.Width / 2 - 40, 0), level.windowSize, Color.White);
            BestScoreLabel = new CustomLabel(level.BestScore.ToString(), new Point(level.windowSize.Width / 2 - 40, 0), level.windowSize, Color.Black);
            CrystalCountLabel = new CustomLabel(level.CrystalsCount.ToString(), new Point(5*level.windowSize.Width/6, level.windowSize.Height/30), level.windowSize, Color.Black);
        }
    }

    public class CustomLabel : Label
    {
        public CustomLabel(string text, Point position, Size windowSize, Color color)
        {
            Location = position;
            Text = text;
            BorderStyle = BorderStyle.None;
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.Transparent;
            ForeColor = color;
            Font = new Font("Abril Fatface", 40, FontStyle.Regular);
            Size = new Size(windowSize.Width/5, windowSize.Height / 10);
        }
    }
}
