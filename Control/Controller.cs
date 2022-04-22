using System.Collections.Generic;
using static Game.MainForm;
using System.Windows.Forms;
using static Game.Player;

namespace Game
{
    class Controller
    {
        private static List<TargetDirection> targetDirections;
        public static void KeyController(GameModel level)
        {
            targetDirections = new List<TargetDirection>();
            if (pressedKeys.Contains(Keys.Space))
            {
                targetDirections.Add(TargetDirection.Up);
                //pressedKeys.Remove(Keys.Space);
            }
            if (pressedKeys.Contains(Keys.D))
            {
                targetDirections.Add(TargetDirection.Right);
            }
            if (pressedKeys.Contains(Keys.A))
            {
                targetDirections.Add(TargetDirection.Left);
            }
            if (!pressedKeys.Contains(Keys.Space) &&
                !pressedKeys.Contains(Keys.D) &&
                !pressedKeys.Contains(Keys.A))
            {
                targetDirections.Add(TargetDirection.Nowhere);
            }
            level.player.MoveTo(targetDirections);
        }
    }
}
