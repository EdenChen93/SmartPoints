using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SmartPoints;


namespace SPCwindowUI
{
    public partial class SPCwindow: UserControl
    {
        enum GrapTools 
        {
            None=-1,
            Apoint=0,
            ALine=1,
            ARect=2,
            ACircle=3,
            GetInfo=4

        }
        private GrapTools tools = GrapTools.None;
        Point Mpoint;Point Epoint;Point Spoint;
        private Bitmap Origal_Bitmap;
        private Bitmap Pant_Bitmap;
        private Graphics graphics;
        public bool IsChecked = false;
        private bool GrapOn = false;
        //public SmartPoints.SmartPoints.RoiPointtList points = new SmartPoints.SmartPoints.RoiPointtList();
        //public SmartPoints.SmartPoints.RoiRectList pointsCloud.rects = new SmartPoints.SmartPoints.RoiRectList();
        //public SmartPoints.SmartPoints.RoiLineList pointsCloud.lines = new SmartPoints.SmartPoints.RoiLineList();
        //public SmartPoints.SmartPoints.RoiCircletList pointsCloud.circles = new SmartPoints.SmartPoints.RoiCircletList();
        public SmartPoints.SmartPoints.SmartPointsCloud pointsCloud;

        public SPCwindow()
        {
            InitializeComponent();
        }

        private void SPCWPictureBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.KeyCode:
                    break;
                case Keys.Modifiers:
                    break;
                case Keys.None:
                    break;
                case Keys.LButton:
                    break;
                case Keys.RButton:
                    break;
                case Keys.Cancel:
                    break;
                case Keys.MButton:
                    break;
                case Keys.XButton1:
                    break;
                case Keys.XButton2:
                    break;
                case Keys.Back:
                    switch (tools)
                    {
                        case GrapTools.Apoint:
                            if (pointsCloud.points.Count > 0)
                            {
                                
                                pointsCloud.points.RemoveAtIndex(pointsCloud.points.index);
                                FlushSPCW();
                                foreach (var item in pointsCloud.points)
                                {
                                    DrawPoint(item, Color.AliceBlue);
                                }
                            }
                            break;
                        case GrapTools.ALine:
                            if (pointsCloud.lines.Count>0)
                            {
                                pointsCloud.lines.RemoveAtIndex(pointsCloud.lines.index);
                                FlushSPCW();
                                foreach (var item in pointsCloud.lines)
                                {
                                    DrawLine(item, Color.White);
                                }
                            }
                            break;
                        case GrapTools.ARect:
                            if (pointsCloud.rects.Count>0)
                            {
                                if (pointsCloud.rects[pointsCloud.rects.index].SPCwindow != null)
                                {
                                    pointsCloud.rects[pointsCloud.rects.index].SPCwindow.Dispose();
                                }
                                pointsCloud.rects.RemoveAtIndex(pointsCloud.rects.index);
                                FlushSPCW();
                                foreach (var item in pointsCloud.rects)
                                {
                                    DrawRect(item, Color.White);
                                }
                            }
                            break;
                        case GrapTools.ACircle:
                            if (pointsCloud.circles.Count > 0)
                            {
                                if (pointsCloud.circles[pointsCloud.circles.index].SPCwindow != null)
                                {
                                    pointsCloud.circles[pointsCloud.circles.index].SPCwindow.Dispose();
                                }
                                pointsCloud.circles.RemoveAtIndex(pointsCloud.circles.index);
                                FlushSPCW();
                                foreach (var item in pointsCloud.circles)
                                {
                                    DrawCircle(item, Color.White);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.Tab:
                    break;
                case Keys.LineFeed:
                    break;
                case Keys.Clear:
                    break;
                case Keys.Return:
                    break;
                case Keys.ShiftKey:
                    break;
                case Keys.ControlKey:
                    break;
                case Keys.Menu:
                    break;
                case Keys.Pause:
                    break;
                case Keys.Capital:
                    break;
                case Keys.KanaMode:
                    break;
                case Keys.JunjaMode:
                    break;
                case Keys.FinalMode:
                    break;
                case Keys.HanjaMode:
                    break;
                case Keys.Escape:
                    break;
                case Keys.IMEConvert:
                    break;
                case Keys.IMENonconvert:
                    break;
                case Keys.IMEAccept:
                    break;
                case Keys.IMEModeChange:
                    break;
                case Keys.Space:
                    break;
                case Keys.Prior:
                    break;
                case Keys.Next:
                    break;
                case Keys.End:
                    break;
                case Keys.Home:
                    break;
                case Keys.Left:
                    break;
                case Keys.Up:
                    break;
                case Keys.Right:
                    break;
                case Keys.Down:
                    break;
                case Keys.Select:
                    break;
                case Keys.Print:
                    break;
                case Keys.Execute:
                    break;
                case Keys.Snapshot:
                    break;
                case Keys.Insert:
                    break;
                case Keys.Delete:
                    break;
                case Keys.Help:
                    break;
                case Keys.D0:
                    break;
                case Keys.D1:
                    break;
                case Keys.D2:
                    break;
                case Keys.D3:
                    break;
                case Keys.D4:
                    break;
                case Keys.D5:
                    break;
                case Keys.D6:
                    break;
                case Keys.D7:
                    break;
                case Keys.D8:
                    break;
                case Keys.D9:
                    break;
                case Keys.A:
                    switch (tools)
                    {
                        case GrapTools.Apoint:
                            Point point = pointsCloud.points[pointsCloud.points.index].rectangle.Location;
                            point.X--;
                            pointsCloud.points[pointsCloud.points.index].rectangle = new Rectangle(point, pointsCloud.points[pointsCloud.points.index].rectangle.Size);
                            FlushSPCW();
                            DrawPoint(pointsCloud.points[pointsCloud.points.index], Color.Orange);
                            break;
                        case GrapTools.ALine:
                            Point apointline = pointsCloud.lines[pointsCloud.lines.index].rectangle.Location;
                            apointline.X--;
                            pointsCloud.lines[pointsCloud.lines.index].rectangle = new Rectangle(apointline, pointsCloud.lines[pointsCloud.lines.index].rectangle.Size);
                            FlushSPCW();
                            DrawLine(pointsCloud.lines[pointsCloud.lines.index], Color.Orange);
                            break;
                        case GrapTools.ARect:
                            Point apoint = pointsCloud.rects[pointsCloud.rects.index].rectangle.Location;
                            apoint.X--;
                            pointsCloud.rects[pointsCloud.rects.index].rectangle = new Rectangle(apoint, pointsCloud.rects[pointsCloud.rects.index].rectangle.Size);
                            FlushSPCW();
                            DrawRect(pointsCloud.rects[pointsCloud.rects.index], Color.Orange);
                            break;
                        case GrapTools.ACircle:
                            Point apointcircle = pointsCloud.circles[pointsCloud.circles.index].rectangle.Location;
                            apointcircle.X--;
                            pointsCloud.circles[pointsCloud.circles.index].rectangle = new Rectangle(apointcircle, pointsCloud.circles[pointsCloud.circles.index].rectangle.Size);
                            FlushSPCW();
                            DrawCircle(pointsCloud.circles[pointsCloud.circles.index], Color.Orange);
                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.B:
                    break;
                case Keys.C:
                    tools = GrapTools.ACircle;
                    foreach (var item in pointsCloud.circles)
                    {
                        DrawCircle(item, Color.White);
                    }
                    break;
                case Keys.D:
                    switch (tools)
                    {
                        case GrapTools.Apoint:
                            Point point = pointsCloud.points[pointsCloud.points.index].rectangle.Location;
                            point.X++;
                            pointsCloud.points[pointsCloud.points.index].rectangle = new Rectangle(point, pointsCloud.points[pointsCloud.points.index].rectangle.Size);
                            FlushSPCW();
                            DrawPoint(pointsCloud.points[pointsCloud.points.index], Color.Orange);
                            break;
                        case GrapTools.ALine:
                            Point apointline = pointsCloud.lines[pointsCloud.lines.index].rectangle.Location;
                            apointline.X++;
                            pointsCloud.lines[pointsCloud.lines.index].rectangle = new Rectangle(apointline, pointsCloud.lines[pointsCloud.lines.index].rectangle.Size);
                            FlushSPCW();
                            DrawLine(pointsCloud.lines[pointsCloud.lines.index], Color.Orange);
                            break;
                        case GrapTools.ARect:
                            Point apoint = pointsCloud.rects[pointsCloud.rects.index].rectangle.Location;
                            apoint.X++;
                            pointsCloud.rects[pointsCloud.rects.index].rectangle = new Rectangle(apoint, pointsCloud.rects[pointsCloud.rects.index].rectangle.Size);
                            FlushSPCW();
                            DrawRect(pointsCloud.rects[pointsCloud.rects.index], Color.Orange);
                            break;
                        case GrapTools.ACircle:
                            Point apointcircle = pointsCloud.circles[pointsCloud.circles.index].rectangle.Location;
                            apointcircle.X++;
                            pointsCloud.circles[pointsCloud.circles.index].rectangle = new Rectangle(apointcircle, pointsCloud.circles[pointsCloud.circles.index].rectangle.Size);
                            FlushSPCW();
                            DrawCircle(pointsCloud.circles[pointsCloud.circles.index], Color.Orange);
                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.E:
                    break;
                case Keys.F:
                    FlushSPCW();
                    break;
                case Keys.G:
                    break;
                case Keys.H:
                    break;
                case Keys.I:
                    tools = GrapTools.GetInfo;
                    break;
                case Keys.J:
                    break;
                case Keys.K:
                    break;
                case Keys.L:
                    tools = GrapTools.ALine;
                    foreach (var item in pointsCloud.lines)
                    {
                        DrawLine(item,Color.White);
                    }
                    break;
                case Keys.M:
                    break;
                case Keys.N:
                    break;
                case Keys.O:
                    OpenDataFile();
                    break;
                case Keys.P:
                    tools = GrapTools.Apoint;
                    foreach (var item in pointsCloud.points)
                    {
                        DrawPoint(item, Color.Black);
                    }
                    break;
                case Keys.Q:
                    switch (tools)
                    {
                        case GrapTools.Apoint:
                            pointsCloud.points.index++;
                            if (pointsCloud.points.index >= pointsCloud.points.Count)
                            {
                                pointsCloud.points.index = 0;
                            }
                            for (int i = 0; i < pointsCloud.points.Count; i++)
                            {
                                if (i == pointsCloud.points.index)
                                {
                                    DrawPoint(pointsCloud.points[i], Color.Red);
                                }
                                else
                                {
                                    DrawPoint(pointsCloud.points[i], Color.White);
                                }
                            }
                            break;
                        case GrapTools.ALine:
                            pointsCloud.lines.index++;
                            if (pointsCloud.lines.index >= pointsCloud.lines.Count)
                            {
                                pointsCloud.lines.index = 0;
                            }
                            for (int i = 0; i < pointsCloud.lines.Count; i++)
                            {
                                if (i == pointsCloud.lines.index)
                                {
                                    DrawLine(pointsCloud.lines[i], Color.Red);
                                }
                                else
                                {
                                    DrawLine(pointsCloud.lines[i], Color.White);
                                }
                            }
                            break;
                        case GrapTools.ARect:
                            pointsCloud.rects.index++;
                            if (pointsCloud.rects.index >= pointsCloud.rects.Count)
                            {
                                pointsCloud.rects.index = 0;
                            }
                            for (int i = 0; i < pointsCloud.rects.Count; i++)
                            {
                                if (i == pointsCloud.rects.index)
                                {
                                    DrawRect(pointsCloud.rects[i], Color.Red);
                                }
                                else
                                {
                                    DrawRect(pointsCloud.rects[i], Color.White);
                                }
                            }
                            break;
                        case GrapTools.ACircle:
                            pointsCloud.circles.index++;
                            if (pointsCloud.circles.index >= pointsCloud.circles.Count)
                            {
                                pointsCloud.circles.index = 0;
                            }
                            for (int i = 0; i < pointsCloud.circles.Count; i++)
                            {
                                if (i == pointsCloud.circles.index)
                                {
                                    DrawCircle(pointsCloud.circles[i], Color.Red);
                                }
                                else
                                {
                                    DrawCircle(pointsCloud.circles[i], Color.White);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.R:
                    tools = GrapTools.ARect;
                    foreach (var item in pointsCloud.rects)
                    {
                        DrawRect(item,Color.White);
                    }
                    break;
                case Keys.S:
                    switch (tools)
                    {
                        case GrapTools.Apoint:
                            Point point = pointsCloud.points[pointsCloud.points.index].rectangle.Location;
                            point.Y++;
                            pointsCloud.points[pointsCloud.points.index].rectangle = new Rectangle(point, pointsCloud.points[pointsCloud.points.index].rectangle.Size);
                            FlushSPCW();
                            DrawPoint(pointsCloud.points[pointsCloud.points.index], Color.Orange);
                            break;
                        case GrapTools.ALine:
                            Point apointline = pointsCloud.lines[pointsCloud.lines.index].rectangle.Location;
                            apointline.Y++;
                            pointsCloud.lines[pointsCloud.lines.index].rectangle = new Rectangle(apointline, pointsCloud.lines[pointsCloud.lines.index].rectangle.Size);
                            FlushSPCW();
                            DrawLine(pointsCloud.lines[pointsCloud.lines.index], Color.Orange);
                            break;
                        case GrapTools.ARect:
                            Point apoint = pointsCloud.rects[pointsCloud.rects.index].rectangle.Location;
                            apoint.Y++;
                            pointsCloud.rects[pointsCloud.rects.index].rectangle =new Rectangle( apoint, pointsCloud.rects[pointsCloud.rects.index].rectangle.Size);
                            FlushSPCW();
                            DrawRect(pointsCloud.rects[pointsCloud.rects.index], Color.Orange);
                            break;
                        case GrapTools.ACircle:
                            Point apointcircle = pointsCloud.circles[pointsCloud.circles.index].rectangle.Location;
                            apointcircle.Y++;
                            pointsCloud.circles[pointsCloud.circles.index].rectangle = new Rectangle(apointcircle, pointsCloud.circles[pointsCloud.circles.index].rectangle.Size);
                            FlushSPCW();
                            DrawCircle(pointsCloud.circles[pointsCloud.circles.index], Color.Orange);
                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.T:
                    break;
                case Keys.U:
                    break;
                case Keys.V:
                    break;
                case Keys.W:
                    switch (tools)
                    {
                        case GrapTools.Apoint:
                            Point point = pointsCloud.points[pointsCloud.points.index].rectangle.Location;
                            point.Y--;
                            pointsCloud.points[pointsCloud.points.index].rectangle = new Rectangle(point, pointsCloud.points[pointsCloud.points.index].rectangle.Size);
                            FlushSPCW();
                            DrawPoint(pointsCloud.points[pointsCloud.points.index], Color.Orange);
                            break;
                        case GrapTools.ALine:
                            Point apointline = pointsCloud.lines[pointsCloud.lines.index].rectangle.Location;
                            apointline.Y--;
                            pointsCloud.lines[pointsCloud.lines.index].rectangle = new Rectangle(apointline, pointsCloud.lines[pointsCloud.lines.index].rectangle.Size);
                            FlushSPCW();
                            DrawLine(pointsCloud.lines[pointsCloud.lines.index], Color.Orange);
                            break;
                        case GrapTools.ARect:
                            Point apoint = pointsCloud.rects[pointsCloud.rects.index].rectangle.Location;
                            apoint.Y--;
                            pointsCloud.rects[pointsCloud.rects.index].rectangle = new Rectangle(apoint, pointsCloud.rects[pointsCloud.rects.index].rectangle.Size);
                            FlushSPCW();
                            DrawRect(pointsCloud.rects[pointsCloud.rects.index], Color.Orange);
                            break;
                        case GrapTools.ACircle:
                            Point apointcircle = pointsCloud.circles[pointsCloud.circles.index].rectangle.Location;
                            apointcircle.Y--;
                            pointsCloud.circles[pointsCloud.circles.index].rectangle = new Rectangle(apointcircle, pointsCloud.circles[pointsCloud.circles.index].rectangle.Size);
                            FlushSPCW();
                            DrawCircle(pointsCloud.circles[pointsCloud.circles.index], Color.Orange);
                            break;
                        default:
                            break;
                    }
                    break;
                case Keys.X:
                    break;
                case Keys.Y:
                    break;
                case Keys.Z:
                    break;
                case Keys.LWin:
                    break;
                case Keys.RWin:
                    break;
                case Keys.Apps:
                    break;
                case Keys.Sleep:
                    break;
                case Keys.NumPad0:
                    break;
                case Keys.NumPad1:
                    break;
                case Keys.NumPad2:
                    break;
                case Keys.NumPad3:
                    break;
                case Keys.NumPad4:
                    break;
                case Keys.NumPad5:
                    break;
                case Keys.NumPad6:
                    break;
                case Keys.NumPad7:
                    break;
                case Keys.NumPad8:
                    break;
                case Keys.NumPad9:
                    break;
                case Keys.Multiply:
                    break;
                case Keys.Add:
                    break;
                case Keys.Separator:
                    break;
                case Keys.Subtract:
                    break;
                case Keys.Decimal:
                    break;
                case Keys.Divide:
                    break;
                case Keys.F1:
                    break;
                case Keys.F2:
                    break;
                case Keys.F3:
                    break;
                case Keys.F4:
                    break;
                case Keys.F5:
                    break;
                case Keys.F6:
                    break;
                case Keys.F7:
                    break;
                case Keys.F8:
                    break;
                case Keys.F9:
                    break;
                case Keys.F10:
                    break;
                case Keys.F11:
                    break;
                case Keys.F12:
                    break;
                case Keys.F13:
                    break;
                case Keys.F14:
                    break;
                case Keys.F15:
                    break;
                case Keys.F16:
                    break;
                case Keys.F17:
                    break;
                case Keys.F18:
                    break;
                case Keys.F19:
                    break;
                case Keys.F20:
                    break;
                case Keys.F21:
                    break;
                case Keys.F22:
                    break;
                case Keys.F23:
                    break;
                case Keys.F24:
                    break;
                case Keys.NumLock:
                    break;
                case Keys.Scroll:
                    break;
                case Keys.LShiftKey:
                    break;
                case Keys.RShiftKey:
                    break;
                case Keys.LControlKey:
                    break;
                case Keys.RControlKey:
                    break;
                case Keys.LMenu:
                    break;
                case Keys.RMenu:
                    break;
                case Keys.BrowserBack:
                    break;
                case Keys.BrowserForward:
                    break;
                case Keys.BrowserRefresh:
                    break;
                case Keys.BrowserStop:
                    break;
                case Keys.BrowserSearch:
                    break;
                case Keys.BrowserFavorites:
                    break;
                case Keys.BrowserHome:
                    break;
                case Keys.VolumeMute:
                    break;
                case Keys.VolumeDown:
                    break;
                case Keys.VolumeUp:
                    break;
                case Keys.MediaNextTrack:
                    break;
                case Keys.MediaPreviousTrack:
                    break;
                case Keys.MediaStop:
                    break;
                case Keys.MediaPlayPause:
                    break;
                case Keys.LaunchMail:
                    break;
                case Keys.SelectMedia:
                    break;
                case Keys.LaunchApplication1:
                    break;
                case Keys.LaunchApplication2:
                    break;
                case Keys.OemSemicolon:
                    break;
                case Keys.Oemplus:
                    break;
                case Keys.Oemcomma:
                    break;
                case Keys.OemMinus:
                    break;
                case Keys.OemPeriod:
                    break;
                case Keys.OemQuestion:
                    break;
                case Keys.Oemtilde:
                    break;
                case Keys.OemOpenBrackets:
                    break;
                case Keys.OemPipe:
                    break;
                case Keys.OemCloseBrackets:
                    break;
                case Keys.OemQuotes:
                    break;
                case Keys.Oem8:
                    break;
                case Keys.OemBackslash:
                    break;                
                case Keys.ProcessKey:
                    break;
                case Keys.Packet:
                    break;
                case Keys.Attn:
                    break;
                case Keys.Crsel:
                    break;
                case Keys.Exsel:
                    break;
                case Keys.EraseEof:
                    break;
                case Keys.Play:
                    break;
                case Keys.Zoom:
                    break;
                case Keys.NoName:
                    break;
                case Keys.Pa1:
                    break;
                case Keys.OemClear:
                    break;
                case Keys.Shift:
                    break;
                case Keys.Control:
                    break;
                case Keys.Alt:
                    break;
                default:
                    break;
            }
        }

        private void SPCWPictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.SPCWPictureBox.Focus();
            this.IsChecked = true;
        }

        private void SPCWPictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.IsChecked = false;
        }

        private void SPCWPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            Spoint = SmartPoints.SmartPoints.SP_Translate2D.TransImageToPicbox(e.Location, SPCWPictureBox);
            GrapOn = true;
        }

        private void SPCWPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Epoint = SmartPoints.SmartPoints.SP_Translate2D.TransImageToPicbox(e.Location, SPCWPictureBox);
            GrapOn = false;
            switch (tools)
            {
                case GrapTools.Apoint:
                    DrawPoint(Epoint, Spoint);
                    SmartPoints.SmartPoints.RoiAreaPoint point = new SmartPoints.SmartPoints.RoiAreaPoint(new Point(Spoint.X + (Epoint.X - Spoint.X) / 2, Spoint.Y + (Epoint.Y - Spoint.Y) / 2), new Size(Epoint.X - Spoint.X, Epoint.Y - Spoint.Y));
                    pointsCloud.points.AddItem(point);
                    break;
                case GrapTools.ALine:
                    SmartPoints.SmartPoints.RoiAreaLine line = new SmartPoints.SmartPoints.RoiAreaLine(Spoint, new Size(Epoint.X - Spoint.X, Epoint.Y - Spoint.Y));
                    
                    pointsCloud.lines.AddItem(line);
                    break;
                case GrapTools.ARect:
                    SmartPoints.SmartPoints.RoiAreaRect rect = new SmartPoints.SmartPoints.RoiAreaRect(Spoint, new Size(Epoint.X - Spoint.X, Epoint.Y - Spoint.Y));
                    pointsCloud.rects.AddItem(rect);
                    break;
                case GrapTools.ACircle:
                    SmartPoints.SmartPoints.RoiAreaCircle circle = new SmartPoints.SmartPoints.RoiAreaCircle(Spoint, new Size((Epoint.X - Spoint.X)*2,2*( Epoint.X - Spoint.X)));
                    pointsCloud.circles.AddItem(circle);
                    break;
                default:
                    break;
            }
        }

        private void SPCWPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            Mpoint = SmartPoints.SmartPoints.SP_Translate2D.TransImageToPicbox(e.Location, SPCWPictureBox);
            switch (tools)
            {
                case GrapTools.Apoint:
                    break;
                case GrapTools.ALine:
                    if (GrapOn)
                    {
                        DrawLine(Mpoint, Spoint);
                    }
                    break;
                case GrapTools.ARect:
                    if (GrapOn)
                    {
                        DrawRect(Mpoint, Spoint);
                    }
                    break;
                case GrapTools.ACircle:
                    if (GrapOn)
                    {
                        DrawCircle(Mpoint, Spoint);
                    }
                    break;
                case GrapTools.GetInfo:
                    if (GrapOn)
                    {
                        GetInfoEvent(this.pointsCloud.GetValue(Mpoint.X, Mpoint.Y)[2].ToString());
                    }
                    break;
                default:
                    break;
            }
        }

        private void OpenDataFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "3D数据|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName.Length>0)
            {
                string[] FilePath = openFileDialog.FileName.Split('.');
                string FileName = FilePath[0];
                string FileType = FilePath[1];
                switch (FileType)
                {
                    case "mpdat":
                        pointsCloud = SmartPoints.SmartPoints.SP_FileReader.GetSpcPointsFromMpdataFile(FileName + "." + FileType);
                        Origal_Bitmap =pointsCloud.GetBitmapColor();
                        Pant_Bitmap = (Bitmap)Origal_Bitmap.Clone();
                        graphics = Graphics.FromImage(Pant_Bitmap);
                        SPCWPictureBox.Image = Origal_Bitmap;
                        DelSPCWE();
                        RegSPCWE();
                        pointsCloud.lines.Clear();
                        pointsCloud.rects.Clear();
                        pointsCloud.circles.Clear();
                        pointsCloud.points.Clear();
                        break;
                    case "bmp":
                        Origal_Bitmap= new Bitmap(FileName + "." + FileType);
                        Pant_Bitmap = (Bitmap)Origal_Bitmap.Clone();
                        graphics = Graphics.FromImage(Pant_Bitmap);
                        SPCWPictureBox.Image = Origal_Bitmap;
                        DelSPCWE();
                        RegSPCWE();
                        pointsCloud.lines.Clear();
                        pointsCloud.rects.Clear();
                        break;
                    case "jpg":
                        Origal_Bitmap = new Bitmap(FileName + "." + FileType);
                        Pant_Bitmap = (Bitmap)Origal_Bitmap.Clone();
                        graphics = Graphics.FromImage(Pant_Bitmap);
                        SPCWPictureBox.Image = Origal_Bitmap;
                        DelSPCWE();
                        RegSPCWE();
                        pointsCloud.lines.Clear();
                        pointsCloud.rects.Clear();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("选择正确的数据");
            }
        }
        private void DrawPoint(Point epoint,Point spoint)
        {
            graphics.DrawImage(Origal_Bitmap, Point.Empty);
            graphics.DrawRectangle(new Pen(Color.Yellow, 4), new Rectangle(spoint, new Size((epoint.X - spoint.X), epoint.Y - spoint.Y)));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void DrawRect(Point mpoint,Point spoint)
        {
            graphics.DrawImage(Origal_Bitmap, Point.Empty);
            graphics.DrawEllipse(new Pen(Color.Red, 4), new Rectangle(spoint.X - 10, spoint.Y - 10, 20, 20));
            graphics.DrawRectangle(new Pen(Color.White, 4), new Rectangle(spoint, new Size(mpoint.X - spoint.X, mpoint.Y - spoint.Y)));
            graphics.DrawEllipse(new Pen(Color.Blue, 4), new Rectangle(mpoint.X - 10, mpoint.Y - 10, 20, 20));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void DrawLine(Point mpoint, Point spoint)
        {
            graphics.DrawImage(Origal_Bitmap, Point.Empty);
            graphics.DrawEllipse(new Pen(Color.Red, 4), new Rectangle(spoint.X - 10, spoint.Y - 10, 20, 20));
            graphics.DrawLine(new Pen(Color.White, 4),mpoint,spoint);
            graphics.DrawEllipse(new Pen(Color.Blue, 4), new Rectangle(mpoint.X - 10, mpoint.Y - 10, 20, 20));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void DrawCircle(Point mpoint,Point spoint)
        {
            graphics.DrawImage(Origal_Bitmap, Point.Empty);
            Point circlepoint =spoint;
            int w =(mpoint.X - spoint.X)*2;
            int h = w;
            mpoint.Y = circlepoint.Y;
            graphics.DrawEllipse(new Pen(Color.Red, 4), new Rectangle(circlepoint.X - 10, circlepoint.Y - 10, 20, 20));
            graphics.DrawEllipse(new Pen(Color.White, 4), new Rectangle(new Point(circlepoint.X-w/2,circlepoint.Y-h/2), new Size(w, h)));
            graphics.DrawEllipse(new Pen(Color.Blue, 4), new Rectangle(mpoint.X - 10, mpoint.Y - 10, 20, 20));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void DrawPoint(SmartPoints.SmartPoints.RoiAreaPoint areaPoint,Color color)
        {
            graphics.DrawRectangle(new Pen(color, 4), new Rectangle(areaPoint.rectangle.X - areaPoint.rectangle.Width / 2, areaPoint.rectangle.Y - areaPoint.rectangle.Height / 2, areaPoint.rectangle.Width, areaPoint.rectangle.Height));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void DrawLine(SmartPoints.SmartPoints.RoiAreaLine areaLine,Color color)
        {
            //graphics.DrawImage(Origal_Bitmap, Point.Empty);
            graphics.DrawEllipse(new Pen(Color.Red, 4), new Rectangle(areaLine.rectangle.X - 10, areaLine.rectangle.Y - 10, 20, 20));
            graphics.DrawLine(new Pen(color, 4),areaLine.rectangle.Location,new Point(areaLine.rectangle.X+areaLine.rectangle.Width,areaLine.rectangle.Y+areaLine.rectangle.Height));
            graphics.DrawEllipse(new Pen(Color.Blue, 4), new Rectangle(areaLine.rectangle.X + areaLine.rectangle.Width-10, areaLine.rectangle.Y + areaLine.rectangle.Height-10, 20, 20));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void DrawRect(SmartPoints.SmartPoints.RoiAreaRect areaRect,Color color)
        {
            //graphics.DrawImage(Origal_Bitmap, Point.Empty);
            graphics.DrawEllipse(new Pen(Color.Red, 4), new Rectangle(areaRect.rectangle.X - 10, areaRect.rectangle.Y - 10, 20, 20));
            graphics.DrawRectangle(new Pen(color, 4), areaRect.rectangle);
            graphics.DrawEllipse(new Pen(Color.Blue, 4), new Rectangle(areaRect.rectangle.X + areaRect.rectangle.Width - 10, areaRect.rectangle.Y + areaRect.rectangle.Height - 10, 20, 20));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void DrawCircle(SmartPoints.SmartPoints.RoiAreaCircle areaCircle,Color color)
        {
            Rectangle rectangle = new Rectangle(areaCircle.rectangle.X - areaCircle.rectangle.Width / 2, areaCircle.rectangle.Y - areaCircle.rectangle.Height / 2, areaCircle.rectangle.Width, areaCircle.rectangle.Height);
            graphics.DrawEllipse(new Pen(Color.Red, 4), new Rectangle(areaCircle.rectangle.X - 10, areaCircle.rectangle.Y - 10, 20, 20));
            graphics.DrawEllipse(new Pen(color, 4), rectangle);
            graphics.DrawEllipse(new Pen(Color.Blue, 4), new Rectangle(areaCircle.rectangle.X - 10+areaCircle.rectangle.Width/2, areaCircle.rectangle.Y - 10, 20, 20));
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void FlushSPCW()
        {
            graphics.DrawImage(Origal_Bitmap, Point.Empty);
            SPCWPictureBox.Image = Pant_Bitmap;
        }
        private void RegSPCWE()
        {
            this.SPCWPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SPCWPictureBox_MouseDown);
            this.SPCWPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SPCWPictureBox_MouseMove);
            this.SPCWPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SPCWPictureBox_MouseUp);
            this.SPCWPictureBox.GotFocus += SPCWPictureBox_GotFocus;
            this.SPCWPictureBox.LostFocus += SPCWPictureBox_LostFocus;
            pointsCloud.rects.AddItemEvent += Rects_AddItemEvent;
            pointsCloud.rects.RemoveAtEvent += Rects_RemoveAtEvent;
        }

        private void SPCWPictureBox_LostFocus(object sender, EventArgs e)
        {
            this.BackColor = Color.Black;
        }

        private void SPCWPictureBox_GotFocus(object sender, EventArgs e)
        {
            this.BackColor = Color.Red;
        }

        private void Rects_RemoveAtEvent(int i)
        {

        }
        private void Rects_AddItemEvent(SmartPoints.SmartPoints.RoiAreaRect i)
        {

        }

        private void DelSPCWE()
        {
            this.SPCWPictureBox.MouseDown -= new System.Windows.Forms.MouseEventHandler(this.SPCWPictureBox_MouseDown);
            this.SPCWPictureBox.MouseMove -= new System.Windows.Forms.MouseEventHandler(this.SPCWPictureBox_MouseMove);
            this.SPCWPictureBox.MouseUp -= new System.Windows.Forms.MouseEventHandler(this.SPCWPictureBox_MouseUp);
            this.SPCWPictureBox.GotFocus -= SPCWPictureBox_GotFocus;
            this.SPCWPictureBox.LostFocus -= SPCWPictureBox_LostFocus;
            pointsCloud.rects.AddItemEvent -= Rects_AddItemEvent;
            pointsCloud.rects.RemoveAtEvent -= Rects_RemoveAtEvent;
        }
        public void Inilize()
        {
            Origal_Bitmap = pointsCloud.GetBitmapColor();
            Pant_Bitmap = (Bitmap)Origal_Bitmap.Clone();
            graphics = Graphics.FromImage(Pant_Bitmap);
            SPCWPictureBox.Image = Origal_Bitmap;
            DelSPCWE();
            RegSPCWE();
            pointsCloud.lines.Clear();
            pointsCloud.rects.Clear();
            pointsCloud.circles.Clear();
            pointsCloud.points.Clear();
        }
        public delegate void GetaInfoDelegate(string datainfo);
        public event GetaInfoDelegate GetInfoEvent;
    }
}
