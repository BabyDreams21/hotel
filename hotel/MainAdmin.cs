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
    public partial class MainAdmin : Form
    {
        private int childFormNumber = 0;

        Master_Employee emp;
        MasterFoodAndDrinks fd;
        Master_Item itm;
        Master_Room room;
        MasterRoomType rt;

        public MainAdmin()
        {
            InitializeComponent();
            label1.Text = Session.name;
            label2.Text = DateTime.Now.ToString("dddd, dd-MM-yyyy");
        }

        void emp_FormClosed(Object sender, FormClosedEventArgs e)
        {
            emp = null;
        }

        void fd_FormClosed(Object sender, FormClosedEventArgs e)
        {
            fd = null;
        }

        void itm_FormClosed(Object sender, FormClosedEventArgs e)
        {
            itm = null;
        }

        void room_FormClosed(Object sender, FormClosedEventArgs e)
        {
            room = null;
        }

        void rt_FormClosed(Object sender, FormClosedEventArgs e)
        {
            rt = null;
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

        private void employeeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (emp == null)
            {
                emp = new Master_Employee();
                emp.MdiParent = this;
                emp.FormClosed += new FormClosedEventHandler(emp_FormClosed);
                emp.Show();
            }
            else
            {
                emp.Activate();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin lgn = new MainLogin();
                this.Hide();
                lgn.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin keluar?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void foodAndDrinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fd == null)
            {
                fd = new MasterFoodAndDrinks();
                fd.MdiParent = this;
                fd.FormClosed += new FormClosedEventHandler(fd_FormClosed);
                fd.Show();
            }
            else
            {
                fd.Activate();
            }
        }

        private void itemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (itm == null)
            {
                itm = new Master_Item();
                itm.MdiParent = this;
                itm.FormClosed += new FormClosedEventHandler(itm_FormClosed);
                itm.Show();
            }
            else
            {
                itm.Activate();
            }
        }

        private void roomTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rt == null)
            {
                rt = new MasterRoomType();
                rt.MdiParent = this;
                rt.FormClosed += new FormClosedEventHandler(rt_FormClosed);
                rt.Show();
            }
            else
            {
                rt.Activate();
            }
        }

        private void roomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (room == null)
            {
                room = new Master_Room();
                room.MdiParent = this;
                room.FormClosed += new FormClosedEventHandler(room_FormClosed);
                room.Show();
            }
            else
            {
                room.Activate();
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            if (emp == null)
            {
                emp = new Master_Employee();
                emp.MdiParent = this;
                emp.FormClosed += new FormClosedEventHandler(emp_FormClosed);
                emp.Show();
            }
            else
            {
                emp.Activate();
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin lgn = new MainLogin();
                this.Hide();
                lgn.Show();
            }
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Anda yakin ingin keluar?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
