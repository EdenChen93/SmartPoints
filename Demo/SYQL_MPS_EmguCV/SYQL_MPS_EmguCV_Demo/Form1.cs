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
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SPCwindow_GetInfoEvent(string datainfo)
        {
            this.Text = datainfo;
        }

        private void 设置色谱范围ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sPCwindow!=null)
            {
                sPCwindow.pointsCloud.FilterZRangeID();
                sPCwindow.Inilize();
            }
            else
            {
                MessageBox.Show("无数据导入");
            }
        }

        private void 腐蚀ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sPCwindow.pointsCloud = SmartPoints.SmartPoints.SPCV.CvErode(sPCwindow.pointsCloud);
            sPCwindow.Inilize();
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
                        sPCwindow.Location = Point.Empty;
                        sPCwindow.Dock = DockStyle.Fill;
                        sPCwindow.pointsCloud = SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromMpdataFile(FilePath);
                        sPCwindow.Inilize();
                        sPCwindow.Name = "SpcWindow";
                        sPCwindow.GetInfoEvent += SPCwindow_GetInfoEvent;
                        if (this.Controls["SpcWindow"]!=null)
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

        private void 灰度图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sPCwindow.SPCWPictureBox.Image = sPCwindow.pointsCloud.GetBitmapGray();
        }
    }
}
