using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Miniproject
{
    public partial class group_info : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public group_info()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Managing_Groups();
            a.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
           SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, RegistrationNo As [Registration Num], " +
               "Status, AssignmentDate FROM GroupStudent JOIN Student ON " +
               "(Student.Id = GroupStudent.StudentId)", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            //this.dataGridView1.Columns["GroupId"].Visible = false;
            conn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs grid)
        {
            if (grid.RowIndex < 0 || grid.RowIndex == dataGridView1.NewRowIndex)
            {
                return;
            }
            if (grid.ColumnIndex == dataGridView1.Columns["DELETE"].Index)
            {
                conn.Open();

                DialogResult Check = MessageBox.Show("Do you really want to delete?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Check == DialogResult.Yes)
                {
                    string p_id = String.Format(dataGridView1.Rows[grid.RowIndex].Cells["GroupId"].Value.ToString());

                    string query2 = String.Format("DELETE FROM [GroupProject] WHERE [GroupId]='{0}'", p_id);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.ExecuteNonQuery();
                    string query3 = String.Format("DELETE FROM [GroupStudent] WHERE [GroupId]='{0}'", p_id);
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    cmd3.ExecuteNonQuery();
                    string query4 = String.Format("DELETE FROM [GroupEvaluation] WHERE [GroupId]='{0}'", p_id);
                    SqlCommand cmd4 = new SqlCommand(query4, conn);
                    cmd4.ExecuteNonQuery();

                    string query1 = String.Format("DELETE FROM [Group] WHERE Id='{0}'", p_id);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, RegistrationNo As [Registration Num], " +
               "Status, AssignmentDate FROM GroupStudent JOIN Student ON " +
               "(Student.Id = GroupStudent.StudentId)", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    //dataGridView1.Columns["GroupId"].Visible = false;


                }

                conn.Close();
            }
        }

        private void group_info_Load(object sender, EventArgs e)
        {
           
            var DELETE = new DataGridViewButtonColumn();
            DELETE.FlatStyle = FlatStyle.Popup;
            DELETE.HeaderText = "DELETE";
            DELETE.Name = "DELETE";
            DELETE.UseColumnTextForButtonValue = true;
            DELETE.Text = "DELETE";
            DELETE.Width = 60;
            this.dataGridView1.Columns.Add(DELETE);
            
        }
    }
}
