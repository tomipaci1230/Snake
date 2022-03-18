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
    public partial class Movement : Form
    {
        private bool mov;

        public bool Mov
        {
            get { return mov; }
            set { mov = value; }
        }

        public Movement()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                Mov = true;
            else
                Mov = false;
        }
    }
}
