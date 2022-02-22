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

namespace SPCWTest
{
    public partial class SPTestWindow : Form
    {
        public int SpcIndex = 0;
        public SPTestWindow()
        {
            InitializeComponent();
        }
        public static SPCwindowUI.SPCwindow Oringal_SpcW = new SPCwindow();
        private ChartWindowForm chartWindowForm;
        private MegaPhaseHD megaPhaseHD;
        private bool DeviceCtn = false;
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Oringal_SpcW.Dispose();
            SpcwPanle.Update();
            AddNewSpcFromFile();
        }
        private void AddNewSpcFromFile()
        {
            this.SpcwPanle.Controls.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3D数据|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            string FilePath = openFileDialog.FileName;
            if (FilePath.Length > 0)
            {
                string[] FilePathA = FilePath.Split('.');
                if (FilePathA.Length != 2)
                {
                    MessageBox.Show("不支持的格式");
                    return;
                }
                else
                {
                    string FileName = FilePathA[0];
                    string FileType = FilePathA[1];
                    switch (FileType)
                    {
                        case "mpdat":
                            Oringal_SpcW = new SPCwindow();
                            Oringal_SpcW.Location = Point.Empty;
                            Oringal_SpcW.BorderStyle = BorderStyle.FixedSingle;
                            Oringal_SpcW.BackColor = Color.Black;
                            Oringal_SpcW.pointsCloud = SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromMpdataFile(FilePath);
                            Oringal_SpcW.pointsCloud.rects.AddItemEvent += Rects_AddItemEvent;
                            Oringal_SpcW.pointsCloud.rects.RemoveAtEvent += Rects_RemoveAtEvent;
                            Oringal_SpcW.pointsCloud.circles.AddItemEvent += Circles_AddItemEvent;
                            Oringal_SpcW.pointsCloud.circles.RemoveAtEvent += Circles_RemoveAtEvent;
                            Oringal_SpcW.pointsCloud.points.AddItemEvent += Points_AddItemEvent;
                            Oringal_SpcW.pointsCloud.points.RemoveAtEvent += Points_RemoveAtEvent;
                            Oringal_SpcW.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent;
                            Oringal_SpcW.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent;
                            Oringal_SpcW.GetInfoEvent += SPCwindow_GetInfoEvent;
                            Oringal_SpcW.Size = new Size(500, 500);
                            this.SpcwPanle.Controls.Add(Oringal_SpcW);
                            Oringal_SpcW.Inilize();
                            SPCTree.Nodes.Clear();
                            SPCTree.Nodes.Add(Oringal_SpcW.pointsCloud.SpcName);
                            SpcTreeNodesAddNewInfo(SPCTree.Nodes[0].Nodes, Oringal_SpcW);
                            MessageBox.Show("读取数据成功");
                            break;
                        case "m3dm":
                            Oringal_SpcW = new SPCwindow();
                            Oringal_SpcW.Location = Point.Empty;
                            Oringal_SpcW.BorderStyle = BorderStyle.FixedSingle;
                            Oringal_SpcW.BackColor = Color.Black;
                            Oringal_SpcW.pointsCloud = SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromM3dmFile(FilePath);
                            Oringal_SpcW.pointsCloud.rects.AddItemEvent += Rects_AddItemEvent;
                            Oringal_SpcW.pointsCloud.rects.RemoveAtEvent += Rects_RemoveAtEvent;
                            Oringal_SpcW.pointsCloud.circles.AddItemEvent += Circles_AddItemEvent;
                            Oringal_SpcW.pointsCloud.circles.RemoveAtEvent += Circles_RemoveAtEvent;
                            Oringal_SpcW.pointsCloud.points.AddItemEvent += Points_AddItemEvent;
                            Oringal_SpcW.pointsCloud.points.RemoveAtEvent += Points_RemoveAtEvent;
                            Oringal_SpcW.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent;
                            Oringal_SpcW.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent;
                            Oringal_SpcW.GetInfoEvent += SPCwindow_GetInfoEvent;
                            Oringal_SpcW.Size = new Size(500, 500);
                            this.SpcwPanle.Controls.Add(Oringal_SpcW);
                            Oringal_SpcW.Inilize();
                            SPCTree.Nodes.Clear();
                            SPCTree.Nodes.Add(Oringal_SpcW.pointsCloud.SpcName);
                            SpcTreeNodesAddNewInfo(SPCTree.Nodes[0].Nodes, Oringal_SpcW);
                            MessageBox.Show("读取数据成功");
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("未选择数据文件");
            }

        }
        private void Lines_RemoveAtEvent(int i)
        {
            SPCTree.Nodes[0].Nodes["Lines:"].Nodes.RemoveAt(i);
        }
        private void Lines_AddItemEvent(SmartPoints.SmartPoints.RoiAreaLine i)
        {
            i.RoiAreaLineChangeEvent += I_RoiAreaLineChangeEvent;
            string nodename = "L" + SPCTree.Nodes[0].Nodes["Lines:"].Nodes.Count;
            SPCTree.Nodes[0].Nodes["Lines:"].Nodes.Add(nodename, nodename + ":" + i.rectangle.ToString());
            Oringal_SpcW.pointsCloud.lines.index = Oringal_SpcW.pointsCloud.lines.Count-1;

            if (chartWindowForm==null)
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
            }
            if (chartWindowForm.IsDisposed)
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
            }
            List<float> data;
            Oringal_SpcW.pointsCloud.LineClipingFloatA(i.rectangle, out data);
            chartWindowForm.data_source = data;
            chartWindowForm._data_source = data;
            chartWindowForm.control_type = 0;
            chartWindowForm.line_index = Oringal_SpcW.pointsCloud.lines.index;
            chartWindowForm.control_index = 0;
            chartWindowForm.UpdateChartData();
            chartWindowForm.Show();
        }
        private Rectangle I_RoiAreaLineChangeEvent(Rectangle rectangle)
        {
            int il = Oringal_SpcW.pointsCloud.lines.index;
            string nodename = "L" + il+":";
            SPCTree.Nodes[0].Nodes["Lines:"].Nodes[il].Text = nodename + rectangle;
            if (chartWindowForm.IsDisposed)
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
                chartWindowForm.Show();
            }
            int i = Oringal_SpcW.pointsCloud.lines.index;
            List<float> data;
            Oringal_SpcW.pointsCloud.LineClipingFloatA(rectangle, out data);
            chartWindowForm.data_source = data;
            chartWindowForm._data_source = data;
            chartWindowForm.control_index = 0;
            chartWindowForm.control_type = 0;
            chartWindowForm.line_index = i;
            chartWindowForm.UpdateChartData();
            return rectangle;

        }
        private void Points_RemoveAtEvent(int i)
        {
            SPCTree.Nodes[0].Nodes["Points:"].Nodes.RemoveAt(i);
        }
        private void Points_AddItemEvent(SmartPoints.SmartPoints.RoiAreaPoint i)
        {
            i.RoiAreaRectChangeEvent += I_RoiAreaRectChangeEvent1;
            string nodename = "P" + SPCTree.Nodes[0].Nodes["Points:"].Nodes.Count+":";
            SPCTree.Nodes[0].Nodes["Points:"].Nodes.Add(nodename,nodename+ i.rectangle.Location.ToString());

        }
        private Rectangle I_RoiAreaRectChangeEvent1(Rectangle rectangle)
        {
            int i = Oringal_SpcW.pointsCloud.points.index;
            string nodename = "P" + i+":";
            SPCTree.Nodes[0].Nodes["Points:"].Nodes[i].Text =nodename+ rectangle.Location.ToString();
            return rectangle;
        }
        private void Circles_RemoveAtEvent(int i)
        {
            UpdateAllSpcWindowLocation();
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes.RemoveAt(i);
            //Oringal_SpcW.pointsCloud.circles[i].SPCwindow.Dispose();
           //Oringal_SpcW.pointsCloud.circles.RemoveAt(i);
        }
        private void Circles_AddItemEvent(SmartPoints.SmartPoints.RoiAreaCircle i)
        {
            i.RoiAreaCircleChangeEvent += I_RoiAreaCircleChangeEvent;
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes.Add(i.rectangle.ToString(), i.rectangle.ToString());
            SpcTreeNodesAddNewInfo(SPCTree.Nodes[0].Nodes["Circles:"].Nodes[i.rectangle.ToString()].Nodes, AddNewSpcwFromRoi(Oringal_SpcW.pointsCloud, i));
        }
        private Rectangle I_RoiAreaCircleChangeEvent(Rectangle rectangle)
        {
            int i = Oringal_SpcW.pointsCloud.circles.index ;
            Oringal_SpcW.pointsCloud.circles[i].SPCwindow.pointsCloud = Oringal_SpcW.pointsCloud.CircleCliping(new SmartPoints.SmartPoints.RoiAreaCircle(rectangle.Location, rectangle.Size));
            Oringal_SpcW.pointsCloud.circles[i].SPCwindow.Inilize();
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Text = rectangle.ToString();
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes.Clear();
            SpcTreeNodesAddNewInfo(SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes, Oringal_SpcW.pointsCloud.circles[i].SPCwindow);
            Oringal_SpcW.pointsCloud.circles[i].SPCwindow.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent1;
            Oringal_SpcW.pointsCloud.circles[i].SPCwindow.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent1;
            return rectangle;
        }
        private void Rects_RemoveAtEvent(int i)
        {
            UpdateAllSpcWindowLocation();
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes.RemoveAt(i);
            //Oringal_SpcW.pointsCloud.rects[i].SPCwindow.Dispose();
            //Oringal_SpcW.pointsCloud.rects.RemoveAt(i);
        }
        private void Rects_AddItemEvent(SmartPoints.SmartPoints.RoiAreaRect i)
        {
            i.RoiAreaRectChangeEvent += I_RoiAreaRectChangeEvent;
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes.Add(i.rectangle.ToString(),i.rectangle.ToString());
            SpcTreeNodesAddNewInfo(SPCTree.Nodes[0].Nodes["Rects:"].Nodes[i.rectangle.ToString()].Nodes, AddNewSpcwFromRoi(Oringal_SpcW.pointsCloud, i));
        }
        private Rectangle I_RoiAreaRectChangeEvent(Rectangle rectangle)
        {
            int i = Oringal_SpcW.pointsCloud.rects.index;
            Oringal_SpcW.pointsCloud.rects[i].SPCwindow.pointsCloud = Oringal_SpcW.pointsCloud.RectangleCliping(rectangle.Location, new Point(rectangle.Location.X + rectangle.Width, rectangle.Y + rectangle.Height));
            Oringal_SpcW.pointsCloud.rects[i].SPCwindow.Inilize();
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Text = rectangle.ToString();
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes.Clear();
            SpcTreeNodesAddNewInfo(SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes, Oringal_SpcW.pointsCloud.rects[i].SPCwindow);
            Oringal_SpcW.pointsCloud.rects[i].SPCwindow.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent2;
            Oringal_SpcW.pointsCloud.rects[i].SPCwindow.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent2;

            return rectangle;
        }
        private int SPCwindow_GotFocus()
        {
            for (int i = 0; i < this.SpcwPanle.Controls.Count; i++)
            {
                if (((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[i]).IsChecked)
                {
                    if (i==0)
                    {
                        return i;
                    }
                    else if (i>0&&i<Oringal_SpcW.pointsCloud.rects.Count+1)
                    {
                       return i-1;
                    }
                    else
                    {
                        return i - Oringal_SpcW.pointsCloud.rects.Count-1;
                    }
                }
            }
            return -1;
        }
        private void Lines_RemoveAtEvent2(int i)
        {
            Oringal_SpcW.pointsCloud.rects.index = SPCwindow_GotFocus();
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Lines:"].Nodes.RemoveAt(i);
            chartWindowForm.control_index = Oringal_SpcW.pointsCloud.rects.index;
            chartWindowForm.line_index = Oringal_SpcW.pointsCloud.rects[Oringal_SpcW.pointsCloud.rects.index].SPCwindow.pointsCloud.lines.index;
        }
        private void Lines_AddItemEvent2(SmartPoints.SmartPoints.RoiAreaLine i)
        {
            Oringal_SpcW.pointsCloud.rects.index = SPCwindow_GotFocus();
            i.RoiAreaLineChangeEvent += I_RoiAreaLineChangeEvent1;
            string nodename = "L" + SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Lines:"].Nodes.Count;
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Lines:"].Nodes.Add(nodename, nodename + ":" + i.rectangle.ToString());
            if (chartWindowForm == null)
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
            }
            if (chartWindowForm.IsDisposed)
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
            }
            List<float> data;
            Oringal_SpcW.pointsCloud.rects[Oringal_SpcW.pointsCloud.rects.index].SPCwindow.pointsCloud.LineClipingFloatA(i.rectangle, out data);
            chartWindowForm.control_type = 1;
            chartWindowForm.control_index = Oringal_SpcW.pointsCloud.rects.index;
            chartWindowForm.line_index = Oringal_SpcW.pointsCloud.rects[Oringal_SpcW.pointsCloud.rects.index].SPCwindow.pointsCloud.lines.index;
            chartWindowForm._data_source = data;
            chartWindowForm.data_source = data;
            chartWindowForm.UpdateLineTree();
            chartWindowForm.UpdateChartData();
            chartWindowForm.Show();
        }
        private Rectangle I_RoiAreaLineChangeEvent1(Rectangle rectangle)
        {
            chartWindowForm.control_type = 1;
            Oringal_SpcW.pointsCloud.rects.index = SPCwindow_GotFocus();
            int i = Oringal_SpcW.pointsCloud.rects[Oringal_SpcW.pointsCloud.rects.index].SPCwindow.pointsCloud.lines.index;
            string nodename = "L" +i+":";
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Lines:"].Nodes[i].Text =nodename+rectangle.ToString();
            if (chartWindowForm != null && !chartWindowForm.IsDisposed)
            {
                List<float> data;
                Oringal_SpcW.pointsCloud.rects[Oringal_SpcW.pointsCloud.rects.index].SPCwindow.pointsCloud.LineClipingFloatA(rectangle, out data);
                chartWindowForm._data_source = data;
                chartWindowForm.data_source = data;
                chartWindowForm.Process(data);
            }
            else
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
                List<float> data;
                Oringal_SpcW.pointsCloud.rects[Oringal_SpcW.pointsCloud.rects.index].SPCwindow.pointsCloud.LineClipingFloatA(rectangle, out data);
                chartWindowForm._data_source = data;
                chartWindowForm.data_source = data;
                chartWindowForm.UpdateChartData();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.Show();
            }
            return rectangle;
        }
        private SPCwindowUI.SPCwindow AddNewSpcwFromRoi(SmartPoints.SmartPoints.SmartPointsCloud cloud, SmartPoints.SmartPoints.RoiAreaRect rect)
        {
            SPCwindowUI.SPCwindow aSpcw = new SPCwindow();
            aSpcw.BorderStyle = BorderStyle.FixedSingle;
            aSpcw.BackColor = Color.Black;
            aSpcw.pointsCloud = cloud.RectangleCliping(rect.rectangle.Location, new Point(rect.rectangle.Location.X + rect.rectangle.Width, rect.rectangle.Y + rect.rectangle.Height));
            aSpcw.Size = new Size(500, 500);
            aSpcw.Dock = DockStyle.None;
            aSpcw.Inilize();
            Oringal_SpcW.pointsCloud.rects.Last().SPCwindow = new SPCwindow();
            Oringal_SpcW.pointsCloud.rects.Last().SPCwindow = aSpcw;
            Oringal_SpcW.pointsCloud.rects.Last().SPCwindow.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent2;
            Oringal_SpcW.pointsCloud.rects.Last().SPCwindow.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent2;
            Oringal_SpcW.pointsCloud.rects.Last().SPCwindow.pointsCloud.points.AddItemEvent += Points_AddItemEvent2;
            Oringal_SpcW.pointsCloud.rects.Last().SPCwindow.pointsCloud.points.RemoveAtEvent += Points_RemoveAtEvent2;
            Oringal_SpcW.pointsCloud.rects.Last().SPCwindow.GetInfoEvent += SPCwindow_GetInfoEvent;
            this.SpcwPanle.Controls.Add(Oringal_SpcW.pointsCloud.rects.Last().SPCwindow);
            UpdateAllSpcWindowLocation();
            return aSpcw;
        }
        private void Points_RemoveAtEvent2(int i)
        {
            Oringal_SpcW.pointsCloud.rects.index = SPCwindow_GotFocus();
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Points:"].Nodes.RemoveAt(i);

        }
        private void Points_AddItemEvent2(SmartPoints.SmartPoints.RoiAreaPoint i)
        {
            Oringal_SpcW.pointsCloud.circles.index = SPCwindow_GotFocus();
            i.RoiAreaRectChangeEvent += I_RoiAreaRectChangeEvent3;
            string nodename = "P" + SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Points:"].Nodes.Count;
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Points:"].Nodes.Add(nodename, nodename + ":" + i.rectangle.Location.ToString());

        }
        private Rectangle I_RoiAreaRectChangeEvent3(Rectangle rectangle)
        {
            Oringal_SpcW.pointsCloud.rects.index = SPCwindow_GotFocus();
            int i = Oringal_SpcW.pointsCloud.rects[Oringal_SpcW.pointsCloud.rects.index].SPCwindow.pointsCloud.points.index;
            string nodename = "P" + i + ":";
            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[Oringal_SpcW.pointsCloud.rects.index].Nodes["Points:"].Nodes[i].Text = nodename + rectangle.Location.ToString();
            return rectangle;

        }    
        private SPCwindowUI.SPCwindow AddNewSpcwFromRoi(SmartPoints.SmartPoints.SmartPointsCloud cloud, SmartPoints.SmartPoints.RoiAreaCircle circle)
        {
            SPCwindowUI.SPCwindow aSpcw = new SPCwindow();
            aSpcw.BorderStyle = BorderStyle.FixedSingle;
            aSpcw.BackColor = Color.Black;
            aSpcw.pointsCloud = cloud.CircleCliping(circle);
            aSpcw.Size = new Size(500, 500);
            aSpcw.Dock = DockStyle.None;
            aSpcw.Inilize();
            Oringal_SpcW.pointsCloud.circles.Last().SPCwindow = new SPCwindow();
            Oringal_SpcW.pointsCloud.circles.Last().SPCwindow = aSpcw;
            Oringal_SpcW.pointsCloud.circles.Last().SPCwindow.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent1;
            Oringal_SpcW.pointsCloud.circles.Last().SPCwindow.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent1;
            Oringal_SpcW.pointsCloud.circles.Last().SPCwindow.pointsCloud.points.AddItemEvent += Points_AddItemEvent1;
            Oringal_SpcW.pointsCloud.circles.Last().SPCwindow.pointsCloud.points.RemoveAtEvent += Points_RemoveAtEvent1;
            Oringal_SpcW.pointsCloud.circles.Last().SPCwindow.GetInfoEvent += SPCwindow_GetInfoEvent;
            this.SpcwPanle.Controls.Add(Oringal_SpcW.pointsCloud.circles.Last().SPCwindow);
            UpdateAllSpcWindowLocation();
            return aSpcw;
        }
        private void SPCwindow_GetInfoEvent(string datainfo)
        {
            this.Text = datainfo;
        }
        private void Points_RemoveAtEvent1(int i)
        {
            Oringal_SpcW.pointsCloud.circles.index = SPCwindow_GotFocus();
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Points:"].Nodes.RemoveAt(i);
        }
        private void Points_AddItemEvent1(SmartPoints.SmartPoints.RoiAreaPoint i)
        {
            Oringal_SpcW.pointsCloud.circles.index = SPCwindow_GotFocus();
            i.RoiAreaRectChangeEvent += I_RoiAreaRectChangeEvent2;
            string nodename = "P" + SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Points:"].Nodes.Count;
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Points:"].Nodes.Add(nodename, nodename + ":" + i.rectangle.Location.ToString());
        }
        private Rectangle I_RoiAreaRectChangeEvent2(Rectangle rectangle)
        {
            Oringal_SpcW.pointsCloud.circles.index = SPCwindow_GotFocus();
            int i = Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.points.index;
            string nodename = "P" + i + ":";
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Points:"].Nodes[i].Text = nodename + rectangle.Location.ToString();
            return rectangle;
        }
        private void Lines_RemoveAtEvent1(int i)
        {
            Oringal_SpcW.pointsCloud.circles.index=SPCwindow_GotFocus();
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Lines:"].Nodes.RemoveAt(i);
            chartWindowForm.control_index = Oringal_SpcW.pointsCloud.circles.index;
            chartWindowForm.line_index = Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.lines.index;
        }
        private void Lines_AddItemEvent1(SmartPoints.SmartPoints.RoiAreaLine i)
        {
            Oringal_SpcW.pointsCloud.circles.index = SPCwindow_GotFocus();
            i.RoiAreaLineChangeEvent += I_RoiAreaLineChangeEvent2;
            string nodename = "L" + SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Lines:"].Nodes.Count;
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Lines:"].Nodes.Add(nodename, nodename + ":" + i.rectangle.ToString());
            if (chartWindowForm == null)
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
            }
            if (chartWindowForm.IsDisposed)
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
            }
            List<float> data;
            Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.LineClipingFloatA(i.rectangle, out data);
            chartWindowForm._data_source = data;
            chartWindowForm.data_source = data;
            chartWindowForm.control_type = 2;
            chartWindowForm.control_index = Oringal_SpcW.pointsCloud.circles.index;
            chartWindowForm.line_index = Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.lines.index;
            chartWindowForm.UpdateChartData();
            chartWindowForm.UpdateLineTree();
            chartWindowForm.Show();
        }
        private Rectangle I_RoiAreaLineChangeEvent2(Rectangle rectangle)
        {
            chartWindowForm.control_type = 2;
            Oringal_SpcW.pointsCloud.circles.index = SPCwindow_GotFocus();
            int i = Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.lines.index;
            string nodename = "L" + i+":";
            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[Oringal_SpcW.pointsCloud.circles.index].Nodes["Lines:"].Nodes[i].Text = nodename + rectangle.ToString();
            if (chartWindowForm != null && !chartWindowForm.IsDisposed)
            {
                List<float> data;
                Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.LineClipingFloatA(rectangle, out data);
                chartWindowForm._data_source = data;
                chartWindowForm.data_source = data;
                chartWindowForm.Process(data);
            }
            else
            {
                chartWindowForm = new ChartWindowForm();
                chartWindowForm.ProcessEvent += ChartWindowForm_ProcessEvent;
                List<float> data;
                Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.LineClipingFloatA(rectangle, out data);
                chartWindowForm.data_source = data;
                chartWindowForm._data_source = data;
                chartWindowForm.UpdateChartData();
                chartWindowForm.UpdateLineTree();
                chartWindowForm.Show();
            }
            return rectangle;
        }
        private void UpdateAllSpcWindowLocation()
        {
            this.SpcwPanle.VerticalScroll.Value = 0;
            this.SpcwPanle.HorizontalScroll.Value = 0;
            if (Oringal_SpcW.pointsCloud!=null)
            {
                int y = 0; int x = -1;
                for (int i = -1; i < Oringal_SpcW.pointsCloud.rects.Count + Oringal_SpcW.pointsCloud.circles.Count; i++)
                {
                    if ((x + 2) * 500 > this.SpcwPanle.Width)
                    {
                        y++;
                        x = 0;
                    }
                    else
                    {
                        x++;
                    }
                    if (i == -1)
                    {
                        Oringal_SpcW.Location = new Point(x * 500, y * 500);

                    }
                    else if (i < Oringal_SpcW.pointsCloud.rects.Count)
                    {
                        Oringal_SpcW.pointsCloud.rects[i].SPCwindow.Location = new Point(x * 500, y * 500);
                    }
                    else
                    {
                        Oringal_SpcW.pointsCloud.circles[i - Oringal_SpcW.pointsCloud.rects.Count].SPCwindow.Location = new Point(x * 500, y * 500);
                    }
                }
                this.SpcwPanle.Update();
            }
        }
        private void SPTestWindow_SizeChanged(object sender, EventArgs e)
        {
            SPCTree.Width = 300;
            SpcwPanle.Width = this.Width - SPCTree.Width;
            SPCTree.Height = this.Height - toolStrip1.Height - menuStrip1.Height;
            SpcwPanle.Height = SPCTree.Height;
            SpcwPanle.Location = new Point(SPCTree.Location.X + 300, SPCTree.Location.Y);
            UpdateAllSpcWindowLocation();
        }
        private void SpcTreeNodesAddNewInfo(TreeNodeCollection nodes, SPCwindowUI.SPCwindow cwindow)
        {
            nodes.Add("SpcSize:", "SpcSize:" + cwindow.pointsCloud.Width + '*' + cwindow.pointsCloud.Height);
            nodes.Add("ColorRange:", "ColorRange:" + cwindow.pointsCloud.Zmin + ',' + cwindow.pointsCloud.Zmax);
            nodes.Add("Points:", "Points:"); 
            nodes.Add("Lines:","Lines:"); 
            nodes.Add("Rects:","Rects:"); 
            nodes.Add("Circles:","Circles");
            nodes.Add("Process:", "Process");
            nodes.Add("Output:", "Output");
        }
        private void 触发ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceCtn = false;
          for (int i = 0; i < 10; i++)
          {
             megaPhaseHD.HDSensor.FireSoftwareTrigger();
             System.Threading.Thread.Sleep(2000);
          }
        }
        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            megaPhaseHD = new MegaPhaseHD();
            megaPhaseHD.ProcessHDEvent += MegaPhaseHD_ProcessHDEvent;
            Oringal_SpcW = new SPCwindow();
            Oringal_SpcW.Location = Point.Empty;
            Oringal_SpcW.BorderStyle = BorderStyle.FixedSingle;
            Oringal_SpcW.BackColor = Color.Black;
            Oringal_SpcW.GetInfoEvent += SPCwindow_GetInfoEvent;
            Oringal_SpcW.Size = new Size(500, 500);
            this.SpcwPanle.Controls.Add(Oringal_SpcW);
            megaPhaseHD.HDSensor.FireSoftwareTrigger();
        }
        private void MegaPhaseHD_ProcessHDEvent(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            if (DeviceCtn)
            {
                Oringal_SpcW.pointsCloud = cloud;
                Oringal_SpcW.pointsCloud.rects.AddItemEvent += Rects_AddItemEvent;
                Oringal_SpcW.pointsCloud.rects.RemoveAtEvent += Rects_RemoveAtEvent;
                Oringal_SpcW.pointsCloud.circles.AddItemEvent += Circles_AddItemEvent;
                Oringal_SpcW.pointsCloud.circles.RemoveAtEvent += Circles_RemoveAtEvent;
                Oringal_SpcW.pointsCloud.points.AddItemEvent += Points_AddItemEvent;
                Oringal_SpcW.pointsCloud.points.RemoveAtEvent += Points_RemoveAtEvent;
                Oringal_SpcW.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent;
                Oringal_SpcW.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent;
                Oringal_SpcW.Inilize();
            }
            else
            {
                if (Oringal_SpcW.pointsCloud == null)
                {
                    Oringal_SpcW.pointsCloud = cloud;
                    this.BeginInvoke(new Action(() =>
                    {
                        SPCTree.Nodes.Clear();
                        SPCTree.Nodes.Add(Oringal_SpcW.pointsCloud.SpcName);
                        SpcTreeNodesAddNewInfo(SPCTree.Nodes[SPCTree.Nodes.Count - 1].Nodes, Oringal_SpcW);
                    }));
                    Oringal_SpcW.pointsCloud.rects.AddItemEvent += Rects_AddItemEvent;
                    Oringal_SpcW.pointsCloud.rects.RemoveAtEvent += Rects_RemoveAtEvent;
                    Oringal_SpcW.pointsCloud.circles.AddItemEvent += Circles_AddItemEvent;
                    Oringal_SpcW.pointsCloud.circles.RemoveAtEvent += Circles_RemoveAtEvent;
                    Oringal_SpcW.pointsCloud.points.AddItemEvent += Points_AddItemEvent;
                    Oringal_SpcW.pointsCloud.points.RemoveAtEvent += Points_RemoveAtEvent;
                    Oringal_SpcW.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent;
                    Oringal_SpcW.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent;
                    Oringal_SpcW.Inilize();
                }
                else
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        for (int i = 0; i < Oringal_SpcW.pointsCloud.lines.Count; i++)
                        {
                            List<float> ds;
                            List<string> res;
                            cloud.LineClipingFloatA(Oringal_SpcW.pointsCloud.lines[i].rectangle, out ds);
                            SmartPoints.SmartPoints.SP_Translate.ListFloatProcess(ds, SPCTree.Nodes[0].Nodes["Lines:"].Nodes[i].Nodes["Process"].Nodes, out res);
                            List<float> ps = SmartPoints.SmartPoints.SP_Translate.FindPointsAtValue(ds, ds.GetRange(50, 5).Average() - 3.6f);
                            float Gap = (float)(Math.Round(ps[1]) - Math.Round(ps[0])) * 0.009715f;
                            System.IO.StreamWriter writer = new System.IO.StreamWriter("Gap.txt", true);
                            if (i == Oringal_SpcW.pointsCloud.lines.Count - 1)
                            {
                                writer.Write(Gap + "\r\n");
                            }
                            else
                            {
                                writer.Write(Gap + ",");
                            }
                            writer.Close();
                            writer.Dispose();
                        }
                        cloud = null;
                    }));
                }

            }
        }
        private void ChartWindowForm_ProcessEvent(TreeNodeCollection nodes)
        {
            switch (chartWindowForm.control_type)
            {
                case 0:
                    if (SPCTree.Nodes[0].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Count>0)
                    {
                        SPCTree.Nodes[0].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Clear();
                    }
                    else
                    {
                        SPCTree.Nodes[0].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Add("Process", "Process");
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            SPCTree.Nodes[0].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes["Process"].Nodes.Add(nodes[i].Name,nodes[i].Text);
                        }
                    }
                    break;
                case 1:
                    if (SPCTree.Nodes[0].Nodes["Rects:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Count>0)
                    {
                        SPCTree.Nodes[0].Nodes["Rects:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Clear();
                    }
                    else
                    {
                        SPCTree.Nodes[0].Nodes["Rects:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Add("Process", "Process");
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            SPCTree.Nodes[0].Nodes["Rects:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes["Process"].Nodes.Add(nodes[i].Name, nodes[i].Text);
                        }
                    }
                    break;
                case 2:
                    if (SPCTree.Nodes[0].Nodes["Circles:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Count>0)
                    {
                        SPCTree.Nodes[0].Nodes["Circles:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Clear();
                    }
                    else
                    {
                        SPCTree.Nodes[0].Nodes["Circles:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Add("Process", "Process");
                        for (int i = 0; i < nodes.Count; i++)
                        {
                            SPCTree.Nodes[0].Nodes["Circles:"].Nodes[chartWindowForm.control_index].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes["Process"].Nodes.Add(nodes[i].Name, nodes[i].Text);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private void 中值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.FillNaN(5);
            UpdateSpctree_ProcessInfo();
            ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).Inilize();

        }
        private void xyzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Oringal_SpcW.pointsCloud.SpcPath.Length>0)
            {
                
            }
        }
        private void csvToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (SpcIndex == 0)
            {
                if (Oringal_SpcW.pointsCloud.SpcPath.Length > 0)
                {
                    Oringal_SpcW.pointsCloud.SaveCsv();
                }
            }
            else if (SpcIndex > 0 && SpcIndex < Oringal_SpcW.pointsCloud.rects.Count + 1)
            {
                 Oringal_SpcW.pointsCloud.rects[SpcIndex - 1].SPCwindow.pointsCloud.SaveCsv();
            }
            else
            {
                 Oringal_SpcW.pointsCloud.circles[SpcIndex - Oringal_SpcW.pointsCloud.circles.Count - 1].SPCwindow.pointsCloud.SaveCsv();
            }
        }
        private void 打开ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DeviceCtn = true;
            System.Threading.ThreadStart threadts_hd_ctn= new System.Threading.ThreadStart(Device_MPHD_CTN);
            System.Threading.Thread thread_hd_ctn = new System.Threading.Thread(threadts_hd_ctn);
            thread_hd_ctn.Start();
        }
        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceCtn = false;
        }
        private void Device_MPHD_CTN()
        {
            while (DeviceCtn)
            {
                megaPhaseHD.HDSensor.FireSoftwareTrigger();
                System.Threading.Thread.Sleep(2000);
            }
        }
        private void zRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.FilterZRangeID();
            UpdateSpctree_ProcessInfo();
            ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).Inilize();
        }
        private void pointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Point> ps = new List<Point>();
            ps.Add(((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.points[0].rectangle.Location);
            ps.Add(((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.points[1].rectangle.Location);
            ps.Add(((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.points[2].rectangle.Location);
            ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.MatLeveling_3points(ps);
            UpdateSpctree_ProcessInfo();
            ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).Inilize();
        }
        private void MatLeveling3points()
        {
            
        }
        private void SPCTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string eps = e.Node.Parent.Name;
                switch (eps)
                {
                    case "Rects:":
                        SpcIndex = e.Node.Index+1;
                        break;
                    case "Circles:":
                        SpcIndex = e.Node.Index+Oringal_SpcW.pointsCloud.rects.Count+1;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                SpcIndex = 0;
            }
        }
        private void UpdateSpctree_ProcessInfo()
        {
            if (SpcIndex==0)
            {
                foreach (var item in ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.ProcessXml.CmdList)
                {
                    SPCTree.Nodes[SpcIndex].Nodes["Process:"].Nodes.Add(item);
                }
            }
            else if(SpcIndex>0&&SpcIndex<Oringal_SpcW.pointsCloud.rects.Count+1)
            {
                foreach (var item in ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.ProcessXml.CmdList)
                {
                    SPCTree.Nodes[0].Nodes["Rects:"].Nodes[SpcIndex-1].Nodes["Process:"].Nodes.Add(item);
                }
            }
            else
            {
                foreach (var item in ((SPCwindowUI.SPCwindow)this.SpcwPanle.Controls[SpcIndex]).pointsCloud.ProcessXml.CmdList)
                {
                    SPCTree.Nodes[0].Nodes["Circles:"].Nodes[SpcIndex- Oringal_SpcW.pointsCloud.rects.Count -1].Nodes["Process:"].Nodes.Add(item);
                }
            }
        }
        private void tiffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SpcIndex == 0)
            {
                if (Oringal_SpcW.pointsCloud.SpcPath.Length > 0)
                {
                    Oringal_SpcW.pointsCloud.SaveTiffUshort();
                }
            }
            else if (SpcIndex > 0 && SpcIndex < Oringal_SpcW.pointsCloud.rects.Count + 1)
            {
                Oringal_SpcW.pointsCloud.rects[SpcIndex - 1].SPCwindow.pointsCloud.SaveTiffUshort();
            }
            else
            {
                Oringal_SpcW.pointsCloud.circles[SpcIndex - Oringal_SpcW.pointsCloud.circles.Count - 1].SPCwindow.pointsCloud.SaveTiffUshort();
            }
        }
        private void mpdatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SpcIndex == 0)
            {
                if (Oringal_SpcW.pointsCloud.SpcPath.Length > 0)
                {
                    Oringal_SpcW.pointsCloud.SaveMpdat();
                }
            }
            else if (SpcIndex > 0 && SpcIndex < Oringal_SpcW.pointsCloud.rects.Count + 1)
            {
                Oringal_SpcW.pointsCloud.rects[SpcIndex - 1].SPCwindow.pointsCloud.SaveMpdat();
            }
            else
            {
                Oringal_SpcW.pointsCloud.circles[SpcIndex - Oringal_SpcW.pointsCloud.circles.Count - 1].SPCwindow.pointsCloud.SaveMpdat();
            }
        }
    }
}
