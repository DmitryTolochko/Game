using System.Collections.Generic;
using static Game.MainForm;
using System.Windows.Forms;
using static Game.Player;

namespace Game
{
    class Controller
    {
        private static HashSet<TargetDirection> targetDirections;
        public static void KeyController(GameModel level)
        {
            targetDirections = new HashSet<TargetDirection>();
            if (pressedKeys.Contains(Keys.Space))
                targetDirections.Add(TargetDirection.Up);
            if (pressedKeys.Contains(Keys.D))
                targetDirections.Add(TargetDirection.Right);
            if (pressedKeys.Contains(Keys.A))
                targetDirections.Add(TargetDirection.Left);
            level.player.MoveTo(targetDirections, level.windowSize);
            if (pressedKeys.Contains(Keys.Escape))
                level.IsGamePaused = true;
        }
    }
}
