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
        private double x4 = 0;
        private Random random = new Random();
        private List<Bitmap[]> previousImages = new List<Bitmap[]>();
        private bool IsFisrtFrame;
        

        public void UpdateScenery(Size windowSize, GameModel level, Dictionary<string, Bitmap> images, bool IsFisrtFrame, Player player, List<Obstacle> obstacles, List<Diamond> diamonds)
        {
            this.windowSize = windowSize;
            this.IsFisrtFrame = IsFisrtFrame;
            level.windowElements.Clear();
            if (previousImages.Count() == 0)
            {
                previousImages.Add(new Bitmap[2]);
                previousImages.Add(new Bitmap[2]);
                previousImages.Add(new Bitmap[2]);
                previousImages.Add(new Bitmap[2]);
            }
            AddElement(level, images["Background_Night_time"], 0, 0);
            AddElement(level, images["City_Night_ver"], 0, 0);
            AddElement(level, new List<Bitmap> { images["_1_Layer_Trees_ver1"], images["_1_Layer_Trees_ver2"] }, x1, 0, windowSize, previousImages[0]);
            AddElement(level, new List<Bitmap> { images["_2_Layer_Trees_ver1"], images["_2_Layer_Trees_ver2"], images["_2_Layer_Trees_ver3"] }, x2, 0, windowSize, previousImages[1]);
            AddElement(level, new List<Bitmap> { images["Front_Bushes_1"], images["Front_Bushes_2"] }, x3, 0, windowSize, previousImages[2]);
            AddElement(level, new List<Bitmap> { images["Road"], images["Road"] }, x4, 0, windowSize, previousImages[3]);

            foreach (var obstacle in obstacles)
            {
                AddElement(level, obstacle.Image, obstacle.ActualLocation, obstacle.Size);
                level.windowElements.Last().rectangle = obstacle.WorkSpace;
            }
            foreach (var diamond in diamonds)
                diamond.Animator.Animate(windowSize, level, diamond);
            WindowElement playerAnim = null;
            for (int i = 0; i < (int)level.Acceleration; i++)
            {
                if (player.IsJumping)
                    playerAnim = player.JumpAnimation.AnimatePlayer(windowSize, level);
                else
                    playerAnim = player.RunAnimation.AnimatePlayer(windowSize, level);
            }
            level.windowElements.Add(playerAnim);
            level.windowElements.Last().rectangle = player.WorkSpace;
            AddElement(level, images["Crystals_count"], 0, 0);
            RecalculateImagesPositions(level.Acceleration);
        }

        private void RecalculateImagesPositions(double acceleration)
        {
            if (x1 >= -windowSize.Width)
                x1 -= windowSize.Width * (int)acceleration / 500;
            else
                x1 = 0;
            if (x2 >= -windowSize.Width)
                x2 -= windowSize.Width * (int)acceleration / 300;
            else
                x2 = 0;
            if (x3 >= -windowSize.Width)
                x3 -= windowSize.Width * (int)acceleration / 200;
            else
                x3 = 0;
            if (x4 >= -windowSize.Width)
                x4 -= windowSize.Width * (int)acceleration / 100;
            else
                x4 = 0;
        }

        private void AddElement(GameModel level, Bitmap image, double dx, double dy)
        {
            level.windowElements.Add(new WindowElement(dx, dy, image, windowSize));
        }

        private void AddElement(GameModel level, Bitmap image, Point location, Size size)
        {
            level.windowElements.Add(new WindowElement(location.X, location.Y, image, size));
        }

        private void AddElement(GameModel level, List<Bitmap> images, double xPos, double yPos, Size size, Bitmap[] previousImages)
        {
            var image = images[random.Next() % images.Count];
            if (IsFisrtFrame)
            {
                level.windowElements.Add(new WindowElement(xPos, yPos, images[0], size));
                previousImages[0] = images[0];
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
