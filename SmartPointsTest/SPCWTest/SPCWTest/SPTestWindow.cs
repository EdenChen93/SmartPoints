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
        public SPTestWindow()
        {
            InitializeComponent();
        }
        public static SPCwindowUI.SPCwindow Oringal_SpcW = new SPCwindow();
        private ChartWindowForm chartWindowForm;
        private MegaPhaseHD megaPhaseHD;
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Oringal_SpcW.Dispose();
            SpcwPanle.Update();
            AddNewSpcFromFile();
        }
        private void AddNewSpcFromFile()
        {
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
        }

        private Rectangle I_RoiAreaLineChangeEvent(Rectangle rectangle)
        {
            int i = Oringal_SpcW.pointsCloud.lines.index;
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
            chartWindowForm = new ChartWindowForm();
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
            chartWindowForm= new ChartWindowForm();
            List<float> data;
            Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.LineClipingFloatA(i.rectangle, out data);
            chartWindowForm._data_source = data;
            chartWindowForm.data_source = data;
            chartWindowForm.control_index = Oringal_SpcW.pointsCloud.circles.index;
            chartWindowForm.line_index = Oringal_SpcW.pointsCloud.circles[Oringal_SpcW.pointsCloud.circles.index].SPCwindow.pointsCloud.lines.index;
            chartWindowForm.UpdateChartData();
            chartWindowForm.UpdateLineTree();
            chartWindowForm.Show();
        }
        private Rectangle I_RoiAreaLineChangeEvent2(Rectangle rectangle)
        {
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
        }
        private void 触发ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            megaPhaseHD.HDSensor.FireSoftwareTrigger();
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
        }
        private void MegaPhaseHD_ProcessHDEvent(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            SPCwindow cwindow = new SPCwindow();
            cwindow.pointsCloud = cloud;
            cwindow.pointsCloud.rects.AddItemEvent += Rects_AddItemEvent;
            cwindow.pointsCloud.rects.RemoveAtEvent += Rects_RemoveAtEvent;
            cwindow.pointsCloud.circles.AddItemEvent += Circles_AddItemEvent;
            cwindow.pointsCloud.circles.RemoveAtEvent += Circles_RemoveAtEvent;
            cwindow.pointsCloud.points.AddItemEvent += Points_AddItemEvent;
            cwindow.pointsCloud.points.RemoveAtEvent += Points_RemoveAtEvent;
            cwindow.pointsCloud.lines.AddItemEvent += Lines_AddItemEvent;
            cwindow.pointsCloud.lines.RemoveAtEvent += Lines_RemoveAtEvent;
            cwindow.Inilize();
            this.BeginInvoke(new Action(() =>
            {
                SPCTree.Nodes.Add(cwindow.pointsCloud.SpcName);
                SpcTreeNodesAddNewInfo(SPCTree.Nodes[SPCTree.Nodes.Count - 1].Nodes, cwindow);
            }));
            if (Oringal_SpcW.pointsCloud==null)
            {
                Oringal_SpcW.pointsCloud = cwindow.pointsCloud;
                Oringal_SpcW.Inilize();
            }
            else
            {
                SpcwPanle.Controls.Add(cwindow);
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
                        foreach (var item in nodes)
                        {
                            SPCTree.Nodes[0].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes["Process"].Nodes.Add(item.ToString());
                        }
                    }
                    break;
                case 1:
                    if (SPCTree.Nodes[0].Nodes["Rects:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Count>0)
                    {
                        SPCTree.Nodes[0].Nodes["Rects:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Clear();
                    }
                    else
                    {
                        SPCTree.Nodes[0].Nodes["Rects:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Add("Process", "Process");
                        foreach (var item in nodes)
                        {
                            SPCTree.Nodes[0].Nodes["Rects:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes["Process"].Nodes.Add(item.ToString());
                        }
                    }
                    break;
                case 2:
                    if (SPCTree.Nodes[0].Nodes["Circles:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Count>0)
                    {
                        SPCTree.Nodes[0].Nodes["Circles:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Clear();
                    }
                    else
                    {
                        SPCTree.Nodes[0].Nodes["Circles:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes.Add("Process", "Process");
                        foreach (var item in nodes)
                        {
                            SPCTree.Nodes[0].Nodes["Circles:"].Nodes["Lines:"].Nodes[chartWindowForm.line_index].Nodes["Process"].Nodes.Add(item.ToString());
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
