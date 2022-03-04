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
namespace PickRoi
{
    public partial class DataForm : Form
    {
        private SPCwindow cwindow = new SPCwindow();
        public DataForm()
        {
            InitializeComponent();
            AddSpcwToForm();
        }

        private void mpdatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MP数据|*.mpdat";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length>0)
            {
                cwindow.pointsCloud = SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromMpdataFile(openFileDialog.FileName);
                cwindow.Inilize();
            }
        }
        private void AddSpcwToForm()
        {
            cwindow.Dock = DockStyle.Fill;
            cwindow.Location = Point.Empty;
            this.Controls.Add(cwindow);
        }
        private void OutPutRects()
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter("regionmask.txt",false);
            foreach (var item in cwindow.pointsCloud.rects)
            {
                string s = "";
                s += item.rectangle.Location.X + " " + item.rectangle.Location.Y + " " + item.rectangle.Width + " " + item.rectangle.Height + " ";
                if (item.rectangle.Width>=item.rectangle.Height)
                {
                    s += 1 + " " + 1 + " " + 0 + " " + 0;
                }
                else
                {
                    s += 0 + " " + 0 + " " + 1 + " " + 1;

                }
                writer.WriteLine(s);
            }
            writer.Close();
        }

        private void rectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OutPutRects();
        }
    }
}
