using System.Collections.Generic;
using System.Drawing;
using System.Media;
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
        public SoundPlayer soundPlayer;

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
            soundPlayer = new SoundPlayer();
            sceneryGenerator = new SceneryGenerator();
            obstacleGenerator = new ObstacleGenerator();
            buttons = new Buttons(this,  windowSize);
            player = new Player(windowSize);
            obstacles = new List<Obstacle>();
            Controls.Clear();
            Controls.Add(buttons.PauseButton);
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
            if (obstacles.Count == 0)
                obstacleGenerator.GetNewObstacle(obstacles, windowSize);
            else if (obstacles[0].ActualLocation.X < -windowSize.Width )
                obstacles.RemoveAt(0);
            foreach (var obstacle in obstacles)
                obstacle.Move(10);
            sceneryGenerator.UpdateScenery(windowSize, this, images, IsFirstFrame, player, obstacles);
            IsFirstFrame = false;
        }

        private void CheckCollision(Size windowSize)
        {
            foreach (var obstacle in obstacles)
            {
                var playerX = player.ActualLocation.X + player.Size.Width / 2;
                var playerY = player.ActualLocation.Y + player.Size.Height * 2 / 3;
                var obstX = obstacle.ActualLocation.X - playerX;
                var obstY = obstacle.ActualLocation.Y - playerY;
                var dx = System.Math.Abs(obstX * 1920 / (2.4 * windowSize.Width));
                if (obstX == 0 && player.ActualLocation.Y + player.Size.Height <= obstacle.ActualLocation.Y)
                    IsGameFinished = true;
                else if (obstacle.ActualLocation.X <= player.ActualLocation.X + player.Size.Width &&
                    obstacle.ActualLocation.Y <= player.ActualLocation.Y + player.Size.Height &&
                    obstacle.ActualLocation.X + obstacle.Size.Width >= player.ActualLocation.X &&
                    obstacle.ActualLocation.Y + obstacle.Size.Height >= player.ActualLocation.Y &&
                    obstX <= 0 && obstY <= 0)
                {
                    try
                    {
                        if (obstX <= 0 && obstY <= 0 && obstacle.pixels[(int)dx, 108] != 0)
                            IsGameFinished = true;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
    }
}
