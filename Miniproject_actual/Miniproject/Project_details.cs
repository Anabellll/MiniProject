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
    public partial class Project_details : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public Project_details()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new management();
            a.Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string des = textBox6.Text;
            string title = textBox5.Text;
            if (string.IsNullOrEmpty(des) || !Regex.IsMatch(textBox6.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Description type again");
                textBox6.Clear();
                conn.Close();
                return;

            }
            if (string.IsNullOrEmpty(title) || !Regex.IsMatch(textBox5.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Title type again");
                textBox5.Clear();
                conn.Close();
                return;

            }


            conn.Open();

            
            string query1 = String.Format("INSERT INTO Project(Description, Title) VALUES ( @Description, @Title)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            
            command1.Parameters.AddWithValue("@Description", textBox6.Text);
            command1.Parameters.AddWithValue("@Title", textBox5.Text);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            MessageBox.Show("Data inserted Successfully!!!");
            textBox5.Clear();
            textBox6.Clear();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id As [Project ID], Description, Title " +
                "FROM Project", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["Project ID"].Visible = false;
            conn.Close();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            conn.Open();
            
            string a = string.Format("UPDATE Project SET Description = @Description, Title = @Title where Id='{0}'", id);
            SqlCommand cmd1 = new SqlCommand(a, conn);
            string des = textBox6.Text;
            string title = textBox5.Text;
            if (string.IsNullOrEmpty(des) || !Regex.IsMatch(textBox6.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Description type again");
                textBox6.Clear();
                conn.Close();
                return;

            }
            if (string.IsNullOrEmpty(title) || !Regex.IsMatch(textBox5.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Title type again");
                textBox5.Clear();
                conn.Close();
                return;

            }
            cmd1.Parameters.AddWithValue("@Description", textBox5.Text);
            cmd1.Parameters.AddWithValue("@Title", textBox6.Text);
            cmd1.ExecuteNonQuery();
            MessageBox.Show("UPDATED successfully!!!");
            textBox5.Clear();
            textBox6.Clear();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id As [Project ID], Description, Title " +
                "FROM Project", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["Project ID"].Visible = false;
            conn.Close();
        }
        string desc, title, id;

        private void Project_details_Load(object sender, EventArgs e)
        {
            button10.Hide();
            conn.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id As [Project ID], Description, Title " +
                "FROM Project", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["Project ID"].Visible = false;


            var DELETE = new DataGridViewButtonColumn();
            DELETE.FlatStyle = FlatStyle.Popup;
            DELETE.HeaderText = "DELETE";
            DELETE.Name = "DELETE";
            DELETE.UseColumnTextForButtonValue = true;
            DELETE.Text = "DELETE";
            DELETE.Width = 50;
            this.dataGridView1.Columns.Add(DELETE);

            var UPDATE = new DataGridViewButtonColumn();
            UPDATE.FlatStyle = FlatStyle.Popup;
            UPDATE.Name = "UPDATE";
            UPDATE.HeaderText = "UPDATE";
            UPDATE.UseColumnTextForButtonValue = true;
            UPDATE.Text = "UPDATE";
            UPDATE.Width = 50;
            this.dataGridView1.Columns.Add(UPDATE);
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
                    string a = dataGridView1.Rows[grid.RowIndex].Cells["Project ID"].Value.ToString();
                    string query2 = String.Format("DELETE FROM ProjectAdvisor WHERE ProjectId='{0}'", a);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.ExecuteNonQuery();

                    string query3 = String.Format("DELETE FROM GroupProject WHERE ProjectId='{0}'", a);
                    SqlCommand cmd3 = new SqlCommand(query3, conn);
                    cmd3.ExecuteNonQuery();

                    string query1 = String.Format("DELETE FROM Project WHERE Id='{0}'", a);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
              
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id As [Project ID], Description, Title " +
                "FROM Project", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    dataGridView1.Columns["Project ID"].Visible = false;
                }
                conn.Close();
            }
            if (grid.ColumnIndex == dataGridView1.Columns["UPDATE"].Index)
            {
                conn.Open();
                
                id = dataGridView1.Rows[grid.RowIndex].Cells["Project ID"].Value.ToString();
                textBox6.Text = dataGridView1.Rows[grid.RowIndex].Cells["Description"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[grid.RowIndex].Cells["Title"].Value.ToString();
                desc = textBox6.Text;
                title= textBox5.Text;
                button10.Show();
                button7.Hide();
                conn.Close();

            }
        }
    }
}
