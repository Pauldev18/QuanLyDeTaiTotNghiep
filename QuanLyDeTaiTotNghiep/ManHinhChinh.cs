using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDeTaiTotNghiep
{
    public partial class ManHinhChinh : Form
    {
        public ManHinhChinh()
        {
            InitializeComponent();
        }
        private Form currentFormChild;
        private void openChildForm(Form childForm)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            pnl_trangchu.Controls.Add(childForm);
            pnl_trangchu.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void quảnLíĐềTàiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new QuanLyDeTaics());
        }

        private void pnl_trangchu_Resize(object sender, EventArgs e)
        {
            FormCollection fc = Application.OpenForms;
            foreach (Form f in fc)
            {
                if (f.Name == "QuanLyDeTaics")
                {
                    f.Height = pnl_trangchu.Height;
                    f.Width = pnl_trangchu.Width;
                }
                
            }
        }

        private void lậpLịchBảoVệToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new LapLichBaoVe());
        }

        private void cậpNhậpĐiểmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new CapNhatDiem());
        }

        private void thốngKêToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new ThongKe());
        }

        private void tìmKiếmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openChildForm(new TimKiem());
        }
    }
}
