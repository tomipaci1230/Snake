using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ROP
{
    public partial class Difficulty : Form
    {

        private int diff;
        public int Diff
        {
            get { return diff; }
            set { diff = value; }
        }

        public Difficulty()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Diff = 1;
            else if (radioButton2.Checked)
                Diff = 2;
            else
                Diff = 3;
                
        }
    }
}
