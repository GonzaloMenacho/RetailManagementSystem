using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComputerParts
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        int startPoint = 0;

        //progress bar simulation
        private void timer1_Tick(object sender, EventArgs e)
        {
            startPoint += 1;                ;
            progress.Value = startPoint;
            if (progress.Value == 100)
            {
                progress.Value = 0;
                timer1.Stop();
                Form1 login = new Form1();
                this.Hide();
                login.Show();
            }
        }
        //start timer
        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
