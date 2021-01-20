using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriggerQLVT
{
    public partial class CTPX : Form
    {
        int vitri = 0;
        public CTPX()
        {
            InitializeComponent();
        }
        
        private void CTPX_Load(object sender, EventArgs e)
        {
            this.vattuTableAdapter.Fill(this.dS.Vattu);
            this.cTPXTableAdapter.Fill(this.dS.CTPX); 
            
            
        }

        private void cTPXBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsCTPX.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsCTPX.Position;
            groupBox1.Enabled = true;
            bdsCTPX.AddNew();
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            btnGhi.Enabled = btnPhuchoi.Enabled = true;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn xóa phiếu này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {

                    bdsCTPX.RemoveCurrent();
                    this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPXTableAdapter.Update(this.dS.CTPX);
                    this.cTPXTableAdapter.Fill(this.dS.CTPX);
                    this.vattuTableAdapter.Fill(this.dS.Vattu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa phiếu. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.cTPXTableAdapter.Fill(this.dS.CTPX);

                    return;
                }
            }

            if (bdsCTPX.Count == 0) btnXoa.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaPX.Text.Trim() == "")
            {
                MessageBox.Show("Mã phiếu không được để trống!", "", MessageBoxButtons.OK);
                txtMaPX.Focus();
                return;
            }
            if (txtSL.Text.Trim() == "")
            {
                MessageBox.Show("Số lượng không được để trống!", "", MessageBoxButtons.OK);
                txtSL.Focus();
                return;
            }
            if (txtDG.Text.Trim() == "")
            {
                MessageBox.Show("Đơn giá không được để trống!", "", MessageBoxButtons.OK);
                txtDG.Focus();
                return;
            }
            if (Program.KetNoi() == 0) return;
            string strLenh = "SELECT COUNT(l.MAPX) FROM dbo.CTPX l WHERE l.MAPX='" + txtMaPX.Text + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            Program.myReader.Read();
            int s = Program.myReader.GetInt32(0);
            if (s == 1)
            {
                MessageBox.Show("Mã phiếu đã có", "", MessageBoxButtons.OK);
                txtMaPX.Focus();
                Program.myReader.Close();
                return;
            }
            Program.myReader.Close();

            string temp = "SELECT SOLUONGTON FROM dbo.Vattu l WHERE l.MAVT='" + txtMaVT.Text + "'";
            Program.myReader = Program.ExecSqlDataReader(temp);
            Program.myReader.Read();
            int sl = Program.myReader.GetInt32(0);
            if (sl < Convert.ToInt32(txtSL.Text)){
                MessageBox.Show("Số lượng sửa lớn hơn số lượng tồn", "", MessageBoxButtons.OK);
                return;
            }
            Program.myReader.Close();
            try
            {
                bdsCTPX.EndEdit();
                bdsCTPX.ResetCurrentItem();
                this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPXTableAdapter.Update(this.dS.CTPX);
                this.cTPXTableAdapter.Fill(this.dS.CTPX);
                this.vattuTableAdapter.Fill(this.dS.Vattu);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi\n" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = true;
            btnGhi.Enabled = btnPhuchoi.Enabled = false;

            groupBox1.Enabled = false;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsCTPX.Position;
            groupBox1.Enabled = true;
        
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            btnGhi.Enabled = btnPhuchoi.Enabled = true;

        }

        private void btnPhuchoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsCTPX.CancelEdit();
            if (btnThem.Enabled == false) bdsCTPX.Position = vitri;
            groupBox1.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = true;
            btnGhi.Enabled = btnPhuchoi.Enabled = false;
        }

        private void ItemThem_Click(object sender, EventArgs e)
        {
            vitri = bdsVT.Position;
            groupBox2.Enabled = true;
            bdsVT.AddNew();
            SLT.Value = 0;
        }

        private void ItemXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn xóa vật tư này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {

                    bdsVT.RemoveCurrent();
                    this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.vattuTableAdapter.Update(this.dS.Vattu);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa vật tư. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.vattuTableAdapter.Fill(this.dS.Vattu);

                    return;
                }

            }
        }

        private void ItemSua_Click(object sender, EventArgs e)
        {
            vitri = bdsVT.Position;
        }

        private void ItemGhi_Click(object sender, EventArgs e)
        {
            if (MaVT.Text.Trim() == "")
            {
                MessageBox.Show("Mã VT không được để trống!", "", MessageBoxButtons.OK);
                MaVT.Focus();
                return;
            }
            if (TenVT.Text.Trim() == "")
            {
                MessageBox.Show("Tên không được để trống!", "", MessageBoxButtons.OK);
                TenVT.Focus();
                return;
            }
            if (DVT.Text.Trim() == "")
            {
                MessageBox.Show("Đơn vị tính không được để trống!", "", MessageBoxButtons.OK);
                DVT.Focus();
                return;
            }
         
            if (Program.KetNoi() == 0) return;
            string strLenh = "SELECT COUNT(l.MAVT) FROM dbo.VATTU l WHERE l.MAVT='" + MaVT.Text + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            Program.myReader.Read();
            int s = Program.myReader.GetInt32(0);
            if (s == 1)
            {
                MessageBox.Show("Mã vật tư đã có", "", MessageBoxButtons.OK);
                MaVT.Focus();
                Program.myReader.Close();
                return;
            }
            Program.myReader.Close();
            try
            {
                bdsVT.EndEdit();
                bdsVT.ResetCurrentItem();
                this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                this.vattuTableAdapter.Update(this.dS.Vattu);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi\n" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }

        }

        private void ItemPhuchoi_Click(object sender, EventArgs e)
        {
            bdsVT.CancelEdit();
            if (ItemThem.Enabled == false) bdsVT.Position = vitri;
            groupBox2.Enabled = false;
        }
    }
}
