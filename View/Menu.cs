using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Media;
using static System.Windows.Forms.Control;
using static Game.GameModel;

namespace Game
{
    public class Menu
    {
        private static Stream stream;
        private static SoundPlayer player;

        public Menu()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            stream = assembly.GetManifestResourceStream(@"Game.Sounds.Pulse1.wav");
        }

        public void MainMenu(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            if (player == null)
            {
                player = new SoundPlayer(stream);
                player.Play();
            }
            Controls.Clear();
            Controls.Add(level.buttons.ExitButton);
            Controls.Add(level.buttons.PlayButton);
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Background"], windowSize));
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Background"], windowSize));
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Light_flash_1"], windowSize));
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Name"], windowSize));
        }

        public void PauseMenu(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            Controls.Clear();
            Controls.Add(level.buttons.BackToMainMenuButton);
            Controls.Add(level.buttons.ResumeButton);
            Controls.Add(level.buttons.RecetButton);
            level.windowElements.Add(new WindowElement(0, 0, images["Pause_Background"], windowSize));
        }

        public void GameOverMenu(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            Controls.Clear();
            Controls.Add(level.buttons.RecetButton);
            Controls.Add(level.buttons.BackToMainMenuButton);
            level.windowElements.Add(new WindowElement(0, 0, images["Game_Over_BG"], windowSize));
        }
    }
}
