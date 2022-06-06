using System.Drawing;
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

        public readonly Point Game_ScoreLocation;
        public readonly Point Game_CrystalCountLocation;
        public readonly Point GameOver_BestScoreLocation;
        public readonly Point GameOver_BestCrystalCountLocation;
        public readonly Point GameOver_ScoreLocation;
        public readonly Point GameOver_CrystalCountLocation;
        public readonly Point Store_BestCrystalCountLocation;

        public Labels(GameModel level)
        {
            Game_ScoreLocation = new Point(level.windowSize.Width / 2 - 40 * level.windowSize.Width / 1366, 0);
            Game_CrystalCountLocation = new Point(5 * level.windowSize.Width / 6, 0);

            GameOver_ScoreLocation = new Point(
                level.windowSize.Width / 2 + 40 * level.windowSize.Width / 1366,
                level.windowSize.Height / 2 - 127 * level.windowSize.Height / 768);
            GameOver_BestScoreLocation = new Point(
                level.windowSize.Width / 2 + 40 * level.windowSize.Width / 1366,
                level.windowSize.Height / 2 - 105 * level.windowSize.Height / 768);
            GameOver_CrystalCountLocation = new Point(
                level.windowSize.Width / 2 - 40 * level.windowSize.Width / 1366,
                level.windowSize.Height / 2 - 80 * level.windowSize.Height / 768);
            GameOver_BestCrystalCountLocation = new Point(
                level.windowSize.Width / 2 - 40 * level.windowSize.Width / 1366,
                level.windowSize.Height / 2);

            Store_BestCrystalCountLocation = new Point(
                200 * level.windowSize.Width / 1366,
                level.windowSize.Height - 110 * level.windowSize.Height / 768);

            ScoreLabel = new CustomLabel(
                level.Score.ToString(),
                Game_ScoreLocation,
                level.windowSize,
                Color.White,
                40);
            BestScoreLabel = new CustomLabel(
                level.BestScore.ToString(),
                GameOver_BestScoreLocation,
                level.windowSize,
                Color.Black,
                40);
            CrystalCountLabel = new CustomLabel(
                level.CrystalCount.ToString(),
                Game_CrystalCountLocation,
                level.windowSize,
                Color.Black,
                40);
            BestCrystalCountLabel = new CustomLabel(
                level.BestCrystalCount.ToString(),
                GameOver_BestCrystalCountLocation,
                level.windowSize,
                Color.Black,
                40);

            GameOver_ScoreLabel = new CustomLabel(
                level.Score.ToString(),
                GameOver_ScoreLocation,
                level.windowSize,
                Color.Black,
                10);
            GameOver_BestScoreLabel = new CustomLabel(
                level.BestScore.ToString(),
                GameOver_BestScoreLocation,
                level.windowSize,
                Color.Black,
                10);
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
            Font = new Font("Abril Fatface", FontSize * windowSize.Width / 1366, FontStyle.Regular);
            AutoSize = true;
        }
    }
}
