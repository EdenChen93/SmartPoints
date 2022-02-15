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
            chart1.Series[0].Points.DataBindY(data_source);
            chart1.Update();
        }
        public void UpdateLineTree()
        {
            LineTree.Nodes.Clear();
            LineTree.Nodes.Add("Data", "Data");
            LineTree.Nodes["Data"].Nodes.Add("Process", "Process");
        }
        #region 分割
        private void 分割点集ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<List<int>> ps = SmartPoints.SmartPoints.SP_Translate.FindPointsCollection(data_source, 5);
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("FPC", "FPC," + "5");
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
            SmartPoints.SmartPoints.SP_Translate.LevelPointsCollection(data_source, 50 ,data_source.Count- 50);
            UpdateChartData();
            LineTree.Nodes["Data"].Nodes["Process"].Nodes.Add("LPC", "LPC,"+"50"+","+ (data_source.Count - 50));
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
    }
}
