using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Resources;

namespace Game
{
    class ObstacleGenerator
    {
        private readonly Random random = new Random();
        private readonly List<Bitmap> images = new List<Bitmap>();
        private readonly List<string> imagesNames = new List<string>();

        public void GetNewObstacle(List<Obstacle> obstacles, Size windowSize)
        {
            if (images.Count == 0)
                GetImages();
            var index = random.Next() % images.Count;
            obstacles.Add(new Obstacle(windowSize, images[index], imagesNames[index]));
            
        }

        private void GetImages()
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(Resource3));
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                images.Add((Bitmap)entry.Value);
                imagesNames.Add((string)entry.Key);
            }
        }
    }
}
