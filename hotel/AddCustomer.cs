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
    public partial class AddCustomer : Form
    {
        SqlDataReader rd;
        SqlConnection con = new SqlConnection(Utils.conn);
        string gender;
        public AddCustomer()
        {
            InitializeComponent();
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox1.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || dateTimePicker1.Value == null || radioButton1.Checked && radioButton2.Checked)
            {
                MessageBox.Show("All field must be filled!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand cmd = new SqlCommand ("Select * from Customer Where NIK = '"+textBox4.Text+"'",con);
            con.Open();
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                con.Close();
                MessageBox.Show("NIK already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            con.Close();
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (val())
            {
                TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dateTimePicker1.Value.Ticks);
                int age = Convert.ToInt32(ts.Days) / 365;
                if (radioButton1.Checked)
                {
                    gender = "Male";
                }else if (radioButton2.Checked)
                {
                    gender = "Female";
                }

                SqlCommand cmd = new SqlCommand("Insert into Customer(NIK,Email,Gender,Name,PhoneNumber,Age,DateOfBirth)values('" + textBox4.Text + "','" + textBox2.Text + "','" + gender + "','" + textBox1.Text + "','" + textBox3.Text + "'," + age + ",'" + Convert.ToDateTime(dateTimePicker1.Value) + "')",con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully added new customer!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
        }
    }
}
