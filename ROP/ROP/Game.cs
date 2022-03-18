using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace ROP
{
    public partial class Game : Form
    {
        SoundPlayer crunch = new SoundPlayer();

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;
        int score;

        bool mov;

        Random rand = new Random();

        bool goLeft, goRight, goDown, goUp;

        int min = 0, sek = 0;

        public Game()
        {
            InitializeComponent();

            new Settings();

            crunch.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "Sound/Crunch_sound.wav";

            mov = FormProvider.MainMenu.Mov;
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            FormProvider.MainMenu.Show();
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (mov)
            {
                if (e.KeyCode == Keys.Left && Settings.directions != "right")
                    goLeft = true;
                else if (e.KeyCode == Keys.Right && Settings.directions != "left")
                    goRight = true;
                else if (e.KeyCode == Keys.Up && Settings.directions != "down")
                    goUp = true;
                else if (e.KeyCode == Keys.Down && Settings.directions != "up")
                    goDown = true;
            }
            else
            {
                if (e.KeyCode == Keys.A && Settings.directions != "right")
                    goLeft = true;
                else if (e.KeyCode == Keys.D && Settings.directions != "left")
                    goRight = true;
                else if (e.KeyCode == Keys.W && Settings.directions != "down")
                    goUp = true;
                else if (e.KeyCode == Keys.S && Settings.directions != "up")
                    goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (mov)
            {
                if (e.KeyCode == Keys.Left)
                    goLeft = false;
                else if (e.KeyCode == Keys.Right)
                    goRight = false;
                else if (e.KeyCode == Keys.Up)
                    goUp = false;
                else if (e.KeyCode == Keys.Down)
                    goDown = false;
            }
            else
            {
                if (e.KeyCode == Keys.A)
                    goLeft = false;
                else if (e.KeyCode == Keys.D)
                    goRight = false;
                else if (e.KeyCode == Keys.W)
                    goUp = false;
                else if (e.KeyCode == Keys.S)
                    goDown = false;
            }
            if (e.KeyCode == Keys.F1 && !startButton.Enabled)
                GamePause();
        }

        private void Game_Load(object sender, EventArgs e)
        {
            int diff = FormProvider.MainMenu.Diff;
            if(diff==1)
            {
                Size = new Size(624, 565);
                picCanvas.Size = new Size(400, 500);
                picCanvas.Location = new Point(14, 12);
                startButton.Location = new Point(460, 47);
                scoreSave.Location = new Point(460, 297);
                txtScore.Location = new Point(466, 172);
                time.Location = new Point(451, 221);
                gameTimer.Interval = 100;
                Text = "Easy";
                label1.Location = new Point(469, 490);
            }
            else if(diff==2)
            {
                Size = new Size(697, 648);
                picCanvas.Size = new Size(480, 580);
                picCanvas.Location = new Point(12, 12);
                startButton.Location = new Point(534, 49);
                scoreSave.Location = new Point(534, 299);
                txtScore.Location = new Point(540, 174);
                time.Location = new Point(525, 223);
                gameTimer.Interval = 85;
                Text = "Medium";
                label1.Location = new Point(543, 570);
            }
            else
            {
                Size = new Size(778, 743);
                picCanvas.Size = new Size(580, 680);
                picCanvas.Location = new Point(12, 12);
                startButton.Location = new Point(617, 46);
                scoreSave.Location = new Point(617, 296);
                txtScore.Location = new Point(623, 171);
                time.Location = new Point(608, 220);
                gameTimer.Interval = 75;
                Text = "Hard";
                label1.Location = new Point(626, 670);
            }
            label1.Visible = false;
        }

        private Image GetTexture(int i, Image newImage)
        {
            if (i == 0)
            {
                switch (Settings.directions)
                {
                    case "left":
                        newImage = Image.FromFile("Textures/Body/Head_Left.png");
                        break;
                    case "right":
                        newImage = Image.FromFile("Textures/Body/Head_Right.png");
                        break;
                    case "up":
                        newImage = Image.FromFile("Textures/Body/Head_Top.png");
                        break;
                    case "down":
                        newImage = Image.FromFile("Textures/Body/Head_Bottom.png");
                        break;
                }
            }
            else if (i == Snake.Count - 1)
            {
                if (Snake[i - 1].Y == maxHeight && Snake[i].Y == 0)
                    newImage = Image.FromFile("Textures/Body/Tail_Top.png");
                else if (Snake[i].Y == maxHeight && Snake[i - 1].Y == 0)
                    newImage = Image.FromFile("Textures/Body/Tail_Bottom.png");
                else if (Snake[i].X == maxWidth && Snake[i - 1].X == 0)
                    newImage = Image.FromFile("Textures/Body/Tail_Right.png");
                else if (Snake[i].X == 0 && Snake[i - 1].X == maxWidth)
                    newImage = Image.FromFile("Textures/Body/Tail_Left.png");
                else if (Snake[i].X > Snake[i - 1].X)
                    newImage = Image.FromFile("Textures/Body/Tail_Left.png");
                else if (Snake[i].X < Snake[i - 1].X)
                    newImage = Image.FromFile("Textures/Body/Tail_Right.png");
                else if (Snake[i].Y < Snake[i - 1].Y)
                    newImage = Image.FromFile("Textures/Body/Tail_Bottom.png");
                else
                    newImage = Image.FromFile("Textures/Body/Tail_Top.png");
            }
            else
            {
                bool changed = false;
                if (Snake[i - 1].Y == maxHeight && Snake[i].Y == 0) 
                {
                    if(Snake[i + 1].X>Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopRight.png");
                    }
                    else if(Snake[i+1].X<Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopLeft.png");
                    }
                }
                else if (Snake[i].Y == maxHeight && Snake[i-1].Y == 0)
                {
                    if (Snake[i + 1].X > Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotRight.png");
                    }
                    else if (Snake[i + 1].X < Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotLeft.png");
                    }
                }
                else if (Snake[i].Y == maxHeight && Snake[i + 1].Y == 0)
                {
                    if (Snake[i - 1].X > Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotRight.png");
                    }
                    else if (Snake[i - 1].X < Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotLeft.png");
                    }
                }
                else if (Snake[i + 1].Y == maxHeight && Snake[i].Y == 0)
                {
                    if (Snake[i - 1].X > Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopRight.png");
                    }
                    else if (Snake[i - 1].X < Snake[i].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopLeft.png");
                    }
                }
                else if (Snake[i].X == maxWidth && Snake[i - 1].X == 0)
                {
                    if (Snake[i + 1].Y > Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotRight.png");
                    }
                    else if (Snake[i + 1].Y < Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopRight.png");
                    }
                }
                else if (Snake[i].X == 0 && Snake[i - 1].X == maxWidth)
                {
                    if (Snake[i + 1].Y > Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotLeft.png");
                    }
                    else if (Snake[i + 1].Y < Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopLeft.png");
                    }
                }
                else if (Snake[i].X == 0 && Snake[i + 1].X == maxWidth)
                {
                    if (Snake[i - 1].Y > Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotLeft.png");
                    }
                    else if (Snake[i - 1].Y < Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopLeft.png");
                    }
                }
                else if (Snake[i].X == maxWidth && Snake[i + 1].X == 0)
                {
                    if (Snake[i - 1].Y > Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotRight.png");
                    }
                    else if (Snake[i - 1].Y < Snake[i].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopRight.png");
                    }
                }
                else if (Snake[i].X < Snake[i - 1].X)
                {
                    if (Snake[i].Y < Snake[i + 1].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotRight.png");
                    }
                    else if (Snake[i].Y > Snake[i + 1].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopRight.png");
                    }
                }
                else if (Snake[i].X > Snake[i - 1].X)
                {
                    if (Snake[i].Y < Snake[i + 1].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotLeft.png");
                    }
                    else if (Snake[i].Y > Snake[i + 1].Y)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopLeft.png");
                    }
                }
                else if (Snake[i].Y < Snake[i - 1].Y)
                {
                    if (Snake[i].X < Snake[i + 1].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotRight.png");
                    }
                    else if (Snake[i].X > Snake[i + 1].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_BotLeft.png");
                    }
                }
                else if (Snake[i].Y > Snake[i - 1].Y)
                {
                    if (Snake[i].X < Snake[i + 1].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopRight.png");
                    }
                    else if (Snake[i].X > Snake[i + 1].X)
                    {
                        changed = true;
                        newImage = Image.FromFile("Textures/Body/Corner_TopLeft.png");
                    }
                }
                if (!changed)
                {
                    if (Snake[i].X > Snake[i - 1].X)
                        newImage = Image.FromFile("Textures/Body/Body_Left.png");
                    else if (Snake[i].X < Snake[i - 1].X)
                        newImage = Image.FromFile("Textures/Body/Body_Right.png");
                    else if (Snake[i].Y < Snake[i - 1].Y)
                        newImage = Image.FromFile("Textures/Body/Body_Bottom.png");
                    else
                        newImage = Image.FromFile("Textures/Body/Body_Top.png");
                }
            }
            return newImage;
        }

        private void UpdatePictureBox(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Image newImage = Image.FromFile("Textures/Body/Body_Top.png");
            for(int i = 0; i < Snake.Count; i++)
            {
                newImage = GetTexture(i, newImage);
                canvas.DrawImage(newImage,
                        Snake[i].X * Settings.Width,
                        Snake[i].Y * Settings.Height,
                        Settings.Width, Settings.Height
                    );
            }
            newImage = Image.FromFile("Textures/Food.png");
            canvas.DrawImage(newImage, 
                        food.X * Settings.Width,
                        food.Y * Settings.Height,
                        Settings.Width, Settings.Height
                    );
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
            label1.Visible = true;
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            if (goLeft)
            {
                Settings.directions = "left";
            }
            else if (goRight)
            {
                Settings.directions = "right";
            }
            else if (goUp)
            {
                Settings.directions = "up";
            }
            else if (goDown)
            {
                Settings.directions = "down";
            }
            if (Snake[0].X == food.X && Snake[0].Y == food.Y)
            {
                EatFood();
            }
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].X--;
                            break;
                        case "right":
                            Snake[i].X++;
                            break;
                        case "up":
                            Snake[i].Y--;
                            break;
                        case "down":
                            Snake[i].Y++;
                            break;
                    }

                    if (Snake[i].X < 0)
                    {
                        Snake[i].X = maxWidth;
                    }
                    else if (Snake[i].X > maxWidth)
                    {
                        Snake[i].X = 0;
                    }
                    if (Snake[i].Y < 0)
                    {
                        Snake[i].Y = maxHeight;
                    }
                    else if (Snake[i].Y > maxHeight)
                    {
                        Snake[i].Y = 0;
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X && Snake[i].Y == Snake[j].Y)
                            GameOver();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }

            picCanvas.Invalidate();
        }

        private void RestartGame()
        {
            scoreSave.Visible = false;

            min = 0; 
            sek = 0;
            time.Text = string.Format("Time: {0}:0{1}", min, sek);

            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Snake.Clear();
            startButton.Enabled = false;

            score = 0;
            txtScore.Text = "Score: " + score;

            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            int x = rand.Next(2, maxWidth);
            int y = rand.Next(2, maxHeight);
            while (x != 10 && y != 5)
            {
                x = rand.Next(2, maxWidth);
                y = rand.Next(2, maxHeight);
            }
            food = new Circle { X = x, Y = y };

            timeTimer.Start();
            gameTimer.Start();
        }

        private void scoreSave_Click(object sender, EventArgs e)
        {
            Save save = new Save();
            if(save.ShowDialog()==DialogResult.OK)
            {
                int diff = FormProvider.MainMenu.Diff;
                string difficulty;
                if (diff == 1)
                    difficulty = "Easy";
                else if (diff == 2)
                    difficulty = "Medium";
                else
                    difficulty = "Hard";
                string time;
                if (sek < 10)
                    time = min + ":0" + sek;
                else
                    time = min + ":" + sek;
                FileStream fs = new FileStream("Score.dat", FileMode.OpenOrCreate, FileAccess.Write);
                fs.Position = fs.Length;
                using(BinaryWriter bw = new BinaryWriter(fs))
                {
                    //string text = save.Jmeno + " ; " + score + " ; " + time;
                    //string text = string.Format("{0} {1,15} {2,20} {3,25}", save.Jmeno, score, time, difficulty);
                    bw.Write(save.Jmeno);
                    bw.Write(score);
                    bw.Write(time);
                    bw.Write(difficulty);
                }
                scoreSave.Visible = false;
            }
        }

        private void timeTimer_Tick(object sender, EventArgs e)
        {
            sek++;
            if(sek==60)
            {
                min++;
                sek = 0;
            }
            //time.Text = "Time: " + min + ":" + sek;
            if(sek<10)
                time.Text = string.Format("Time: {0}:0{1}", min, sek);
            else
                time.Text = string.Format("Time: {0}:{1}", min, sek);
        }

        private void EatFood()
        {
            score += 1;
            txtScore.Text = "Score: " + score;

            crunch.Play();

            Circle body = new Circle
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };

            Snake.Add(body);
            if (score == 50)
            {
                GameOver();
                Congratulations congratulations = new Congratulations();
                congratulations.Show();
            }
            else
            {
                int x = 0;
                int y = 0;
                bool generovalo = false;
                while (!generovalo)
                {
                    x = rand.Next(2, maxWidth);
                    y = rand.Next(2, maxHeight);
                    generovalo = true;
                    for (int i = 0; i < Snake.Count && generovalo; i++)
                    {
                        if (Snake[i].X == x && Snake[i].Y == y)
                            generovalo = false;
                    }
                }
                food = new Circle { X = x, Y = y };
            }
        }

        private void GameOver()
        {
            gameTimer.Stop();
            timeTimer.Stop();
            startButton.Enabled = true;
            label1.Visible = false;
            scoreSave.Visible = true;
        }

        private void GamePause()
        {
            if(gameTimer.Enabled)
            {
                gameTimer.Stop();
                timeTimer.Stop();
                label1.Text = "Spustit: F1";
            }
            else
            {
                gameTimer.Start();
                timeTimer.Start();
                label1.Text = "Zastavit: F1";
            }
        }

    }
}
