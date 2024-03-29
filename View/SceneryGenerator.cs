﻿using System;
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
        private double[] speeds = new double[5];
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
            var cloudImages = new Queue<Bitmap>();
            foreach (var image in images)
            {
                if (image.Key.Contains("Cloud"))
                    cloudImages.Enqueue(image.Value);
            }
            foreach (var cloud in clouds)
            {
                var image = cloudImages.Dequeue();
                cloud.Add(AdjustTransparency(image, 0));
                cloud.Add(AdjustTransparency(image, 100));
            }
            ResetScenery();
        }

        public void UpdateScenery(Size windowSize, GameModel level,
            bool isFirstFrame, Player player, Queue<Obstacle> obstacles, Queue<Diamond> crystals)
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
            speeds[0], 0, windowSize, previousImages[0]);
            AddElement(level, new List<Bitmap>
            {
                images["_2_Layer_Trees_ver1"],
                images["_2_Layer_Trees_ver2"],
                images["_2_Layer_Trees_ver3"]
            },
            speeds[1], 0, windowSize, previousImages[1]);
            AddElement(level, new List<Bitmap>
            {
                images["Front_Bushes_1"],
                images["Front_Bushes_2"]
            },
            speeds[2], 0, windowSize, previousImages[2]);
            AddElement(level, new List<Bitmap>
            {
                images["Road"],
                images["Road"]
            },
            speeds[3], 0, windowSize, previousImages[3]);
            if (speeds[1] > -windowSize.Width && isInstructionNeeded)
                AddElement(level, images["Instruction"], speeds[1], 0);
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
            for (int i = 0; i < 5; i++)
                speeds[i] = 0;
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

        private void UpdateObstacles(GameModel level, Queue<Obstacle> obstacles)
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

                if ((player.IsCollised || player.IsJumping) && i > 0)
                    break;
            }
            level.windowElements.Add(playerAnim);
            level.windowElements.Last().rectangle = player.WorkSpace;
        }

        private void UpdateCrystals(GameModel level, Queue<Diamond> crystals)
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
            AddElement(level, cl, speeds[4], 0, new Size(cl[0].Size.Width * 3 * level.windowSize.Width / 1366, cl[0].Size.Height * 3 * level.windowSize.Height / 768), previousImages[4]);
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
            var koefs = new int[] { 500, 300, 200, 100, 700 };
            for (int i = 0; i < 5; i++)
            {
                if (speeds[i] >= -windowSize.Width)
                    speeds[i] -= windowSize.Width * acceleration / koefs[i];
                else
                    speeds[i] = 0;
            }

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
                isADay = !isADay;
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
