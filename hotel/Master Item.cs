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
    public partial class Master_Item : Form
    {
        int id, cond;
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd;
        SqlDataReader rd;
        public Master_Item()
        {
            InitializeComponent();
            dis();
            loadgrid();
        }

        void loadgrid()
        {
            string sql = "Select * from Item";
            dataGridView1.DataSource = Command.GetData(sql);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        void clear()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
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
            button3.Enabled = false ;
            button4.Enabled = true;
            button5.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
        }

        bool val()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength  < 1 )
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from Item where Name = '" + textBox1.Text + "'", con);
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin menghapus item?", "Konfirmasi ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                cmd = new SqlCommand("Delete from Item where ID="+id,con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sukses!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dis();
                    clear();
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
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string sql = " Select * from Item where name Like '%" + textBox1.Text + "%'";
            dataGridView1.DataSource = Command.GetData(sql);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //string com = " Select * from Item where name Like '%" + textBox1.Text + "%'";
            //dataGridView1.DataSource = Command.GetData(com);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox1.Text = (dataGridView1.SelectedRows[0].Cells[1].Value.ToString());
            textBox2.Text = (dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
            textBox3.Text = (dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (cond == 1 && val())
            {
                cmd = new SqlCommand("INSERT INTO Item values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "')", con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sukses!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dis();
                    enable();
                    clear();
                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    con.Close();
                }
            }else if(cond == 2 || val_up())
            {
                cmd = new SqlCommand("Update  Item set Name = '"+textBox1.Text+ "',RequestPrice ='" + textBox2.Text + "',CompensationFee = '" + textBox3.Text + "' where ID="+id, con);
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Sukses!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult ressult = MessageBox.Show("Anda yakin ingin keluar ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ressult == DialogResult.Yes)
            {
                this.Close();
            }
        }

        bool val_up()
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1 || textBox3.TextLength <1 )
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            SqlCommand command = new SqlCommand("select * from Item where Name = '" + textBox1.Text + "'", con);
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                if (Convert.ToInt32(reader["ID"]) != id)
                {
                    con.Close();
                    MessageBox.Show("Item was used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            con.Close();
            return true;
        }


    }
}
