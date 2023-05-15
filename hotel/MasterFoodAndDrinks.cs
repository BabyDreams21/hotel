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
    public partial class MasterFoodAndDrinks : Form
    {
        int id, cond;
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd;
        SqlDataReader rd;
        public MasterFoodAndDrinks()
        {
            InitializeComponent();
            loadgrid();
            dis();
        }

        void clear()
        {
            textBox2.Clear();
            textBox1.Clear();
            pictureBox1.Image = null;
        }

        void loadgrid()
        {
            string com = "select * from FoodAndDrinks";
            dataGridView1.DataSource = Command.GetData(com);
            dataGridView1.Columns[4].Visible= false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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
            button6.Enabled = false;
            comboBox1.Enabled = false;
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
            button6.Enabled = true;
            comboBox1.Enabled = true;
        }

        //bool val()
        //{
        //    if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || comboBox1.Text.Length < 1 && pictureBox1.Image == null)
        //    {
        //        MessageBox.Show("Semua kolom harus diisi!!!", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    cmd = new SqlCommand("Select * from FoodAndDrinks where Name '" + textBox1.Text + "'", con);
        //    con.Open();
        //    rd = cmd.ExecuteReader();
        //    rd.Read();
        //    if (rd.HasRows)
        //    {

        //        MessageBox.Show("Nama telah dipakai!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //    con.Close();
        //    return true;
        //}

        //bool valup()
        //{
        //    if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || comboBox1.Text.Length < 1 && pictureBox1.Image == null)
        //    {
        //        MessageBox.Show("Semua kolom harus diisi!!!", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }

        //    cmd = new SqlCommand("Select * from FoodAndDrinks where Name '" + textBox1.Text + "'", con);
        //    con.Open();
        //    rd = cmd.ExecuteReader();
        //    rd.Read();
        //    if (rd.HasRows)
        //    {
        //        con.Close();
        //        if(Convert.ToInt32 (rd ["ID"]) != id)
        //        MessageBox.Show("Nama telah dipakai!!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //    con.Close();
        //    return true;
        //}

        bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || pictureBox1.Image == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from FoodAndDrinks where Name = '" + textBox2.Text + "'", con);
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                con.Close();
                MessageBox.Show("Name was used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            con.Close();
            return true;
        }

        bool val_up()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || pictureBox1.Image == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from FoodAndDrinks where Name = '" + textBox2.Text + "'", con);
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                if (Convert.ToInt32(reader["ID"]) != id)
                {
                    con.Close();
                    MessageBox.Show("Name was used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            con.Close();
            return true;
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
            MemoryStream ms = new MemoryStream(b);
            Image img = Image.FromStream(ms);
            Bitmap bmp = (Bitmap)img;
            pictureBox1.Image = bmp;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cond = 1 ;
            enable();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                cond = 2;
                enable();
            }
            else
            {
                MessageBox.Show("Tolong pilih item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images | *.png;*.jpg;*.jpeg;*.bmp;";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin menghapus item?", "Konfirmasi ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlCommand cmd = new SqlCommand("Delete from FoodAndDrinks where ID=" + id, con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sukses!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    dis();
                    loadgrid();
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

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (cond == 1 && val())
            {
                ImageConverter convert = new ImageConverter();
                byte[] b = (byte[])convert.ConvertTo(pictureBox1.Image, typeof(byte[]));

                SqlCommand cmd = new SqlCommand("INSERT INTO FoodAndDrinks values('"+textBox1.Text+ "','" + comboBox1.Text + "','" + textBox2.Text + "',@img)", con);
                cmd.Parameters.AddWithValue("@img", b);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data sukses di Insert !!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    dis();                   
                    loadgrid();
                }catch(Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    con.Close();
                }

            }
            else if(cond == 2 && val_up())
            {
                ImageConverter conv = new ImageConverter();
                byte[] b = (byte[])conv.ConvertTo(pictureBox1.Image, typeof(byte[]));
                cmd = new SqlCommand("UPDATE FoodAndDrinks set Name='" + textBox1.Text + "',Type='" + comboBox1.Text + "',Price='" + textBox2.Text + "',Photo = @img WHERE ID=" + id, con);
                cmd.Parameters.AddWithValue("@img", b);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data sukses di update!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dis();
                    clear();
                    loadgrid();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    con.Close();
                }



            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dis();
            clear();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
           
                string com = "select * from FoodAndDrinks where Name like '%" + textBox4.Text + "%'";
                dataGridView1.DataSource = Command.GetData(com);
                       
        }
    }
}
