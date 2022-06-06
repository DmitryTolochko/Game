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
        public CustomButton GoToStoreButton;
        public CustomButton VolumeButton;
        public CustomButton ResumeForDiamonds;

        public Buttons(GameModel gameModel)
        {
            PauseButton = new CustomButton(
                new Point(0, 0), 
                Resource1.Game_pause_button_up,
                Resource1.Game_pause_button_down,
                gameModel.windowSize);
            PauseButton.Click += (s, a) => gameModel.IsGamePaused = true;

            PlayButton = new CustomButton(
                new Point(gameModel.windowSize.Width/6, gameModel.windowSize.Height/3), 
                Resource1.Menu_Play_button_up, 
                Resource1.Menu_Play_button_down,
                gameModel.windowSize);
            PlayButton.Click += (s, a) => gameModel.IsGameStarted = true;
            PlayButton.Click += (s, a) => gameModel.BackToMenu = false;

            GoToStoreButton = new CustomButton(
                new Point(PlayButton.Location.X,
                (int)(PlayButton.Location.Y + PlayButton.Size.Height * 1.5)),
                Resource1.Store_button_up, 
                Resource1.Store_button_down,
                gameModel.windowSize);
            GoToStoreButton.Click += (s, a) => gameModel.GoToStore = true;

            ExitButton = new CustomButton(
                new Point(PlayButton.Location.X, 
                (int)(GoToStoreButton.Location.Y + PlayButton.Size.Height*1.5)), 
                Resource1.Menu_Exit_button_up, 
                Resource1.Menu_Exit_button_down,
                gameModel.windowSize);
            ExitButton.Click += (s, a) => Application.Exit();

            PauseBackToMainMenuButton = new CustomButton(
                new Point(
                gameModel.windowSize.Width - PlayButton.Size.Width, 
                (int)(gameModel.windowSize.Height - PlayButton.Size.Height*1.5)), 
                Resource1.Menu_button_up, 
                Resource1.Menu_button_down,
                gameModel.windowSize);
            PauseBackToMainMenuButton.Click += (s, a) => gameModel.BackToMenu = true;
            PauseBackToMainMenuButton.Click += (s, a) => gameModel.IsGameStarted = false;
            PauseBackToMainMenuButton.Click += (s, a) => gameModel.GoToStore = false;

            ResumeButton = new CustomButton(
                new Point(PauseBackToMainMenuButton.Location.X, 
                (int)(PauseBackToMainMenuButton.Location.Y - PlayButton.Size.Height * 1.5)), 
                Resource1.Resume_button_up, 
                Resource1.Resume_button_down,
                gameModel.windowSize);
            ResumeButton.Click += (s, a) => gameModel.IsGameResumed = true;

            PauseRecetButton = new CustomButton(
                new Point(ResumeButton.Location.X,
                (int)(ResumeButton.Location.Y - PlayButton.Size.Height * 1.5)), 
                Resource1.Reset_button_up, 
                Resource1.Reset_button_down,
                gameModel.windowSize);
            PauseRecetButton.Click += (s, a) => gameModel.Reset = true;

            GameOverRecetButton = new CustomButton(
                new Point(
                gameModel.windowSize.Width / 2 - PlayButton.Size.Width,
                gameModel.windowSize.Height * 2 / 3),
                Resource1.Reset_button_up,
                Resource1.Reset_button_down,
                gameModel.windowSize);
            GameOverRecetButton.Click += (s, a) => gameModel.Reset = true;

            GameOverBackToMainMenuButton = new CustomButton(
                new Point(gameModel.windowSize.Width / 2,
                gameModel.windowSize.Height * 2 / 3),
                Resource1.Menu_button_up,
                Resource1.Menu_button_down,
                gameModel.windowSize);
            GameOverBackToMainMenuButton.Click += (s, a) => gameModel.BackToMenu = true;
            GameOverBackToMainMenuButton.Click += (s, a) => gameModel.IsGameStarted = false;
            GameOverBackToMainMenuButton.Click += (s, a) => gameModel.GoToStore = false;

            var image = MusicPlayer.VolumeImage;
            VolumeButton = new CustomButton(
                new Point(10 * gameModel.windowSize.Width / 1366, 
                gameModel.windowSize.Height - 46 * gameModel.windowSize.Height/768),
                image,
                image,
                gameModel.windowSize);
            VolumeButton.Click += (s, a) =>
            {
                var imageNew = CustomButton.ResizedImage(gameModel.windowSize, MusicPlayer.ChangeVolume());
                VolumeButton.Image = imageNew;
                VolumeButton.GotFocus += (sender, args) => VolumeButton.Image = imageNew;
                VolumeButton.LostFocus += (sender, args) => VolumeButton.Image = imageNew;
            };

            ResumeForDiamonds = new CustomButton(
                new Point(GameOverRecetButton.Location.X-7*gameModel.windowSize.Width/1366,
                (int)(GameOverRecetButton.Location.Y + GameOverRecetButton.Size.Height*1.7)),
                Resource1.Continue_Button_Up,
                Resource1.Continue_Button_Down,
                gameModel.windowSize);
            ResumeForDiamonds.Click += (s, a) =>
            {
                gameModel.IsGameFinished = false;
                gameModel.obstacles.Clear();
                gameModel.diamonds.Clear();
                gameModel.BestCrystalCount -= 300;
                gameModel.IsGameResumed = true;
                gameModel.IsFirstFrame = true;
            };
        }
    }

    public class CustomButton : Button
    {
        public CustomButton(Point location, Bitmap UnpluggedButton, Bitmap PluggedButton, Size windowSize)
        {
            Location = location;
            Image = ResizedImage(windowSize, UnpluggedButton);
            FlatAppearance.BorderSize = 0;
            FlatStyle = FlatStyle.Flat;
            BackColor = Color.Transparent;
            FlatAppearance.MouseOverBackColor = Color.Transparent;
            FlatAppearance.MouseDownBackColor = Color.Transparent;
            GotFocus += (sender, args) => Image = ResizedImage(windowSize, PluggedButton);
            LostFocus += (sender, args) => Image = ResizedImage(windowSize, UnpluggedButton);
            Size = new Size(Image.Width, Image.Height);
            Click += (s, a) => MusicPlayer.Play(SoundType.Button);
        }

        public static Bitmap ResizedImage(Size windowSize, Bitmap bitmap)
        {
            return new Bitmap(bitmap, new Size(windowSize.Width * bitmap.Width / 1960,
                windowSize.Height * bitmap.Height / 1080));
        }
    }
}
