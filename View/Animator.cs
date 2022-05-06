using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Game
{
    public class Animator
    {
        private Bitmap[] Animation;
        private int countFrame;

        public Animator(ResourceManager resourceManager)
        {
            var collection = new Dictionary<string, Bitmap>();
            var MyResourceClass = resourceManager;
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                collection.Add(entry.Key.ToString(), (Bitmap)entry.Value);
            }
            Animation = new Bitmap[collection.Count()];
            Animation = collection.OrderBy(y => y.Key).Select(y => y.Value).ToArray();
        }

        public WindowElement AnimatePlayer(Size windowSize, GameModel level)
        {
            if ((countFrame == 0 || countFrame == Animation.Length / 2) && !level.player.IsJumping)
                MusicPlayer.Play(SoundType.Run);
            //else if ((countFrame == 0) && level.player.IsJumping)
            //    MusicPlayer.Play(SoundType.Jump);
            //level.windowElements.Add(new WindowElement(level.player.ActualLocation.X, level.player.ActualLocation.Y,
                //Animation[countFrame], new Size(windowSize.Width*36/100, windowSize.Height*64/100)));
            if (countFrame != Animation.Length - 1)
                countFrame++;
            else
                countFrame = 0;
            return new WindowElement(level.player.ActualLocation.X, level.player.ActualLocation.Y,
                Animation[countFrame], new Size(windowSize.Width * 36 / 100, windowSize.Height * 64 / 100));
        }

        public void Animate(Size windowSize, GameModel level, Diamond diamond)
        {
            level.windowElements.Add(new WindowElement(diamond.ActualLocation.X, diamond.ActualLocation.Y, Animation[countFrame], diamond.Size));
            if (countFrame != Animation.Length - 1)
                countFrame++;
            else
                countFrame = 0;
        }

        public void RestartAnimation()
        {
            countFrame = 0;
        }
    }
}
