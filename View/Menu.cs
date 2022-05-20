using System.Collections.Generic;
using System.Drawing;
using static System.Windows.Forms.Control;

namespace Game
{
    public class Menu
    {
        public bool IsFirstFrame = true;

        public void MainMenu(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            if (IsFirstFrame)
            {
                Controls.Clear();
                level.windowElements.Clear();
                Controls.Add(level.buttons.ExitButton);
                Controls.Add(level.buttons.PlayButton);
                Controls.Add(level.buttons.GoToStoreButton);
                level.windowElements.Add(new WindowElement(0, 0, images["Menu_Background"], windowSize));
                level.windowElements.Add(new WindowElement(0, 0, images["Menu_Light_flash_1"], windowSize));
                level.windowElements.Add(new WindowElement(0, 0, images["Menu_Name"], windowSize));
            }
            MusicPlayer.Play(MusicType.MainMenu, IsFirstFrame);
            IsFirstFrame = false;
        }

        public void PauseMenu(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            if (IsFirstFrame)
            {
                Controls.Clear();
                Controls.Add(level.buttons.PauseBackToMainMenuButton);
                Controls.Add(level.buttons.ResumeButton);
                Controls.Add(level.buttons.PauseRecetButton);
                level.windowElements.Add(new WindowElement(0, 0, images["Pause_Background"], windowSize));
                IsFirstFrame = false;
            }
        }

        public void GameOverMenu(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            if (IsFirstFrame)
            {
                MusicPlayer.Play(MusicType.GameOver, IsFirstFrame);
                Controls.Clear();
                Controls.Add(level.buttons.GameOverRecetButton);
                Controls.Add(level.buttons.GameOverBackToMainMenuButton);
                level.windowElements.Add(new WindowElement(0, 0, images["Game_Over_BG"], windowSize));

                Controls.Add(level.labels.BestCrystalCountLabel);
                Controls.Add(level.labels.GameOver_BestScoreLabel);
                Controls.Add(level.labels.GameOver_ScoreLabel);
                level.labels.CrystalCountLabel.Location = level.labels.GameOver_CrystalCountLocation;
                Controls.Add(level.labels.CrystalCountLabel);
                IsFirstFrame = false;
            }
        }
    }
}
