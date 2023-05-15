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
    public partial class ReportGuest : Form
    {
       
        SqlConnection con = new SqlConnection(Utils.conn);
        SqlCommand cmd;
        SqlDataReader rd;
        string [] bulan = { "Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec" };

        public ReportGuest()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            chart1.ChartAreas[0].AxisX.Interval = 1;
            for (int i = 0; i < 12; i++)
            {
                int x = i + 1;
                int d = 0;
                for (int j = 0; j < 12; j++)
                {
                    cmd = new SqlCommand("select count (ID) as num From Reservation  where year (datetime) = " + dateTimePicker1.Value.ToString("yyyy") + " and month (datetime)= " + x + "group by month ", con);
                    con.Open();
                    rd = cmd.ExecuteReader();
                    rd.Read();
                    if (rd.HasRows)
                    {
                        d = rd.GetInt32(0);
                    }
                    else
                    {
                        d = 0;
                    }
                    con.Close();
                }

                chart1.Series["Guest(es)"].Points.Add(d);
                chart1.Series["Guest(es)"].Points[i].AxisLabel = bulan[i];
                chart1.Series["Guest(es)"].Points[i].LegendText = bulan[i];
                chart1.Series["Guest(es)"].Points[i].Label = d.ToString();
            }
        }
    }
}
