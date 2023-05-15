using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel
{
    public partial class Master_Employee : Form
    {
        int cond, id;
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd = new SqlCommand();
        public Master_Employee()
        {
            InitializeComponent();
            loadgrid();
            loadjob();
            dis();
        }

        void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            dateTimePicker1.Value = DateTime.Now;
            pictureBox1.Image = null;
        }

        void enable()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            button6.Enabled = true;
            dateTimePicker1.Enabled = true;
            comboBox1.Enabled = true;
        }

        void dis()
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            button6.Enabled = false;
            dateTimePicker1.Enabled = false;
            comboBox1.Enabled = false;
        }

        void loadgrid()
        {
            string com = "Select * from emp_view";
            dataGridView1.DataSource = Command.GetData(com);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
        }

        void loadjob()
        {
            string com = "select * from Job";
            comboBox1.DataSource = Command.GetData(com);
            comboBox1.DisplayMember = "name";
            comboBox1.ValueMember = "id";
            //cmd = new SqlCommand("Select * from Job", con);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //da.Fill(dt);
            //comboBox1.DataSource = dt;
            //comboBox1.DisplayMember = "Name";
            //comboBox1.ValueMember = "ID";

        }

        bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || textBox6.TextLength < 1 || dateTimePicker1.Value == null || pictureBox1.Image == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("Semua kolo harus diisi", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("Confirm Password Tidak Sama", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (textBox2.TextLength < 8 || textBox3.TextLength < 8)
            {
                MessageBox.Show("Password dan Confirm Password harus 8 characters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SqlCommand command = new SqlCommand("select * from Employee where username = '" + textBox2.Text + "'", con);
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Username telah diapakai!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            con.Close();
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                cond = 2;
                enable();
                textBox2.Enabled = false;
                textBox3.Enabled = false;
            }
            else
            {
                MessageBox.Show("Tolong pilih item!", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                DialogResult ressult = MessageBox.Show("Anda yakin ingin menghapus item ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if ( ressult == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("Delete from Employee where id=" + id, con);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        loadgrid();
                        dis();
                    }
                    catch(Exception x)
                    {
                        MessageBox.Show(x.Message);
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Tolong pilih Item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images|*.png;*.jpg;*.jpeg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == false)
            {
               
                textBox2.UseSystemPasswordChar = true;
                textBox3.UseSystemPasswordChar = true;
            }
            else
            {
                textBox2.UseSystemPasswordChar = false;
                textBox3.UseSystemPasswordChar = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cond == 1)
            {
                ImageConverter convert = new ImageConverter();
                byte[] b = (byte[])convert.ConvertTo(pictureBox1.Image,typeof(byte[]));

                SqlCommand cmd = new SqlCommand(" Insert into Employee values ('" + textBox1.Text + "','" + Encrypt.enc(textBox2.Text) + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox6.Text + "','" + Convert.ToDateTime(dateTimePicker1.Value) + "','" + comboBox1.SelectedValue + "',@img)", con);

                cmd.Parameters.AddWithValue("@img", b);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Berhasil di insert!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
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

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
            dis();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult ressult = MessageBox.Show("Anda yakin ingin keluar ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ressult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox5.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox6.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
           
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[6].Value);
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[7].Value);

            if (dataGridView1.SelectedRows[0].Cells[8].Value != System.DBNull.Value)
            {
                byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[8].Value;
                MemoryStream stream = new MemoryStream(b);
                Image img = Image.FromStream(stream);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pictureBox1.Image = null;

            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            { 
                string com = "select * from emp_view where name like '%" + textBox7.Text + "%'";
                dataGridView1.DataSource = Command.GetData(com);
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
            }
            //else
            //{
            //    MessageBox.Show(" Enter pressed ");
            //}
        }

        bool val_up()
        {
            if (textBox1.TextLength < 1 || textBox4.TextLength < 1 || textBox3.TextLength < 1 || textBox6.TextLength < 1 || dateTimePicker1.Value == null || pictureBox1.Image == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            SqlCommand command = new SqlCommand("select * from Employee where Username = '" + textBox1.Text + "'", con);
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                if (Convert.ToInt32(reader["id"]) != id)
                {
                    con.Close();
                    MessageBox.Show("Username telah dipakai!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            con.Close();
            return true;
        }

    }
}
