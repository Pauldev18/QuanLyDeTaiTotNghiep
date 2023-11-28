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
    public partial class ThongKe : Form
    {
        public ThongKe()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        DataClasses1DataContext dataContext = new DataClasses1DataContext();
        private void LoadData()
        {
            dataContext = new DataClasses1DataContext();
            var khoas = dataContext.Khoas;


            var query = from sinhVien in dataContext.SinhViens
                        join deTai in dataContext.DeTaiDoAns on sinhVien.id_detai equals deTai.id_detai
                        join khoa in dataContext.Khoas on sinhVien.id_khoa equals khoa.id_khoa
                        select new
                        {
                            IDDeTai = deTai.id_detai,
                            TenDeTai = deTai.ten_detai,
                            Nam = khoa.nam,
                            MaSinhVien = sinhVien.ma_sv,
                            TenSinhVien = sinhVien.ho_ten,
                            Lop = sinhVien.lop,
                            sinhVien.id_sinhvien,
                            khoa.id_khoa,
                            deTai.id_detai,
                            Điểm = sinhVien.diem
                        };


            data_thongke.DataSource = query.ToList();
            data_thongke.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            data_thongke.Columns["id_sinhvien"].Visible = false;
            data_thongke.Columns["id_khoa"].Visible = false;
            data_thongke.Columns["id_detai"].Visible = false;
        }
        private void ThongKe_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataContext != null)
            {
                // Kiểm tra giá trị nhập vào là số hợp lệ
                if (int.TryParse(txt_tuDiem.Text, out int diemTu) && int.TryParse(txt_denDiem.Text, out int diemDen))
                {
                    // Kiểm tra diemTu và diemDen hợp lệ (ví dụ: diemTu <= diemDen)
                    if (diemTu <= diemDen)
                    {
                        // Thực hiện truy vấn để lấy danh sách sinh viên có điểm trong khoảng
                        var sinhViensTrongKhoangDiem = dataContext.SinhViens
                            .Where(sv => sv.diem >= diemTu && sv.diem <= diemDen)
                            .Select(sv => new
                            {
                                Điểm = sv.diem,
                                Họ_Tên =sv.ho_ten,
                                Lớp = sv.lop,
                                MSV = sv.ma_sv,
                                Tên_đề_tài = sv.DeTaiDoAn.ten_detai, // Thêm tên đề tài từ bảng DeTaiDoAn
                                Niên_khóa= sv.Khoa.nam // Thêm năm từ bảng Khoa
                            })
                            .ToList();

                        // Hiển thị thông tin sinh viên trong DataGridView hoặc thực hiện các thao tác khác
                        data_thongke.DataSource = sinhViensTrongKhoangDiem;
                        

                    }
                    else
                    {
                        MessageBox.Show("Điểm từ phải nhỏ hơn hoặc bằng điểm đến.");
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập giá trị điểm hợp lệ.");
                }
            }
            else
            {
                MessageBox.Show("Data context is null.");
            }
        }
    }
}
