using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
//using System.Media;
using System.Windows.Media;

namespace ROP
{
    public partial class Form1 : Form
    {
        private int diff = 1;
        public int Diff
        {
            get { return diff; }
            set { diff = value; }
        }
        //SoundPlayer music = new SoundPlayer();
        WMPLib.WindowsMediaPlayer music = new WMPLib.WindowsMediaPlayer();

        private bool mov = true;
        public bool Mov
        {
            get { return mov; }
            set { mov = value; }
        }

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = imageList1.Images[0];
        }

        int img = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Image = imageList1.Images[img];
            if (img == 1)
                img = 0;
            else
                img++;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //music.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\8-bit Detective.wav";
            //music.PlayLooping();
            string path = AppDomain.CurrentDomain.BaseDirectory + "Sound/8-bit Detective.wav";
            music.URL = path;
            music.settings.setMode("loop", true);
            music.settings.volume = 50;
            music.controls.play();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            music.settings.volume = trackBar1.Value;
        }

        private void NewGame(object sender, EventArgs e)
        {
            Game game = new Game();
            FormProvider.MainMenu.Hide();
            game.Show();
        }

        private void DifficultyChange(object sender, EventArgs e)
        {
            Difficulty difficulty = new Difficulty();
            if (difficulty.ShowDialog() == DialogResult.OK)
            {
                diff = difficulty.Diff;
            }
            if (diff == 1)
            {
                button2.Text = "Obtížnost: Easy";
            }
            else if (diff == 2)
            {
                button2.Text = "Obtížnost: Medium";
            }
            else
            {
                button2.Text = "Obtížnost: Hard";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Score score = new Score();
            FormProvider.MainMenu.Hide();
            score.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Movement movement = new Movement();
            if(movement.ShowDialog()==DialogResult.OK)
            {
                Mov = movement.Mov;
            }
        }
    }
}
