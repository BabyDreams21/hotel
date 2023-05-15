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

namespace hotel
{
    public partial class CheckIn : Form
    {
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd;
        SqlDataReader rd;
        int idcus, cond;
        String gender;
        public CheckIn()
        {
            InitializeComponent();

        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        bool val()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || dateTimePicker1.Value == null || !radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (radioButton1.Checked && radioButton2.Checked)
            {
                MessageBox.Show("Please select a gender!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dateTimePicker1.Value.Date > DateTime.Now.Date)
            {
                MessageBox.Show("Please isert a correct date of birth!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox5.TextLength != 16)
            {
                MessageBox.Show("NIK must be 16 characters!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            cmd = new SqlCommand("select * from customer where nik = '" + textBox5.Text + "'", con);
            con.Open();
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                con.Close();
                MessageBox.Show("NIK was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            con.Close();

            cmd = new SqlCommand("select * from customer where phoneNumber = '" + textBox2.Text + "'", con);
            con.Open();
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                con.Close();
                MessageBox.Show("Phone number was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            con.Close();
            return true;
        }

        bool valup()
        {
            if (textBox2.TextLength > 1 || textBox3.TextLength > 1 || textBox4.TextLength > 1 || textBox5.TextLength > 1 || dateTimePicker1.Value == null || !radioButton1.Checked && !radioButton2.Checked)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }else if( radioButton2.Checked || radioButton2.Checked)
            {
                MessageBox.Show(" Please select a your gender!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }else if(dateTimePicker1.Value == null)
            {
                MessageBox.Show("Please select your Date of birth!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }else if(textBox5.TextLength > 15)
            {
                MessageBox.Show("NIK must be 15 character!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            cmd = new SqlCommand("Select * from Customer where NIK = '" + textBox5.Text + "'",con);
            con.Open();
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows && idcus!= rd.GetInt32(0))
            {
                con.Close();
                MessageBox.Show("NIK was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            cmd = new SqlCommand("Select * from Customer where PhoneNumber= '" + textBox2.Text + "'",con);
            con.Open();
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                con.Close();
                MessageBox.Show("Phone was already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string com = " select * from VCheckin where BookingCode = '" + textBox1.Text + "'";

            cmd = new SqlCommand("select * from Reservation where BookingCode = '" + textBox1.Text + "'", con);
            con.Open();
            rd = cmd.ExecuteReader();
            rd.Read();
            if (!rd.HasRows)
            {
                con.Close();
                MessageBox.Show("Wrong booking code", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                con.Close();
                dataGridView1.DataSource = Command.GetData(com);
                //dataGridView1.Columns[2].Visible = false;
                //dataGridView1.Columns[3].Visible = false;
                //dataGridView1.Columns[5].Visible = false;
                //dataGridView1.Columns[6].Visible = false;
                //dataGridView1.Columns[7].Visible = false;
                //dataGridView1.Columns[8].Visible = false;
                //dataGridView1.Columns[9].Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.RowCount > 1)
            //{
            //    for (int i = 0;i < dataGridView1.RowCount; i++)
            //    {
            //        string sql  = "Update ReservationRoom set CheckInDateTime = getDate() where ID ="+dataGridView1.Rows[i].Cells[2].Value;
            //        try
            //        {
            //            Command.exec(sql);
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show("" + ex);
            //        }
            //        finally
            //        {
            //            con.Close();
            //        }
            //    }
            //    if (textBox2.TextLength > 0)
            //    {
            //        cust();

            //    }
            //}

            TimeSpan ts = new TimeSpan(DateTime.Now.Ticks - dateTimePicker1.Value.Ticks);
            int age = Convert.ToInt32(ts.Days) / 365;
            if (radioButton1.Checked)
            {
                gender = "Male";
            }
            else if (radioButton2.Checked)
            {
                gender = "Female";
            }
            if (idcus != 0 && valup())
            {
                string sql = "update customer set Name = '" + textBox3.Text + "', NIK = '" + textBox5.Text + "', Email = '" + textBox4.Text + "', Gender = '" + gender + "', age = " + age + ", DateOfBirth = '" + Convert.ToDateTime(dateTimePicker1.Value) + "' where id = " + idcus;
                try
                {
                    Command.exec(sql);
                    MessageBox.Show("Successfully edited customer!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
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
            else if (idcus == 0)
            {
                SqlCommand command = new SqlCommand("insert into Customer(NIK, Email, Name, Gender, PhoneNumber, Age, DateOfBirth) values ('" + textBox5.Text + "', '" + textBox4.Text + "', '" + textBox3.Text + "', '" + gender + "', '" + textBox2.Text + "', " + age + ", '" + Convert.ToDateTime(dateTimePicker1.Value) + "')", con);
                try
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Successfully added new customer!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Reservation reservation = new Reservation();
                    reservation.ShowDialog();
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

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            cmd = new SqlCommand("select top(1) * from Customer where PhoneNumber like '%" + textBox2.Text + "%'", con);
            con.Open();
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                //cond = 1;
                //idcus = rd.GetInt32(0);
                //textBox3.Text = rd.GetString(1);
                //textBox4.Text = rd.GetString(3);
                //if (rd.GetString(4) == "Male")
                //    radioButton1.Checked = true;
                //else
                //    radioButton2.Checked = true;
                //textBox5.Text = rd.GetString(2);
                //dateTimePicker1.Value = rd.GetDateTime(4);
                con.Close();
            }
            else
            {
                con.Close();
                cond = 2;
            }
        }

        void cust()
        {
            if (cond == 1)
            {
                if (val())
                {
                    string g = "";
                    if (radioButton1.Checked)
                    {
                        g = " Male";
                    }else if (radioButton2.Checked)
                    {
                        g = "Female";
                    }
                    int i = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                    cmd = new SqlCommand("Insert into Customer  values('" + textBox3.Text.Replace("'", "'") + "','" + textBox5.Text + "','" + textBox4.Text.Replace("'", "'") + "','" + g + "','" + textBox2.Text + "','" + dateTimePicker1.Value.Date + "'," + i + ")", con);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sukses!!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }catch ( Exception x)
                    {
                        MessageBox.Show("" + x.Message);
                    }
                    finally
                    {
                        con.Close();
                    }

                }
            }else if (valup() && cond == 2)
            {
                string g = "";
                if (radioButton1.Checked)
                {
                    g = "male";
                }else if (radioButton2.Checked)
                {
                    g = "female";
                }
                int i = Convert.ToInt32(DateTime.Now.ToString("yyyy")) - Convert.ToInt32(dateTimePicker1.Value.ToString("yyyy"));
                cmd = new SqlCommand("Update Customer set Name = '" + textBox3.Text.Replace("'", "'") + "',NIK = '" + textBox5.Text + "',Email = '" + textBox4.Text + "','" + g + "',DateOfBirth = '" + dateTimePicker1.Value.Date + "',Age = " + i + ",PhoneNumber = '" + textBox2.Text + "' where ID= " + idcus, con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sukses!!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }catch(Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

    }
}
