using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertWindowsFormsApplication2
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CrystalReport1 cr = new CrystalReport1();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["myconn"].ToString();
            string sql = "Select * from regi1 where UserName='" + textBox1.Text + "' ";
            DataSet ds = new DataSet();
            SqlDataAdapter sd = new SqlDataAdapter(sql, conn);
            sd.Fill(ds, "regi1");

            DataTable dt = ds.Tables["regi1"];
            try
            {
                if (dt.Rows.Count > 0)
                {
                    cr.SetDataSource(ds.Tables["regi1"]);
                    crystalReportViewer1.ReportSource = cr;
                    crystalReportViewer1.Refresh();
                }
                else
                {
                    MessageBox.Show("Data is not avaliable");
                    crystalReportViewer1.RefreshReport();

                }
            }
            catch
            {
                MessageBox.Show("This some time error occures");

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            CrystalReport1 cr = new CrystalReport1();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["myconn"].ToString();
            string sql = "Select * from Regi";
            DataSet ds = new DataSet();
            SqlDataAdapter sd = new SqlDataAdapter(sql, conn);
            sd.Fill(ds, "Regi");

            DataTable dt = ds.Tables["Regi"];
            try
            {
                if (dt.Rows.Count > 0)
                {
                    cr.SetDataSource(ds.Tables["Regi"]);
                    crystalReportViewer1.ReportSource = cr;
                    crystalReportViewer1.Refresh();
                }
                else
                {
                    MessageBox.Show("Data is not avaliable");
                    crystalReportViewer1.RefreshReport();

                }
            }
            catch
            {
                MessageBox.Show("This some time error occures");

            }
        }
    }
}
