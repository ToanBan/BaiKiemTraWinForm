using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BaiKiemTra
{
    public partial class Form1 : Form
    {
        private Model1 dbcontext;

        public Form1()
        {
            InitializeComponent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void LoadDataGridView()
        {
            List<Sanpham> sanphams = dbcontext.Sanphams.ToList();
            dataGridView1.Rows.Clear();
            foreach (var sanpham in sanphams)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["colMa"].Value = sanpham.MaSP;
                dataGridView1.Rows[index].Cells["colName"].Value = sanpham.TenSP;
                dataGridView1.Rows[index].Cells["colNgay"].Value = sanpham.NgayNhap;
                dataGridView1.Rows[index].Cells["colLoai"].Value = sanpham.LoaiSP.TenLoai;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetData()
        {
            txtMa.Clear();
            txtName.Clear();    
            cmbLoai.SelectedIndex = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            dbcontext = new Model1();
            List<Sanpham> sanphams = dbcontext.Sanphams.ToList();
            List<LoaiSP> loais = dbcontext.LoaiSPs.ToList();
            cmbLoai.DataSource = loais;
            cmbLoai.DisplayMember = "TenLoai";
            cmbLoai.ValueMember = "MaLoai";
            dataGridView1.Rows.Clear();
            foreach (var sanpham in sanphams)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["colMa"].Value = sanpham.MaSP;
                dataGridView1.Rows[index].Cells["colName"].Value = sanpham.TenSP;
                dataGridView1.Rows[index].Cells["colNgay"].Value = sanpham.NgayNhap;
                dataGridView1.Rows[index].Cells["colLoai"].Value = sanpham.LoaiSP.TenLoai;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Bạn có muốn thoát không", "thông báo", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string fullname = txtName.Text.Trim();
                string maLoaiStr = cmbLoai.SelectedValue.ToString();  
                DateTime ngaynhap = dateTimePicker1.Value;
                string MaSP = txtMa.Text;

                Sanpham s = new Sanpham()
                {
                    MaSP = MaSP,
                    TenSP = fullname,
                    NgayNhap = ngaynhap,
                    MaLoai = maLoaiStr 
                };

                dbcontext.Sanphams.Add(s);
                dbcontext.SaveChanges();
                LoadDataGridView();
                ResetData();
                MessageBox.Show("Thêm Sản Phẩm Thành Công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm Sản Phẩm Không Thành Công: " + ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string sanphamID = (string)dataGridView1.SelectedRows[0].Cells["colMa"].Value;
                Sanpham sanphamEdit = dbcontext.Sanphams.FirstOrDefault(s => s.MaSP == sanphamID);
                if (sanphamEdit != null)
                {
                    sanphamEdit.MaSP = txtMa.Text;
                    sanphamEdit.TenSP = txtName.Text;
                    sanphamEdit.NgayNhap = dateTimePicker1.Value;
                    sanphamEdit.MaLoai = cmbLoai.SelectedValue.ToString();
                    LoadDataGridView();
                    MessageBox.Show("Cập Nhật Sản Phẩm Thành Công");
                    ResetData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string sanphamID = (string)dataGridView1.SelectedRows[0].Cells["colMa"].Value;
                Sanpham sanphamDelete = dbcontext.Sanphams.FirstOrDefault(s => s.MaSP == sanphamID);
                if (sanphamDelete != null)
                {
                    var result = MessageBox.Show("Bạm có muốn xoá không", "Thông Báo", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        dbcontext.Sanphams.Remove(sanphamDelete);
                        dbcontext.SaveChanges();
                        LoadDataGridView();
                        MessageBox.Show("Xoá Sản Phẩm Thành Công");
                    }
                }
                else
                {
                    MessageBox.Show("Sản phẩm không tồn tại.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm cần xóa.");
            }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            string tenSearch = txtSearch.Text;
            if (string.IsNullOrWhiteSpace(tenSearch))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm cần tìm kiếm.");
                return;
            }
            var sanphams = dbcontext.Sanphams
                .Where(s => s.TenSP.Contains(tenSearch))
                .ToList();
            dataGridView1.Rows.Clear();
            foreach (var sanpham in sanphams)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["colMa"].Value = sanpham.MaSP;
                dataGridView1.Rows[index].Cells["colName"].Value = sanpham.TenSP;
                dataGridView1.Rows[index].Cells["colNgay"].Value = sanpham.NgayNhap;
                dataGridView1.Rows[index].Cells["colLoai"].Value = sanpham.MaLoai;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
