using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Game
{
    class SceneryGenerator
    {
        private Size windowSize;
        private double x1 = 0;
        private double x2 = 0;
        private double x3 = 0;
        Random random = new Random();
        private List<Image[]> previousImages = new List<Image[]>();
        private bool IsFisrtFrame;

        public void UpdateScenery(Size windowSize, Level level, Dictionary<string, Image> images, bool IsFisrtFrame, PlayerAnimator playerAnimator)
        {
            this.windowSize = windowSize;
            this.IsFisrtFrame = IsFisrtFrame;
            level.windowElements.Clear();
            if (previousImages.Count() == 0)
            {
                previousImages.Add(new Image[2]);
                previousImages.Add(new Image[2]);
                previousImages.Add(new Image[2]);
            }
            AddElement(level, images["Background_Night_time"], 0);
            AddElement(level, images["City_Night_ver"], 0);
            AddElement(level, images["_1_Layer_Trees_ver1"], images["_1_Layer_Trees_ver2"], x3, 0, windowSize, previousImages[0]);
            AddElement(level, images["_2_Layer_Trees_ver1"], images["_2_Layer_Trees_ver2"], x1, 0, windowSize, previousImages[1]);
            playerAnimator.AnimatePlayer(windowSize, level);
            AddElement(level, images["Front_Bushes_1"], images["Front_Bushes_2"], x2, 0, windowSize, previousImages[2]);
            if (x1 >= -windowSize.Width)
                x1 -= windowSize.Width / 300;
            else
                x1 = 0;
            if (x2 >= -windowSize.Width)
                x2 -= windowSize.Width / 100;
            else
                x2 = 0;
            if (x3 >= -windowSize.Width)
                x3 -= windowSize.Width / 500;
            else
                x3 = 0;
        }

        private void AddElement(Level level, Image image, double xPos)
        {
            level.windowElements.Add(new WindowElement(xPos, 0, image, windowSize));
            //if (level.windowElements.Last().xPosition + level.windowElements.Last().Image.Width <= windowSize.Width)
            //    level.windowElements.Add(new WindowElement(xPos + windowSize.Width, 0, image, windowSize));
        }

        private void AddElement(Level level, Image image1, Image image2, double xPos, double yPos, Size size, Image[] previousImages)
        {
            Image image = null;
            if (random.Next() % 2 == 0)
                image = image1;
            else
                image = image2;

            if (IsFisrtFrame)
            {
                level.windowElements.Add(new WindowElement(xPos, yPos, image1, size));
                previousImages[0] = image1;
            }
            else if (xPos == 0)
            {
                level.windowElements.Add(new WindowElement(xPos, yPos, previousImages[1], size));
            }
            else
            {
                level.windowElements.Add(new WindowElement(xPos, yPos, previousImages[0], size));
            }

            if (xPos <= 0)
            {
                if (xPos == 0)
                {
                    if (!IsFisrtFrame)
                        previousImages[0] = previousImages[1];
                    level.windowElements.Add(new WindowElement(xPos + windowSize.Width, yPos, image, size));
                    previousImages[1] = image;
                }
                else
                {
                    level.windowElements.Add(new WindowElement(xPos + windowSize.Width, yPos, previousImages[1], size));
                }
            }
        }
    }
}
