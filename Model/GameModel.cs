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
        private List<Obstacle> obstacles;
        public List<Diamond> diamonds;
        public SoundPlayer soundPlayer;
        public int BestScore;
        public int Score = 0;
        private int count = 0;
        public int CrystalsCount = 0;
        private Labels labels;

        public Buttons buttons;
        public List<WindowElement> windowElements = new List<WindowElement>();
        public Keys KeyPressed;
        public Size windowSize;

        public bool IsGameStarted = false;
        public bool IsGamePaused = false;
        public bool IsGameResumed = false;
        public bool IsGameFinished = false;
        public bool Reset = false;
        public bool IsFirstFrame = true;

        public GameModel(ControlCollection Controls, Size windowSize)
        {
            this.windowSize = windowSize;
            StreamReader stream = new StreamReader(@"Data.txt");
            BestScore = int.Parse(stream.ReadLine());
            CrystalsCount = int.Parse(stream.ReadLine());
            stream.Close();
            soundPlayer = new SoundPlayer();
            sceneryGenerator = new SceneryGenerator();
            obstacleGenerator = new ObstacleGenerator();
            buttons = new Buttons(this,  windowSize);
            player = new Player(windowSize);
            obstacles = new List<Obstacle>();
            diamonds = new List<Diamond>();
            Controls.Clear();
            Controls.Add(buttons.PauseButton);
            labels = new Labels(this);
            Controls.Add(labels.ScoreLabel);
            Controls.Add(labels.CrystalCountLabel);
        }

        public void NextFrame(ControlCollection Controls, Size windowSize, Dictionary<string, Bitmap> images)
        {
            this.windowSize = windowSize;
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
            IsFirstFrame = false;
        }

        private void UpdateScore(ControlCollection Controls)
        {
            if (count != 10)
                count += 1;
            else
            {
                count = 0;
                Score += 1;
                labels.ScoreLabel.Text = Score.ToString();
                labels.CrystalCountLabel.Text = CrystalsCount.ToString();
                Controls.Remove(labels.ScoreLabel);
                Controls.Add(labels.ScoreLabel);
            }
            if (Score > BestScore)
                BestScore = Score;
            Controls.Remove(labels.CrystalCountLabel);
            Controls.Add(labels.CrystalCountLabel);
        }

        private void CheckCollision(Size windowSize)
        {
            foreach (var obstacle in obstacles)
            {
                var playerX = player.ActualLocation.X + player.Size.Width / 2;
                var playerY = player.ActualLocation.Y + player.Size.Height * 2 / 3;
                var distanceX = obstacle.ActualLocation.X - playerX;
                var distanceY = obstacle.ActualLocation.Y - playerY;
                var dx = System.Math.Abs(distanceX * 1920 / (2.4 * windowSize.Width));
                //if (distanceX == 0 && (player.ActualLocation.Y + player.Size.Height <= obstacle.ActualLocation.Y))
                //    IsGameFinished = true;
                if (distanceX == 0 && player.ActualLocation.Y + player.Size.Height <= obstacle.ActualLocation.Y)
                    IsGameFinished = true;
                else if (obstacle.ActualLocation.X <= player.ActualLocation.X + player.Size.Width &&
                    obstacle.ActualLocation.Y <= player.ActualLocation.Y + player.Size.Height &&
                    obstacle.ActualLocation.X + obstacle.Size.Width >= player.ActualLocation.X &&
                    obstacle.ActualLocation.Y + obstacle.Size.Height >= player.ActualLocation.Y &&
                    distanceX <= 0 && distanceY <= 0)
                {
                    try
                    {
                        if (distanceX <= 0 && distanceY <= 0 && obstacle.pixels[(int)dx, 108] != 0)
                            IsGameFinished = true;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
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
                obstacle.Move(13);

            if (diamonds.Count == 0)
                diamonds.Add(new Diamond(windowSize, new Point(windowSize.Width, windowSize.Height - (int)(50 * 2.4 * windowSize.Height*3 / 1080))));

            else if (diamonds.Count < 3 && random.Next() % 6 == 0 && diamonds.Last().ActualLocation.X + diamonds.Last().Size.Width <= windowSize.Width && !obstacles.Last().HadCrystal)
            {
                diamonds.Add(new Diamond(windowSize, new Point(
                    obstacles.Last().ActualLocation.X + obstacles.Last().Size.Width/2,
                    obstacles.Last().ActualLocation.Y - (int)(50 * 2.4 * windowSize.Height / 1080))));
                obstacles.Last().HadCrystal = true;
            }

            //else if (diamonds.Count < 6 && random.Next() % 15 == 0 && diamonds.Last().ActualLocation.X + diamonds.Last().Size.Width <= windowSize.Width)
            //    diamonds.Add(new Diamond(windowSize, new Point(windowSize.Width, windowSize.Height - (int)(50 * 2.4 * windowSize.Height / 1080))));
            if (diamonds.Count > 0 && diamonds[0].ActualLocation.X < -windowSize.Width)
                diamonds.RemoveAt(0);
            foreach (var diamond in diamonds)
                diamond.Move(13);
        }

        private void CheckDiamondCollision()
        {
            foreach (var diamond in diamonds)
                if (Math.Abs(player.ActualLocation.X + diamond.Size.Width - diamond.ActualLocation.X) <= diamond.Size.Width)
                {
                    diamond.IsCollected = true;
                    CrystalsCount++;
                }
            diamonds = diamonds.Where(x => x.IsCollected == false).ToList();
        }
    }
}
