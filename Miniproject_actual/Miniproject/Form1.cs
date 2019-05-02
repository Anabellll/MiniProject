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
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        
        
        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void button3_Click(object sender, EventArgs e)
        //add button//
        {
            conn.Open();
            string firstname = textBox4.Text;
            string lastname = textBox5.Text;
            string contact = textBox6.Text;
            string email = textBox7.Text;
            string RegNo = textBox2.Text;
            if (string.IsNullOrEmpty(firstname) || !Regex.IsMatch(textBox4.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Firstname, type here again!");
                textBox4.Clear();
                firstname = textBox4.Text;
                conn.Close();
                return;

            }


            if (string.IsNullOrEmpty(lastname) || !Regex.IsMatch(textBox5.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Lastname,type here again!");
                textBox5.Clear();
                lastname = textBox5.Text;
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
            string q12 = string.Format("SELECT * FROM Person WHERE Email = '{0}'", textBox7.Text);
            SqlCommand c1 = new SqlCommand(q12, conn);
            SqlDataReader reader1 = c1.ExecuteReader();
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(textBox7.Text, @"^([\w]+)@([\w]+)\.([\w]+)$"))
            {
                MessageBox.Show("Invalid email type again");
                textBox7.Clear();
                email = textBox7.Text;
                conn.Close();
                return;

            }
            if (reader1.HasRows)
            {
                MessageBox.Show("Email has already been taken! Try another one!");
                textBox7.Clear();
                email = textBox7.Text;
                conn.Close();
                return;
            }
            reader1.Close();
            string q11 = string.Format("SELECT * FROM Student WHERE RegistrationNo = '{0}'", textBox2.Text);
            SqlCommand c = new SqlCommand(q11, conn);
            SqlDataReader reader = c.ExecuteReader();
            if (string.IsNullOrEmpty(RegNo) || !Regex.IsMatch(textBox2.Text, @"^\d{4}(-[A-Za-z][A-Za-z])(-\d{2,3})"))
            {
                
                MessageBox.Show("Invalid RegistrationNo,type here again.Kindly follow this format: 2016-CE-52");
                textBox2.Clear();
                RegNo = textBox2.Text;
                conn.Close();
                return;

            }
            if (reader.HasRows)
            {
                MessageBox.Show("Registration number has already been taken! Try another one!");
                textBox2.Clear();
                RegNo = textBox2.Text;
                conn.Close();
                
                return;
            }
            reader.Close();
            string gender;
            gender = string.Format("select Id from Lookup where Category='GENDER' AND Value = '{0}'", comboBox1.Text);
            SqlCommand cmd = new SqlCommand(gender, conn);
            int id = (Int32)cmd.ExecuteScalar();
            string query1 = String.Format("INSERT INTO Person(FirstName, LastName, Contact, Email,DateOfBirth,Gender) VALUES (@FirstName, @LastName," +
                "@Contact, @Email,@DateOfBirth,@Gender)");
            SqlCommand command1 = new SqlCommand(query1, conn);
            command1.Parameters.AddWithValue("@FirstName", textBox4.Text);
            command1.Parameters.AddWithValue("@LastName", textBox5.Text);
            command1.Parameters.AddWithValue("@Contact", textBox6.Text);
            command1.Parameters.AddWithValue("@Email", textBox7.Text);
            command1.Parameters.AddWithValue("@DateOfBirth", dateTimePicker1.Value);
            command1.Parameters.AddWithValue("@Gender", id);
            command1.ExecuteNonQuery();
            command1.Parameters.Clear();

            string P_id = string.Format("SELECT Id FROM Person WHERE Email= '{0}'", textBox7.Text);
            SqlCommand command3 = new SqlCommand(P_id, conn);
            int Id = (Int32)command3.ExecuteScalar();
            string query2 = "INSERT INTO Student(Id, RegistrationNo) VALUES (@Id, @RegistrationNo)";
            SqlCommand command2 = new SqlCommand(query2, conn);

            command2.Parameters.AddWithValue("@Id", Id);
            command2.Parameters.AddWithValue("@RegistrationNo", textBox2.Text);
            command2.ExecuteNonQuery();
            command2.Parameters.Clear();
            MessageBox.Show("Data inserted");
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();

            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Student.Id As [Id], RegistrationNo,FirstName AS [First Name],LastName AS [Last Name], " +
                 "Contact, Email, DateOfBirth, Gender FROM Student JOIN Person ON (Person.id=Student.id)", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView2.DataSource = dtbl;
            dataGridView2.Columns["Id"].Visible = false;
            conn.Close();
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void button13_Click(object sender, EventArgs e)
        {






        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
        string x;
        string y, fname, lname, regno;
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs grid)
        {

            if (grid.RowIndex < 0 || grid.RowIndex == dataGridView2.NewRowIndex)
            {
                return;
            }
            if (grid.ColumnIndex == dataGridView2.Columns["DELETE"].Index)
            {
                conn.Open();


                ////int selectedID = Convert.ToInt32(dataGridView2.Rows[grid.RowIndex].Cells[0].Value.ToString());
                DialogResult Check = MessageBox.Show("Do you really want to delete?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Check == DialogResult.Yes)
                {
                    string rnum = String.Format(dataGridView2.Rows[grid.RowIndex].Cells["Id"].Value.ToString());

                    string query2 = String.Format("DELETE FROM GroupStudent WHERE StudentId='{0}'", rnum);
                    SqlCommand cmd2 = new SqlCommand(query2, conn);
                    cmd2.ExecuteNonQuery();
                    string query1 = String.Format("DELETE FROM Student WHERE Id='{0}'", rnum);
                    SqlCommand cmd1 = new SqlCommand(query1, conn);
                    cmd1.ExecuteNonQuery();
                    string email = String.Format(dataGridView2.Rows[grid.RowIndex].Cells["Id"].Value.ToString());
                    cmd1.CommandText = string.Format("DELETE FROM Person WHERE Id='{0}'", rnum);
                    cmd1.ExecuteNonQuery();
                    MessageBox.Show("DELETED Successfully!!!");
                    SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Student.Id As [Id], RegistrationNo,FirstName AS [First Name],LastName AS [Last Name], " +
                "Contact, Email, DateOfBirth, Gender FROM Student JOIN Person ON (Person.id=Student.id)", conn);
                    DataTable dtbl = new DataTable();
                    sqlData.Fill(dtbl);
                    dataGridView2.DataSource = dtbl;
                    dataGridView2.Columns["Id"].Visible = false;
                    dataGridView2.Refresh();
                }

                    conn.Close();
                }
           
                if (grid.ColumnIndex == dataGridView2.Columns["UPDATE"].Index)
                {

                    conn.Open();
                    //z = dataGridView2.Rows[grid.RowIndex].Cells["ID"].Value.ToString();
                    textBox7.Text = dataGridView2.Rows[grid.RowIndex].Cells["Email"].Value.ToString();
                    textBox6.Text = dataGridView2.Rows[grid.RowIndex].Cells["Contact"].Value.ToString();
                    textBox2.Text = dataGridView2.Rows[grid.RowIndex].Cells["RegistrationNo"].Value.ToString();
                    textBox4.Text = dataGridView2.Rows[grid.RowIndex].Cells["First Name"].Value.ToString();
                    textBox5.Text = dataGridView2.Rows[grid.RowIndex].Cells["Last Name"].Value.ToString();
                
                    x = textBox7.Text;
                    y = textBox6.Text;
                    fname = textBox2.Text;
                    lname = textBox4.Text;
                    regno = textBox5.Text;
                    button4.Show();
                    button3.Hide();
                    conn.Close();


                }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            comboBox1.Items.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            var a = new management();
            a.Show();
           

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            string a = string.Format("UPDATE Person SET Email = @Email, Contact = @Contact , FirstName = @firstname," +
                " LastName = @lastname From Person JOIN Student " +
                        "On (Person.Id = Student.Id) WHERE Person.Email = '{0}'", x);
            SqlCommand cmd = new SqlCommand(a, conn);
            string firstname = textBox4.Text;
            string lastname = textBox5.Text;
            string contact = textBox6.Text;
            string email = textBox7.Text;
            string RegNo = textBox2.Text;
            if (string.IsNullOrEmpty(firstname) || !Regex.IsMatch(textBox4.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Firstname, type here again!");
                textBox4.Clear();
                firstname = textBox4.Text;
                conn.Close();
                return;

            }


            if (string.IsNullOrEmpty(lastname) || !Regex.IsMatch(textBox5.Text, @"[A-Za-z]+$"))
            {
                MessageBox.Show("Invalid Lastname,type here again!");
                textBox5.Clear();
                lastname = textBox5.Text;
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


            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(textBox7.Text, @"^([\w]+)@([\w]+)\.([\w]+)$"))
            {
                MessageBox.Show("Invalid email type again");
                textBox7.Clear();
                email = textBox7.Text;
                conn.Close();
                return;

            }


            if (string.IsNullOrEmpty(RegNo) || !Regex.IsMatch(textBox2.Text, @"^\d{4}(-[A-Za-z][A-Za-z])(-\d{2,3})"))
            {

                MessageBox.Show("Invalid RegistrationNo,type here again.Kindly follow this format: 2016-CE-52");
                textBox2.Clear();
                RegNo = textBox2.Text;
                conn.Close();
                return;

            }
            
            
            cmd.Parameters.AddWithValue("@Contact", textBox6.Text);
            cmd.Parameters.AddWithValue("@Email", textBox7.Text);
            cmd.Parameters.AddWithValue("@firstname", textBox4.Text);
            cmd.Parameters.AddWithValue("@lastname", textBox5.Text);
            
            cmd.ExecuteNonQuery();
            string b = string.Format("UPDATE Student SET RegistrationNo = @regno From Person JOIN Student " +
                        "On (Person.Id = Student.Id) WHERE Person.Email = '{0}'", x);
            SqlCommand cmd1 = new SqlCommand(b, conn);
           
            cmd1.Parameters.AddWithValue("@regno",RegNo);
            cmd1.ExecuteNonQuery();
            MessageBox.Show("UPDATED successfully!!!");
            textBox2.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Student.Id As [Id], RegistrationNo,FirstName AS [First Name],LastName AS [Last Name], " +
                "Contact, Email, DateOfBirth, Gender FROM Student JOIN Person ON (Person.id=Student.id)", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView2.DataSource = dtbl;
            dataGridView2.Columns["Id"].Visible = false;
            conn.Close();
            button4.Hide();
            button3.Show();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            button4.Hide();
            conn.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Student.Id AS [Id], RegistrationNo,FirstName AS [First Name],LastName AS [Last Name], " +
                 "Contact, Email, DateOfBirth, Gender FROM Student JOIN Person ON (Person.id=Student.id)", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView2.DataSource = dtbl;
            dataGridView2.Columns["Id"].Visible = false;


            var DELETE = new DataGridViewButtonColumn();
            DELETE.FlatStyle = FlatStyle.Popup;
            DELETE.HeaderText = "DELETE";
            DELETE.Name = "DELETE";
            DELETE.UseColumnTextForButtonValue = true;
            DELETE.Text = "DELETE";
            DELETE.Width = 60;
            this.dataGridView2.Columns.Add(DELETE);

            var UPDATE = new DataGridViewButtonColumn();
            UPDATE.Name = "UPDATE";
            UPDATE.HeaderText = "UPDATE";
            UPDATE.UseColumnTextForButtonValue = true;
            UPDATE.Text = "UPDATE";
            UPDATE.Width = 60;
            this.dataGridView2.Columns.Add(UPDATE);
            conn.Close();

        }
    }
    
    
}
