using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Game
{
    public class PlayerAnimator
    {
        private Bitmap[] Animation;
        private int countFrame;

        public PlayerAnimator()
        {
            var collection = new Dictionary<string, Bitmap>();
            ResourceManager MyResourceClass = new ResourceManager(typeof(Resource2));
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                collection.Add(entry.Key.ToString(), (Bitmap)entry.Value);
            }
            Animation = new Bitmap[collection.Count()];
            Animation = collection.OrderBy(y => y.Key).Select(y => y.Value).ToArray();
        }

        public void AnimatePlayer(Size windowSize, GameModel level)
        {
            level.windowElements.Add(new WindowElement(level.player.ActualLocation.X, level.player.ActualLocation.Y,
                Animation[countFrame], new Size(windowSize.Width*36/100, windowSize.Height*64/100)));
            if (countFrame != 34)
                countFrame++;
            else
                countFrame = 0;
        }
    }
}
