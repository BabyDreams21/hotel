using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel
{
    public partial class MainLogin : Form
    {
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd;
        
        public MainLogin()
        {
            InitializeComponent();
            textBox1.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string pass = Encrypt.enc(textBox2.Text);
                SqlCommand command = new SqlCommand("select * from Employee where Username = '" + textBox1.Text + "' and Password = @pass", con);
                con.Open();
                command.Parameters.AddWithValue("@pass", pass);
                SqlDataReader reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Session.id = Convert.ToInt32(reader["ID"]);
                    Session.JobID = Convert.ToInt32(reader["JoBID"]);
                    Session.name = reader["Name"].ToString();
                    Session.Username = reader["Username"].ToString();
                    Session.Email = reader["Email"].ToString();
                    Session.Address = reader["Address"].ToString();
                    con.Close();
                    if (Session.JobID == 1)
                    {
                        MainAdmin ma = new MainAdmin();
                        this.Hide();
                        ma.ShowDialog();
                    }
                    else if (Session.JobID == 2)
                    {
                        MainFrontOffice ma = new MainFrontOffice();
                        this.Hide();
                        ma.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Cek kembali Username dan Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                con.Close();
                
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                textBox2.UseSystemPasswordChar = true;
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
            }
        }
    }
}
