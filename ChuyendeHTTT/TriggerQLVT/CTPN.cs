using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TriggerQLVT
{
    public partial class CTPN : Form
    {
        int vitri = 0;
        public CTPN()
        {
            InitializeComponent();
        }

        private void CTPN_Load(object sender, EventArgs e)
        {
           // dS.EnforceConstraints = false;
            this.vattuTableAdapter.Fill(this.dS.Vattu);
            this.cTPNTableAdapter.Fill(this.dS.CTPN);
        }
       
        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVT.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);
        }
        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsCTPN.Position;
            groupBox1.Enabled = true;
            bdsCTPN.AddNew();
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            btnGhi.Enabled = btnPhuchoi.Enabled = true;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaPN.Text.Trim() == "")
            {
                MessageBox.Show("Mã phiếu không được để trống!", "", MessageBoxButtons.OK);
                txtMaPN.Focus();
                return;
            }
            if (txtSL.Text.Trim() == "")
            {
                MessageBox.Show("Số lượng không được để trống!", "", MessageBoxButtons.OK);
                txtSL.Focus();
                return;
            }
            if (txtDongia.Text.Trim() == "")
            {
                MessageBox.Show("Đơn giá không được để trống!", "", MessageBoxButtons.OK);
                txtDongia.Focus();
                return;
            }
            if (Program.KetNoi() == 0) return;
            string strLenh = "SELECT COUNT(l.MAPN) FROM dbo.CTPN l WHERE l.MAPN='" + txtMaPN.Text + "'";
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            Program.myReader.Read();
            int s = Program.myReader.GetInt32(0);
            if (s == 1)
            {
                MessageBox.Show("Mã phiếu đã có", "", MessageBoxButtons.OK);
                txtMaPN.Focus();
                Program.myReader.Close();
                return;
            }
            Program.myReader.Close();
            try
            {
                bdsCTPN.EndEdit();
                bdsCTPN.ResetCurrentItem();
                this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPNTableAdapter.Update(this.dS.CTPN);
                this.cTPNTableAdapter.Fill(this.dS.CTPN);
                this.vattuTableAdapter.Fill(this.dS.Vattu);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi\n" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled =  true;
            btnGhi.Enabled = btnPhuchoi.Enabled = false;

            groupBox1.Enabled = false;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MessageBox.Show("Bạn có thật sự muốn xóa phiếu này ?? ", "Xác nhận",
                       MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                   
                    bdsCTPN.RemoveCurrent();
                    this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPNTableAdapter.Update(this.dS.CTPN);
                    this.cTPNTableAdapter.Fill(this.dS.CTPN);
                    this.vattuTableAdapter.Fill(this.dS.Vattu);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa phiếu. Bạn hãy xóa lại\n" + ex.Message, "",
                        MessageBoxButtons.OK);
                    this.cTPNTableAdapter.Fill(this.dS.CTPN);
                    
                    return;
                }
            }

            if (bdsCTPN.Count == 0) btnXoa.Enabled = false;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            vitri = bdsCTPN.Position;
            groupBox1.Enabled = true;

            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled  = false;
            btnGhi.Enabled = btnPhuchoi.Enabled = true;
        }

        private void btnPhuchoi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsCTPN.CancelEdit();
            if (btnThem.Enabled == false) bdsCTPN.Position = vitri;
            groupBox1.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled  = true;
            btnGhi.Enabled = btnPhuchoi.Enabled = false;
        }

        private void ItemThemVT_Click(object sender, EventArgs e)
        {
            vitri = bdsVT.Position;
            groupBox2.Enabled = true;
            bdsVT.AddNew();
            SLT.Value = 0;

        }

        private void ItemGhiVT_Click(object sender, EventArgs e)
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
            //if (SLT.Text.Trim() == "")
            //{
            //    SLT.Value = 0;
            //}

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

        private void ItemSuaVT_Click(object sender, EventArgs e)
        {
            vitri = bdsVT.Position;
        }

        private void ItemXoaVT_Click(object sender, EventArgs e)
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

        private void ItemPhuchoiVT_Click(object sender, EventArgs e)
        {
            bdsVT.CancelEdit();
            if (ItemThemVT.Enabled == false) bdsVT.Position = vitri;
            groupBox2.Enabled = false;
        }
    }
}
