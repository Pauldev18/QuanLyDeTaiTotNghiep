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
    public partial class LapLichBaoVe: Form
    {
        public LapLichBaoVe()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
        DataClasses1DataContext data = new DataClasses1DataContext();
        private void LoadData()
        {
            data = new DataClasses1DataContext();

            var hoidongs = data.HoiDongBaoVes;
            cbx_hoiDong.DataSource = hoidongs;
            cbx_hoiDong.DisplayMember = "ten_hoidong";
            cbx_hoiDong.ValueMember = "id_hoidong";
            //
            var detais = data.DeTaiDoAns;
            cbx_deTai.DataSource = detais;
            cbx_deTai.DisplayMember = "ten_detai";
            cbx_deTai.ValueMember = "id_detai";
            //
            var khoas = data.Khoas;
            cbx_nienKhoa.DataSource = khoas;
            cbx_nienKhoa.DisplayMember = "nam";
            cbx_nienKhoa.ValueMember = "id_khoa";



            var query = from LichBaoVe in data.LichBaoVes
                        join deTai in data.DeTaiDoAns on LichBaoVe.id_detai equals deTai.id_detai
                        join khoa in data.Khoas on LichBaoVe.id_khoa equals khoa.id_khoa
                        join hoidong in data.HoiDongBaoVes on LichBaoVe.id_hoidong equals hoidong.id_hoidong
                        select new
                        {
                            Phòng = LichBaoVe.phong,
                            Ngày = LichBaoVe.ngay,
                            Giờ = LichBaoVe.gio,
                            HoiDong = hoidong.ten_hoidong,
                            DeTai = deTai.ten_detai,
                            NienKhoa = khoa.nam,
                            IDLich = LichBaoVe.id_lich
                        };
            data_lichBaoVe.DataSource = query.ToList();
            data_lichBaoVe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            data_lichBaoVe.Columns["IDLich"].Visible = false;
        }
        private void LapLichBaoVe_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LichBaoVe newLich = new LichBaoVe();
            newLich.phong = txt_phong.Text;
            newLich.ngay = Convert.ToDateTime(txt_ngay.Text);

            // Chuyển đổi DateTime thành TimeSpan
            DateTime selectedTime = txt_gio.Value;
            TimeSpan timeSpan = selectedTime.TimeOfDay;

            newLich.gio = timeSpan;
            //
            var selected = cbx_nienKhoa.SelectedItem as Khoa;
            int IDKhoa = selected.id_khoa;
            //
            var selectedHoiDong = cbx_hoiDong.SelectedItem as HoiDongBaoVe;
            int IDHoiDong = selectedHoiDong.id_hoidong;
            //
            var selectedDeTai = cbx_deTai.SelectedItem as DeTaiDoAn;
            int IDDeTai = selectedDeTai.id_detai;
            //
            newLich.id_detai = IDDeTai;
            newLich.id_hoidong = IDHoiDong;
            newLich.id_khoa = IDKhoa;
            data.LichBaoVes.InsertOnSubmit(newLich);
            data.SubmitChanges();

            txt_gio.Text = "";
            txt_ngay.Text = "";
            txt_phong.Text = "";
            cbx_deTai.Text = "";
            cbx_hoiDong.Text = "";
            cbx_nienKhoa.Text = "";
            MessageBox.Show("Thêm Thành Công");
            LoadData();
        }
        private int IdLich;
        private void data_lichBaoVe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = data_lichBaoVe.Rows[e.RowIndex];
            cbx_deTai.Text = row.Cells["DeTai"].Value.ToString();
            cbx_hoiDong.Text = row.Cells["HoiDong"].Value.ToString();
            cbx_nienKhoa.Text = row.Cells["NienKhoa"].Value.ToString();
            txt_phong.Text = row.Cells["Phòng"].Value.ToString();
            txt_ngay.Text = row.Cells["Ngày"].Value.ToString();
            txt_gio.Text = row.Cells["Giờ"].Value.ToString();
            IdLich = Convert.ToInt32(row.Cells["IDLich"].Value.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selected = cbx_nienKhoa.SelectedItem as Khoa;
            int IDKhoa = selected.id_khoa;
            //
            var selectedHoiDong = cbx_hoiDong.SelectedItem as HoiDongBaoVe;
            int IDHoiDong = selectedHoiDong.id_hoidong;
            //
            var selectedDeTai = cbx_deTai.SelectedItem as DeTaiDoAn;
            int IDDeTai = selectedDeTai.id_detai;
            LichBaoVe editLich = data.LichBaoVes.Where(l => l.id_lich == IdLich).FirstOrDefault();
            editLich.phong = txt_phong.Text;
            DateTime selectedTime = txt_gio.Value;
            TimeSpan timeSpan = selectedTime.TimeOfDay;
            editLich.gio = timeSpan;
            editLich.ngay = Convert.ToDateTime(txt_ngay.Text);
            editLich.id_khoa = IDKhoa;
            editLich.id_hoidong = IDHoiDong;
            editLich.id_detai = IDDeTai;
            data.SubmitChanges();
            LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LichBaoVe deleteLich = data.LichBaoVes.Where(l => l.id_lich == IdLich).FirstOrDefault();
            data.LichBaoVes.DeleteOnSubmit(deleteLich);
            data.SubmitChanges();
            LoadData();
        }
    }
}
