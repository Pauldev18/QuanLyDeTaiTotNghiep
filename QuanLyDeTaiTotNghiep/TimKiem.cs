using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyDeTaiTotNghiep
{
    public partial class TimKiem : Form
    {
        public TimKiem()
        {
            InitializeComponent();
        }
        DataClasses1DataContext data = new DataClasses1DataContext();
        private void LoadData()
        {
            data = new DataClasses1DataContext();
            var query = from sinhVien in data.SinhViens
                        join deTai in data.DeTaiDoAns on sinhVien.id_detai equals deTai.id_detai
                        join khoa in data.Khoas on sinhVien.id_khoa equals khoa.id_khoa
                        select new
                        {
                            TenDeTai = deTai.ten_detai,
                            Nam = khoa.nam,
                            MaSinhVien = sinhVien.ma_sv,
                            TenSinhVien = sinhVien.ho_ten,
                            Lop = sinhVien.lop
                        };
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var msv = txt_msv.Text;
            data = new DataClasses1DataContext();
            var query = from sinhVien in data.SinhViens
                        join deTai in data.DeTaiDoAns on sinhVien.id_detai equals deTai.id_detai
                        join khoa in data.Khoas on sinhVien.id_khoa equals khoa.id_khoa
                        where sinhVien.ma_sv == msv
                        select new
                        {
                            TenDeTai = deTai.ten_detai,
                            Nam = khoa.nam,
                            MaSinhVien = sinhVien.ma_sv,
                            TenSinhVien = sinhVien.ho_ten,
                            Lop = sinhVien.lop
                        };
            data_detai1.DataSource = query;
            data_detai1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var tenDeTai = txt_tenDeTai.Text;
            data = new DataClasses1DataContext();
            var query = from sinhVien in data.SinhViens
                        join deTai in data.DeTaiDoAns on sinhVien.id_detai equals deTai.id_detai
                        join khoa in data.Khoas on sinhVien.id_khoa equals khoa.id_khoa
                        where deTai.ten_detai.Contains(tenDeTai)
                        select new
                        {
                            TenDeTai = deTai.ten_detai,
                            Nam = khoa.nam,
                            MaSinhVien = sinhVien.ma_sv,
                            TenSinhVien = sinhVien.ho_ten,
                            Lop = sinhVien.lop
                        };
            data_detai2.DataSource = query;
            data_detai2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }
}
