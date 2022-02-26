using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SPCWTest
{
    public partial class ChartWindowForm : Form
    {
        public List<float> data_source;
        public List<float> _data_source;
        
        List<int> ps = new List<int>();int PointsNum=0;
        public int control_index;
        public int line_index;
        public int control_type;
        public ChartWindowForm()
        {
            InitializeComponent();
            _data_source = new List<float>();
        }
        public void UpdateChartData()
        {
            float maxf = data_source.Max();
            float minf = data_source.Min();
            chart1.Series.Clear();
            chart1.Series.Add("Data");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            for (int i = 0; i < data_source.Count; i++)
            {
                data_source[i] = (float)Math.Round(data_source[i],3);
            }
            chart1.Series[0].Points.DataBindY(data_source);
            chart1.Update();
        }
        public void UpdateLineTree()
        {
            LineTree.Nodes.Clear();
            LineTree.Nodes.Add("Data", "Data");
            LineTree.Nodes["Data"].Nodes.Add("Process", "Process");
            LineTree.Nodes["Data"].Nodes.Add("OutPut", "OutPut");
        }
        #region 分割
        private void 分割点集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<List<int>> ps = SmartPoints.SmartPoints.SP_Translate.FindPointsCollection(data_source, 10);
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FPC", "FPC," + "10");
            UpdateChartData();
            foreach (var item in ps)
            {
                Random random = new Random();
                Color color = Color.FromArgb(random.Next(122, 255), random.Next(0, 122), random.Next(0, 255));
                foreach (var p in item)
                {
                    chart1.Series[0].Points[p].Color = color;
                }
            }
        }
        #endregion
        #region 插补NAN
        private void 左值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmartPoints.SmartPoints.SP_Translate.FillPointsCollection(data_source, "l", 0);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FIPC", "FIPC," + "l,0");
        }
        private void 右值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmartPoints.SmartPoints.SP_Translate.FillPointsCollection(data_source, "r", 0);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FIPC", "FIPC," + "r,0");
        }
        private void 中值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmartPoints.SmartPoints.SP_Translate.FillPointsCollection(data_source, "m", 0);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FIPC", "FIPC," + "m,0");
        }
        private void 高值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmartPoints.SmartPoints.SP_Translate.FillPointsCollection(data_source, "t", 0);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FIPC", "FIPC," + "t,0");
        }
        private void 低值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmartPoints.SmartPoints.SP_Translate.FillPointsCollection(data_source, "b", 0);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FIPC", "FIPC,"+"b,0");

        }
        #endregion
        #region 校平
        private void 两点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PointsNum = 2;
            System.Threading.ThreadStart PickPointsThreadStart = new System.Threading.ThreadStart(PickPoints);
            System.Threading.Thread PickPointsThread = new System.Threading.Thread(PickPointsThreadStart);
            PickPointsThread.Start();

            System.Threading.ThreadStart LineLevelingThreadStart = new System.Threading.ThreadStart(LineLeveling);
            System.Threading.Thread LineLevelingThread = new System.Threading.Thread(LineLevelingThreadStart);
            LineLevelingThread.Start();

        }
        #endregion
        #region 寻点
        private void 指定高度值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<float> ps = SmartPoints.SmartPoints.SP_Translate.FindPointsAtValue(data_source, data_source.GetRange(50, 5).Average()-3.6f);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FPAV", "FPAV," + (data_source.GetRange(50, 5).Average() - 3.6f).ToString());
        }
        #endregion
        #region 过滤
        private void 噪音峰ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SmartPoints.SmartPoints.SP_Translate.FilterPeak(data_source,0.05f);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FRP", "FRP," + "0.05");
        }

        #endregion
        private void ChartWindowForm_SizeChanged(object sender, EventArgs e)
        {
            chart1.Width = this.Width - LineTree.Width;
        }
        public void Process(List<float> fs)
        {
            List<string> outtext;
            SmartPoints.SmartPoints.SP_Translate.ListFloatProcess(fs, LineTree.Nodes["Data"].Nodes["Process"].Nodes, out outtext);
            UpdateChartData();
            //Temp_CalGap();
            ProcessEvent(LineTree.Nodes["Data"].Nodes["Process"].Nodes);
        }
        public float Temp_CalGap()
        {
            List<List<int>> points= SmartPoints.SmartPoints.SP_Translate.FindPointsCollection(data_source, 25);
            SmartPoints.SmartPoints.SP_Translate.LevelPointsCollection(data_source, 50, data_source.Count - 50);
            SmartPoints.SmartPoints.SP_Translate.FillPointsCollection(data_source, "m", 0);
            List<float> ps = SmartPoints.SmartPoints.SP_Translate.FindPointsAtValue(data_source, data_source.GetRange(50, 5).Average() - 3.6f);
            float Gap = (float)(Math.Round( ps[1]) -Math.Round( ps[0])) * 0.009715f;
            Console.WriteLine(ps[0] + "\t" + ps[1] + "\t" + Gap);
            return Gap;
        }
        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process(_data_source);
        }
        public delegate void ProcessDelegate(TreeNodeCollection nodes);
        public event ProcessDelegate ProcessEvent;
        private void lineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
        }
        private void pointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
        }
        private void splineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    chart1.ChartAreas[0].CursorX.SetCursorPixelPosition(e.Location, true);
                    chart1.ChartAreas[0].CursorY.LineColor = Color.DarkBlue;
                    if (chart1.ChartAreas[0].CursorX.Position - 1 > 0 && chart1.ChartAreas[0].CursorX.Position - 1 < data_source.Count) { chart1.ChartAreas[0].CursorY.Position = data_source[(int)Math.Abs(chart1.ChartAreas[0].CursorX.Position - 1)]; }
                    chart1.Series[0].Points.Clear();
                    chart1.Series[0].Points.DataBindY(data_source);
                    chart1.Series[0].Points[(int)Math.Abs(chart1.ChartAreas[0].CursorX.Position - 1)].Label = Math.Round(chart1.Series[0].Points[(int)Math.Abs(chart1.ChartAreas[0].CursorX.Position - 1)].YValues[0], 3).ToString();
                    
                }
                catch (Exception)
                {
                    MessageBox.Show("Error:    游标超过显示范围");
                }
            }
        }
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    chart1.ChartAreas[0].CursorX.SetCursorPixelPosition(e.Location, true);
                    chart1.ChartAreas[0].CursorY.LineColor = Color.DarkBlue;
                    int i = (int)Math.Abs(chart1.ChartAreas[0].CursorX.Position - 1);
                    if (i > 0 && i < data_source.Count) { chart1.ChartAreas[0].CursorY.Position = data_source[i]; }
                    chart1.Series[0].Points.Clear();
                    chart1.Series[0].Points.DataBindY(data_source);
                    chart1.Series[0].Points[i].Label = Math.Round(chart1.Series[0].Points[i].YValues[0], 3).ToString();
                    chart1.Series[0].Points[i].Color = Color.Red;
                    ps.Add(i);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error:    选择错误点");
                }
            }

        }


        private void 自动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisY.Maximum = Math.Round((data_source.Max() + 0.05f),3);
            chart1.ChartAreas[0].AxisY.Minimum =Math.Round( (data_source.Min() - 0.05f),3);
        }


        private void PickPoints()
        {
            ps.Clear();
            MessageBox.Show("请选择" + PointsNum + "个点");
            while (ps.Count< PointsNum)
            {
                System.Threading.Thread.Sleep(500);
            }
        }
        private void LineLeveling()
        {
            while (ps.Count!=2)
            {
                System.Threading.Thread.Sleep(50);
            }
            SmartPoints.SmartPoints.SP_Translate.LevelPointsCollection(data_source, ps[0], ps[1]);
            this.BeginInvoke(new Action(() =>
            {
                UpdateChartData();
                LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("LPC", "LPC," + ps[0] + "," + ps[1]);
            }
));

            }
        private void CalPointTPoint()
        {
            while (ps.Count != 2)
            {
                System.Threading.Thread.Sleep(50);
            }
            int s = ps.Min();int e = ps.Max();int c = e - s;
            float width = (float)Math.Round((ps[1] - ps[0]) * 0.00675, 3);
            float height = (data_source[s]+data_source[e])/2- data_source.GetRange(s, c).Min();
            this.BeginInvoke(new Action(() =>
            {
                for (int i = s; i < e; i++)
                {
                    chart1.Series[0].Points[i].Color = Color.Orange;
                }
                LineTree.Nodes["Data"].Nodes["OutPut"].Nodes.Add("P2P," + ps[0] + "," + ps[1] + ",width" +width+",height"+height);
            }
));

        }





        private void 两点ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PointsNum = 2;
            System.Threading.ThreadStart PickPointsThreadStart = new System.Threading.ThreadStart(PickPoints);
            System.Threading.Thread PickPointsThread = new System.Threading.Thread(PickPointsThreadStart);
            PickPointsThread.Start();

            System.Threading.ThreadStart CalPointTPointThreadStart = new System.Threading.ThreadStart(CalPointTPoint);
            System.Threading.Thread CalPointTPointThread = new System.Threading.Thread(CalPointTPointThreadStart);
            CalPointTPointThread.Start();


        }
    }
}
