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
    public partial class CheckOut : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataAdapter da;
        SqlDataReader reader;
        int idFd, reserId;

        public CheckOut()
        {
            InitializeComponent();
            loadroom();
            loadstatus();

        }

        void clear()
        {
            dataGridView1.Rows.Clear();
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
            loadroom();
        }

        void loadstatus()
        {
            string sql = "Select * from ItemStatus ";
            comboBox3.DataSource = Command.GetData(sql);
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "ID";
        }

        void loadroom()
        {
            //string cod = "Select * from Room  where  status =  'unavail'";
            //comboBox1.DisplayMember = "RoomNumberID";
            //comboBox1.ValueMember = "ID";
            //comboBox1.DataSource = Command.GetData(cod);

            //cmd = new SqlCommand("Select top (1) ID From ReservationRoom where RoomID = " + comboBox1.SelectedValue + " order by ID desc", con);
            //con.Open();
            //dr = cmd.ExecuteReader();
            //dr.Read();
            //reserId = dr.GetInt32(0);
            //con.Close();

            //if (comboBox1.Text.Length > 0 || comboBox1.SelectedValue != null)
            //{
            //    loaditem();
            //    //loadfd();

            //    //button3.Enabled = true;
            //    //button4.Enabled = true;
            //    //button5.Enabled = true;
            //}

            string sql = "select Room.RoomNumberID, ReservationRoom.ID from Room join ReservationRoom on Room.ID = ReservationRoom.RoomId where status = 'unavail'";
            //string sql = "select Room.RoomNumberID, ReservationRoom.ID from Room join ReservationRoom on Room.ID = ReservationRoom.RoomId where status = 'unavail'";
            comboBox1.DataSource = Command.GetData(sql);
            comboBox1.DisplayMember = "RoomNumberID";
            comboBox1.ValueMember = "ID";

        }
        void loaditem()
        {
            string sql = "Select * from reqitem_view";
            comboBox2.DataSource = Command.GetData(sql);
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "ID";
            getsub();
        }

        void loadfd()
        {
            string sql = "select * from FDCheckout where ReservationRoomId = " + reserId;
            dataGridView2.DataSource = Command.GetData(sql);

        }

        int countSubItem()
        {
            int t = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView1.Rows[i].Cells[8].Value);
            }

            return t;
        }

        int countsubFd()
        {
            int t = 0;
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView2.Rows[i].Cells[8].Value);
            }

            return t;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            getsub();
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
            
        }

        void getPrice()
        {
            command = new SqlCommand("select RequestPrice,CompensationFee from Item where ID =" + comboBox2.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                textBox1.Text = reader.GetInt32(1).ToString();
                textBox2.Text = (reader.GetInt32(0) * numericUpDown1.Value).ToString();
                connection.Close();
            }
            connection.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            reserId = Convert.ToInt32(comboBox1.SelectedValue);
            loaditem();
            
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            getsub();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(comboBox2.SelectedValue) != 0 && numericUpDown1.Value > 0)
            {
                int rows = dataGridView1.Rows.Add();
                int charge = 0;
                if (comboBox3.Text.ToLower() == "good")
                {
                    charge = 0;
                }
                else
                {
                    charge = Convert.ToInt32(textBox2.Text);
                }
                dataGridView1.Rows[rows].Cells[0].Value = Convert.ToInt32(comboBox1.SelectedValue);
                dataGridView1.Rows[rows].Cells[1].Value = comboBox1.Text;
                dataGridView1.Rows[rows].Cells[2].Value = comboBox2.SelectedValue;
                dataGridView1.Rows[rows].Cells[3].Value = comboBox2.Text;
                dataGridView1.Rows[rows].Cells[4].Value = comboBox3.SelectedValue;
                dataGridView1.Rows[rows].Cells[5].Value = comboBox3.Text;
                dataGridView1.Rows[rows].Cells[6].Value = numericUpDown1.Value;
                dataGridView1.Rows[rows].Cells[7].Value = textBox1.Text;
                dataGridView1.Rows[rows].Cells[8].Value = charge;
            }
            else
            {
                MessageBox.Show("Please select an item and Quantity must be more than 0!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            label9.Text = countSubItem().ToString();
            label6.Text = (countSubItem() + countsubFd()).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (Convert.ToInt32(comboBox1.SelectedValue) != 0)
            {
                int s = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    try
                    {

                        if (dataGridView1.Rows[i].Cells[2].Value.ToString() == "Good")
                        {
                            s = 1;
                        }
                        else
                        {
                            s = 2;
                        }

                        command = new SqlCommand("Insert into ReservationCheckOut values(" + comboBox1.SelectedValue + ", " + dataGridView1.Rows[i].Cells[0].Value + ", " + s + ", " + dataGridView1.Rows[i].Cells[4].Value + ", " + dataGridView1.Rows[i].Cells[5].Value + ")", connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

                command = new SqlCommand("update Room set status = 'avail' where RoomNumberID = " + comboBox1.Text, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                command = new SqlCommand("update ReservationRoom set CheckOutDatetime = getdate() where ID = " + comboBox1.SelectedValue, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Room Successfully check outed", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clear();

            }
        }

        void getsub()
        {
            if (comboBox2.Text.Length > 0)
            {
                command = new SqlCommand("select * from Item where Name = '" + comboBox2.Text + "'", connection);
                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    textBox2.Text = (Convert.ToInt32(reader["CompensationFee"]) * numericUpDown1.Value).ToString();
                    textBox1.Text = (Convert.ToInt32(reader["RequestPrice"]) * numericUpDown1.Value).ToString();
                }
                connection.Close();
            }
        }
    }
}
