using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InsertWindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection();
        int Sr_No = 0;
        SqlCommand cmddel;
        public Form1()
        {
            InitializeComponent();
            refreshdata();
            datagrid();
        }
       
        public void datagrid()
        {
            //con.ConnectionString = ConfigurationManager.ConnectionStrings["myconn"].ToString();
            //SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Regi", con);  
            // DataSet ds = new DataSet();
            //da.Fill(ds, "Regi");
            //dataGridView1.DataSource = ds.Tables["Regi"].DefaultView;
            con.Open();
            DataTable dt = new DataTable();
            SqlDataAdapter  adapt = new SqlDataAdapter("select * from Regi", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        public void refreshdata()
        {
            DataRow dr;
            con.ConnectionString = ConfigurationManager.ConnectionStrings["myconn"].ToString();
            con.Open();
            SqlCommand cmd = new SqlCommand("SPCity", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            dr = dt.NewRow();
            dr.ItemArray = new object[] { 0, "--Select City--" };
            dt.Rows.InsertAt(dr, 0);

            comboBox1.ValueMember = "No";

            comboBox1.DisplayMember = "CityName";
            comboBox1.DataSource = dt;

            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string mobile = textBox4.Text.Trim();
                
                Regex mRegxExpression;
                Regex phoneNumpattern = new Regex(@"\+[0-9]{3}\s+[0-9]{3}\s+[0-9]{5}\s+[0-9]{3}");
                if(textBox1.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Please Enter UserName");
                }
                else if(textBox2.Text.Trim().Equals(""))
                {   
                    MessageBox.Show("Please Enter Address");
                }
                else if (textBox3.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Please Enter Email Address");
                }
                

                else if (textBox3.Text.Trim() != string.Empty)

                {

                    mRegxExpression = new Regex(@"^([a-zA-Z0-9_\-])([a-zA-Z0-9_\-\.]*)@(\[((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}|((([a-zA-Z0-9\-]+)\.)+))([a-zA-Z]{2,}|(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\])$");

                    if (!mRegxExpression.IsMatch(textBox3.Text.Trim()))

                    {

                        MessageBox.Show("E-mail address format is not correct.", "MojoCRM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        textBox3.Focus();

                    }
                    else if (comboBox1.Text.Trim().Equals("--Select City--"))
                    {
                        string myStringVariable3 = string.Empty;
                        MessageBox.Show("Select CityName");
                    }
                    else if (mobile.Equals(""))
                    {
                        
                            MessageBox.Show("Please Enter Mobile");
                    }

                     else if (mobile.Length > 10 || mobile.Length <= 9)
                        {
                            MessageBox.Show("Please Enter Valid Mobile");
                        }
                    else
                    {
                        con.ConnectionString = ConfigurationManager.ConnectionStrings["myconn"].ToString();
                        con.Open();

                        
                        SqlCommand cmddel = new SqlCommand("Exist", con);  //creating  SqlCommand  object  
                        cmddel.CommandType = CommandType.StoredProcedure;  //here we declaring command type as stored Procedure  
                        cmddel.Parameters.AddWithValue("@Email", textBox3.Text.ToString());        //first Name  
                        DataTable dt = new DataTable();
                        SqlDataAdapter sa = new SqlDataAdapter(cmddel);                                 
                        sa.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            MessageBox.Show("Your Emaild Is Alredy Exists");
                            con.Close();
                        }
                        else
                        {
                            //string query= "insert into Regi(UserName,Address,Email,City,MobileNo) values('"+textBox1.Text.ToString()+ "','" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" + comboBox1.Text.ToString() + "','" + textBox4.Text.ToString() + "')";
                            SqlCommand com1 = new SqlCommand("Insert", con);  //creating  SqlCommand  object  
                            com1.CommandType = CommandType.StoredProcedure;  //here we declaring command type as stored Procedure  
                            com1.Parameters.AddWithValue("@UserName", textBox1.Text.ToString());        //first Name  
                            com1.Parameters.AddWithValue("@Address ", textBox2.Text.ToString());     //middle Name  
                            com1.Parameters.AddWithValue("@Email ", textBox3.Text.ToString());       //Last Name  
                            com1.Parameters.AddWithValue("@City", comboBox1.Text.ToString());        //first Name  
                            com1.Parameters.AddWithValue("@MobileNo ", textBox4.Text.ToString());     //middle Name  
                            com1.ExecuteNonQuery();


                            MessageBox.Show("Records are Submitted Successfully");

                            con.Close();

                            Mail();
                            datagrid();
                            ClearData();
                        }
                    }
                    

                }
               


           }
            catch
            {
                MessageBox.Show("error");
            }
           

    }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Sr_No = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void ClearData()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            comboBox1.Text = "--Select City--";
            textBox4.Text = "";
            Sr_No = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && comboBox1.Text != "" && textBox4.Text != "")
            {
                //cmddel = new SqlCommand("update Regi set UserName=@UserName,Address=@Address,Email=@Email,City=@City,MobileNo=@MobileNo where Sr_No=@Sr_No", con);
                cmddel = new SqlCommand("update", con);
                cmddel.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmddel.Parameters.AddWithValue("@Sr_No", Sr_No);
                cmddel.Parameters.AddWithValue("@UserName", textBox1.Text);
                cmddel.Parameters.AddWithValue("@Address", textBox2.Text);
                cmddel.Parameters.AddWithValue("@Email", textBox3.Text);
                cmddel.Parameters.AddWithValue("@City", comboBox1.Text);
                cmddel.Parameters.AddWithValue("@MobileNo", textBox4.Text);
                cmddel.ExecuteNonQuery();
                MessageBox.Show("Record Updated Successfully");
                con.Close();
                Mail();
                datagrid();
                ClearData();
               
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Sr_No != 0)
            {
                //  cmddel = new SqlCommand("delete Regi where Sr_No=@Sr_No", con);
                cmddel = new SqlCommand("delete", con);
                cmddel.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmddel.Parameters.AddWithValue("@Sr_No", Sr_No);

                cmddel.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Deleted Successfully!");
                Mail();
                datagrid();
                ClearData();
               
            }
            else
            {
                MessageBox.Show("Please Select Record to Delete");
            }
        }
            public void Mail()
           {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["myconn"].ToString();
            conn.Open();
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress("jignesh.ranait@gmail.com");
            message.To.Add(new MailAddress(textBox3.Text));
            message.Subject = "Test";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = "Hello";
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com"; //for gmail host  
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("jignesh.ranait@gmail.com", "jignesh20894");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {

                smtp.Send(message);
            }
            catch
            { }
            conn.Close();
        }

        

        private void button4_Click_1(object sender, EventArgs e)
        {
            Form2 f1 = new Form2();
            this.Hide();

            f1.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearData();
        }
    }
}
