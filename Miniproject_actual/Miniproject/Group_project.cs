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
    public partial class Group_project : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public Group_project()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Managing_Groups();
            a.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Group_project_Load(object sender, EventArgs e)
        {
            conn.Open();
            //group_ID//
            SqlCommand sc = new SqlCommand("select Id FROM [Group]", conn);
            SqlDataReader reader;
            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Load(reader);
            comboBox2.ValueMember = "Id";
            comboBox2.DataSource = dt;

            SqlCommand ad = new SqlCommand("select Title FROM Project ", conn);
            SqlDataReader reader1;

            reader1 = ad.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Title", typeof(string));
            dt1.Load(reader1);
            comboBox1.ValueMember = "Title";
            comboBox1.DataSource = dt1;

            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, Project.Title As [Project], AssignmentDate " +
                "FROM GroupProject JOIN Project ON (Project.Id = GroupProject.ProjectId) ", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["GroupId"].Visible = false;
           
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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


                    string query1 = String.Format("DELETE FROM GroupProject WHERE GroupId='{0}'", p_id);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, Project.Title As [Project], AssignmentDate " +
                "FROM GroupProject JOIN Project ON (Project.Id = GroupProject.ProjectId) ", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    dataGridView1.Columns["GroupId"].Visible = false;
                    
                }

                conn.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            string p_id, g_id;
            string q11 = string.Format("SELECT GroupProject.ProjectId FROM GroupProject JOIN " +
                "Project On(GroupProject.ProjectId = Project.Id) WHERE Project.Title = '{0}'", comboBox1.Text);
            SqlCommand c = new SqlCommand(q11, conn);

            SqlDataReader reader = c.ExecuteReader();
            if (reader.HasRows)
            {
                MessageBox.Show("Project Title has already been assigned to group! Try another one!");
                comboBox1.ResetText();
                p_id = comboBox1.Text;
                conn.Close();

                return;
            }
            reader.Close();
            string q12 = string.Format("SELECT [GroupProject].GroupId FROM [GroupProject] JOIN " +
                " [Group] ON ([Group].Id = [GroupProject].GroupId) Where [Group].Id = '{0}'", comboBox2.Text);
            SqlCommand c1 = new SqlCommand(q12, conn);

            SqlDataReader reader1 = c1.ExecuteReader();
            if (reader1.HasRows)
            {
                MessageBox.Show("Group has been taken a project already! Try another one!");
                comboBox2.ResetText();
                g_id = comboBox2.Text;
                conn.Close();

                return;
            }
            reader1.Close();

            string group, project;
            group = string.Format("select Id FROM [Group] where Id = '{0}'", comboBox2.Text);
            SqlCommand cmd1 = new SqlCommand(group, conn);
            int id1 = (Int32)cmd1.ExecuteScalar();
            project = string.Format("select Project.Id FROM Project where Title = '{0}'", comboBox1.Text);
            SqlCommand cmd2 = new SqlCommand(project, conn);
            int id3 = (Int32)cmd2.ExecuteScalar();
            string query1 = String.Format("INSERT INTO [GroupProject](GroupId, ProjectId,AssignmentDate) VALUES (@g_id, @p_id,@a_date)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@g_id", id1);
            command1.Parameters.AddWithValue("@p_id", id3);
            string as_date = DateTime.Now.ToString("M/d/yyyy");
            command1.Parameters.AddWithValue("@a_date", as_date);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            MessageBox.Show("Data inserted");
            comboBox1.ResetText();
            comboBox2.ResetText();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, Project.Title As [Project], AssignmentDate " +
                "FROM GroupProject JOIN Project ON (Project.Id = GroupProject.ProjectId) ", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["GroupId"].Visible = false;
            conn.Close();
        }
    }
}
