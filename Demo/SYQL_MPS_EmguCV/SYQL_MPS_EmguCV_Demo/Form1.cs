using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartPoints;
using SPCwindowUI;

namespace SYQL_MPS_EmguCV_Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private SPCwindow sPCwindow;
        private List<SPCwindow> sPCwindows=new List<SPCwindow>();
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SPCwindow_GetInfoEvent(string datainfo)
        {
            this.Text = datainfo;
        }

        private void 设置色谱范围ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in sPCwindows)
            {
                if (item != null)
                {
                    item.pointsCloud.FilterZRangeID();
                    item.Inilize();
                }
                else
                {
                    MessageBox.Show("无数据导入");
                }
            }
        }

        private void 腐蚀ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in sPCwindows)
            {
                item.pointsCloud = SmartPoints.SmartPoints.SPCV.CvErode(item.pointsCloud);
                item.Inilize();
            }
        }

        private void 找圆ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sPCwindow.SPCWPictureBox.Image = SmartPoints.SmartPoints.SPCV.CvFindHoughCircle((Bitmap)sPCwindow.SPCWPictureBox.Image);
            sPCwindow.SPCWPictureBox.Update();
        }

        private void 三点校平ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point> points = new List<Point>();
            for (int i = 0; i < 3; i++)
            {
                points.Add(sPCwindow.pointsCloud.points[i].rectangle.Location);
            }
            sPCwindow.pointsCloud.MatLeveling_3points(points);
            sPCwindow.Inilize();
        }

        private void md3mToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3D数据|*.m3dm";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length > 0)
            {
                string FilePath = openFileDialog.FileName;
                string SpcName = FilePath.Split('.')[0];
                string SpcType = FilePath.Split('.')[1];
                switch (SpcType)
                {
                    default:
                        sPCwindow = new SPCwindow();
                        sPCwindow.Location = Point.Empty;
                        sPCwindow.Dock = DockStyle.Fill;
                        sPCwindow.pointsCloud = SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromM3dmFile(FilePath);
                        sPCwindow.Inilize();
                        sPCwindow.Name = "SpcWindow";
                        sPCwindow.GetInfoEvent += SPCwindow_GetInfoEvent;
                        if (this.Controls["SpcWindow"] != null)
                        {
                            this.Controls.RemoveAt(1);
                            this.Controls.Add(sPCwindow);
                            this.Update();
                        }
                        else
                        {
                            this.Controls.Add(sPCwindow);
                            this.Update();
                        }
                        break;
                }
            }
            else
            {
                MessageBox.Show("打开文件失败");
            }

        }
        private void mpdatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3D数据|*.mpdat";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length > 0)
            {
                string FilePath = openFileDialog.FileName;
                string SpcName = FilePath.Split('.')[0];
                string SpcType = FilePath.Split('.')[1];
                switch (SpcType)
                {
                    default:
                        sPCwindow = new SPCwindow();
                        sPCwindow.Location = new Point((this.Controls.Count-1)* 250, 0);
                        sPCwindow.Size=new Size(250, 250);
                        sPCwindow.Dock = DockStyle.None;
                        sPCwindow.pointsCloud = SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromMpdataFile(FilePath);
                        sPCwindow.Inilize();
                        sPCwindow.Name = "SpcWindow";
                        sPCwindow.GetInfoEvent += SPCwindow_GetInfoEvent;
                        this.Controls.Add(sPCwindow);
                        this.Update();
                        //if (this.Controls["SpcWindow"]!=null)
                        //{
                        //    this.Controls.RemoveAt(1);
                        //    this.Controls.Add(sPCwindow);
                        //    this.Update();
                        //}
                        //else
                        //{
                        //    this.Controls.Add(sPCwindow);
                        //    this.Update();
                        //}
                        sPCwindows.Add(sPCwindow);
                        break;
                }
            }
            else
            {
                MessageBox.Show("打开文件失败");
            }

        }

        private void 灰度图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sPCwindow.SPCWPictureBox.Image = sPCwindow.pointsCloud.GetBitmapGray();
        }

        private void 融合ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int y = 0; y < sPCwindow.pointsCloud.Height; y++)
            {
                for (int x = 0; x < sPCwindow.pointsCloud.Width; x++)
                {

                    if (x>1112&&x<1772&&y>1366&&y<2015)
                    {
                        float v0 = sPCwindows[0].pointsCloud.Spcpoints.pointsz[x + y * sPCwindow.pointsCloud.Width];
                        float v1 = sPCwindows[1].pointsCloud.Spcpoints.pointsz[x + y * sPCwindow.pointsCloud.Width];
                        if (Math.Abs(v0-v1)>0.01)
                        {
                            float v=  Math.Min(v0, v1);
                            if (float.IsNaN(v))
                            {
                                sPCwindows[0].pointsCloud.Spcpoints.pointsz[x + y * sPCwindow.pointsCloud.Width] = float.NaN;
                            }
                            else
                            {
                                sPCwindows[0].pointsCloud.Spcpoints.pointsz[x + y * sPCwindow.pointsCloud.Width] =v;
                            }
                        }
                        else
                        {

                        }
                    }

                }
            }
            sPCwindows[0].pointsCloud.SaveMpdat();
        }
    }
}
