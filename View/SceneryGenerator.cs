using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ImageProcessor;

namespace Game
{
    public class SceneryGenerator
    {
        private Size windowSize;
        private double x1;
        private double x2;
        private double x3;
        private double x4;
        private double x5;
        private double transparenсy;
        private bool isADay = true;
        private readonly Random random = new Random();
        private List<Bitmap[]> previousImages;
        private bool isFirstFrame;
        private Dictionary<string, Bitmap> images;
        private readonly List<Bitmap> backgroundSequence = new List<Bitmap>();
        private readonly List<Bitmap> citySequence = new List<Bitmap>();
        private readonly List<List<Bitmap>> clouds = new List<List<Bitmap>>();
        private bool isInstructionNeeded = true;

        public SceneryGenerator(Dictionary<string, Bitmap> images)
        {
            this.images = images;
            for (int i = 0; i < 7; i++)
                clouds.Add(new List<Bitmap>());
            for (int i = 100; i >= 0; i--)
            {
                backgroundSequence.Add(AdjustTransparency(images["Background_Day_time"], i));
                citySequence.Add(AdjustTransparency(images["City_Night_ver"], 100 - i));
            }
            clouds[0].Add(AdjustTransparency(images["Cloud_1"], 0));
            clouds[1].Add(AdjustTransparency(images["Cloud_2"], 0));
            clouds[2].Add(AdjustTransparency(images["Cloud_3"], 0));
            clouds[3].Add(AdjustTransparency(images["Cloud_4"], 0));
            clouds[4].Add(AdjustTransparency(images["Cloud_5"], 0));
            clouds[5].Add(AdjustTransparency(images["Cloud_6"], 0));
            clouds[6].Add(AdjustTransparency(images["Cloud_7"], 0));
            clouds[0].Add(AdjustTransparency(images["Cloud_1"], 100));
            clouds[1].Add(AdjustTransparency(images["Cloud_2"], 100));
            clouds[2].Add(AdjustTransparency(images["Cloud_3"], 100));
            clouds[3].Add(AdjustTransparency(images["Cloud_4"], 100));
            clouds[4].Add(AdjustTransparency(images["Cloud_5"], 100));
            clouds[5].Add(AdjustTransparency(images["Cloud_6"], 100));
            clouds[6].Add(AdjustTransparency(images["Cloud_7"], 100));
            ResetScenery();
        }

        public void UpdateScenery(Size windowSize, GameModel level, 
            bool isFirstFrame, Player player, List<Obstacle> obstacles, List<Diamond> crystals)
        {
            this.windowSize = windowSize;
            this.isFirstFrame = isFirstFrame;
            level.windowElements.Clear();
            UpdateSky(level);
            AddElement(level, new List<Bitmap> 
            { 
                images["_1_Layer_Trees_ver1"], 
                images["_1_Layer_Trees_ver2"] 
            }, 
            x1, 0, windowSize, previousImages[0]);
            AddElement(level, new List<Bitmap> 
            { 
                images["_2_Layer_Trees_ver1"], 
                images["_2_Layer_Trees_ver2"], 
                images["_2_Layer_Trees_ver3"] 
            }, 
            x2, 0, windowSize, previousImages[1]);
            AddElement(level, new List<Bitmap> 
            { 
                images["Front_Bushes_1"], 
                images["Front_Bushes_2"] 
            }, 
            x3, 0, windowSize, previousImages[2]);
            AddElement(level, new List<Bitmap> 
            { 
                images["Road"], 
                images["Road"] 
            }, 
            x4, 0, windowSize, previousImages[3]);
            if (x2 > -windowSize.Width && isInstructionNeeded)
                AddElement(level, images["Instruction"], x2, 0);
            else
                isInstructionNeeded = false;

            UpdateObstacles(level, obstacles);
            UpdateCrystals(level, crystals);
            UpdatePlayer(level, player);
            AddElement(level, images["Crystals_count"], 0, 0);
            RecalculateImagesPositions(level.Acceleration);
        }

        public void ResetScenery()
        {
            x1 = 0;
            x2 = 0;
            x3 = 0;
            x4 = 0;
            x5 = 0;
            transparenсy = 100;
            isADay = true;
            previousImages = new List<Bitmap[]>
            {
                new Bitmap[2],
                new Bitmap[2],
                new Bitmap[2],
                new Bitmap[2],
                new Bitmap[2]
            };
        }

        private void UpdateObstacles(GameModel level, List<Obstacle> obstacles)
        {
            foreach (var obstacle in obstacles)
            {
                AddElement(level, obstacle.Image, obstacle.ActualLocation, obstacle.Size);
                level.windowElements.Last().rectangle = obstacle.WorkSpace;
            }
        }

        private void UpdatePlayer(GameModel level, Player player)
        {
            WindowElement playerAnim = null;
            var count = (int)level.Acceleration <= 1 ? 1 : (int)level.Acceleration;
            for (int i = 0; i < count; i++)
            {
                if (player.IsCollised)
                    playerAnim = player.DriftAnimation.AnimatePlayer(windowSize, level);
                else if (player.IsJumping)
                    playerAnim = player.JumpAnimation.AnimatePlayer(windowSize, level);
                else
                    playerAnim = player.RunAnimation.AnimatePlayer(windowSize, level);
            }
            level.windowElements.Add(playerAnim);
            level.windowElements.Last().rectangle = player.WorkSpace;
        }

        private void UpdateCrystals(GameModel level, List<Diamond> crystals)
        {
            foreach (var crystal in crystals)
                crystal.Animator.AnimateDiamond(windowSize, level, crystal);
        }

        private void UpdateSky(GameModel level)
        {
            AddElement(level, images["Background_Night_time"], 0, 0);
            AddElement(level, backgroundSequence[(int)transparenсy], 0, 0);
            var cl = new List<Bitmap>();
            foreach (var cloud in clouds)
            {
                if (transparenсy < 50)
                    cl.Add(cloud[1]);
                else
                    cl.Add(cloud[0]);
            }
            AddElement(level, cl, x3, 0, new Size(cl[0].Size.Width*3*level.windowSize.Width/1366, cl[0].Size.Height*3*level.windowSize.Height/768), previousImages[4]);
            AddElement(level, images["City_Day_ver"], 0, 0);
            AddElement(level, citySequence[(int)transparenсy], 0, 0);
        }

        private Bitmap AdjustTransparency(Bitmap input, int transparency)
        {
            var outStream = new MemoryStream();
            var imageFactory = new ImageFactory(preserveExifData: true);
            imageFactory.Load(input)
                        .Alpha(transparency)
                        .Save(outStream);
            return new Bitmap(outStream);
        }

        private void RecalculateImagesPositions(double acceleration)
        {
            if (x1 >= -windowSize.Width)
                x1 -= windowSize.Width * acceleration / 500;
            else
                x1 = 0;
            if (x2 >= -windowSize.Width)
                x2 -= windowSize.Width * acceleration / 300;
            else
                x2 = 0;
            if (x3 >= -windowSize.Width)
                x3 -= windowSize.Width * acceleration / 200;
            else
                x3 = 0;
            if (x4 >= -windowSize.Width)
                x4 -= windowSize.Width * acceleration / 100;
            else
                x4 = 0;
            if (x5 >= -windowSize.Width)
                x5 -= windowSize.Width * acceleration / 700;
            else
                x5 = windowSize.Width;

            if (transparenсy > 0 && isADay)
            {
                transparenсy -= 0.1;
            }
            else if (transparenсy < 100 && !isADay)
            {
                transparenсy += 0.1;
            }
            else
            {
                isADay = isADay ? false : true;
            }
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
            if (isFirstFrame)
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
                    if (!isFirstFrame)
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
