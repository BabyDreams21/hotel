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
    public partial class MainFrontOffice : Form
    {
        private int childFormNumber = 0;

        Reservation rsv;
        CheckIn c;
        CheckOut co;
        RequestAdditionalItem reqi;
        ReportCheckIn rpc;
        ReportGuest rpg;

        public MainFrontOffice()
        {
            InitializeComponent();
            label1.Text = Session.name;
            label2.Text = DateTime.Now.ToString("dddd,dd-MM-yyyy");

        }

        void rpg_a(Object sender,FormClosedEventArgs e)
        {
            rpg = null;
        }
        void rpc_a(Object sender,FormClosedEventArgs e)
        {
            rpc = null;
        }
        void rsv_FormClosed(Object sender, FormClosedEventArgs e)
        {
            rsv = null;
        }

        void c_FormClosed(Object sender,FormClosedEventArgs e)
        {
            c = null;
        }

        void co_FormClosed(Object sender,FormClosedEventArgs e)
        {
            co = null;
        }

        void reqi_a(Object sender,FormClosedEventArgs e)
        {
            reqi = null;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void reservationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rsv == null)
            {
                rsv = new Reservation();
                rsv.MdiParent = this;
                rsv.FormClosed += new FormClosedEventHandler(rsv_FormClosed);
                rsv.Show();
            }
            else
            {
                rsv.Activate();
            }
        }

        private void MainFrontOffice_Load(object sender, EventArgs e)
        {

        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            AddCustomer a = new AddCustomer();
            a.ShowDialog();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if (c == null)
            {
                c = new CheckIn();
                c.MdiParent = this;
                c.FormClosed += new FormClosedEventHandler(c_FormClosed);
                c.Show();
            }
            else
            {
                rsv.Activate();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (reqi == null)
            {
                reqi = new RequestAdditionalItem();
                reqi.MdiParent = this;
                reqi.FormClosed += new FormClosedEventHandler(reqi_a);
                reqi.Show();
            }
            else
            {
                reqi.Activate();
            }
        }

        private void checkOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (co == null)
            {
                co = new CheckOut();
                co.MdiParent = this;
                co.FormClosed += new FormClosedEventHandler(co_FormClosed);
                co.Show();
            }
            else
            {
                reqi.Activate();
            }
        }

        private void chekInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rpc == null)
            {
                rpc = new ReportCheckIn();
                rpc.MdiParent = this;
                rpc.FormClosed += new FormClosedEventHandler(rpc_a);
                rpc.Show();
            }
            else
            {
                rpc.Activate();
            }
        }

        private void guestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rpg == null)
            {
                rpg = new ReportGuest();
                rpg.MdiParent = this;
                rpg.FormClosed += new FormClosedEventHandler(rpg_a);
                rpg.Show();
            }
            else
            {
                rpg.Activate();
            }
        }
    }
}
