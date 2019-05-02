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
    public partial class Eval_info : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public Eval_info()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Evaluation();
            a.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            conn.Open();
            string name = textBox6.Text;
            int tmarks, tweight;
            string q12 = string.Format("SELECT * FROM Evaluation WHERE Name = '{0}'", textBox6.Text);
            SqlCommand c1 = new SqlCommand(q12, conn);
            SqlDataReader reader1 = c1.ExecuteReader();
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(textBox6.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid name,type here again!");
                textBox6.Clear();
                name = textBox6.Text;
                conn.Close();
                return;

            }
            if (reader1.HasRows)
            {
                MessageBox.Show("Name has already been taken! Try another one!");
                textBox6.Clear();
                name = textBox6.Text;
                conn.Close();
                return;

            }
            reader1.Close();
            if (!int.TryParse(textBox2.Text, out tmarks))
            {
                MessageBox.Show("Enter digit as Total Marks only!");
                textBox2.Clear();
                conn.Close();
                return;
            }
            tmarks = Convert.ToInt32(textBox2.Text);
           
            
            if (!int.TryParse(textBox1.Text, out tweight))
            {
                MessageBox.Show("Enter digit as Total Weightage only!");
                textBox1.Clear();
                conn.Close();
                return;
            }
            string query1 = String.Format("INSERT INTO Evaluation(Name, TotalMarks,TotalWeightage) VALUES (@Name, @TMarks, @Tweight)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@Name", textBox6.Text);
            command1.Parameters.AddWithValue("@TMarks", textBox2.Text);
            command1.Parameters.AddWithValue("@Tweight", textBox1.Text);

            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            MessageBox.Show("Data inserted");
            textBox1.Clear();
            textBox2.Clear();
            textBox6.Clear();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id, Name, TotalMarks, TotalWeightage FROM Evaluation", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["Id"].Visible = false;
            conn.Close();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        string n, tmarks, tw;
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
                    string Id = String.Format(dataGridView1.Rows[grid.RowIndex].Cells["Id"].Value.ToString());

                    string query2 = String.Format("DELETE FROM GroupEvaluation WHERE EvaluationId='{0}'", Id);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.ExecuteNonQuery();
                    string query1 = String.Format("DELETE FROM Evaluation WHERE Id='{0}'", Id);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id, Name, TotalMarks, TotalWeightage FROM Evaluation", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    dataGridView1.Columns["Id"].Visible = false;
                    dataGridView1.Refresh();
                }
                conn.Close();
            }
            if (grid.ColumnIndex == dataGridView1.Columns["UPDATE"].Index)
            {

                conn.Open();
                //id = dataGridView1.Rows[grid.RowIndex].Cells["Id"].Value.ToString();
                textBox6.Text = dataGridView1.Rows[grid.RowIndex].Cells["Name"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[grid.RowIndex].Cells["TotalMarks"].Value.ToString();
                textBox1.Text = dataGridView1.Rows[grid.RowIndex].Cells["TotalWeightage"].Value.ToString();
                
                
                n= textBox6.Text;
                tmarks = textBox2.Text;
                tw = textBox1.Text;

                button10.Show();
                button7.Hide();
                conn.Close();


            }
        }

        private void Eval_info_Load(object sender, EventArgs e)
        {
            button10.Hide();
            conn.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id, Name, TotalMarks, TotalWeightage FROM Evaluation", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["Id"].Visible = false;


            var DELETE = new DataGridViewButtonColumn();
            DELETE.FlatStyle = FlatStyle.Popup;
            DELETE.HeaderText = "DELETE";
            DELETE.Name = "DELETE";
            DELETE.UseColumnTextForButtonValue = true;
            DELETE.Text = "DELETE";
            DELETE.Width = 60;
            this.dataGridView1.Columns.Add(DELETE);

            var UPDATE = new DataGridViewButtonColumn();
            UPDATE.Name = "UPDATE";
            UPDATE.HeaderText = "UPDATE";
            UPDATE.UseColumnTextForButtonValue = true;
            UPDATE.Text = "UPDATE";
            UPDATE.Width = 60;
            this.dataGridView1.Columns.Add(UPDATE);
            conn.Close();
            conn.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            conn.Open();
            string a = string.Format("UPDATE Evaluation SET Name = @name, TotalMarks = @tmarks, TotalWeightage = @tw " +
                "where Name = '{0}'", n);
            SqlCommand cmd = new SqlCommand(a, conn);
            string name = textBox6.Text;
            int tmarks, tweight;
            string q12 = string.Format("SELECT * FROM Evaluation WHERE Name = '{0}'", textBox6.Text);
            SqlCommand c1 = new SqlCommand(q12, conn);
            SqlDataReader reader1 = c1.ExecuteReader();
            if (string.IsNullOrEmpty(name) || !Regex.IsMatch(textBox6.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid name,type here again!");
                textBox6.Clear();
                name = textBox6.Text;
                conn.Close();
                return;

            }
            if (reader1.HasRows)
            {
                MessageBox.Show("Name has already been taken! Try another one!");
                textBox6.Clear();
                name = textBox6.Text;
                conn.Close();
                return;

            }
            reader1.Close();
            if (!int.TryParse(textBox2.Text, out tmarks))
            {
                MessageBox.Show("Enter digit as Total Marks only!");
                textBox2.Clear();
                conn.Close();
                return;
            }
            if (!int.TryParse(textBox1.Text, out tweight))
            {
                MessageBox.Show("Enter digit as Total Weightage only!");
                textBox1.Clear();
                conn.Close();
                return;
            }
            cmd.Parameters.AddWithValue("@name", textBox6.Text);
            cmd.Parameters.AddWithValue("@tmarks", textBox2.Text);
            cmd.Parameters.AddWithValue("@tw", textBox1.Text);
            cmd.ExecuteNonQuery();
            MessageBox.Show("UPDATED successfully!!!");
            textBox2.Clear();
            textBox1.Clear();
            textBox6.Clear();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Id, Name, TotalMarks, TotalWeightage FROM Evaluation", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["Id"].Visible = false;
            conn.Close();
            button10.Hide();
            button7.Show();

        }
    }
}
