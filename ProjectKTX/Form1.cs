using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS;
using DAO;
using DTO;

namespace ProjectKTX
{
    public partial class Form1 : Form
    {
        private readonly SinhVienBUS BUS;
        public Form1()
        {
            InitializeComponent();
            BUS = new SinhVienBUS(new SinhVienDAO(), new PhongDAO(), new HopDongDAO());
        }

        #region Sinh viên - Tìm kiếm

        // Nút tìm kiếm được bấm
        private void Button_SinhVien_TimKiem_TimKiem_Click(object sender, EventArgs e)
        {
            // Tạo đối tượng sinh viên mới
            SINHVIENDTO sv = new SINHVIENDTO();
            // Lấy mã sinh viên từ textbox
            sv.MaSV = TextBox_SinhVien_TimKiem_MaSV.Text;
            //Kiểm tra số CMND để lấy hay không
            int a;
            if (Int32.TryParse(TextBox_SinhVien_TimKiem_SoCMND.Text, out a) == true)
            {
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_TimKiem_SoCMND.Text);
            }
            // Lấy số điện thoại từ textbox
            sv.SoDT = TextBox_SinhVien_TimKiem_SoDT.Text;
            // Lấy tên sinh viên từ textbox
            sv.TenSV = TextBox_SinhVien_TimKiem_TenSV.Text;
            
            // Tạo danh sách kết quả mới (BindingList mới làm datasource cho datagrid được)
            BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();

            // Với mỗi kết quả trong tìm kiếm thì add vào danh sách kết quả ở trên
            foreach (var item in BUS.TimSV(sv))
            {
                dataSource.Add(item);
            }
            // Nếu kết quả rỗng
            if (dataSource.Count == 0)
            {
                // Thay đổi text của hộp thông báo
                NotificationBox_SinhVien_TimKiem.Text = "Không tìm thấy dữ liệu!";
                // Hiện hộp thông báo
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            // Nếu tìm thấy kết quả
            else
            {
                // Hiện kết quả trong datagrid 
                dataGridView_SinhVien_TimKiem.DataSource = dataSource;
            }
        }

        // Nút tìm theo phòng được bấm
        private void Button_SinhVien_TimKiem_TimTheoPhong_Click(object sender, EventArgs e)
        {
            BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
            foreach (var item in BUS.TimSVTheoPhong(ComboBox_SinhVien_TimKiem_Phong.SelectedValue.ToString()))
            {
                dataSource.Add(item);
            }

            if (dataSource.Count == 0)
            {
                NotificationBox_SinhVien_TimKiem.Text = "Hiện không có sinh viên nào ở phòng này!";
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            else
            {
                dataGridView_SinhVien_TimKiem.DataSource = dataSource;
            }
        }

        // Nút xóa được bấm
        private void Button_SinhVien_TimKiem_Xoa_Click(object sender, EventArgs e)
        {

            if (dataGridView_SinhVien_TimKiem.SelectedRows.Count == 0)
            {
                NotificationBox_SinhVien_TimKiem.Text = "Chưa có sinh viên nào được chọn để xóa!";
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            else
            {
                SINHVIEN selected = dataGridView_SinhVien_TimKiem.SelectedRows[0].DataBoundItem as SINHVIEN;
                String result = BUS.XoaSV(selected.MaSV);
                if (result == null)
                {
                    NotificationBox_SinhVien_TimKiem.Text = "Xóa sinh viên thành công!";
                    NotificationBox_SinhVien_TimKiem.Visible = true;
                    dataGridView_SinhVien_TimKiem.DataSource = new BindingList<SINHVIEN>();

                }
                else
                {
                    NotificationBox_SinhVien_TimKiem.Text = result;
                    NotificationBox_SinhVien_TimKiem.Visible = true;
                }
            }
        }

        // Nút sửa được bấm
        private void Button_SinhVien_TimKiem_Sua_Click(object sender, EventArgs e)
        {
            if (dataGridView_SinhVien_TimKiem.SelectedRows.Count == 0)
            {
                NotificationBox_SinhVien_TimKiem.Text = "Chưa có sinh viên nào được chọn để sửa!";
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            else
            {
                SINHVIEN selected = dataGridView_SinhVien_TimKiem.SelectedRows[0].DataBoundItem as SINHVIEN;
                // code để chuyển dữ liệu sang tabpage sửa
                TextBox_SinhVien_ThemSua_MaSV.Text = selected.MaSV;
                TextBox_SinhVien_ThemSua_DiaChi.Text = selected.DiaChi;
                TextBox_SinhVien_ThemSua_SoCMND.Text = selected.SoCMND.ToString();
                TextBox_SinhVien_ThemSua_SoDT.Text = selected.SoDT;
                TextBox_SinhVien_ThemSua_TenSV.Text = selected.TenSV;
                dateTimePicker_SinhVien_ThemSua_NgaySinh.Value = selected.NgaySinh;

                // Hiện tất cả sinh viên khi chuyển sang tabpage ThemSua
                BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
                foreach (var item in BUS.TimTatCaSV())
                {
                    dataSource.Add(item);
                }
                dataGridView_SinhVien_ThemSua.DataSource = dataSource;
                TabControl_Child_SinhVien.SelectedTab = TabPage_Child_SinhVien_ThemSua;
            }
        }

        // Làm trắng tất cả các textbox, notificationbox và datagrid khi chuyển tab
        private void TabPage_Child_SinhVien_TimKiem_Leave(object sender, EventArgs e)
        {
            // Ẩn hộp thông báo
            NotificationBox_SinhVien_TimKiem.Visible = false;
            // Xóa dữ liệu của datagrid
            dataGridView_SinhVien_TimKiem.DataSource = new BindingList<SINHVIEN>();

            Control ctrl = TabPage_Child_SinhVien_TimKiem;
            // Xóa tất cả textbox
            ClearAllText(ctrl);
        }

        #endregion

        #region Sinh viên - Thêm mới/Sửa

        // Nút thêm mới được ấn
        private void Button_SinhVien_ThemSua_ThemMoi_Click(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_SinhVien_ThemSua_SoCMND.Text, out a) == true)
            {
                SINHVIENDTO sv = new SINHVIENDTO();
                sv.MaSV = TextBox_SinhVien_ThemSua_MaSV.Text;
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_ThemSua_SoCMND.Text);
                sv.SoDT = TextBox_SinhVien_ThemSua_SoDT.Text;
                sv.TenSV = TextBox_SinhVien_ThemSua_TenSV.Text;
                sv.NgaySinh = dateTimePicker_SinhVien_ThemSua_NgaySinh.Value;
                sv.DiaChi = TextBox_SinhVien_ThemSua_DiaChi.Text;

                String result = BUS.ThemSV(sv);
                if(result == null)
                {
                    NotificationBox_SinhVien_ThemSua.Text = "Thêm mới sinh viên thành công!";
                    NotificationBox_SinhVien_ThemSua.Visible = true;

                    BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
                    foreach (var item in BUS.TimSV(sv))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_SinhVien_ThemSua.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_SinhVien_ThemSua.Text = result;
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                }
            }
            else
            {
                NotificationBox_SinhVien_ThemSua.Text = "Số chứng minh nhân dân không đúng định dạng!";
                NotificationBox_SinhVien_ThemSua.Visible = true;
            }
        }

        // Nút sửa được bấm
        private void Button_SinhVien_ThemSua_Sua_Click(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_SinhVien_ThemSua_SoCMND.Text, out a) == true)
            {
                SINHVIENDTO sv = new SINHVIENDTO();
                sv.MaSV = TextBox_SinhVien_ThemSua_MaSV.Text;
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_ThemSua_SoCMND.Text);
                sv.SoDT = TextBox_SinhVien_ThemSua_SoDT.Text;
                sv.TenSV = TextBox_SinhVien_ThemSua_TenSV.Text;
                sv.NgaySinh = dateTimePicker_SinhVien_ThemSua_NgaySinh.Value;
                sv.DiaChi = TextBox_SinhVien_ThemSua_DiaChi.Text;

                String result = BUS.SuaSV(sv);
                if (result == null)
                {
                    NotificationBox_SinhVien_ThemSua.Text = "Sửa sinh viên thành công!";
                    NotificationBox_SinhVien_ThemSua.Visible = true;

                    BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();
                    foreach (var item in BUS.TimSV(sv))
                    {
                        dataSource.Add(item);
                    }
                    dataGridView_SinhVien_ThemSua.DataSource = dataSource;
                }
                else
                {
                    NotificationBox_SinhVien_ThemSua.Text = result;
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                }
            }
            else
            {
                NotificationBox_SinhVien_ThemSua.Text = "Số chứng minh nhân dân không đúng định dạng!";
                NotificationBox_SinhVien_ThemSua.Visible = true;
            }
        }

        // Nút xóa được bấm
        private void Button_SinhVien_ThemSua_Xoa_Click(object sender, EventArgs e)
        {
            if (dataGridView_SinhVien_ThemSua.SelectedRows.Count == 0)
            {
                NotificationBox_SinhVien_ThemSua.Text = "Chưa có sinh viên nào được chọn để xóa!";
                NotificationBox_SinhVien_ThemSua.Visible = true;
            }
            else
            {
                SINHVIEN selected = dataGridView_SinhVien_ThemSua.SelectedRows[0].DataBoundItem as SINHVIEN;
                String result = BUS.XoaSV(selected.MaSV);
                if (result == null)
                {
                    NotificationBox_SinhVien_ThemSua.Text = "Xóa sinh viên thành công!";
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                    dataGridView_SinhVien_ThemSua.DataSource = new BindingList<SINHVIEN>();
                }
                else
                {
                    NotificationBox_SinhVien_ThemSua.Text = result;
                    NotificationBox_SinhVien_ThemSua.Visible = true;
                }
            }
        }

        // Làm trắng textbox và datagrid khi chuyển tab
        private void TabPage_Child_SinhVien_ThemSua_Leave(object sender, EventArgs e)
        {
            NotificationBox_SinhVien_ThemSua.Visible = false;

            dataGridView_SinhVien_ThemSua.DataSource = new BindingList<SINHVIEN>();

            Control ctrl = TabPage_Child_SinhVien_ThemSua;

            ClearAllText(ctrl);
        }

        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'project_KTXDataSet.PHONG' table. You can move, or remove it, as needed.
            this.PHONGTableAdapter.Fill(this.project_KTXDataSet.PHONG);
        }

        // Hàm làm trắng textbox trong control
        private void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else
                    ClearAllText(c);
            }
        }
    }
}
