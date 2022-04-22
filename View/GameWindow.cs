using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Globalization;
using System.Media;

namespace Game
{
    public partial class MainForm : Form
    {
        public Dictionary<string, Bitmap> images = new Dictionary<string, Bitmap>();
        public static readonly HashSet<Keys> pressedKeys = new HashSet<Keys>();

        private GameModel gameModel;
        private Menu menu;
        private readonly Timer timer = new Timer();

        public MainForm(Size windowSize)
        {
            DoubleBuffered = true;
            ClientSize = windowSize;
            gameModel = new GameModel(Controls, ClientSize);
            menu = new Menu();
            //FormBorderStyle = FormBorderStyle.FixedDialog;
            //System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly();
            //Stream s = a.GetManifestResourceStream(@"Game.Sounds.Pulse.wav");
            //SoundPlayer player = new SoundPlayer(s);
            
            GetImages();
            //InitializeComponent();
            timer.Interval = 16;
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs args)
        {
            if (gameModel.IsGameFinished)
            {
                gameModel = new GameModel(Controls, ClientSize);
                menu.MainMenu(Controls, ClientSize, gameModel, images);
            }
            if (gameModel.Reset)
                gameModel = new GameModel(Controls, ClientSize) { IsGameStarted = true };

            if (gameModel.IsGameStarted && !gameModel.IsGamePaused || gameModel.IsGameResumed)
                gameModel.NextFrame(Controls, ClientSize, images);
            else if (gameModel.IsGamePaused && gameModel.IsGameStarted)
                menu.PauseMenu(Controls, ClientSize, gameModel, images);
            else if (gameModel.IsGameFinished && !gameModel.IsGameStarted)
            {
                gameModel = new GameModel(Controls, ClientSize);
                menu.MainMenu(Controls, ClientSize, gameModel, images);
            }
            else
                menu.MainMenu(Controls, ClientSize, gameModel, images);
            //Game.Menu.MainMenu(Controls, ClientSize, gameModel, images);
            Invalidate();
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
            foreach (var windowElement in gameModel.windowElements)
            {
                g.DrawImage(windowElement.Image,
                    (float)windowElement.xPosition,
                    (float)windowElement.yPosition,
                    windowElement.Size.Width,
                    windowElement.Size.Height);
            }
            ResumeLayout();
        }
    }
}
