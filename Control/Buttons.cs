using System.Drawing;
using System.Windows.Forms;

namespace Game
{
    public class Buttons
    {
        public CustomButton ExitButton;
        public CustomButton PlayButton;
        public CustomButton PauseButton;
        public CustomButton PauseRecetButton;
        public CustomButton GameOverRecetButton;
        public CustomButton ResumeButton;
        public CustomButton PauseBackToMainMenuButton;
        public CustomButton GameOverBackToMainMenuButton;

        public Buttons(GameModel gameModel, Size windowSize)
        {
            PauseButton = new CustomButton(new Point(0, 0), Resource1.Game_pause_button_up, Resource1.Game_pause_button_down, windowSize);
            PauseButton.Click += (sender, args) => gameModel.IsGamePaused = true;

            PlayButton = new CustomButton(new Point(windowSize.Width/6, windowSize.Height/3), Resource1.Menu_Play_button_up, Resource1.Menu_Play_button_down, windowSize);
            PlayButton.Click += (sender, args) => gameModel.IsGameStarted = true;
            PlayButton.Click += (sender, args) => gameModel.BackToMenu = false;

            ExitButton = new CustomButton(new Point(PlayButton.Location.X, 
                (int)(PlayButton.Location.Y + PlayButton.Size.Height*1.5)), 
                Resource1.Menu_Exit_button_up, Resource1.Menu_Exit_button_down, windowSize);
            ExitButton.Click += (sender, args) => Application.Exit();

            PauseBackToMainMenuButton = new CustomButton(new Point(
                (int)(windowSize.Width - PlayButton.Size.Width), 
                (int)(windowSize.Height - PlayButton.Size.Height*1.5)), 
                Resource1.Menu_button_up, Resource1.Menu_button_down, windowSize);
            PauseBackToMainMenuButton.Click += (sender, args) => gameModel.BackToMenu = true;
            PauseBackToMainMenuButton.Click += (sender, args) => gameModel.IsGameStarted = false;

            ResumeButton = new CustomButton(new Point(PauseBackToMainMenuButton.Location.X, 
                (int)(PauseBackToMainMenuButton.Location.Y - PlayButton.Size.Height * 1.5)), 
                Resource1.Resume_button_up, Resource1.Resume_button_down, windowSize);
            ResumeButton.Click += (sender, args) => gameModel.IsGameResumed = true;

            PauseRecetButton = new CustomButton(new Point(ResumeButton.Location.X,
                (int)(ResumeButton.Location.Y - PlayButton.Size.Height * 1.5)), 
                Resource1.Reset_button_up, Resource1.Reset_button_down, windowSize);
            PauseRecetButton.Click += (sender, args) => gameModel.Reset = true;

            GameOverRecetButton = new CustomButton(new Point(
                (int)(windowSize.Width / 2 - PlayButton.Size.Width), 
                windowSize.Height * 2 / 3),
                Resource1.Reset_button_up, Resource1.Reset_button_down, windowSize);
            GameOverRecetButton.Click += (sender, args) => gameModel.Reset = true;

            GameOverBackToMainMenuButton = new CustomButton(new Point(
                (int)(windowSize.Width / 2), 
                windowSize.Height * 2 / 3),
                Resource1.Menu_button_up, Resource1.Menu_button_down, windowSize);
            GameOverBackToMainMenuButton.Click += (sender, args) => gameModel.BackToMenu = true;
            GameOverBackToMainMenuButton.Click += (sender, args) => gameModel.IsGameStarted = false;
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
