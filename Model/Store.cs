using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace Game
{
    public class ScoreChoice
    {
        public int SkinNumber;
        public bool IsAcquired = false;
        public bool IsChosen = false;
        public bool IsPlugged = false;
        public Button button = new Button();
        public readonly Bitmap Unplugged;
        public readonly Bitmap Plugged;
        public Point ActualLocation;
        public Size Size;
        public Label label = new Label();
        public readonly int Cost;

        public ScoreChoice(int SkinNumber, Point location, Bitmap PluggedButton, Bitmap UnpluggedButton, Size windowSize, string text, int cost)
        {
            this.SkinNumber = SkinNumber;
            Cost = cost;
            ActualLocation = location;
            Unplugged = UnpluggedButton;
            Plugged = PluggedButton;
            
            button.Image = new Bitmap(Unplugged, new Size(
                windowSize.Width * Unplugged.Width / 1960,
                windowSize.Height * Unplugged.Height / 1080));
            button.Location = ActualLocation;
            button.Size = button.Image.Size;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Color.Transparent;
            button.Enabled = false;
            button.Click += (s, args) => IsChosen = true;
            button.Text = cost.ToString();

            label.Text = text;
            label.Location = new Point(button.Location.X, button.Bottom);
            label.BorderStyle = BorderStyle.None;
            label.FlatStyle = FlatStyle.Flat;
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.Black;
            label.Font = new Font("Abril Fatface", 40, FontStyle.Regular);
            label.Size = new Size(windowSize.Width / 5, windowSize.Height / 10);

            Size = new Size(button.Width, button.Height + label.Height);
        }

        public void ChangeSize(Size windowSize)
        {
            button.Image = new Bitmap(button.Image, new Size(
                windowSize.Width * Unplugged.Width / 1960,
                windowSize.Height * Unplugged.Height / 1080));
            button.Size = button.Image.Size;
            Size = new Size(button.Width, button.Height + label.Height);
        }
    }

    public class Store
    {
        public bool IsFirstFrame = true;
        public List<ScoreChoice> scoreChoices = new List<ScoreChoice>();

        public Store(Size windowSize)
        {
            scoreChoices.Add(new ScoreChoice(1, new Point(0, 0), Resource1.Character_Opened_00001, Resource1.Character_Unopened_00001, windowSize, "Richard" , 100));
            scoreChoices.Add(new ScoreChoice(2, new Point(scoreChoices.Last().ActualLocation.X  + scoreChoices.Last().Size.Width, 0), Resource1.Character_Opened_00002, Resource1.Character_Opened_00002, windowSize, "Wilson", 0));
            scoreChoices.Add(new ScoreChoice(3, new Point(scoreChoices.Last().ActualLocation.X + scoreChoices.Last().Size.Width, 0), Resource1.Character_Opened_00003, Resource1.Character_Unopened_00003, windowSize, "Mom", 1000));
            scoreChoices.Add(new ScoreChoice(4, new Point(scoreChoices[0].ActualLocation.X, scoreChoices.Last().ActualLocation.Y + scoreChoices.Last().Size.Height), Resource1.Character_Opened_00004, Resource1.Character_Unopened_00004, windowSize, "Butler", 1000));
            scoreChoices.Add(new ScoreChoice(5, new Point(scoreChoices[1].ActualLocation.X, scoreChoices.Last().ActualLocation.Y), Resource1.Character_Opened_00005, Resource1.Character_Unopened_00005, windowSize, "Marvin", 1000));

        }

        public void NextFrame(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            if (IsFirstFrame)
            {
                level.windowElements.Clear();
                Controls.Clear();
                Controls.Add(level.buttons.PauseBackToMainMenuButton);
                foreach (var choice in scoreChoices)
                {
                    Controls.Add(choice.button);
                    Controls.Add(choice.label);
                    if (level.AcquiredSkins.ToString().Contains(choice.SkinNumber.ToString()))
                    {
                        choice.IsAcquired = true;
                        choice.button.Image = new Bitmap(choice.Plugged, choice.button.Image.Size);
                    }
                    //if (choice.IsChosen)
                    //    choice.IsPlugged = true;

                    if ((level.BestCrystalCount >= choice.Cost && !choice.IsAcquired) || choice.IsAcquired)
                        choice.button.Enabled = true;
                    else
                        choice.button.Enabled = false;

                }
                level.windowElements.Add(new WindowElement(0, 0, images["Store_BG"], windowSize));
                level.windowElements.Add(new WindowElement(0, 0, images["Crystals"], windowSize));
            }

            IsFirstFrame = false;
            foreach (var choice in scoreChoices)
            {
                choice.ChangeSize(windowSize);
                if (choice.IsChosen && !choice.IsAcquired)
                {
                    choice.button.Image = new Bitmap(choice.Plugged, choice.button.Image.Size);
                    level.BestCrystalCount -= choice.Cost;
                    choice.IsAcquired = true;
                    level.AcquiredSkins = int.Parse(level.AcquiredSkins.ToString() + choice.SkinNumber.ToString());
                    choice.IsChosen = false;
                }
                if (choice.IsChosen && level.SkinNumber != choice.SkinNumber)
                {
                    level.SkinNumber = choice.SkinNumber;
                }
                choice.IsChosen = false;
            }
        }
    }
}
