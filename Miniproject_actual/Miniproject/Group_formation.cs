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
using System.Net.Mail;

namespace Miniproject
{
    public partial class Group_formation : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public Group_formation()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Managing_Groups();
            a.Show();
        }

        private void Group_formation_Load(object sender, EventArgs e)
        {
            label3.Hide();
            button4.Hide();
            button6.Hide();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand sc = new SqlCommand("select RegistrationNo FROM Student", conn);
            SqlDataReader reader12;

            reader12 = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("RegistrationNo", typeof(string));
            dt.Load(reader12);
            comboBox1.ValueMember = "RegistrationNo";
            comboBox1.DataSource = dt;
           
            string date = DateTime.Now.ToString("M/d/yyyy");
            string query1 = String.Format("INSERT INTO [Group](Created_On) VALUES (@date)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@date", date);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            MessageBox.Show("Group created");
            button2.Hide();
            button4.Show();
            
            string a = String.Format("SELECT TOP 1 Id FROM [Group] ORDER BY Id DESC");
            SqlCommand command2 = new SqlCommand(a, conn);
            SqlDataReader reader = command2.ExecuteReader();
            while (reader.Read())
            {
                 
                label3.Text = reader[0].ToString();
            }
            reader.Close();

            command2.ExecuteNonQuery();
            command2.Parameters.Clear();

            string query11 = string.Format("SELECT GroupId, StudentId, RegistrationNo, Status, AssignmentDate AS [Assignment Date] " +
                "FROM GroupStudent JOIN Student ON(Student.Id = GroupStudent.StudentId) WHERE GroupId = '{0}'", label3.Text);
            SqlDataAdapter sqlData = new SqlDataAdapter(query11, conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["GroupId"].Visible = false;
            dataGridView1.Columns["StudentId"].Visible = false;

            var DELETE = new DataGridViewButtonColumn();
            DELETE.FlatStyle = FlatStyle.Popup;
            DELETE.HeaderText = "DELETE";
            DELETE.Name = "DELETE";
            DELETE.UseColumnTextForButtonValue = true;
            DELETE.Text = "DELETE";
            DELETE.Width = 60;
            this.dataGridView1.Columns.Add(DELETE);
            label3.Show();
             conn.Close();
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
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


                ////int selectedID = Convert.ToInt32(dataGridView2.Rows[grid.RowIndex].Cells[0].Value.ToString());
                DialogResult Check = MessageBox.Show("Do you really want to delete?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Check == DialogResult.Yes)
                {
                    string g = String.Format(dataGridView1.Rows[grid.RowIndex].Cells["GroupId"].Value.ToString());
                    string s = String.Format(dataGridView1.Rows[grid.RowIndex].Cells["StudentId"].Value.ToString());
                   
                    string query1 = String.Format("DELETE FROM GroupStudent WHERE GroupId='{0}' AND StudentId ='{1}'", g,s);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    string query11 = string.Format("SELECT GroupId, StudentId, RegistrationNo, Status, AssignmentDate AS [Assignment Date] " +
                "FROM GroupStudent JOIN Student ON(Student.Id = GroupStudent.StudentId) WHERE GroupId = '{0}'", label3.Text);
                    SqlDataAdapter sqlData = new SqlDataAdapter(query11, conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    dataGridView1.Columns["GroupId"].Visible = false;
                    dataGridView1.Columns["StudentId"].Visible = false;


                }

                conn.Close();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn.Open();
            string status, check;
           
            string q12 = string.Format("SELECT GroupStudent.StudentId FROM [GroupStudent] JOIN Student ON (Student.Id = GroupStudent.StudentId) " +
                " WHERE [GroupStudent].Status = '3' AND Student.RegistrationNo = '{0}'", comboBox1.Text);
            SqlCommand c1 = new SqlCommand(q12, conn);
           SqlDataReader reader1 = c1.ExecuteReader();
            
            if (reader1.HasRows)
            {
                
                
                    MessageBox.Show("This student is already in another group! select another one !");
                    comboBox1.ResetText();
                    check = comboBox1.Text;
                    conn.Close();
                    return;
                
                
            }
            reader1.Close();

            status = string.Format("select Id from Lookup where Category='STATUS' AND Value = '{0}'", comboBox2.Text);
            SqlCommand cmd = new SqlCommand(status, conn);
            int id = (Int32)cmd.ExecuteScalar();
            string q11 = string.Format("SELECT Id FROM Student Where RegistrationNo = '{0}'", comboBox1.Text);
            SqlCommand c = new SqlCommand(q11, conn);
            int s_id = (Int32)c.ExecuteScalar();
            string as_date = DateTime.Now.ToString("M/d/yyyy");
            string query1 = String.Format("INSERT INTO GroupStudent (GroupId, StudentId, Status, AssignmentDate) " +
                "VALUES (@g_id, @s_id, @status, @as_date)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@g_id", label3.Text);
            command1.Parameters.AddWithValue("@s_id", s_id);
            command1.Parameters.AddWithValue("@status", id);
            command1.Parameters.AddWithValue("@as_date", as_date);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            MessageBox.Show("Data inserted");
            button6.Show();
            comboBox1.ResetText();
            comboBox2.ResetText();
           
            string query11 = string.Format("SELECT GroupId, StudentId, RegistrationNo, Status, AssignmentDate AS [Assignment Date] " +
                "FROM GroupStudent JOIN Student ON(Student.Id = GroupStudent.StudentId) WHERE GroupId = '{0}'", label3.Text);
            SqlDataAdapter sqlData = new SqlDataAdapter(query11, conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["GroupId"].Visible = false;
            dataGridView1.Columns["StudentId"].Visible = false;
            conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            button4.Hide();
            conn.Open();
            MessageBox.Show("you have Successfully grouped students!!!");
            button2.Show();
            label3.Hide();
            button6.Hide();

            Group_formation abc = new Group_formation();
            this.Dispose();
            abc.Show();
            conn.Close();
        }
    }
}
