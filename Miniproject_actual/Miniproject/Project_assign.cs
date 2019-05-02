using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Miniproject
{
    public partial class Project_assign : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public Project_assign()
        {
            InitializeComponent();
        }
        void Fillcombo()
        {
            conn.Open();

            string query1 = "SELECT * FROM Advisor";
            SqlCommand cmd = new SqlCommand(query1, conn);
            SqlDataReader myread = cmd.ExecuteReader();
            int j = 0;
            if (myread.HasRows)
            { 
                while (myread.Read())
                {
                    string num = myread.GetString(0);
                    comboBox3.Items.Add(num);
                }
                j++;
            }
                
        }
        private void Project_assign_Load(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

            this.Close();
            var a = new Form4();
            a.Show();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
