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
    public partial class Managing_Groups : Form
    {
        public Managing_Groups()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Form4();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Group_formation();
            a.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Group_project();
            a.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new group_info();
            a.Show();
        }
    }
}
