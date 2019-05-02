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
    public partial class Pro_adv : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public Pro_adv()
        {
            InitializeComponent();
        }
        string Ad, Pd;
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
                    string p_id = String.Format(dataGridView1.Rows[grid.RowIndex].Cells["ProjectId"].Value.ToString());

                   
                    string query1 = String.Format("DELETE FROM ProjectAdvisor WHERE ProjectId='{0}'", p_id);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT AdvisorId, ProjectId, AdvisorRole, AssignmentDate AS [Assignment Date] " +
                "FROM ProjectAdvisor", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    dataGridView1.Columns["AdvisorId"].Visible = false;
                    dataGridView1.Columns["ProjectId"].Visible = false;
                    dataGridView1.Refresh();
                }

                conn.Close();
            }

           

        }


        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new management();
            a.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {//add button//
            conn.Open();
            string ad_role, p_id;
            ad_role = string.Format("select Id from Lookup where Category='ADVISOR_ROLE' AND Value = '{0}'", comboBox3.Text);
            SqlCommand cmd = new SqlCommand(ad_role, conn);
            int id = (Int32)cmd.ExecuteScalar();
            string q11 = string.Format("SELECT ProjectId FROM ProjectAdvisor JOIN Project ON(Project.Id = ProjectAdvisor.ProjectId) " +
                " WHERE Project.Title = '{0}'", comboBox1.Text);
            SqlCommand c = new SqlCommand(q11, conn);
            
            SqlDataReader reader = c.ExecuteReader();
            if (reader.HasRows)
            {
                MessageBox.Show("Project Title has already been assigned to advisor! Try another one!");
                comboBox1.ResetText();
                p_id = comboBox1.Text;
                conn.Close();

                return;
            }
            reader.Close();
            string advisor, project;
            advisor = string.Format("select Advisor.Id FROM Advisor JOIN Person ON (Advisor.Id = Person.Id) Where " +
                "FirstName+' '+LastName = '{0}'", comboBox2.Text);
            SqlCommand cmd1 = new SqlCommand(advisor, conn);
            int id1 = (Int32)cmd1.ExecuteScalar();
            project = string.Format("select Project.Id FROM Project where Title = '{0}'", comboBox1.Text);
            SqlCommand cmd2 = new SqlCommand(project, conn);
            int id3 = (Int32)cmd2.ExecuteScalar();


            string query1 = String.Format("INSERT INTO ProjectAdvisor(AdvisorId, ProjectId, AdvisorRole,AssignmentDate) VALUES (@ad_id, @p_id, @ad_role, @a_date)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@ad_id", id1);
            command1.Parameters.AddWithValue("@p_id", id3);
            command1.Parameters.AddWithValue("@ad_role", id);
            string as_date = DateTime.Now.ToString("M/d/yyyy");
            command1.Parameters.AddWithValue("@a_date", as_date);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            MessageBox.Show("Data inserted");
            comboBox1.ResetText();
            comboBox2.ResetText();
            comboBox3.ResetText();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT AdvisorId, ProjectId, AdvisorRole, AssignmentDate AS [Assignment Date] " +
                "FROM ProjectAdvisor", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["AdvisorId"].Visible = false;
            dataGridView1.Columns["ProjectId"].Visible = false;
            conn.Close();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Pro_adv_Load(object sender, EventArgs e)
        {
           
            conn.Open();
            SqlCommand sc = new SqlCommand("select FirstName+' '+LastName AS Name From Person " +
                "JOIN Advisor ON(Advisor.Id = Person.Id) ", conn);
            SqlDataReader reader;

            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Load(reader);
            comboBox2.ValueMember = "Name";
            comboBox2.DataSource = dt;


            SqlCommand ad = new SqlCommand("select Title FROM Project ", conn);
            SqlDataReader reader1;

            reader1 = ad.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Title", typeof(string));
            dt1.Load(reader1);
            comboBox1.ValueMember = "Title";
            comboBox1.DataSource = dt1;

            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT AdvisorId, ProjectId, AdvisorRole, AssignmentDate AS [Assignment Date] " +
                "FROM ProjectAdvisor", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["AdvisorId"].Visible = false;
            dataGridView1.Columns["ProjectId"].Visible = false;


            var DELETE = new DataGridViewButtonColumn();
            DELETE.FlatStyle = FlatStyle.Popup;
            DELETE.HeaderText = "DELETE";
            DELETE.Name = "DELETE";
            DELETE.UseColumnTextForButtonValue = true;
            DELETE.Text = "DELETE";
            DELETE.Width = 60;
            this.dataGridView1.Columns.Add(DELETE);

           


            conn.Close();
        }
        
        private void button5_Click(object sender, EventArgs e)
        {
            
            }
            

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
   