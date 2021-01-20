using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TriggerQLVT
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void ShowMdiChildren(Type fType)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == fType)
                {
                    f.Activate();
                    return;
                }
            }
            Form form = (Form)Activator.CreateInstance(fType);
            form.MdiParent = this;
            form.Show();
        }
        private void barPN_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowMdiChildren(typeof(CTPN));
        }

        private void barPX_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ShowMdiChildren(typeof(CTPX));
        }
    }
}
