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
    public partial class Advisor : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        
        public Advisor()
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
            
            int sal;
            string firstname = textBox2.Text;
            string lastname = textBox1.Text;
            string contact = textBox6.Text;
            string email = textBox4.Text;
            conn.Open();
            if (string.IsNullOrEmpty(firstname) || !Regex.IsMatch(textBox2.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Firstname, type here again!");
                textBox2.Clear();
                firstname = textBox2.Text;
                conn.Close();
                return;

            }


            if (string.IsNullOrEmpty(lastname) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Lastname,type here again!");
                textBox1.Clear();
                lastname = textBox1.Text;
                conn.Close();
                return;

            }
            if (string.IsNullOrEmpty(contact) || !Regex.IsMatch(textBox6.Text, @"^[0-9]{10,12}$"))
            {
                MessageBox.Show("Invalid contact type again");
                textBox6.Clear();
                contact = textBox6.Text;
                conn.Close();
                return;

            }
           
            string q12 = string.Format("SELECT * FROM Person WHERE Email = '{0}'", textBox4.Text);
            SqlCommand c1 = new SqlCommand(q12, conn);
            SqlDataReader reader1 = c1.ExecuteReader();
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(textBox4.Text, @"^([\w]+)@([\w]+)\.([\w]+)$"))
            {
                MessageBox.Show("Invalid email type again");
                textBox4.Clear();
                email = textBox4.Text;
                conn.Close();
                return;

            }
            if (reader1.HasRows)
            {
                MessageBox.Show("Email has already been taken! Try another one!");
                textBox4.Clear();
                email = textBox4.Text;
                conn.Close();
                return;
            }
            reader1.Close();
            if (!int.TryParse(textBox3.Text, out sal))
            {
                MessageBox.Show("Enter digit as Salary only!");
                textBox3.Clear();
                conn.Close();
                return;
            }
           
            string Designation;
            Designation = string.Format("select Id from Lookup where Category='DESIGNATION' AND Value = '{0}'", comboBox1.Text);
            SqlCommand cmd = new SqlCommand(Designation, conn);
            int id = (Int32)cmd.ExecuteScalar();
            string gender;
            gender = string.Format("select Id from Lookup where Category='GENDER' AND Value = '{0}'", comboBox2.Text);
            SqlCommand cmd1 = new SqlCommand(gender, conn);
            int gen = (Int32)cmd1.ExecuteScalar();

            string query1 = String.Format("INSERT INTO Person(FirstName, LastName, Contact, Email,DateOfBirth,Gender) VALUES (@FirstName, @LastName," +
                    "@Contact, @Email,@DateOfBirth,@Gender)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@FirstName", textBox2.Text);
            command1.Parameters.AddWithValue("@LastName", textBox1.Text);
            command1.Parameters.AddWithValue("@Contact", textBox6.Text);
            command1.Parameters.AddWithValue("@Email", textBox4.Text);
            command1.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value);
            command1.Parameters.AddWithValue("@Gender", gen);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();
            string P_id = string.Format("SELECT Id FROM Person WHERE Email= '{0}'", textBox4.Text);
            SqlCommand command3 = new SqlCommand(P_id, conn);
            int Id = (Int32)command3.ExecuteScalar();
            string query2 = "INSERT INTO Advisor(Id, Designation, Salary) VALUES (@Id, @Designation, @Salary)";
            SqlCommand command2 = new SqlCommand(query2, conn);

            command2.Parameters.AddWithValue("@Id", Id);
            command2.Parameters.AddWithValue("@Designation", id);
            command2.Parameters.AddWithValue("@Salary", textBox3.Text);
            command2.ExecuteNonQuery();
            command2.Parameters.Clear();
            MessageBox.Show("Data inserted Successfully!!!");
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox6.Clear();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Advisor.Id as [AdvisorId], FirstName, LastName, Contact, Email, " +
                 " DateOfBirth, Gender, Designation, Salary FROM Person JOIN Advisor ON (Person.Id = Advisor.Id)", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["AdvisorId"].Visible = false;
            conn.Close();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Advisor_Load(object sender, EventArgs e)
        {
            button10.Hide();
            conn.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Advisor.Id as [AdvisorId], FirstName, LastName, Contact, Email, " +
                  " DateOfBirth, Gender, Designation, Salary FROM Person JOIN Advisor ON (Person.Id = Advisor.Id)", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["AdvisorId"].Visible = false;



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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            conn.Open();
            string Designation;

            Designation = string.Format("select Id from Lookup where Value = '{0}' AND Category='DESIGNATION' ", comboBox1.Text);
            SqlCommand cmd = new SqlCommand(Designation, conn);
            int des = (Int32)cmd.ExecuteScalar();
            string a = string.Format("UPDATE Person SET Email = @Email, Contact = @Contact , FirstName = @firstname," +
                " LastName = @lastname From Person JOIN Advisor " +
                        "On (Person.Id = Advisor.Id) WHERE Person.Email = '{0}'", em);
            SqlCommand cmd1 = new SqlCommand(a, conn);
            int sal;
            string firstname = textBox2.Text;
            string lastname = textBox1.Text;
            string contact = textBox6.Text;
            string email = textBox4.Text;
            

            if (string.IsNullOrEmpty(firstname) || !Regex.IsMatch(textBox2.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Firstname, type here again!");
                textBox2.Clear();
                firstname = textBox2.Text;
                conn.Close();
                return;

            }


            if (string.IsNullOrEmpty(lastname) || !Regex.IsMatch(textBox1.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Lastname,type here again!");
                textBox1.Clear();
                lastname = textBox1.Text;
                conn.Close();
                return;

            }
            if (string.IsNullOrEmpty(contact) || !Regex.IsMatch(textBox6.Text, @"^[0-9]{10,12}$"))
            {
                MessageBox.Show("Invalid contact type again");
                textBox6.Clear();
                contact = textBox6.Text;
                conn.Close();
                return;

            }
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(textBox4.Text, @"^([\w]+)@([\w]+)\.([\w]+)$"))
            {
                MessageBox.Show("Invalid email type again");
                textBox4.Clear();
                email = textBox4.Text;
                conn.Close();
                return;

            }
            if (!int.TryParse(textBox3.Text, out sal))
            {
                MessageBox.Show("Enter digit as Salary only!");
                textBox3.Clear();
                conn.Close();
                return;
            }
            cmd1.Parameters.AddWithValue("@Contact", textBox6.Text);
            cmd1.Parameters.AddWithValue("@Email", textBox4.Text);
            cmd1.Parameters.AddWithValue("@firstname", textBox2.Text);
            cmd1.Parameters.AddWithValue("@lastname", textBox1.Text);
            cmd1.ExecuteNonQuery();
            string b = string.Format("UPDATE Advisor SET Designation = @designation, Salary = @salary From Advisor JOIN Person " +
                        "On (Person.Id = Advisor.Id) WHERE Person.Email = '{0}'", em);
            SqlCommand cmd2 = new SqlCommand(b, conn);
            cmd2.Parameters.AddWithValue("@designation", des);
            cmd2.Parameters.AddWithValue("@salary", textBox3.Text);
            cmd2.ExecuteNonQuery();
            MessageBox.Show("UPDATED successfully!!!");
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox6.Clear();

            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Advisor.Id as [AdvisorId], FirstName, LastName, Contact, Email, " +
                  " DateOfBirth, Gender, Designation, Salary FROM Person JOIN Advisor ON (Person.Id = Advisor.Id)", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            dataGridView1.Columns["AdvisorId"].Visible = false;
            button10.Hide();
            button7.Show();
            conn.Close();
        }
        string fname, lname, con, em, sal;

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
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

                    string a = dataGridView1.Rows[grid.RowIndex].Cells["AdvisorId"].Value.ToString();
                    string query2 = String.Format("DELETE FROM ProjectAdvisor WHERE AdvisorId='{0}'", a);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.ExecuteNonQuery();
                    string query1 = String.Format("DELETE FROM Advisor WHERE Id='{0}'", a);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    string email = String.Format(dataGridView1.Rows[grid.RowIndex].Cells["Email"].Value.ToString());
                    cmd1.CommandText = string.Format("DELETE FROM Person WHERE Email='{0}'", email);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Advisor.Id as [AdvisorId], FirstName, LastName, Contact, Email, " +
                 " DateOfBirth, Gender, Designation, Salary FROM Person JOIN Advisor ON (Person.Id = Advisor.Id)", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView1.DataSource = dtbl;
                    dataGridView1.Columns["AdvisorId"].Visible = false;
                }
                conn.Close();
            }
            if (grid.ColumnIndex == dataGridView1.Columns["UPDATE"].Index)
            {
                conn.Open();
                //id = dataGridView1.Rows[grid.RowIndex].Cells["Advisor ID"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[grid.RowIndex].Cells["FirstName"].Value.ToString();
                textBox1.Text = dataGridView1.Rows[grid.RowIndex].Cells["LastName"].Value.ToString();
                textBox6.Text = dataGridView1.Rows[grid.RowIndex].Cells["Contact"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[grid.RowIndex].Cells["Email"].Value.ToString();
                textBox3.Text = dataGridView1.Rows[grid.RowIndex].Cells["Salary"].Value.ToString();
                sal= textBox3.Text;
                fname = textBox2.Text;
                lname = textBox1.Text;
                con = textBox6.Text;
                em = textBox4.Text;
                button10.Show();
                button7.Hide();
                conn.Close();

            }

        }


    }
}
