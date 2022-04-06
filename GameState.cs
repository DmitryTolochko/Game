using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    class GameState
    {
        public Dictionary<string, Image> images = new Dictionary<string, Image>();
        public Level level = new Level();

        private Size windowSize;
        private SceneryGenerator sceneryGenerator = new SceneryGenerator();
        private PlayerAnimator playerAnimator = new PlayerAnimator();
        private bool isGameStarted = false;
        private bool isFisrtFrame = true;
        private Buttons buttons;

        public void NextFrame(Size windowSize, TableLayoutPanel controls)
        {
            this.windowSize = windowSize;
            level.windowElements.Clear();
            if (!isGameStarted)
                MainMenu(controls);
            else if (isGameStarted)
                GameProcess(controls);
        }

        private void GameProcess(TableLayoutPanel controls)
        {
            sceneryGenerator.UpdateScenery(windowSize, level, images, isFisrtFrame, playerAnimator);
            //playerAnimator.AnimatePlayer(windowSize, level, images, isFisrtFrame);
            isFisrtFrame = false;
            controls.Controls.Add(buttons.PauseButton);
        }

        private void MainMenu(TableLayoutPanel controls)
        {
            //controls.Controls.Add(buttons.ExitButton);
            controls.Controls.Add(buttons.PlayButton);
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Background"], windowSize));
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Background"], windowSize));
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Light_flash_1"], windowSize));
            level.windowElements.Add(new WindowElement(0, 0, images["Menu_Name"], windowSize));
        }

        public void CreateButtons(TableLayoutPanel controls)
        {
            buttons = new Buttons();
            buttons.PauseButton = new CustomButton(new Point(500, 800), images["Game_pause_button_up"]);
            buttons.PauseButton.Click += (sender, args) =>
            {
                buttons.PauseButton.Image = images["Game_pause_button_down"];
                Application.Exit();
            };

            buttons.ExitButton = new CustomButton(new Point(500, 800), images["Menu_Exit_button_up"]);
            buttons.ExitButton.Click += (sender, args) =>
            {
                buttons.ExitButton.Image = images["Menu_Exit_button_down"];
                Application.Exit();
            };

            buttons.PlayButton = new CustomButton(new Point(windowSize.Width, windowSize.Height), Resource1.Menu_Play_button_up);
            buttons.PlayButton.Click += (sender, args) =>
            {
                buttons.PlayButton.Image = images["Menu_Play_button_down"];
                
                level = new Level();
                isGameStarted = true;
                GameProcess(controls);
            };
        }
    }
}
