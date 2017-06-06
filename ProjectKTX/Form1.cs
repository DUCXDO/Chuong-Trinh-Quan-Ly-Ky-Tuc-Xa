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
            SINHVIENDTO sv = new SINHVIENDTO();
            sv.MaSV = TextBox_SinhVien_TimKiem_MaSV.Text;
            int a;
            if (Int32.TryParse(TextBox_SinhVien_TimKiem_SoCMND.Text, out a) == true)
            {
                sv.SoCMND = Int32.Parse(TextBox_SinhVien_TimKiem_SoCMND.Text);
            }
            sv.SoDT = TextBox_SinhVien_TimKiem_SoDT.Text;
            sv.TenSV = TextBox_SinhVien_TimKiem_TenSV.Text;
            BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();

            foreach (var item in BUS.TimSV(sv))
            {
                dataSource.Add(item);
            }
            if (dataSource == null)
            {
                NotificationBox_SinhVien_TimKiem.Text = "Không tìm thấy dữ liệu!";
                NotificationBox_SinhVien_TimKiem.Visible = true;
            }
            else
            {
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

            if (dataSource == null)
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

        // Nút sửa được bấm - chưa xong
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
                SINHVIENDTO tranfer = BUS.chuyenDoiSVThanhSVDTO(selected);

                BindingList<SINHVIEN> dataSource = new BindingList<SINHVIEN>();

                foreach (var item in BUS.TimTatCaSV())
                {
                    dataSource.Add(item);
                }
                dataGridView_SinhVien_ThemSua.DataSource = dataSource;
                TabControl_Child_SinhVien.SelectedTab = TabPage_Child_SinhVien_ThemSua;
            }
        }



        #endregion

        #region Sinh viên - Thêm mới/Sửa

        // Nút thêm mới được ấn
        private void Button_SinhVien_ThemSua_ThemMoi_Click(object sender, EventArgs e)
        {
            int a;
            if (Int32.TryParse(TextBox_SinhVien_TimKiem_SoCMND.Text, out a) == true)
            {
                SINHVIENDTO sv = new SINHVIENDTO();
                sv.MaSV = TextBox_SinhVien_TimKiem_MaSV.Text;
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
            }
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'project_KTXDataSet.PHONG' table. You can move, or remove it, as needed.
            this.pHONGTableAdapter.Fill(this.project_KTXDataSet.PHONG);
        }
    }
}
