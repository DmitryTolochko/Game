﻿using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public class Buttons
    {
        public CustomButton ExitButton;
        public CustomButton PlayButton;
        public CustomButton PauseButton;
        public CustomButton RecetButton;
        public CustomButton ResumeButton;
        public CustomButton BackToMainMenuButton;

        public Buttons(GameModel gameModel, Size windowSize)
        {
            PauseButton = new CustomButton(new Point(0, 0), Resource1.Game_pause_button_up, Resource1.Game_pause_button_down, windowSize);
            PauseButton.Click += (sender, args) => gameModel.IsGamePaused = true;

            PlayButton = new CustomButton(new Point(windowSize.Width/6, windowSize.Height/3), Resource1.Menu_Play_button_up, Resource1.Menu_Play_button_down, windowSize);
            PlayButton.Click += (sender, args) => gameModel.IsGameStarted = true;

            ExitButton = new CustomButton(new Point(PlayButton.Location.X, 
                (int)(PlayButton.Location.Y + PlayButton.Size.Height*1.5)), 
                Resource1.Menu_Exit_button_up, Resource1.Menu_Exit_button_down, windowSize);
            ExitButton.Click += (sender, args) => Application.Exit();

            BackToMainMenuButton = new CustomButton(new Point(
                (int)(windowSize.Width - PlayButton.Size.Width), 
                (int)(windowSize.Height - PlayButton.Size.Height*1.5)), 
                Resource1.Menu_button_up, Resource1.Menu_button_down, windowSize);
            BackToMainMenuButton.Click += (sender, args) => gameModel.IsGameStarted = false;
            BackToMainMenuButton.Click += (sender, args) => gameModel.IsGameFinished = true;

            ResumeButton = new CustomButton(new Point(BackToMainMenuButton.Location.X, 
                (int)(BackToMainMenuButton.Location.Y - PlayButton.Size.Height * 1.5)), 
                Resource1.Resume_button_up, Resource1.Resume_button_down, windowSize);
            ResumeButton.Click += (sender, args) => gameModel.IsGameResumed = true;

            RecetButton = new CustomButton(new Point(ResumeButton.Location.X,
                (int)(ResumeButton.Location.Y - PlayButton.Size.Height * 1.5)), 
                Resource1.Reset_button_up, Resource1.Reset_button_down, windowSize);
            RecetButton.Click += (sender, args) => gameModel.Reset = true;
        }
    }

    public class CustomButton : Button
    {
        public CustomButton(Point point, Bitmap UnpluggedButton, Bitmap PluggedButton, Size windowSize)
        {
            Location = point;
            Image = ResizedImage(windowSize, UnpluggedButton);
            FlatAppearance.BorderSize = 0;
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            GotFocus += (sender, args) => Image = ResizedImage(windowSize, PluggedButton);
            LostFocus += (sender, args) => Image = ResizedImage(windowSize, UnpluggedButton);
            Size = new Size(Image.Width, Image.Height);
        }

        private Bitmap ResizedImage(Size windowSize, Bitmap bitmap)
        {
            return new Bitmap(bitmap, new Size(windowSize.Width * bitmap.Width / 1960,
                windowSize.Height * bitmap.Height / 1080));
        }
    }
}
