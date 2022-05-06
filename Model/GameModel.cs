using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.Control;
using static Game.MainForm;

namespace Game
{
    public class GameModel
    {
        private SceneryGenerator sceneryGenerator;
        private ObstacleGenerator obstacleGenerator;
        public Player player;
        public List<Obstacle> obstacles;
        public List<Diamond> diamonds;

        public int BestScore;
        public int Score;
        public int count;
        public int BestCrystalCount;
        public int CrystalCount;
        public int SkinNumber;
        public int AcquiredSkins;
        public double Acceleration;

        public Labels labels;
        public Buttons buttons;
        public List<WindowElement> windowElements = new List<WindowElement>();
        public Keys KeyPressed;
        public Size windowSize;

        public bool IsGameStarted = false;
        public bool IsGamePaused = false;
        public bool IsGameResumed = false;
        public bool IsGameFinished = false;
        public bool Reset = false;
        public bool BackToMenu = true;
        public bool IsFirstFrame = true;
        public bool GoToStore = false;

        public GameModel(ControlCollection Controls, Size windowSize)
        {
            this.windowSize = windowSize;
            StreamReader stream = new StreamReader(@"Data.txt");
            BestScore = int.Parse(stream.ReadLine());
            BestCrystalCount = int.Parse(stream.ReadLine());
            SkinNumber = int.Parse(stream.ReadLine());
            AcquiredSkins = int.Parse(stream.ReadLine());
            player = new Player(windowSize, SkinNumber);
            stream.Close();
            sceneryGenerator = new SceneryGenerator();
            obstacleGenerator = new ObstacleGenerator();
            buttons = new Buttons(this,  windowSize);
            obstacles = new List<Obstacle>();
            diamonds = new List<Diamond>();
            Controls.Clear();
            Controls.Add(buttons.PauseButton);
            labels = new Labels(this);
            Controls.Add(labels.ScoreLabel);
            Controls.Add(labels.CrystalCountLabel);
            Acceleration = 1;
        }

        public void NextFrame(ControlCollection Controls, Size windowSize, Dictionary<string, Bitmap> images)
        {
            this.windowSize = windowSize;
            Acceleration += 0.0005;
            if (IsFirstFrame)
            {
                Controls.Clear();
                Controls.Add(buttons.PauseButton);
            }
            if (IsGameResumed)
            {
                Controls.Clear();
                Controls.Add(buttons.PauseButton);
                IsGameResumed = false;
                IsGamePaused = false;
            }
            windowElements.Clear();
            Controller.KeyController(this);
            CheckCollision(windowSize);
            GenerateObstaclesAndCrystals();
            CheckDiamondCollision();
            sceneryGenerator.UpdateScenery(windowSize, this, images, IsFirstFrame, player, obstacles, diamonds);
            UpdateScore(Controls);
            MusicPlayer.Play(MusicType.Game, IsFirstFrame);
            MusicPlayer.Play(MusicType.Park, IsFirstFrame);
            IsFirstFrame = false;
        }

        private void UpdateScore(ControlCollection Controls)
        {
            labels.ScoreLabel.Location = labels.Game_ScoreLocation;
            labels.CrystalCountLabel.Location = labels.Game_CrystalCountLocation;
            if (count != 10)
                count += 1;
            else
            {
                count = 0;
                Score += 1;
                labels.ScoreLabel.Text = Score.ToString();
                Controls.Remove(labels.ScoreLabel);
                Controls.Add(labels.ScoreLabel);
            }
            if (Score > BestScore)
                BestScore = Score;

            labels.CrystalCountLabel.Text = CrystalCount.ToString();
            labels.BestScoreLabel.Text = BestScore.ToString();
            labels.GameOver_ScoreLabel.Text = labels.ScoreLabel.Text;
            labels.GameOver_BestScoreLabel.Text = labels.BestScoreLabel.Text;
            labels.BestCrystalCountLabel.Text = BestCrystalCount.ToString();

            Controls.Remove(labels.CrystalCountLabel);
            Controls.Add(labels.CrystalCountLabel);
        }

        private void CheckCollision(Size windowSize)
        {
            var flags = new HashSet<bool>();
            foreach (var obstacle in obstacles)
            {
                if (Math.Abs(player.WorkSpace.Right - obstacle.WorkSpace.Left) <= 20 &&
                        (player.WorkSpace.Bottom > obstacle.WorkSpace.Top))
                    IsGameFinished = true;
                if ((player.WorkSpace.IntersectsWith(obstacle.WorkSpace) ||
                    player.WorkSpace.Contains(obstacle.WorkSpace)) &&
                    obstacle.Name != "Manhole")
                    flags.Add(true);
            }
            if (flags.Any())
                player.IsCollised = true;
            else
                player.IsCollised = false;
        }

        private void GenerateObstaclesAndCrystals()
        {
            Random random = new Random();
            if (obstacles.Count == 0)
                obstacleGenerator.GetNewObstacle(obstacles, windowSize);
            else if (obstacles.Count < 3 && random.Next() % 3 == 0 && obstacles.Last().ActualLocation.X + obstacles.Last().Size.Width <= windowSize.Width)
                obstacleGenerator.GetNewObstacle(obstacles, windowSize);
            if (obstacles.Count > 0 && obstacles[0].ActualLocation.X < -windowSize.Width)
                obstacles.RemoveAt(0);
            foreach (var obstacle in obstacles)
                obstacle.Move(13, this);

            if (diamonds.Count == 0)
                diamonds.Add(new Diamond(windowSize, new Point(windowSize.Width, windowSize.Height - (int)(50 * 2.4 * windowSize.Height*3 / 1080))));

            else if (diamonds.Count < 3 && random.Next() % 6 == 0 && diamonds.Last().ActualLocation.X + diamonds.Last().Size.Width <= windowSize.Width && !obstacles.Last().HadCrystal)
            {
                diamonds.Add(new Diamond(windowSize, new Point(
                    obstacles.Last().ActualLocation.X + obstacles.Last().Size.Width/2,
                    obstacles.Last().ActualLocation.Y - (int)(50 * 2.4 * windowSize.Height / 1080))));
                obstacles.Last().HadCrystal = true;
            }
            if (diamonds.Count > 0 && diamonds[0].ActualLocation.X < -windowSize.Width)
                diamonds.RemoveAt(0);
            foreach (var diamond in diamonds)
                diamond.Move(13, this);
        }

        private void CheckDiamondCollision()
        {
            foreach (var diamond in diamonds)
                if (player.WorkSpace.Contains(diamond.ActualLocation.X + diamond.Size.Width, diamond.ActualLocation.Y))
                {
                    diamond.IsCollected = true;
                    CrystalCount++;
                    BestCrystalCount++;
                    MusicPlayer.Play(SoundType.Crystal);
                }
            diamonds = diamonds
                .Where(x => x.IsCollected == false)
                .ToList();
        }
    }
}
