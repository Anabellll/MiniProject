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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Project_details();
            open.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            var open = new Project_assign();
            open.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Form4();
            a.Show();
        }
    }
}
