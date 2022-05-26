using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;

namespace Game
{
    public partial class MainForm : Form
    {
        public Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
        public static readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();
        public GameModel gameModel;
        public Store store;
        private readonly Menu menu;
        private readonly Timer timer = new Timer();
        public Size WindowSize;
        private readonly SceneryGenerator sceneryGenerator;

        public MainForm(Size windowSize)
        {
            //FormBorderStyle = FormBorderStyle.SizableToolWindow;
            //StartPosition = FormStartPosition.CenterScreen;
            this.WindowSize = new Size (windowSize.Width*ClientSize.Width/2560, windowSize.Height*ClientSize.Height/1440);
            DoubleBuffered = true;
            ClientSize = windowSize;
            GetImages();
            sceneryGenerator = new SceneryGenerator(images);
            gameModel = new GameModel(Controls, ClientSize, images, sceneryGenerator);
            store = new Store(ClientSize);
            menu = new Menu();            
            timer.Interval = 16;
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (gameModel.GoToStore)
            {
                store.NextFrame(Controls, ClientSize, gameModel, images);
                StreamWriter file = new StreamWriter(@"Data.txt");
                menu.IsFirstFrame = true;
                file.WriteLine(gameModel.BestScore);
                file.WriteLine(gameModel.BestCrystalCount);
                file.WriteLine(gameModel.SkinNumber);
                file.WriteLine(gameModel.AcquiredSkins);
                file.Close();
            }
            else if (gameModel.BackToMenu)
            {
                menu.MainMenu(Controls, ClientSize, gameModel, images);
                gameModel.Reset = true;
                store.IsFirstFrame = true;
            }
            else if (gameModel.Reset)
            {
                menu.IsFirstFrame = true;
                gameModel = new GameModel(Controls, ClientSize, images, sceneryGenerator) { BackToMenu = false };
                GC.Collect();
            }
            else if (gameModel.IsGameResumed)
            {
                menu.IsFirstFrame = true;
                gameModel.NextFrame(Controls, ClientSize, images);
                gameModel.IsGameResumed = false;
                gameModel.IsGamePaused = false;
            }
            else if (gameModel.IsGameFinished)
            {
                StreamWriter file = new StreamWriter(@"Data.txt");
                file.WriteLine(gameModel.BestScore);
                file.WriteLine(gameModel.BestCrystalCount);
                file.WriteLine(gameModel.SkinNumber);
                file.WriteLine(gameModel.AcquiredSkins);
                file.Close();
                menu.GameOverMenu(Controls, ClientSize, gameModel, images);
            }
            else if (gameModel.IsGamePaused)
                menu.PauseMenu(Controls, ClientSize, gameModel, images);
            else
            {
                gameModel.NextFrame(Controls, ClientSize, images);
                menu.IsFirstFrame = true;
            }
            Task.Run(() => Invalidate());
            if ((gameModel.IsGamePaused || gameModel.IsGameFinished) && gameModel.BackToMenu)
            {
                menu.IsFirstFrame = true;
                if (gameModel.IsGamePaused)
                    gameModel.IsGamePaused = false;
                else
                    gameModel.IsGameFinished = false;
            }
        }

        private void GetImages()
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(Resource1));
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                images.Add(entry.Key.ToString(), (Bitmap)entry.Value);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            pressedKeys.Add(e.KeyCode);
            gameModel.KeyPressed = e.KeyCode;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (gameModel != null)
            {
                if (ClientSize.Width / 1366 > 1)
                    gameModel.Acceleration *= ClientSize.Width / 1366;
                gameModel.windowSize = ClientSize;
                gameModel.labels = new Labels(gameModel);
                gameModel.buttons = new Buttons(gameModel);
                store = new Store(ClientSize);
                gameModel.IsFirstFrame = true;
                menu.IsFirstFrame = true;
                gameModel.player.OnResize(gameModel);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
            gameModel.KeyPressed = pressedKeys.Any() ? pressedKeys.Min() : Keys.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            SuspendLayout();
            lock (gameModel.windowElements)
                lock (images)
                    foreach (var windowElement in gameModel.windowElements.AsParallel())
                    {
                        g.Flush();
                        g.DrawImage(windowElement.Image,
                            (float)windowElement.xPosition,
                            (float)windowElement.yPosition,
                            windowElement.Size.Width,
                            windowElement.Size.Height);
                        //if (!windowElement.rectangle.IsEmpty)
                        //    g.DrawRectangle(new Pen(Color.White), windowElement.rectangle);
                    }
            ResumeLayout();
        }
    }
}
