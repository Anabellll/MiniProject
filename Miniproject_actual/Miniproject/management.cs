using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miniproject
{
    public partial class management : Form
    {
        public management()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Form1();
            open.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Advisor();
            open.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Project_details();
            open.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Pro_adv();
            open.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {

            this.Close();
            var a = new Form4();
            a.Show();
        }
    }
}
