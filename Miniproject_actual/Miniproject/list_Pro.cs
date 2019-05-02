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
    public partial class list_Pro : Form
    {
        
        SqlConnection conn = new SqlConnection("Data Source=USER1\\SQLEXPRESS;Initial Catalog=ProjectA;Integrated Security=True");
        public list_Pro()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
           
           
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Project.Title, (FirstName+' '+LastName) AS [Advisor Name], " +
                "Student.RegistrationNo " +
                "FROM Project JOIN ProjectAdvisor " +
                "ON(Project.Id = ProjectAdvisor.ProjectId) " +
                "JOIN Advisor ON (ProjectAdvisor.AdvisorId = Advisor.Id) " +
                "JOIN Person ON (Person.Id = Advisor.Id) " +
                "JOIN GroupProject ON(GroupProject.ProjectId = Project.Id) " +
                "JOIN GroupStudent ON(GroupStudent.GroupId = GroupProject.GroupId) " +
                "JOIN Student ON (Student.Id = GroupStudent.StudentId) ", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            
            
            button2.Hide();
            button1.Show();
            conn.Close();

        }
        string t;
        
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs grid)
        {




        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void list_Pro_Load(object sender, EventArgs e)
        {
            button1.Hide();
            
        }

        
        private void button9_Click(object sender, EventArgs e)
        {
            this.Close();
            var a = new reports();
            a.Show();
        }
       
        private void button1_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            SqlDataAdapter sqlData = new SqlDataAdapter("SELECT Project.Title, (FirstName+' '+LastName) AS [Advisor Name], " +
               "Student.RegistrationNo " +
               "FROM Project JOIN ProjectAdvisor " +
               "ON(Project.Id = ProjectAdvisor.ProjectId) " +
               "JOIN Advisor ON (ProjectAdvisor.AdvisorId = Advisor.Id) " +
               "JOIN Person ON (Person.Id = Advisor.Id) " +
               "JOIN GroupProject ON(GroupProject.ProjectId = Project.Id) " +
               "JOIN GroupStudent ON(GroupStudent.GroupId = GroupProject.GroupId) " +
               "JOIN Student ON (Student.Id = GroupStudent.StudentId) ", conn);
            DataTable dtbl = new DataTable();
            sqlData.Fill(dtbl);
            dataGridView1.DataSource = dtbl;
            MessageBox.Show("Report has been Successfully Generated!");
            



            PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);
            Document doc = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter wri = PdfWriter.GetInstance(doc, new FileStream("ProjectList_Report.pdf", FileMode.Create));
            doc.Open();
            Paragraph para = new Paragraph("Project list Report");
            Paragraph p = new Paragraph("----------------------------------------------------------------------------------------------------------------------------------------");
            BaseFont btn = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
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

            
            conn.Close();
            
        }
    }
}
