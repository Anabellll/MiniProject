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
    public partial class Group_eval : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public Group_eval()
        {
            InitializeComponent();
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
                    string p_id = String.Format(dataGridView1.Rows[grid.RowIndex].Cells["Evaluation Name"].Value.ToString());
                    string q1 = string.Format("SELECT GroupEvaluation.EvaluationId FROM GroupEvaluation JOIN " +
               "Evaluation On(GroupEvaluation.EvaluationId = Evaluation.Id) WHERE Evaluation.Name = '{0}'", p_id);
                    SqlCommand cmd2 = new SqlCommand(q1, conn);
                    int id1 = (Int32)cmd2.ExecuteScalar();


                    string query1 = String.Format("DELETE FROM GroupEvaluation WHERE EvaluationId='{0}'", id1);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, Evaluation.Name As [Evaluation Name], " +
                "ObtainedMarks as [Obtained Marks], EvaluationDate FROM " +
                " GroupEvaluation JOIN Evaluation ON (Evaluation.Id = GroupEvaluation.EvaluationId) ", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    this.dataGridView1.Columns["GroupID"].Visible = false;


                }

                conn.Close();
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new Evaluation();
            a.Show();
        }

        private void Group_eval_Load(object sender, EventArgs e)
        {
            
            conn.Open();
            //evaluation_ID//
            SqlCommand sc = new SqlCommand("select Name FROM Evaluation", conn);
            SqlDataReader reader;
            reader = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Load(reader);
            comboBox1.ValueMember = "Name";
            comboBox1.DataSource = dt;

            //Group ID//
            SqlCommand ad = new SqlCommand("select Id FROM [Group]", conn);
            SqlDataReader reader1;
            reader1 = ad.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Id", typeof(string));
            dt1.Load(reader1);
            comboBox2.ValueMember = "Id";
            comboBox2.DataSource = dt1;

            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, Evaluation.Name As [Evaluation Name], " +
                "ObtainedMarks as [Obtained Marks], EvaluationDate FROM " +
                " GroupEvaluation JOIN Evaluation ON (Evaluation.Id = GroupEvaluation.EvaluationId) ", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            this.dataGridView1.Columns["GroupID"].Visible = false;


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

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            conn.Open();
            string  e_id;
            int omarks;
            string q1 = string.Format("SELECT GroupEvaluation.EvaluationId FROM GroupEvaluation JOIN " +
                "Evaluation On(GroupEvaluation.EvaluationId = Evaluation.Id) WHERE Evaluation.Name = '{0}'", comboBox1.Text);
            SqlCommand c1 = new SqlCommand(q1, conn);
            SqlDataReader reader1 = c1.ExecuteReader();
            if (reader1.HasRows)
            {
                MessageBox.Show("This EvaluationId is done! Try another one!");
                comboBox1.ResetText();
                e_id = comboBox1.Text;
                conn.Close();

                return;
            }
            reader1.Close();
            
            if (!int.TryParse(textBox1.Text, out omarks))
            {
                MessageBox.Show("Enter digit as Total Marks only!");
                textBox1.Clear();
                conn.Close();
                return;
            }
            string e1 = string.Format("SELECT TotalMarks FROM Evaluation WHERE Name = '{0}'", comboBox1.Text);
            SqlCommand e11 = new SqlCommand(e1, conn);
            int O_marks = (Int32)e11.ExecuteScalar();
            if (omarks > O_marks)
            {
                MessageBox.Show("Obtained Marks should be less than Total Marks : " + O_marks);
                textBox1.Clear();
                conn.Close();
                return;
            }
            string eval;
            eval = string.Format("select Evaluation.Id FROM Evaluation where Name = '{0}'", comboBox1.Text);
            SqlCommand cmd2 = new SqlCommand(eval, conn);
            int id3 = (Int32)cmd2.ExecuteScalar();
            string query1 = String.Format("INSERT INTO GroupEvaluation(GroupId, EvaluationId, ObtainedMarks, EvaluationDate) " +
                " VALUES (@g_id, @e_id, @o_marks, @e_date)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@e_id", id3);
            command1.Parameters.AddWithValue("@g_id", comboBox2.Text);
            command1.Parameters.AddWithValue("@o_marks", textBox1.Text);
            string e_date = DateTime.Now.ToString("M/d/yyyy");
            command1.Parameters.AddWithValue("@e_date", e_date);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            MessageBox.Show("Data inserted");
            comboBox1.ResetText();
            comboBox2.ResetText();
            textBox1.Clear();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT GroupId, Evaluation.Name As [Evaluation Name], " +
                 "ObtainedMarks as [Obtained Marks], EvaluationDate FROM " +
                 " GroupEvaluation JOIN Evaluation ON (Evaluation.Id = GroupEvaluation.EvaluationId) ", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            this.dataGridView1.Columns["GroupID"].Visible = false;
            conn.Close();
        }

        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
