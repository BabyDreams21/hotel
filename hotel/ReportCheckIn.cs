using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hotel
{
    public partial class ReportCheckIn : Form
    {
        //Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
        public ReportCheckIn()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //if (dateTimePicker1.Value.Date > dateTimePicker2.Value.Date)
            //{
            //    MessageBox.Show("Insert a correct date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //else
            //{
            //    string com;
            //    if (checkBox1.Checked)
            //    {
            //        com = "select * from vw_report = '" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            //    }
            //    else
            //    {
            //        com = "select Reservation.ID, Reservation.BookingCode, ReservationRoom.StartDateTime, ReservationRoom.DurationNights, ReservationRoom.CheckInDateTime, ReservationRoom.CheckOutDateTime, ReservationRoom.RoomPrice, Room.RoomNumberID, Room.RoomFloor, Room.Description from Reservation join ReservationRoom on Reservation.ID = ReservationRoom.ReservationID join Room on ReservationRoom.RoomID = Room.ID where ReservationRoom.CheckInDateTime >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and ReservationRoom.CheckOutDateTime <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";
            //    }
            //    dataGridView1.DataSource = Command.GetData(com);
            //}
            if (dateTimePicker1.Value > dateTimePicker2.Value)
                MessageBox.Show("Check In date time must be less than check out", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string sql = "select * from ReservationRoom where checkInDatetime >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' and checkoutDatetime <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                dataGridView1.DataSource = Command.GetData(sql);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ( dataGridView1.RowCount > 0 )
            {
                Microsoft.Office.Interop.Excel.Application application = new Microsoft.Office.Interop.Excel.Application();
                application.Application.Workbooks.Add(Type.Missing);
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    application.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
                }
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        application.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }

                application.Columns.AutoFit();
                application.Visible = true;

            }
        }
    }
}
