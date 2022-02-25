using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SPCwindowUI;
using Emgu.CV;

namespace Markpoints_Emgucv_MpHD
{
    public partial class HD_MaskPoints_Form : Form
    {
        SPCwindow cwindow;
        MegaPhaseHD hD;
        public HD_MaskPoints_Form()
        {
            InitializeComponent();
            cwindow = new SPCwindow();
            cwindow.Dock = DockStyle.Fill;
            this.Controls.Add(cwindow);
        }

        private void hDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hD = new MegaPhaseHD();
            hD.ProcessHDEvent += HD_ProcessHDEvent;
            cwindow.GetInfoEvent += Cwindow_GetInfoEvent;
        }

        private void Cwindow_GetInfoEvent(string datainfo)
        {
            this.Text = datainfo;
        }

        private void HD_ProcessHDEvent(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            cloud.FilterZRange(1.0f, 0.3f);
            cloud = SmartPoints.SmartPoints.SPCV.CvGetNewSPCFromContour(cloud, SmartPoints.SmartPoints.SPCV.CvGetMaxContour(cloud.GetBitmapWB()));
            cloud = cloud.RectangleCliping(new Rectangle(400,20, 500,230));
            cwindow.SPCWPictureBox.Image= SmartPoints.SmartPoints.SPCV.CvGetBlob(cloud.GetBitmapGray());
        }

        private void 开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hD.StartCtn();
        }

        private void 结束ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hD.EndCtn();
        }

        private void 单次ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hD.HDSensor.FireSoftwareTrigger();
        }

        private void blobToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cwindow.SPCWPictureBox.Image = SmartPoints.SmartPoints.SPCV.CvGetBlob((Bitmap)cwindow.SPCWPictureBox.Image);
        }
    }
}
