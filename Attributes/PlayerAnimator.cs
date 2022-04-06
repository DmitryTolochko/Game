using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class PlayerAnimator
    {
        private Dictionary<string,Image> Animation = new Dictionary<string, Image>();
        private int countFrame = 0;

        public void AnimatePlayer(Size windowSize, Level level)
        {
            if (Animation.Count() < 1)
                GetImages();
            level.windowElements.Add(new WindowElement(-120, 2*windowSize.Height/5, Animation.ElementAt(countFrame).Value, new Size(windowSize.Width*36/100, windowSize.Height*64/100)));
            if (countFrame != 34)
                countFrame++;
            else
                countFrame = 0;
        }

        private void GetImages()
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(Resource2));
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                Animation.Add(entry.Key.ToString(), (Image)entry.Value);
            }
            Animation = Animation.OrderBy(y => y.Key).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
