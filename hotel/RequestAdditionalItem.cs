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
    public partial class RequestAdditionalItem : Form
    {
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd;
        SqlDataReader rd;
        public RequestAdditionalItem()
        {
            InitializeComponent();
            loadroom();
            loaditem();
            
        }

        void loadroom()
        {
            string com = "select ReservationRoom.ID, Room.RoomNumberID from ReservationRoom join Room on Room.ID = ReservationRoom.RoomID where Room.Status = 'unavail'";
            comboBox1.DataSource = Command.GetData(com);
            comboBox1.DisplayMember = "RoomNumberID";
            comboBox1.ValueMember = "ID";
        }

        void loadgriditm()
        {
            string sql = "Select * from reqitem_view";
            dataGridView1.DataSource = Command.GetData(sql);
        }

        void loaditem()
        {
            string sql = "Select * from Item";
            comboBox2.DataSource = Command.GetData(sql);
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "ID";
        }

       void loaddetail()
        {
            cmd = new SqlCommand("Select * from Item where ID ='" + comboBox2.SelectedValue + "'", con);
            con.Open();
            cmd.ExecuteNonQuery();
            rd = cmd.ExecuteReader();
            rd.Read();
            textBox1.Text = rd.GetInt32(2).ToString();
            textBox2.Text = (Convert.ToInt32(textBox1.Text) * numericUpDown1.Value).ToString();
            con.Close();

            
        }

        int countTotal()
        {
            int t = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
            }

            return t;
        }
        void clear()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            lbltotal.Text = countTotal().ToString();
            numericUpDown1.Value = 0;
            loaddetail();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text.Length > 0 && numericUpDown1.Value > 0)
            {
                //int rows = dataGridView1.Rows.Add();
                //dataGridView1.Rows[rows].Cells[0].Value = comboBox2.SelectedValue;
                //dataGridView1.Rows[rows].Cells[1].Value = comboBox2.Text;
                //dataGridView1.Rows[rows].Cells[2].Value = textBox1.Text;
                //dataGridView1.Rows[rows].Cells[3].Value = numericUpDown1.Value;
                //dataGridView1.Rows[rows].Cells[4].Value = textBox2.Text;

                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = comboBox1.SelectedValue;
                dataGridView1.Rows[row].Cells[1].Value = comboBox1.Text;
                dataGridView1.Rows[row].Cells[2].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[row].Cells[3].Value = comboBox2.Text;
                dataGridView1.Rows[row].Cells[4].Value = textBox1.Text;
                dataGridView1.Rows[row].Cells[5].Value = numericUpDown1.Value;
                dataGridView1.Rows[row].Cells[6].Value = textBox2.Text;


            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(comboBox2.SelectedValue.ToString()) != 0)
            {
                loaddetail();        
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
            }
            else
            {
                MessageBox.Show("Tolong pilih item !!!");
            }
        }

        private void dataGridView1_Move(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                //                try
                //                {
                //                    for(int i=0;i < dataGridView1.RowCount; i++)
                //                    {
                //                        string cmd = /*"Insert into ReservationRequestItem values(" + comboBox1.SelectedValue + "," + dataGridView1.Rows[i].Cells[0].Value + "," + dataGridView1.Rows[i].Cells[3].Value + "," + dataGridView1.Rows[0].Cells[4].Value + ")"*/ "insert into ReservationRequestitem values(" + dataGridView1.Rows[i].Cells[0].Value + ", " + dataGridView1.Rows[i].Cells[2].Value + ", " + dataGridView1.Rows[i].Cells[5].Value + ", " + dataGridView1.Rows[i].Cells[6].Value + ")";
                //                        Command.exec(cmd);

                //;                    }
                //                    MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //                    clear();
                //                }
                //                catch (Exception ex)
                //                {
                //                    MessageBox.Show("" + ex);
                //                }
                //                finally
                //                {3
                //                    con.Close();
                //                }

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string sql = "insert into ReservationRequestitem values(" + dataGridView1.Rows[i].Cells[0].Value + ", " + dataGridView1.Rows[i].Cells[2].Value + ", " + dataGridView1.Rows[i].Cells[5].Value + ", " + dataGridView1.Rows[i].Cells[6].Value + ")";
                    try
                    {
                        Command.exec(sql);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        numericUpDown1.Value = 0;
                        loaddetail();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

            }
        }
    }
}
