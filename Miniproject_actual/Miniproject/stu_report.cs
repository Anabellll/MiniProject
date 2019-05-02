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
using System.Net.Mail;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;


namespace Miniproject
{
    public partial class stu_report : Form
    {
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public stu_report()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new reports();
            a.Show();
        }

        private void stu_report_Load(object sender, EventArgs e)
        {
            conn.Open();
            button1.Hide();
            SqlCommand sc = new SqlCommand("select RegistrationNo FROM Student", conn);
            SqlDataReader reader12;

            reader12 = sc.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("RegistrationNo", typeof(string));
            dt.Load(reader12);
            comboBox1.ValueMember = "RegistrationNo";
            comboBox1.DataSource = dt;
            conn.Close();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            string id = string.Format("SELECT Id FROM Student WHERE RegistrationNo = '{0}'", comboBox1.Text);
            SqlCommand c1 = new SqlCommand(id, conn);
            int id1 = (Int32)c1.ExecuteScalar();
            string query1 = string.Format("SELECT (FirstName + ' '+ LastName) AS [Name], RegistrationNo, " +
                "[GroupStudent].Status, Project.Title, Evaluation.TotalMarks, " +
                "[GroupEvaluation].ObtainedMarks " +
                "FROM Person JOIN Student ON(Person.Id = Student.Id) " +
                "JOIN GroupStudent ON(Student.Id = GroupStudent.StudentId) " +
                "JOIN GroupProject ON (GroupStudent.GroupId = GroupProject.GroupId) " +
                "JOIN Project ON (Project.Id = GroupProject.ProjectId) " +
                "JOIN ProjectAdvisor ON(ProjectAdvisor.ProjectId = Project.Id) " +
                "JOIN GroupEvaluation ON(GroupEvaluation.GroupId = GroupStudent.GroupId) " +
                "JOIN Evaluation ON(Evaluation.Id = GroupEvaluation.EvaluationId) " +
                "WHERE Student.Id = '{0}'", id1);
            
            SqlDataAdapter sqlData = new SqlDataAdapter(query1, conn);
            
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            button2.Hide();
            button1.Show();
            conn.Close();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.Open();
            string id = string.Format("SELECT Id FROM Student WHERE RegistrationNo = '{0}'", comboBox1.Text);
            SqlCommand c1 = new SqlCommand(id, conn);
            int id1 = (Int32)c1.ExecuteScalar();
            string query1 = string.Format("SELECT (FirstName + ' '+ LastName) AS [Name], RegistrationNo, " +
                "[GroupStudent].Status, Project.Title, Evaluation.TotalMarks, " +
                "[GroupEvaluation].ObtainedMarks " +
                "FROM Person JOIN Student ON(Person.Id = Student.Id) " +
                "JOIN GroupStudent ON(Student.Id = GroupStudent.StudentId) " +
                "JOIN GroupProject ON (GroupStudent.GroupId = GroupProject.GroupId) " +
                "JOIN Project ON (Project.Id = GroupProject.ProjectId) " +
                "JOIN ProjectAdvisor ON(ProjectAdvisor.ProjectId = Project.Id) " +
                "JOIN GroupEvaluation ON(GroupEvaluation.GroupId = GroupStudent.GroupId) " +
                "JOIN Evaluation ON(Evaluation.Id = GroupEvaluation.EvaluationId) " +
                "WHERE Student.Id = '{0}'", id1);

            SqlDataAdapter sqlData = new SqlDataAdapter(query1, conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("Student_Report.pdf", FileMode.Create));
            doc.Open();
            Paragraph para = new Paragraph("Student Information Report");
            Paragraph p = new Paragraph("----------------------------------------------------------------------------------------------------------------------------------------");
           
            for (int i = 0; i < dtbl.Columns.Count; i++)
            {
                PdfPCell cell = new PdfPCell();
                cell.AddElement(new Chunk(dtbl.Columns[i].ToString()));
                table.AddCell(cell);
            }

            for (int i = 0; i < dtbl.Rows.Count; i++)
            {
                for (int j = 0; j < dtbl.Columns.Count; j++)
                {

                    table.AddCell(dtbl.Rows[i][j].ToString());
                }

            }
            doc.Add(para);
            doc.Add(p);
            doc.Add(table);
            doc.Close();
            wri.Close();
            MessageBox.Show("Report has been Successfully Generated!");

            conn.Close();
        }
    }
}
