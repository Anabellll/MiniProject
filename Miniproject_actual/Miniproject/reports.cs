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
    public partial class reports : Form
    {
        public reports()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new list_Pro();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new stu_report();
            a.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Form4();
            a.Show();
        }
    }
}
