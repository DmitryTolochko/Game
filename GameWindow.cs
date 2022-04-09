using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Globalization;

namespace Game
{
    public partial class MainForm : Form
    {
        private readonly GameState gameState;
        private readonly Timer timer = new Timer();
        TableLayoutPanel table;

        public MainForm()
        {
            table = new TableLayoutPanel();
            gameState = new GameState();
            DoubleBuffered = true;
            ClientSize = new Size(1920, 1080);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            GetImages();
            gameState.CreateButtons(table);
            InitializeComponent();
            timer.Interval = 16;
            timer.Tick += TimerTick;
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs args)
        {
            gameState.NextFrame(ClientSize, table);
            ShowButtons();
            //Controls.Clear();
            //for (int i = 0; i < table.Controls.Count; i++)
            //    Controls.Add(table.Controls[i]);
            Invalidate();
        }

        private void ShowButtons()
        {
            for (int i = 0; i < table.Controls.Count; i++)
                if (!Controls.Contains(table.Controls[i]))
                    Controls.Add(table.Controls[i]);
        }

        private void GetImages()
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(Resource1));
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
                gameState.images.Add(entry.Key.ToString(), (Image)entry.Value);
        }

        protected override void OnFormClosing(FormClosingEventArgs eventArgs)
        {
            var result = MessageBox.Show("Действительно закрыть?", "",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                eventArgs.Cancel = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            SuspendLayout();
            foreach (var windowElement in gameState.level.windowElements)
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
