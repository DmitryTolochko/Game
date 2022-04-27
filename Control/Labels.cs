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
        public CustomLabel CrystalCountLabel;
        public CustomLabel BestScoreLabel;
        public CustomLabel BestCrystalCountLabel;
        public CustomLabel GameOver_ScoreLabel;
        public CustomLabel GameOver_BestScoreLabel;

        public readonly Point GameScoreLocation;
        public readonly Point GameCrystalCountLocation;
        public readonly Point GameOver_BestScoreLocation;
        public readonly Point GameOver_BestCrystalCountLocation;
        public readonly Point GameOver_ScoreLocation;
        public readonly Point GameOver_CrystalCountLocation;

        public Labels(GameModel level)
        {
            GameScoreLocation = new Point(level.windowSize.Width / 2 - 40, 0);
            GameCrystalCountLocation = new Point(5 * level.windowSize.Width / 6, level.windowSize.Height / 30);

            GameOver_ScoreLocation = new Point(level.windowSize.Width / 2 + 40, level.windowSize.Height / 2 - 127);
            GameOver_BestScoreLocation = new Point(level.windowSize.Width / 2 + 40, level.windowSize.Height / 2 - 105);
            GameOver_CrystalCountLocation = new Point(level.windowSize.Width / 2 - 40, level.windowSize.Height / 2 - 80);
            GameOver_BestCrystalCountLocation = new Point(level.windowSize.Width / 2 - 40, level.windowSize.Height / 2);            

            ScoreLabel = new CustomLabel(level.Score.ToString(), GameScoreLocation, level.windowSize, Color.White, 40);
            BestScoreLabel = new CustomLabel(level.BestScore.ToString(), GameOver_BestScoreLocation, level.windowSize, Color.Black, 40);
            CrystalCountLabel = new CustomLabel(level.CrystalCount.ToString(), GameCrystalCountLocation, level.windowSize, Color.Black, 40);
            BestCrystalCountLabel = new CustomLabel(level.BestCrystalCount.ToString(), GameOver_BestCrystalCountLocation, level.windowSize, Color.Black, 40);

            GameOver_ScoreLabel = new CustomLabel(level.Score.ToString(), GameOver_ScoreLocation, level.windowSize, Color.Black, 10);
            GameOver_BestScoreLabel = new CustomLabel(level.BestScore.ToString(), GameOver_BestScoreLocation, level.windowSize, Color.Black, 10);
        }
    }

    public class CustomLabel : Label
    {
        public CustomLabel(string text, Point position, Size windowSize, Color color, int FontSize)
        {
            Location = position;
            Text = text;
            BorderStyle = BorderStyle.None;
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.Transparent;
            ForeColor = color;
            Font = new Font("Abril Fatface", FontSize, FontStyle.Regular);
            Size = new Size(windowSize.Width/5, windowSize.Height / 10);
        }
    }
}
