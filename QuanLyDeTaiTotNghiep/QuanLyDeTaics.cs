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
            //
            cbx2_khoaHoc.DataSource = khoas;
            cbx2_khoaHoc.DisplayMember = "nam";
            cbx2_khoaHoc.ValueMember = "id_khoa";
            // 
            var detais = dataContext.DeTaiDoAns;
            cbx_deTai.DataSource = detais;
            cbx_deTai.DisplayMember = "ten_detai";
            cbx_deTai.ValueMember = "id_detai";
          
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
                            deTai.id_detai
                        };
           

            dataDeTai.DataSource = query.ToList();
            dataDeTai.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataDeTai.Columns["id_sinhvien"].Visible = false;
            dataDeTai.Columns["id_khoa"].Visible = false;
            dataDeTai.Columns["id_detai"].Visible = false;
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
                        join khoa in dataContext.Khoas on sinhVien.id_khoa equals khoa.id_khoa
                        where sinhVien.id_khoa == IDKhoa
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
                            deTai.id_detai
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

                // Insert newDeTai vào bảng DeTaiDoAn

                var selectedDeTai = cbx_deTai.SelectedItem as DeTaiDoAn;
              
                // Lấy ID vừa insert của newDeTai
                int newDeTaiId = selectedDeTai.id_detai;

                // Tạo đối tượng SinhVien và gán giá trị
                SinhVien newSinhVien = new SinhVien();
                newSinhVien.ho_ten = txt_tenSinhVien.Text;
                newSinhVien.ma_sv = txt_maSinhVien.Text;
                newSinhVien.lop = txt_lop.Text;
                newSinhVien.id_detai = newDeTaiId; // Gán ID vừa insert của newDeTai
                newSinhVien.id_khoa = IDKhoa;
                // Insert newSinhVien vào bảng SinhVien
                dataContext.SinhViens.InsertOnSubmit(newSinhVien);
                dataContext.SubmitChanges();
                MessageBox.Show("Thêm thành công");
                txt_lop.Text = "";
                txt_maSinhVien.Text = "";
                cbx_deTai.Text = "";
                txt_tenSinhVien.Text = "";
                cbx2_khoaHoc.Text = "";
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private int IDSV;
        private int IDDeTai;
     
        private void dataDeTai_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = new DataGridViewRow();
            row = dataDeTai.Rows[e.RowIndex];
            cbx_deTai.Text = row.Cells["TenDeTai"].Value.ToString();
            txt_maSinhVien.Text = row.Cells["MaSinhVien"].Value.ToString();
            txt_tenSinhVien.Text = row.Cells["TenSinhVien"].Value.ToString();
            txt_lop.Text = row.Cells["Lop"].Value.ToString();
            cbx2_khoaHoc.Text = row.Cells["Nam"].Value.ToString();
            IDSV = Convert.ToInt32(row.Cells["id_sinhvien"].Value.ToString());
            IDDeTai = Convert.ToInt32(row.Cells["id_detai"].Value.ToString());
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
               SinhVien sinhVien = dataContext.SinhViens.Where(sv => sv.id_sinhvien == IDSV).FirstOrDefault();
                sinhVien.ho_ten = txt_tenSinhVien.Text;
                sinhVien.ma_sv = txt_maSinhVien.Text;
                sinhVien.lop = txt_maSinhVien.Text;

                var selected = cbx_khoaHoc.SelectedItem as Khoa;
                int IDKhoa = selected.id_khoa;
   
                var selectedDeTai = cbx_deTai.SelectedItem as DeTaiDoAn;
                int newDeTaiId = selectedDeTai.id_detai;

                sinhVien.id_khoa = IDKhoa;
                sinhVien.id_detai = newDeTaiId;
                dataContext.SubmitChanges();
                txt_lop.Text = "";
                txt_maSinhVien.Text = "";
                cbx_deTai.Text = "";
                txt_tenSinhVien.Text = "";
                cbx2_khoaHoc.Text = "";
                LoadData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SinhVien deleteSinhVien = dataContext.SinhViens.Where(sv => sv.id_sinhvien == IDSV).FirstOrDefault();
                dataContext.SinhViens.DeleteOnSubmit(deleteSinhVien);
                dataContext.SubmitChanges();
                MessageBox.Show("Xoa Thanh Cong");
                txt_lop.Text = "";
                txt_maSinhVien.Text = "";
                cbx_deTai.Text = "";
                txt_tenSinhVien.Text = "";
                cbx2_khoaHoc.Text = "";
                LoadData();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
