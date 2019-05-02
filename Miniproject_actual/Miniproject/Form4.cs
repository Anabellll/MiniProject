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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Advisor();
            open.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Form1();
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
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Evaluation();
            open.Show();
        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            this.Hide();
            var open = new management();
            open.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Managing_Groups();
            open.Show();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            var open = new reports();
            open.Show();
        }
    }
}
