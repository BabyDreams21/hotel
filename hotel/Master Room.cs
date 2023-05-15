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
    public partial class Master_Room : Form
    {
        int id, cond;
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd = new SqlCommand();

        public Master_Room()
        {
            InitializeComponent();
            loadgrid();
            loadtype();
            dis();
        }

        int getnum()
        {
            int room;
            cmd = new SqlCommand("Select top (1) * from Room Order by ID desc",con);
            con.Open();
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                room = Convert.ToInt32(rd["ID"] ) + 1;
                
            }
            else
            {
                room = 1;
            }
            con.Close();
            return room;
        }

        void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedItem = null;
        }

        void loadgrid()
        {
            string sql = "Select * from room_view ";
            dataGridView1.DataSource = Command.GetData(sql);
            //dataGridView1.Columns[0].Visible = false;
            
        }

        void loadtype()
        {
            string sql = "Select * from RoomType";
            comboBox1.DataSource = Command.GetData(sql);
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
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
        }

        bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength < 1 || comboBox1.Text.Length <1)
            {
                MessageBox.Show("All fields must be filled!!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin mengahapus item", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                cmd = new SqlCommand("Delete from Room where ID=" + id, con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    clear();
                    loadgrid();
                    dis();
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
            }
            else
            {
                MessageBox.Show("Tolong pilih Item", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            clear();
            dis();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from room_view where RoomNumber like '%" + textBox1.Text + "%' or Name like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.GetData(com);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
                comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                textBox3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                
;            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult ressult = MessageBox.Show("Anda yakin ingin keluar ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ressult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (val())
            {
                if (cond == 1)
                {
                   SqlCommand cmd = new SqlCommand(" insert into Room (RoomNumberID,RoomTypeID,RoomFloor,Description,Status) values('" + textBox1.Text + "','" + comboBox1.SelectedValue + "','" + textBox2.Text + "','" + textBox3.Text + "','avail')",con);
                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Sukses!!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        dis();
                        loadgrid();
                    }
                    catch(Exception x)
                    {
                        MessageBox.Show("" + x);
                    }
                    finally
                    {
                        con.Close();
                    }
                }else if (cond == 2)
                {
                    string com  = ("Update Room set RoomNumber = '" + textBox1.Text + "',RoomType = '" + comboBox1.SelectedValue + "',RoomFloor = '" + textBox2.Text + "',Description = '" + textBox3.Text + "'");
                    try
                    {
                        Command.exec(com);
                        //cmd.ExecuteNonQuery();
                        MessageBox.Show("Sukses mengupdate!!", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        dis();
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
        }
    }

}
