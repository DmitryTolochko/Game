using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        public readonly Bitmap Choised;
        public Point ActualLocation;
        public Size Size;
        public Label label = new Label();
        public readonly int Cost;

        public ScoreChoice(int SkinNumber, Point location, Bitmap PluggedButton, Bitmap UnpluggedButton, Bitmap ChoisedButton, Size windowSize, string text, int cost)
        {
            this.SkinNumber = SkinNumber;
            Cost = cost;
            ActualLocation = location;
            Unplugged = UnpluggedButton;
            Plugged = PluggedButton;
            Choised = ChoisedButton;

            button.Image = new Bitmap(Unplugged, new Size(
                windowSize.Width * Unplugged.Width / 1960,
                windowSize.Height * Unplugged.Height / 1080));
            button.Location = ActualLocation;
            button.Size = button.Image.Size;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.BackColor = Color.Transparent;
            button.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button.Enabled = false;
            button.Click += (s, args) => IsChosen = true;
            button.Font = new Font("Abril Fatface", 20, FontStyle.Regular);
            button.ForeColor = Color.White;

            label.Text = text;
            label.Location = new Point(button.Location.X, button.Bottom);
            label.BorderStyle = BorderStyle.None;
            label.FlatStyle = FlatStyle.Flat;
            label.BackColor = Color.Transparent;
            label.ForeColor = Color.Black;
            label.Font = new Font("Abril Fatface", 40 * windowSize.Width / 1366, FontStyle.Regular);
            label.Size = new Size(windowSize.Width / 5, windowSize.Height / 10);

            Size = new Size(button.Width, button.Height + label.Height);
        }
    }

    public class Store
    {
        public bool IsFirstFrame = true;
        public List<ScoreChoice> scoreChoices = new List<ScoreChoice>();

        public Store(Size windowSize)
        {
            scoreChoices.Add(new ScoreChoice(1,
                new Point(0, 0),
                Resource1.Character_Opened_00001,
                Resource1.Character_Unopened_00001,
                Resource1.Character_Choised_00001,
                windowSize,
                "Richard",
                1000));
            scoreChoices.Add(new ScoreChoice(2,
                new Point((int)(scoreChoices.Last().ActualLocation.X + scoreChoices.Last().Size.Width * 1.5), 0),
                Resource1.Character_Opened_00002,
                Resource1.Character_Opened_00002,
                Resource1.Character_Choised_00002,
                windowSize,
                "Wilson",
                0));
            scoreChoices.Add(new ScoreChoice(3,
                new Point((int)(scoreChoices.Last().ActualLocation.X + scoreChoices.Last().Size.Width * 1.5), 0),
                Resource1.Character_Opened_00005,
                Resource1.Character_Unopened_00005,
                Resource1.Character_Choised_00005,
                windowSize,
                "Marvin",
                3000));

        }

        public void NextFrame(ControlCollection Controls, Size windowSize, GameModel level, Dictionary<string, Bitmap> images)
        {
            if (IsFirstFrame)
            {
                Controls.Clear();
                Controls.Add(level.buttons.PauseBackToMainMenuButton);
                level.labels.BestCrystalCountLabel.Location = level.labels.Store_BestCrystalCountLocation;
                level.labels.BestCrystalCountLabel.Text = level.BestCrystalCount.ToString();
                Controls.Add(level.labels.BestCrystalCountLabel);
                level.windowElements.Clear();
                level.windowElements.Add(new WindowElement(0, 0, images["Store_BG"], windowSize));
                level.windowElements.Add(new WindowElement(0, 0, images["Crystals"], windowSize));
                foreach (var choice in scoreChoices)
                {
                    choice.label.Font = new Font("Abril Fatface", 40 * windowSize.Width / 1366, FontStyle.Regular);
                    choice.label.Size = new Size(windowSize.Width / 5, windowSize.Height / 10);
                    if (!level.AcquiredSkins.ToString().Contains(choice.SkinNumber.ToString()))
                        choice.button.Text = choice.Cost.ToString();
                    else
                        choice.button.Text = null;
                    Controls.Add(choice.button);
                    Controls.Add(choice.label);
                    choice.IsChosen = false;
                }
            }
            else
            {
                foreach (var choice in scoreChoices)
                {
                    InitializeSkins(choice, level);
                    BuySkin(choice, level);
                    MakeAChoise(choice, level);
                    choice.IsChosen = false;
                }
            }
            IsFirstFrame = false;
        }

        private void BuySkin(ScoreChoice choice, GameModel level)
        {
            if (choice.IsChosen && !choice.IsAcquired)
            {
                choice.button.Image = new Bitmap(choice.Plugged, choice.button.Image.Size);
                level.BestCrystalCount -= choice.Cost;
                level.labels.BestCrystalCountLabel.Text = level.BestCrystalCount.ToString();
                choice.IsAcquired = true;
                level.AcquiredSkins = int.Parse(level.AcquiredSkins.ToString() + choice.SkinNumber.ToString());
                choice.IsChosen = false;
                IsFirstFrame = true;
                MusicPlayer.Play(SoundType.StoreBuy);
                StreamWriter file = new StreamWriter(@"Data.txt");
                file.WriteLine(level.BestScore);
                file.WriteLine(level.BestCrystalCount);
                file.WriteLine(level.SkinNumber);
                file.WriteLine(level.AcquiredSkins);
                file.Close();
            }
        }

        private void InitializeSkins(ScoreChoice choice, GameModel level)
        {
            if (level.AcquiredSkins.ToString().Contains(choice.SkinNumber.ToString()))
            {
                choice.IsAcquired = true;
                if (level.SkinNumber == choice.SkinNumber)
                    choice.button.Image = new Bitmap(choice.Choised, choice.button.Image.Size);
                else
                    choice.button.Image = new Bitmap(choice.Plugged, choice.button.Image.Size);
            }

            if ((level.BestCrystalCount >= choice.Cost && !choice.IsAcquired) || choice.IsAcquired)
                choice.button.Enabled = true;
            else
                choice.button.Enabled = false;
        }

        private void MakeAChoise(ScoreChoice choice, GameModel level)
        {
            if (choice.IsChosen && level.SkinNumber != choice.SkinNumber)
            {
                level.SkinNumber = choice.SkinNumber;
                MusicPlayer.Play(SoundType.StoreChoice);
            }
            IsFirstFrame = true;
        }
    }
}
