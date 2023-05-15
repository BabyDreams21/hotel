using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel
{
    public partial class MasterRoomType : Form
    {
        int id, cond;
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd = new SqlCommand();
        SqlDataReader rd;

        public MasterRoomType()
        {
            InitializeComponent();
            dis();
            loadgrid();
        }

       bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || numericUpDown1.Value < 1|| pictureBox1.Image == null)
            {
                MessageBox.Show("Semua kolom harus diisi!!", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            

            cmd = new SqlCommand("Select * from RoomType where Name = '" + textBox1.Text + "'",con);
            con.Open();
            rd = cmd.ExecuteReader();
            if (rd.HasRows)
            {
                con.Close();
                MessageBox.Show("Nama telah dipakai!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            con.Close();
            return true;
        }

        bool val_up()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || numericUpDown1.Value < 1 || pictureBox1.Image == null)
            {
                MessageBox.Show("Semua kolom harus diisi!!!", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            

            cmd = new SqlCommand(" Select * from RoomType where Name = '" + textBox1.Text + "'", con);
            con.Open();
            rd = cmd.ExecuteReader();
            if (rd.HasRows)
            {
               
                if (Convert.ToInt32(rd["ID"]) != id)
                {
                    con.Close();
                    MessageBox.Show("Semua kolom harus diisi!!!", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
            }
            con.Close();
            return true;
        }

        void clear()
        {
            textBox1.Clear();
            numericUpDown1.Value= 0;
            pictureBox1.Image = null;
            textBox2.Clear();
        }

        void dis()
        {
            button2.Enabled = true;
            button3.Enabled = true;
            btn_insert.Enabled = true;
            button5.Enabled = false;
            button4.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;
            button6.Enabled = false;
            numericUpDown1.Enabled = false;
        }

        void enable()
        {
            btn_insert.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
            button5.Enabled = true;
            
            textBox1.Enabled = true;    
            textBox2.Enabled = true;
            button6.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if( dataGridView1.CurrentRow.Selected == true)
            {
                cond = 2 ;
                enable ();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                DialogResult result = MessageBox.Show("Anda yakin ingin menghapus item??", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    cmd = new SqlCommand("Delete from RoomType where ID=" + id, con);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sukses!!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void MasterRoomType_Load(object sender, EventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images | *.png;*.jpeg;*.jpg;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image img = Image.FromFile(ofd.FileName);
                Bitmap bmp = (Bitmap)img;
                pictureBox1.Image = bmp;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cond == 1 && val())
            {
                ImageConverter img = new ImageConverter();
                byte[] b = (byte[])img.ConvertTo(pictureBox1.Image, typeof(byte[]));

                cmd = new SqlCommand("Insert Into RoomType values ('" + textBox1.Text + "','" + Convert.ToInt32(numericUpDown1.Text) + "','" + textBox2.Text + "',@img)",con);
                cmd.Parameters.AddWithValue("@img", b);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully Inserted!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
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
            else if (cond == 2 && val_up())
            {
                ImageConverter conv = new ImageConverter();
                byte[] b = (byte[])conv.ConvertTo(pictureBox1.Image, typeof(byte[]));

                cmd = new SqlCommand("Update RoomType set Name = '" + textBox1.Text + "',Capacity = '" + numericUpDown1.Value + "',RoomPrice = " +Convert.ToInt32 (textBox2.Text) + ",@img",con);
                cmd.Parameters.AddWithValue("@img", b);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sukses!!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true) ;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            numericUpDown1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            byte[] b = (byte[])dataGridView1.SelectedRows[0].Cells[4].Value;
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromStream(ms);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult ressult = MessageBox.Show("Anda yakin ingin keluar ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ressult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        void loadgrid()
        {
            string sql = "Select * from RoomType";
            dataGridView1.DataSource = Command.GetData(sql);
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
