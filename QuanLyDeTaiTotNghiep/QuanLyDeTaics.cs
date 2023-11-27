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
    public partial class QuanLyDeTaics : Form
    {
        public QuanLyDeTaics()
        {
            InitializeComponent();
        }
        DataClasses1DataContext dataContext = new DataClasses1DataContext();
        private void LoadData()
        {
            dataContext = new DataClasses1DataContext();
            var khoas = dataContext.Khoas;
            cbx_khoaHoc.DataSource = khoas;
            cbx_khoaHoc.DisplayMember = "nam";
            cbx_khoaHoc.ValueMember = "id_khoa";
            cbx2_khoaHoc.DataSource = khoas;
            cbx2_khoaHoc.DisplayMember = "nam";
            cbx2_khoaHoc.ValueMember = "id_khoa";
            var query = from sinhVien in dataContext.SinhViens
                        join deTai in dataContext.DeTaiDoAns on sinhVien.id_detai equals deTai.id_detai
                        join khoa in dataContext.Khoas on deTai.id_khoa equals khoa.id_khoa
                        select new
                        {
                            IDDeTai = deTai.id_detai,
                            TenDeTai = deTai.ten_detai,
                            Nam = khoa.nam,
                            MaSinhVien = sinhVien.ma_sv,
                            TenSinhVien = sinhVien.ho_ten,
                            Lop = sinhVien.lop
                        };
            dataDeTai.DataSource = query.ToList();
            dataDeTai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void QuanLyDeTaics_Load(object sender, EventArgs e)
        {

            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var selected = cbx_khoaHoc.SelectedItem as Khoa;
            int IDKhoa = selected.id_khoa;
            var query = from sinhVien in dataContext.SinhViens
                        join deTai in dataContext.DeTaiDoAns on sinhVien.id_detai equals deTai.id_detai
                        join khoa in dataContext.Khoas on deTai.id_khoa equals khoa.id_khoa
                        where khoa.id_khoa == IDKhoa
                        select new
                        {
                            IDDeTai = deTai.id_detai,
                            TenDeTai = deTai.ten_detai,
                            Nam = khoa.nam,
                            MaSinhVien = sinhVien.ma_sv,
                            TenSinhVien = sinhVien.ho_ten,
                            Lop = sinhVien.lop
                        };
            dataDeTai.DataSource = query.ToList();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var selected = cbx_khoaHoc.SelectedItem as Khoa;
                int IDKhoa = selected.id_khoa;
                // Tạo đối tượng DeTaiDoAn và gán giá trị
                DeTaiDoAn newDeTai = new DeTaiDoAn();
                newDeTai.ten_detai = txt_tenDeTai.Text;
                newDeTai.id_khoa = IDKhoa;
                // Insert newDeTai vào bảng DeTaiDoAn
                dataContext.DeTaiDoAns.InsertOnSubmit(newDeTai);
                dataContext.SubmitChanges();

                // Lấy ID vừa insert của newDeTai
                int newDeTaiId = newDeTai.id_detai;

                // Tạo đối tượng SinhVien và gán giá trị
                SinhVien newSinhVien = new SinhVien();
                newSinhVien.ho_ten = txt_tenSinhVien.Text;
                newSinhVien.ma_sv = txt_maSinhVien.Text;
                newSinhVien.lop = txt_lop.Text;
                newSinhVien.id_detai = newDeTaiId; // Gán ID vừa insert của newDeTai

                // Insert newSinhVien vào bảng SinhVien
                dataContext.SinhViens.InsertOnSubmit(newSinhVien);

                // Tạo đối tượng Khoa và gán giá trị
                Khoa newKhoa = new Khoa();
                newKhoa.nam = cbx2_khoaHoc.Text;

                // Insert newKhoa vào bảng Khoa
                dataContext.Khoas.InsertOnSubmit(newKhoa);
                // SubmitChanges để lưu thay đổi vào cơ sở dữ liệu
                dataContext.SubmitChanges();
                MessageBox.Show("Thêm thành công");
                txt_lop.Text = "";
                txt_maSinhVien.Text = "";
                txt_tenDeTai.Text = "";
                txt_tenSinhVien.Text = "";
                cbx2_khoaHoc.Text = "";
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
