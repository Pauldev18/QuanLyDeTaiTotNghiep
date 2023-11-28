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
    public partial class CapNhatDiem : Form
    {
        public CapNhatDiem()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
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


            data_capNhatDiem.DataSource = query.ToList();
            data_capNhatDiem.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            data_capNhatDiem.Columns["id_sinhvien"].Visible = false;
            data_capNhatDiem.Columns["id_khoa"].Visible = false;
            data_capNhatDiem.Columns["id_detai"].Visible = false;
        }
        private void CapNhatDiem_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private int IDSinhVien;
        private void data_capNhatDiem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < data_capNhatDiem.Rows.Count)
            {
                DataGridViewRow row = data_capNhatDiem.Rows[e.RowIndex];

                // Kiểm tra xem ô Điểm có giá trị không
                if (row.Cells["Điểm"].Value != null)
                {
                    txt_diem.Text = row.Cells["Điểm"].Value.ToString();
                }
                else
                {
                    // Nếu ô Điểm có giá trị null, bạn có thể thực hiện các xử lý khác nếu cần thiết
                    // Ví dụ: Hiển thị thông báo hoặc cho phép người dùng nhập điểm
                    txt_diem.Text = ""; // Hoặc có thể để trống nếu muốn
                }

                // Tương tự kiểm tra cho ô id_sinhvien
                if (row.Cells["id_sinhvien"].Value != null)
                {
                    IDSinhVien = Convert.ToInt32(row.Cells["id_sinhvien"].Value.ToString());
                }
                else
                {
                    // Xử lý khi ô id_sinhvien có giá trị null (nếu cần thiết)
                    IDSinhVien = 0; // Hoặc giá trị mặc định nếu không có giá trị
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra dataContext
            if (dataContext != null)
            {
                // Kiểm tra IDSinhVien
                if (IDSinhVien != null)
                {
                    SinhVien capNhatDiem = dataContext.SinhViens.Where(sv => sv.id_sinhvien == IDSinhVien).FirstOrDefault();

                    // Kiểm tra capNhatDiem
                    if (capNhatDiem != null)
                    {
                        // Kiểm tra giá trị nhập vào là số hợp lệ
                        if (int.TryParse(txt_diem.Text, out int diem))
                        {
                            // Kiểm tra giá trị điểm nằm trong khoảng từ 0 đến 10
                            if (diem >= 0 && diem <= 10)
                            {
                                capNhatDiem.diem = diem;
                                dataContext.SubmitChanges();
                                txt_diem.Text = "";
                                MessageBox.Show("Cập nhật thành công");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Giá trị điểm phải nằm trong khoảng từ 0 đến 10.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng nhập giá trị điểm hợp lệ.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên.");
                    }
                }
                else
                {
                    MessageBox.Show("IDSinhVien is null.");
                }
            }
            else
            {
                MessageBox.Show("Data context is null.");
            }
        }


    }
}
