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
    public partial class Reservation : Form
    {
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd;
        SqlDataReader reader;
        int roomtypeid;
        int total = 0;
        int idcus,idreser,idroom;
        string avail;
        public Reservation()
        {
            InitializeComponent();
            loadgrid();
            loadtype();
           // loaditem();
            enable();
            counttotal();
            labelcode.Text = getcode();
            dataGridView4.Columns[0].Visible = false;
            dataGridView4.Columns[1].Visible = false;
            dataGridView4.Columns[2].Visible = false;

        }

        void unvis()
        {
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[5].Visible = false;
            dataGridView2.Columns[6].Visible = false;
            dataGridView2.Columns[7].Visible = false;

            
        }

        void refresh()
        {
            string sql = "select * from room_view where Status = 'avail'";
            dataGridView2.DataSource = Command.GetData(sql);
        }

        void loadgrid()
        {
            string sql = "Select * from Customer";
            dataGridView1.DataSource = Command.GetData(sql);
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
        }


        void clear()
        {
            idcus = 0;
            idreser = 0;
            idroom = 0;
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            numericUpDown1.Value = 0;
            textBox2.Text = "";
            dataGridView2.DataSource = null;
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();

        }

        void loadtype()
        {
            string sql = "Select * From RoomType";
            comboBox1.DataSource = Command.GetData(sql);
            comboBox1.DisplayMember = "Name";
            comboBox1.ValueMember = "ID";
        }

        void loaditem()
        {
            string sql = " select * From Item";
            comboBox2.DataSource = Command.GetData(sql);
            comboBox2.DisplayMember = "Name";
            comboBox2.ValueMember = "ID";
        }

        void loaddetail()
        {
            cmd = new SqlCommand("select * from Item where ID = " + comboBox2.SelectedValue, con);
            con.Open();
            cmd.ExecuteNonQuery();
            reader = cmd.ExecuteReader();
            reader.Read();
            textBox1.Text = reader.GetInt32(2).ToString();
            textBox3.Text = (Convert.ToInt32(textBox1.Text) * numericUpDown1.Value).ToString();
            con.Close();
        }
        //void gettoal()
        //{
        //    for (int i = 0; i < dataGridView3.RowCount; i++)
        //    {
        //        total += Convert.ToInt32(dataGridView3.Rows[i].Cells[4].Value);
        //    }

        //    labeltotal.Text = "Rp. " + total.ToString();
        //}

        int counttotal()
        {
            int room = 0, item = 0;
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                room += Convert.ToInt32(dataGridView3.Rows[i].Cells[6].Value);
            }

            //labeltotal.Text = "Rp. " + total.ToString();

            
            for (int i = 0; i < dataGridView4.RowCount; i++)
            {
                item += Convert.ToInt32(dataGridView4.Rows[i].Cells[4].Value);
            }
            //labeltotal.Text = "Rp." + total.ToString();
            return room + item;
        }

        bool val()
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Check in must be less than check out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if ( dataGridView3.RowCount < 1)
            {
                MessageBox.Show("Please select a room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        bool valRoom()
        {
            if (textBox2.TextLength < 1)
            {
                MessageBox.Show("Fill the staying column", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            {
                MessageBox.Show("Check in must be less than check out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        string getcode()
        {
            cmd = new SqlCommand("Select  top(1) * From Reservation order by ID desc", con);
            con.Open();
            reader = cmd.ExecuteReader();
            reader.Read();
            string code;
            if (reader.HasRows)
            {
                int id = reader.GetInt32(0);
                con.Close();
                string s = "0000";
                code = "B" + s.Substring(0, s.Length - id.ToString().Length) + (id + 1).ToString();
            }
            else
                code = "B0001";
            labelcode.Text = code;
            con.Close();
            return code;
        }

        int getstay()
        {
            TimeSpan ts = new TimeSpan(dateTimePicker2.Value.Ticks - dateTimePicker1.Value.Ticks);
            int stay = Convert.ToInt32(ts.Days);
            textBox2.Text = stay.ToString();
            counttotal();
            return stay;
        }


        void enable()
        {
           
        }

        

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                dataGridView1.Visible = false;
                AddCustomer ad = new AddCustomer();
                ad.ShowDialog();
                textBox4.Enabled = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
            {
                dataGridView1.Visible = true;
                loadgrid();
                AddCustomer ad = new AddCustomer();
                ad.Close();
                textBox4.Enabled = true;
            }
            else
            {
                textBox4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "select * from avail_room where RoomTypeID = " + comboBox1.SelectedValue;
            dataGridView2.DataSource = Command.GetData(sql);
            unvis();

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow.Selected == true)
            {
                dataGridView2.CurrentRow.Selected = true;
                textBox5.Text = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
                textBox6.Text = dataGridView2.SelectedRows[0].Cells[8].Value.ToString();
                textBox7.Text = dataGridView2.SelectedRows[0].Cells[6].Value.ToString();
                textBox8.Text = dataGridView2.SelectedRows[0].Cells[6].Value.ToString();
                textBox9.Text = textBox2.Text;
            }
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {   
            dataGridView3.CurrentRow.Selected = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //if (dataGridView2.CurrentRow.Selected && Convert.ToInt32(textBox2.Text) > 0)
            //{
            //    if (valRoom())
            //    {
            //        int rows = dataGridView3.Rows.Add();
            //        dataGridView3.Rows[rows].Cells[0].Value = Convert.ToInt32(textBox5.Text);
            //        dataGridView3.Rows[rows].Cells[1].Value = textBox6.Text;
            //        dataGridView3.Rows[rows].Cells[2].Value = textBox7.Text;
            //        dataGridView3.Rows[rows].Cells[3].Value = textBox2.Text;
            //        dataGridView3.Rows[rows].Cells[4].Value = (Convert.ToInt32(textBox8.Text) * Convert.ToInt32(textBox2.Text)).ToString();
            //        labeltotal.Text = "Rp. " + counttotal().ToString();
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Please select room/date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            if (dataGridView2.CurrentRow.Selected == true )
            {
                if (valRoom())
                {
                    int rows = dataGridView3.Rows.Add();
                    dataGridView3.Rows[rows].Cells[0].Value = textBox9.Text;
                    dataGridView3.Rows[rows].Cells[1].Value = textBox5.Text;
                    dataGridView3.Rows[rows].Cells[2].Value = textBox2.Text;
                    dataGridView3.Rows[rows].Cells[6].Value = textBox6.Text;
                    dataGridView3.Rows[rows].Cells[4].Value = dateTimePicker1.Value.Date;
                    dataGridView3.Rows[rows].Cells[5].Value = dateTimePicker2.Value.Date;
                    dataGridView3.Rows[rows].Cells[3].Value = Convert.ToInt32(textBox2.Text) * Convert.ToInt32(textBox6.Text);

                    //int id = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);  
                    //string select = "Update Room set Status = 'Selected' where ID = '" + id+ "'"; 
                    //Command.exec( select );
                    //refresh();
                    DataGridViewRow row;
                    int length;

                    length = dataGridView2.SelectedRows.Count;
                    for (int i = length - 1; i >= 0; i--)
                    {
                        row =
                            dataGridView2.SelectedRows[i];
                        dataGridView2.Rows.Remove(row);
                    }


                }
            }
            else
            {
                MessageBox.Show("Please select a room!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            getstay();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            getstay();
        }

        private void dgv_selected_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string com = "select * from Customer where Name like '%" + textBox4.Text + "%'";
            dataGridView1.DataSource = Command.GetData(com);
        }

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.Columns[e.ColumnIndex].Name == "Delete")
            {

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
           
            idcus = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (comboBox2.Text.Length < 1)
            {
                MessageBox.Show("Please select a Room and an item!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (numericUpDown1.Value < 1)
            {
                MessageBox.Show("Quantity values at least has 1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int row = dataGridView4.Rows.Add();
                dataGridView4.Rows[row].Cells[0].Value = comboBox1.SelectedValue;
                dataGridView4.Rows[row].Cells[1].Value = comboBox1.Text;
                dataGridView4.Rows[row].Cells[2].Value = comboBox2.SelectedValue;
                dataGridView4.Rows[row].Cells[3].Value = comboBox2.Text;
                dataGridView4.Rows[row].Cells[4].Value = textBox1.Text;
                dataGridView4.Rows[row].Cells[5].Value = numericUpDown1.Value;
                dataGridView4.Rows[row].Cells[6].Value = textBox3.Text;

                dataGridView4.Columns[0].Visible = false;
                dataGridView4.Columns[1].Visible = false;
                dataGridView4.Columns[2].Visible = false;
               

                labeltotal.Text ="Rp. " + counttotal().ToString();
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
            loaddetail();
        }

        private void Reservation_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'backuphotelDataSet5.reqitem_vw' table. You can move, or remove it, as needed.


        }

        private void dataGridView4_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView4.RowCount > 0)
            {
                for (int i = 0; i < dataGridView4.RowCount; i++)
                {
                    string sql = "insert into ReservationRequestitem values(" + dataGridView4.Rows[i].Cells[0].Value + ", " + dataGridView4.Rows[i].Cells[2].Value + ", " + dataGridView4.Rows[i].Cells[5].Value + ", " + dataGridView4.Rows[i].Cells[6].Value + ")";
                    try
                    {
                        Command.exec(sql);
                        MessageBox.Show("Success", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        dataGridView4.DataSource = null;
                        dataGridView4.Rows.Clear();
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

        private void button4_Click(object sender, EventArgs e)
        {
           if (dataGridView3.CurrentRow.Selected)
            {
             
                dataGridView3.Rows.Remove(dataGridView3.SelectedRows[0]);

                
            }
            counttotal();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
          
        }

        private void comboBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView3_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView3.CurrentRow.Selected = true;
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (idcus == 0)
            {
                MessageBox.Show("Please select a customer", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (dataGridView3.RowCount < 1)
            {
                MessageBox.Show("Please select a room", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string sql = "Insert into Reservation values ('" + dataGridView3.Rows[0].Cells[5].Value+ "'," + (Session.id) + "," + idcus + ",'" + labelcode.Text + "','" + DateTime.Now.ToString("MMMM") + "','" + DateTime.Now.ToString("yyyy") + "')";
                try
                {
                    cmd.Parameters.AddWithValue("@C", dataGridView3.Rows[0].Cells[4].Value);
                    Command.exec(sql);
                    con.Close();

                    cmd = new SqlCommand("Select top (1) ID From Reservation order by ID desc", con);
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();
                    idreser = reader.GetInt32(0);
                    con.Close();

                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        cmd = new SqlCommand("insert into ReservationRoom values (" + idreser + ", " + dataGridView3.Rows[i].Cells[0].Value + ",getdate()," + dataGridView3.Rows[i].Cells[2].Value + ",'" + dataGridView3.Rows[i].Cells[3].Value + "',@checkin,@checkout)", con);
                        cmd.Parameters.AddWithValue("@checkin", dataGridView3.Rows[i].Cells[4].Value);
                        cmd.Parameters.AddWithValue("@checkout", dataGridView3.Rows[i].Cells[5].Value);
                        con.Open();
                        try
                        {
                            cmd.ExecuteNonQuery();
                            con.Close();
                            string sqll = "Update Room set Status = 'unavail' where ID =" + dataGridView3.Rows[i].Cells[0].Value;
                            try
                            {
                                Command.exec(sqll);
                                con.Close();
                            } catch (Exception x)
                            {
                                MessageBox.Show(x.Message);

                            }
                            finally
                            {
                                con.Close();
                            }
                        } catch (Exception xx)
                        {
                            MessageBox.Show("Room" + xx);
                        }
                        finally
                        {
                            con.Close();
                        }
                    }

                    if (dataGridView4.RowCount > 0)
                    {
                        cmd = new SqlCommand("SELECT top (1) ID from ReservationRoom order by ID desc", con);
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();
                        idroom = reader.GetInt32(0);
                        con.Close();

                        for (int i = 0; i < dataGridView4.RowCount; i++)
                        {
                            cmd = new SqlCommand("Insert into ReservatioRequestItem values(" + idroom + "," + dataGridView4.Rows[i].Cells[0].Value + "," + dataGridView4.Rows[i].Cells[3].Value + "," + dataGridView4.Rows[i].Cells[4].Value + ")", con);
                            con.Open();
                            try
                            {
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("");
                            }
                            catch (Exception exc)
                            {
                                MessageBox.Show("req" + exc);
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Successfully add new reservation", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        labelcode.Text = getcode();
                        clear();
                        labeltotal.Text = counttotal().ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("res" + ex);
                }
                finally
                {
                    con.Close();
                }

            }
        
        }
    }
}
