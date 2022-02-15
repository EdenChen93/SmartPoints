using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MPSizectorS_DotNet;
using MPSizectorDotNet;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using BitMiracle.LibTiff;
using GlmNet;
using MathNet.Numerics;
using Microsoft.VisualBasic;
using Emgu.CV.Util;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using SPCwindowUI;

namespace SmartPoints
{
    public class SmartPoints
    {
        public class TaskXml:XmlDocument
        {
            public  TaskXml (string TaskName,List<string> cmdlist,List<List<string>> paramlist,Point[] points)
            {
                XmlDeclaration xmlDeclaration = this.CreateXmlDeclaration("1.0","utf-8","yes");
                this.AppendChild(xmlDeclaration);
                XmlElement RootElement = this.CreateElement("ATask");
                XmlAttribute TaskNameElement = this.CreateAttribute("ATaskName"); TaskNameElement.Value = TaskName;
                RootElement.Attributes.Append(TaskNameElement);
                XmlAttribute CmdElement = this.CreateAttribute("CMDList");CmdElement.Value = ConvertCmdStringListToString(cmdlist);
                RootElement.Attributes.Append(CmdElement);
                XmlAttribute ParamElement = this.CreateAttribute("ParamList");ParamElement.Value = ConvertParamListToString(paramlist);
                RootElement.Attributes.Append(ParamElement);
                XmlAttribute ROIPointElement = this.CreateAttribute("ROIPointList");ROIPointElement.Value = ConvertPointsToString(points);
                RootElement.Attributes.Append(ROIPointElement);
                this.AppendChild(RootElement);
            }
            public static string ConvertCmdStringListToString (List<string> cmdlist)
            {
                string t = "";
                for (int i = 0; i < cmdlist.Count; i++)
                {
                    if (i==cmdlist.Count-1)
                    {
                        t += cmdlist[i] + ",";
                    }
                    else
                    {
                        t += cmdlist[i];
                    }
                }
                return t;
            }
            public static List<string> ConvertStringToCmdList(string cmd)
            {
                List<string> cmdlist = new List<string>();
                string[] cmds = cmd.Split(',');
                for (int i = 0; i < cmd.Length; i++)
                {
                    cmdlist.Add(cmds[i]);
                }
                return cmdlist;
            }
            public static string ConvertParamListToString(List<List<string>> paramlist)
            {
                string t = "";
                for (int i = 0; i < paramlist.Count; i++)
                {
                    for (int n = 0; n < paramlist[i].Count; n++)
                    {
                        if (n==paramlist[i].Count-1)
                        {
                            t += paramlist[i][n] + "\t";
                        }
                        else
                        {
                            t += paramlist[i][n];
                        }
                    }
                    if (i!=paramlist.Count-1)
                    {
                        t += ",";
                    }
                }
                return t;
            }
            public static List<List<string>> ConvertStringToParamlist(string param)
            {
                string[] paramss = param.Split(',');
                List<List<string>> paramlist = new List<List<string>>(paramss.Length);
                for (int i = 0; i < paramss.Length; i++)
                {
                    string[] values = paramss[i].Split('\t');
                    for (int n = 0; n < values.Length; n++)
                    {
                        paramlist[i].Add(values[n]);
                    }
                }
                return paramlist;
            }
            public static Point[] ConvertStringToPoints(string ps)
            {
                Point[] points = new Point[2];
                string[] pss= ps.Split(',');
                points[0].X = int.Parse(pss[0]); points[0].Y = int.Parse(pss[1]);
                points[1].X = int.Parse(pss[2]); points[1].Y = int.Parse(pss[3]);
                return points;
            }
            public static string ConvertPointsToString(Point[] points)
            {
                string t = points[0].X + "," + points[0].Y + "," + points[1].X + "," + points[1].Y;
                return t;
            }
        }
        public enum FileType
        {
            csv = 0,
            asc = 1,
            m3dm = 2,
            mpdata = 3,
            tiff = 4,
            ply = 5,
            xyz = 6,
        }
        public enum DataSource
        {
            KEYENCES = 0,
            PRECITEC = 1,
            MAGEPHASE = 2,
            COG = 3,
        }
        public enum ProcessFunctions
        {
            FZR=0,
            FNE=1,
            CGCC=2,
            CGC=3,
            CGCP=4,
        }
        public class RoiAreaPoint
        {
            public RoiAreaPoint(int w = 0, int h = 0, int x = 0, int y = 0)
            {
                this.rectangle = new Rectangle(x, y, w, h);
            }
            public RoiAreaPoint(Point p, Size s)
            {
                this.rectangle = new Rectangle(p, s);
            }
            public delegate Rectangle RoiAreaPointChange(Rectangle rectangle);
            public event RoiAreaPointChange RoiAreaRectChangeEvent;
            private Rectangle _rectangle;
            public Rectangle rectangle
            {
                get { return _rectangle; }
                set
                {
                    if (_rectangle != null)
                    {
                        if (_rectangle != value)
                        {
                            RoiAreaRectChangeEvent += RoiAreaRect_RoiAreaPointChangeEvent1;
                            RoiAreaRectChangeEvent(value);
                            _rectangle = value;
                        }
                    }
                }
            }
            private Rectangle RoiAreaRect_RoiAreaPointChangeEvent1(Rectangle rectangle)
            {
                return this.rectangle;
                //throw new NotImplementedException();
            }
        }
        public class RoiPointtList : List<RoiAreaPoint>
        {
            public delegate void RemoveAtI(int i);
            public event RemoveAtI RemoveAtEvent;
            public int index { get; set; }

            public delegate void AddItemI(RoiAreaPoint i);
            public event AddItemI AddItemEvent;
            public void RemoveAtIndex(int i)
            {
                this.Remove(this[i]);
                RemoveAtEvent += RoiiPointList_RemoveAtEvent;
                RemoveAtEvent(i);
            }
            public void AddItem(RoiAreaPoint i)
            {
                this.Add(i);
                AddItemEvent += RoiiPointList_AddItemEvent;
                AddItemEvent(i);
            }
            private void RoiiPointList_AddItemEvent(RoiAreaPoint i)
            {
                //
            }
            private void RoiiPointList_RemoveAtEvent(int index)
            {
                //
            }
        }
        public class RoiAreaCircle
        {
            public RoiAreaCircle(int w = 0, int h = 0, int x = 0, int y = 0)
            {
                this.rectangle = new Rectangle(x, y, w, h);
            }
            public RoiAreaCircle(Point p, Size s)
            {
                this.rectangle = new Rectangle(p, s);
            }
            public delegate Rectangle RoiAreaCircleChange(Rectangle rectangle);
            public event RoiAreaCircleChange RoiAreaCircleChangeEvent;
            private Rectangle _rectangle;
            public Rectangle rectangle
            {
                get { return _rectangle; }
                set
                {
                    if (_rectangle != null)
                    {
                        if (_rectangle != value)
                        {
                            RoiAreaCircleChangeEvent += RoiAreaRect_RoiAreaCircleChangeEvent1;
                            RoiAreaCircleChangeEvent(value);
                            _rectangle = value;
                        }
                    }
                }
            }
            public SPCwindow SPCwindow { get; set; }
            private Rectangle RoiAreaRect_RoiAreaCircleChangeEvent1(Rectangle rectangle)
            {
                return this.rectangle;
                //throw new NotImplementedException();
            }
        }
        public class RoiCircletList : List<RoiAreaCircle>
        {
            public delegate void RemoveAtI(int i);
            public event RemoveAtI RemoveAtEvent;
            public int index { get; set; }

            public delegate void AddItemI(RoiAreaCircle i);
            public event AddItemI AddItemEvent;
            public void RemoveAtIndex(int i)
            {
                this.Remove(this[i]);
                RemoveAtEvent += RoiiCircleList_RemoveAtEvent;
                RemoveAtEvent(i);
            }
            public void AddItem(RoiAreaCircle i)
            {
                this.Add(i);
                AddItemEvent += RoiiCircleList_AddItemEvent;
                AddItemEvent(i);
            }
            private void RoiiCircleList_AddItemEvent(RoiAreaCircle i)
            {
                //
            }
            private void RoiiCircleList_RemoveAtEvent(int index)
            {
                //
            }
        }
        public class RoiAreaRect
        {
            public RoiAreaRect(int w = 0, int h = 0, int x = 0, int y = 0)
            {
                this.rectangle = new Rectangle(x,y,w,h);
            }
            public RoiAreaRect(Point p, Size s)
            {
                this.rectangle = new Rectangle(p,s);
            }
            public SPCwindow SPCwindow { get; set; }
            public delegate Rectangle RoiAreaRectChange(Rectangle rectangle);
            public event RoiAreaRectChange RoiAreaRectChangeEvent;
            private Rectangle _rectangle;
            public Rectangle rectangle
            {
                get { return _rectangle; }
                set
                {
                    if (_rectangle!=null)
                    {
                        if (_rectangle != value)
                        {
                            RoiAreaRectChangeEvent += RoiAreaRect_RoiAreaRectChangeEvent1;
                            RoiAreaRectChangeEvent(value);
                            _rectangle = value;
                        }
                    }
                }
            }
            private Rectangle RoiAreaRect_RoiAreaRectChangeEvent1(Rectangle rectangle)
            {
                return this.rectangle;
                //throw new NotImplementedException();
            }
        }
        public class RoiRectList:List<RoiAreaRect>
        {
            public delegate void RemoveAtI(int i);
            public event RemoveAtI RemoveAtEvent;
            public int index { get; set; }

            public delegate void AddItemI(RoiAreaRect i);
            public event AddItemI AddItemEvent;
            public void RemoveAtIndex(int i)
            {
                this.Remove(this[i]);
                RemoveAtEvent(i);
            }
            public void AddItem(RoiAreaRect i)
            {
                this.Add(i);
                AddItemEvent(i);
            }
            private void RoiRectList_AddItemEvent(RoiAreaRect i)
            {
               
            }
            private void RoiRectList_RemoveAtEvent(int index)
            {
                //
            }
        }
        public class RoiAreaLine
        {
            public RoiAreaLine(int w = 0, int h = 0, int x = 0, int y = 0)
            {
                this.rectangle = new Rectangle(x, y, w, h);
            }
            public RoiAreaLine(Point p, Size s)
            {
                this.rectangle = new Rectangle(p, s);
            }
            public delegate Rectangle RoiAreaLineChange(Rectangle rectangle);
            public event RoiAreaLineChange RoiAreaLineChangeEvent;
            private Rectangle _rectangle;
            public Rectangle rectangle
            {
                get { return _rectangle; }
                set
                {
                    if (_rectangle != null)
                    {
                        if (_rectangle != value)
                        {
                            RoiAreaLineChangeEvent += RoiAreaRect_RoiAreaLineChangeEvent1;
                            RoiAreaLineChangeEvent(value);
                            _rectangle = value;
                        }
                    }
                }
            }
            private Rectangle RoiAreaRect_RoiAreaLineChangeEvent1(Rectangle rectangle)
            {
                return this.rectangle;
                //throw new NotImplementedException();
            }
        }
        public class RoiLineList : List<RoiAreaLine>
        {
            public delegate void RemoveAtI(int i);
            public event RemoveAtI RemoveAtEvent;
            public int index { get; set; }
            public delegate void AddItemI(RoiAreaLine i);
            public event AddItemI AddItemEvent;

            public void RemoveAtIndex(int i)
            {
                this.Remove(this[i]);
                RemoveAtEvent += RoiLineList_RemoveAtEvent;
                RemoveAtEvent(i);
            }
            public void AddItem(RoiAreaLine i)
            {
                this.Add(i);
                AddItemEvent += RoiLineList_AddItemEvent;
                AddItemEvent(i);
            }

            private void RoiLineList_AddItemEvent(RoiAreaLine i)
            {
                //
            }

            private void RoiLineList_RemoveAtEvent(int index)
            {
                //
            }
        }
        public struct SpcPoints
        {
            public List<float> pointsx;
            public List<float> pointsy;
            public List<float> pointsz;
            public SpcPoints(int w, int h)
            {
                pointsx = new List<float>(w * h);
                pointsy = new List<float>(w * h);
                pointsz = new List<float>(w * h);
            }
        }
        public class SmartPointsCloud
        {
            public SmartPointsCloud(string spcnme, string spcpath, string spcsource, int width, int height, float zmax, float zmin,Point tlpoint,Point rbpoint)
            {
                this.SpcName = spcnme;
                this.SpcPath = spcpath;
                this.SpcSource = spcsource;
                this.Width = width;
                this.Height = height;
                this.Zmax = zmax;
                this.Zmin = zmin;
                this.RbPoint = rbpoint;
                this.TlPoint = tlpoint;
                this.Spcpoints = new SpcPoints(width, height);
                this.rects = new RoiRectList();
                this.lines = new RoiLineList();
                this.points = new RoiPointtList();
                this.circles = new RoiCircletList();
            }
            /// <summary>
            /// 获取点云的RGB彩色图，24bitsBitmap
            /// </summary>
            /// <returns></returns>
            public Bitmap GetBitmapColor()
            {
                Bitmap bitmap = SP_Image.FloatArray_BMP(this.Spcpoints.pointsz.ToArray(), this.Width, this.Height, this.Zmax, this.Zmin);
                return bitmap;
            }
            /// <summary>
            /// 获取点云的灰度图，24bitsBitmap
            /// </summary>
            /// <returns></returns>
            public Bitmap GetBitmapGray(float maxf=float.MaxValue,float minf=float.MinValue,bool r=true)
            {
                Bitmap bitmap;
                if (maxf!=float.MinValue&&minf!=float.MinValue)
                {
                     bitmap= SP_Image.FloatArray_BMPGray(this.Spcpoints.pointsz.ToArray(), this.Width, this.Height, maxf,minf);
                }
                else
                {
                     bitmap = SP_Image.FloatArray_BMPGray(this.Spcpoints.pointsz.ToArray(), this.Width, this.Height, this.Zmax, this.Zmin);
                }
                return bitmap;
            }
            /// <summary>
            /// 获取点云的黑白图，24bitsBitmap
            /// </summary>
            /// <returns></returns>
            public Bitmap GetBitmapWB()
            {
                Bitmap bitmap = SP_Image.FloatArray_BMPWB(this.Spcpoints.pointsz.ToArray(), this.Width, this.Height, this.Zmax, this.Zmin);
                return bitmap;
            }
            /// <summary>
            /// 获取指定索引出点的XYZ值
            /// </summary>
            /// <param name="x">X位置索引</param>
            /// <param name="y">Y位置索引</param>
            /// <returns></returns>
            public float[] GetValue(int x, int y)
            {
                int n = x + this.Width * y;
                float[] xyz = new float[3];
                xyz[0] = this.Spcpoints.pointsx[n];
                xyz[1] = this.Spcpoints.pointsy[n];
                xyz[2] = this.Spcpoints.pointsz[n];
                return xyz;
            }
            /// <summary>
            /// 保存成Csv格式，‘,'间隔
            /// </summary>
            public void SaveCsv(float pitch_x = 0.01f, float pitch_y = 0.01f)
            {
                try
                {
                    string[] FilePaths = this.SpcPath.Split(new string[] { "\\" }, StringSplitOptions.None);
                    string FilePath = "";
                    for (int i = 0; i < FilePaths.Length - 1; i++)
                    {
                        FilePath += FilePaths[i] + "\\";
                    }
                    string FileName = this.SpcName.Split('.')[0] +"XI_"+pitch_x+"PI_"+pitch_y+".csv";
                    System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(FilePath + FileName);
                    for (int y = 0; y < this.Height; y++)
                    {
                        for (int x = 0; x < this.Width; x++)
                        {
                            streamWriter.Write(this.Spcpoints.pointsz[x + y * this.Width]);
                            if (x < this.Width - 1)
                            {
                                streamWriter.Write(',');
                            }
                            else
                            {
                                if (y < this.Height - 1)
                                {
                                    streamWriter.Write("\r\n");
                                }
                            }
                        }
                    }
                    streamWriter.Close();
                    MessageBox.Show("保存成功");
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");
                    throw;
                }
            }
            /// <summary>
            /// 保存成Xyz格式，文本点云
            /// </summary>
            public void SaveXyz()
            {
                try
                {
                    string[] FilePaths = this.SpcPath.Split(new string[] { "\\" }, StringSplitOptions.None);
                    string FilePath = "";
                    for (int i = 0; i < FilePaths.Length - 1; i++)
                    {
                        FilePath += FilePaths[i] + "\\";
                    }
                    string FileName = this.SpcName.Split('.')[0] + ".xyz";
                    System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(FilePath + FileName);
                    for (int i = 0; i < this.Spcpoints.pointsx.Count; i++)
                    {
                        string data;
                        if (i != this.Spcpoints.pointsx.Count - 1)
                        {
                            data = this.Spcpoints.pointsx[i] + "\t" + this.Spcpoints.pointsy[i] + "\t" + this.Spcpoints.pointsz[i] + "\r\n";
                            streamWriter.Write(data);
                        }
                        else
                        {
                            data = this.Spcpoints.pointsx[i] + "\t" + this.Spcpoints.pointsy[i] + "\t" + this.Spcpoints.pointsz[i];
                            streamWriter.Write(data);
                        }
                    }
                    streamWriter.Close();
                    MessageBox.Show("保存成功");
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");
                    throw;
                }
            }
            /// <summary>
            /// 保存成Asc格式，文本点云
            /// </summary>
            /// <param name="pitch_x">X的点间隔，单位mm</param>
            /// <param name="pitch_y">Y的点间隔，单位mm</param>
            public void SaveASC(float pitch_x=0.01f,float pitch_y=0.01f)
            {
                #region 写头文件
                try
                {
                    if (this.SpcPath != null)
                    {
                        string[] FilePaths = this.SpcPath.Split(new string[] { "\\" }, StringSplitOptions.None);
                        string FilePath = "";
                        for (int i = 0; i < FilePaths.Length - 1; i++)
                        {
                            FilePath += FilePaths[i] + "\\";
                        }
                        string FileName = this.SpcName.Split('.')[0] + ".asc";
                        System.IO.StreamWriter writer = new System.IO.StreamWriter(FilePath + FileName);
                        writer.WriteLine("# File Format = ASCII");
                        writer.WriteLine("# Created " + DateTime.Now.ToString());
                        writer.WriteLine("# x-pixels = " + this.Width);
                        writer.WriteLine("# y-pixels = " + this.Height);
                        writer.WriteLine("# pitch_x = " + pitch_x);
                        writer.WriteLine("# pitch_y = " + pitch_y);
                        writer.WriteLine("# x-offset = " + 1);
                        writer.WriteLine("# y-offset = " + 0);
                        writer.WriteLine("# z-unit = um");
                        writer.WriteLine("# voidpixels = 0");
                        writer.WriteLine("# description =0:");
                        writer.WriteLine("# Start of Data:");
                        #endregion
                        #region 写数据
                        for (int y = 0; y < this.Height; y++)
                        {
                            for (int x = 0; x < this.Width; x++)
                            {
                                writer.Write(this.Spcpoints.pointsz[x+y*this.Width] + "\t");
                            }
                        }
                        #endregion
                        writer.Close();
                        MessageBox.Show("保存成功");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("保存失败");
                }
            }
            /// <summary>
            /// 保存成Tiff格式，点数据格式为浮点
            /// </summary>
            public void SaveTiffFloat(float pitch_x = 0.01f, float pitch_y = 0.01f)
            {
                string[] FilePaths = this.SpcPath.Split(new string[] { "\\" }, StringSplitOptions.None);
                string FilePath = "";
                for (int i = 0; i < FilePaths.Length - 1; i++)
                {
                    FilePath += FilePaths[i] + "\\";
                }
                string FileName = this.SpcName.Split('.')[0]+"XI_" + pitch_x + "YI_" + pitch_y+ ".tiff";
                SP_Image.WriteFloatListToTiff(this.Spcpoints.pointsz, FilePath + FileName, this.Width, this.Height);
            }
            /// <summary>
            /// 保存成Tiff格式，点数据格式为无符号双字节定点整数
            /// </summary>
            public void SaveTiffUshort(float pitch_x=0.01f,float pitch_y=0.01f)
            {
                string[] FilePaths = this.SpcPath.Split(new string[] { "\\" }, StringSplitOptions.None);
                string FilePath = "";
                float Zmin;float ZIncrement;
                for (int i = 0; i < FilePaths.Length - 1; i++)
                {
                    FilePath += FilePaths[i] + "\\";
                }
                List<ushort> ss = SP_Translate.FloatListToUshortList(this.Spcpoints.pointsz, out ZIncrement, out Zmin);
                string FileName = this.SpcName.Split('.')[0]+"XI_"+pitch_x+"YI_"+pitch_y+"Z0_"+Zmin+"ZI_"+ZIncrement+ ".tiff";
                SP_Image.WriteUshortListToTiff(ss, FilePath + FileName, this.Width, this.Height);
            }
            public SmartPointsCloud RectangleCliping(Point TLCorner,Point RBCorner)
            {
                int w = Math.Abs(TLCorner.X - RBCorner.X);
                int h = Math.Abs(TLCorner.Y - RBCorner.Y);
                int s = this.Width * (TLCorner.Y) + TLCorner.X;
                SpcPoints aspcPoints = new SpcPoints(w, h);
                for (int i = 0; i < h; i++)
                {
                    aspcPoints.pointsx.AddRange(this.Spcpoints.pointsx.GetRange(s + this.Width * i, w));
                    aspcPoints.pointsy.AddRange(this.Spcpoints.pointsy.GetRange(s + this.Width * i, w));
                    aspcPoints.pointsz.AddRange(this.Spcpoints.pointsz.GetRange(s + this.Width * i, w));
                }
                float maxf, minf = 0.0f;
                float[] gs = new float[aspcPoints.pointsz.Count];
                aspcPoints.pointsz.CopyTo(gs);
                List<float> ds = gs.ToList();
                ds.RemoveAll((a) => Single.IsNaN(a));
                ds.RemoveAll((a) => Single.IsNegativeInfinity(a));
                ds.RemoveAll((a) => Single.IsPositiveInfinity(a));
                ds.Sort();
                maxf = ds[ds.Count - 5];
                minf = ds[5];
                SmartPointsCloud acloud = new SmartPointsCloud(this.SpcName, this.SpcPath, DataSource.MAGEPHASE.ToString(), w, h, maxf, minf,new Point(0,0), new Point(0, 0));
                acloud.Spcpoints = aspcPoints;
                acloud.TlPoint = TLCorner;
                acloud.RbPoint = RBCorner;
                return acloud;
            }
            public SmartPointsCloud CircleCliping(RoiAreaCircle circle)
            {
                SpcPoints aspcPoints = new SpcPoints(circle.rectangle.Width, circle.rectangle.Height);
                int s = this.Width * (circle.rectangle.Y-circle.rectangle.Height/2) + circle.rectangle.X-circle.rectangle.Width/2;
                for (int i = 0; i < circle.rectangle.Height; i++)
                {
                    aspcPoints.pointsx.AddRange(this.Spcpoints.pointsx.GetRange(s + this.Width * i, circle.rectangle.Width));
                    aspcPoints.pointsy.AddRange(this.Spcpoints.pointsy.GetRange(s + this.Width * i, circle.rectangle.Width));
                    aspcPoints.pointsz.AddRange(this.Spcpoints.pointsz.GetRange(s + this.Width * i, circle.rectangle.Width));
                }
                float maxf, minf = 0.0f;
                float[] gs = new float[aspcPoints.pointsz.Count];
                aspcPoints.pointsz.CopyTo(gs);
                List<float> ds = gs.ToList();
                ds.RemoveAll((a) => Single.IsNaN(a));
                ds.RemoveAll((a) => Single.IsNegativeInfinity(a));
                ds.RemoveAll((a) => Single.IsPositiveInfinity(a));
                ds.Sort();
                maxf = ds.Last();
                minf = ds[0];
                PointF cpoint = new PointF(circle.rectangle.Width / 2.0f, circle.rectangle.Height / 2.0f);
                for (int y = 0; y < circle.rectangle.Height; y++)
                {
                    for (int x = 0; x < circle.rectangle.Width; x++)
                    {
                        int i = y * circle.rectangle.Width + x;

                        float d = (float)Math.Sqrt((x-cpoint.X) * (x-cpoint.X) + (y-cpoint.Y) * (y-cpoint.Y));
                        if (d>circle.rectangle.Width/2.0f)
                        {
                            aspcPoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                SmartPointsCloud acloud = new SmartPointsCloud(this.SpcName, this.SpcPath, DataSource.MAGEPHASE.ToString(), circle.rectangle.Width, circle.rectangle.Height, maxf, minf, new Point(0, 0), new Point(0, 0));
                acloud.Spcpoints = aspcPoints;
                acloud.TlPoint = new Point((circle.rectangle.X - circle.rectangle.Width / 2), (circle.rectangle.Y - circle.rectangle.Height / 2));
                acloud.RbPoint = new Point(acloud.TlPoint.X+circle.rectangle.Width,acloud.TlPoint.Y+circle.rectangle.Height);
                return acloud;
            }
            public List<double> LineCliping( Point[] poss, out List<vec4> point3Ds)
            {
                float[] points = this.Spcpoints.pointsz.ToArray();
                int w = this.Width;
                point3Ds = new List<vec4>();
                Tuple<double, double> tuple = new Tuple<double, double>(0, 0);
                List<double> parameter = new List<double>();
                try
                {
                    if (poss.Length > 1)
                    {
                        vec4 point = new vec4();
                        int Line_num = poss.Length - 1;
                        for (int i = 0; i < Line_num; i++)
                        {
                            double[] x = new double[2]; x[0] = poss[i].X; x[1] = poss[i + 1].X;
                            double[] y = new double[2]; y[0] = poss[i].Y; y[1] = poss[i + 1].Y;
                            tuple = Fit.Line(x, y);
                            parameter.Add(tuple.Item1); parameter.Add(tuple.Item2);
                            if (tuple.Item2 > 1f || tuple.Item2 < -1f)
                            {
                                if (y[0] > y[1])
                                {
                                    for (int n = (int)y[0]; n > (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                    {
                                        point.x = (float)((n - tuple.Item1) / tuple.Item2);
                                        point.y = n;
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Z;
                                        point3Ds.Add(point);
                                    }
                                }
                                else
                                {
                                    for (int n = (int)y[0]; n < (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                    {
                                        point.x = (float)((n - tuple.Item1) / tuple.Item2);
                                        point.y = n;
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        point3Ds.Add(point);
                                    }
                                }
                            }
                            else
                            {
                                if (x[0] > x[1])
                                {
                                    for (int n = (int)x[0]; n > (int)x[1]; n = n - (int)(x[0] - x[1]) / Math.Abs((int)(x[0] - x[1])))
                                    {
                                        point.x = n;
                                        point.y = (float)(tuple.Item2 * n + tuple.Item1);
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        point3Ds.Add(point);
                                    }
                                }
                                else
                                {
                                    for (int n = (int)x[0]; n < (int)x[1]; n = n - (int)(x[0] - x[1]) / Math.Abs((int)(x[0] - x[1])))
                                    {
                                        point.x = n;
                                        point.y = (float)(tuple.Item2 * n + tuple.Item1);
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        point3Ds.Add(point);
                                    }
                                }

                            }
                            if (point3Ds.Count == 0)
                            {
                                for (int n = (int)y[0]; n < (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                {

                                    point.x = (float)x[0];
                                    point.y = n;
                                    point.z = points[(int)point.x + (int)point.y * w];
                                    //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                    //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                    point3Ds.Add(point);
                                }
                            }
                        }
                    }
                    return parameter;

                }
                catch (Exception)
                {
                    return parameter;
                }
            }
            public List<double> LineClipingFloatA(Point[] poss, out List<float> pointsz)
            {
                float[] points = this.Spcpoints.pointsz.ToArray();
                int w = this.Width;
                pointsz = new List<float>();
                Tuple<double, double> tuple = new Tuple<double, double>(0, 0);
                List<double> parameter = new List<double>();
                try
                {
                    if (poss.Length > 1)
                    {
                        vec4 point = new vec4();
                        int Line_num = poss.Length - 1;
                        for (int i = 0; i < Line_num; i++)
                        {
                            double[] x = new double[2]; x[0] = poss[i].X; x[1] = poss[i + 1].X;
                            double[] y = new double[2]; y[0] = poss[i].Y; y[1] = poss[i + 1].Y;
                            tuple = Fit.Line(x, y);
                            parameter.Add(tuple.Item1); parameter.Add(tuple.Item2);
                            if (tuple.Item2 > 1f || tuple.Item2 < -1f)
                            {
                                if (y[0] > y[1])
                                {
                                    for (int n = (int)y[0]; n > (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                    {
                                        point.x = (float)((n - tuple.Item1) / tuple.Item2);
                                        point.y = n;
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Z;
                                        pointsz.Add(point.z);
                                    }
                                }
                                else
                                {
                                    for (int n = (int)y[0]; n < (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                    {
                                        point.x = (float)((n - tuple.Item1) / tuple.Item2);
                                        point.y = n;
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        pointsz.Add(point.z);
                                    }
                                }
                            }
                            else
                            {
                                if (x[0] > x[1])
                                {
                                    for (int n = (int)x[0]; n > (int)x[1]; n = n - (int)(x[0] - x[1]) / Math.Abs((int)(x[0] - x[1])))
                                    {
                                        point.x = n;
                                        point.y = (float)(tuple.Item2 * n + tuple.Item1);
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        pointsz.Add(point.z);
                                    }
                                }
                                else
                                {
                                    for (int n = (int)x[0]; n < (int)x[1]; n = n - (int)(x[0] - x[1]) / Math.Abs((int)(x[0] - x[1])))
                                    {
                                        point.x = n;
                                        point.y = (float)(tuple.Item2 * n + tuple.Item1);
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        pointsz.Add(point.z);
                                    }
                                }

                            }
                            if (pointsz.Count == 0)
                            {
                                for (int n = (int)y[0]; n < (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                {

                                    point.x = (float)x[0];
                                    point.y = n;
                                    point.z = points[(int)point.x + (int)point.y * w];
                                    //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                    //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                    pointsz.Add(point.z);
                                }
                            }
                        }
                    }
                    return parameter;

                }
                catch (Exception)
                {
                    return parameter;
                }
            }
            public List<double> LineClipingFloatA(Rectangle poss, out List<float> pointsz)
            {
                float[] points = this.Spcpoints.pointsz.ToArray();
                int w = this.Width;
                pointsz = new List<float>();
                Tuple<double, double> tuple = new Tuple<double, double>(0, 0);
                List<double> parameter = new List<double>();
                try
                {
                    if (poss!=null)
                    {
                        vec4 point = new vec4();
                        int Line_num = 1;
                        for (int i = 0; i < Line_num; i++)
                        {
                            double[] x = new double[2]; x[0] =poss.X; x[1] = poss.X+poss.Width;
                            double[] y = new double[2]; y[0] = poss.Y; y[1] = poss.Y+poss.Height;
                            tuple = Fit.Line(x, y);
                            parameter.Add(tuple.Item1); parameter.Add(tuple.Item2);
                            if (tuple.Item2 > 1f || tuple.Item2 < -1f)
                            {
                                if (y[0] > y[1])
                                {
                                    for (int n = (int)y[0]; n > (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                    {
                                        point.x = (float)((n - tuple.Item1) / tuple.Item2);
                                        point.y = n;
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Z;
                                        pointsz.Add(point.z);
                                    }
                                }
                                else
                                {
                                    for (int n = (int)y[0]; n < (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                    {
                                        point.x = (float)((n - tuple.Item1) / tuple.Item2);
                                        point.y = n;
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        pointsz.Add(point.z);
                                    }
                                }
                            }
                            else
                            {
                                if (x[0] > x[1])
                                {
                                    for (int n = (int)x[0]; n > (int)x[1]; n = n - (int)(x[0] - x[1]) / Math.Abs((int)(x[0] - x[1])))
                                    {
                                        point.x = n;
                                        point.y = (float)(tuple.Item2 * n + tuple.Item1);
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        pointsz.Add(point.z);
                                    }
                                }
                                else
                                {
                                    for (int n = (int)x[0]; n < (int)x[1]; n = n - (int)(x[0] - x[1]) / Math.Abs((int)(x[0] - x[1])))
                                    {
                                        point.x = n;
                                        point.y = (float)(tuple.Item2 * n + tuple.Item1);
                                        point.z = points[(int)point.x + (int)point.y * w];
                                        //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                        //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                        pointsz.Add(point.z);
                                    }
                                }

                            }
                            if (pointsz.Count == 0)
                            {
                                for (int n = (int)y[0]; n < (int)y[1]; n = n - (int)(y[0] - y[1]) / Math.Abs((int)(y[0] - y[1])))
                                {

                                    point.x = (float)x[0];
                                    point.y = n;
                                    point.z = points[(int)point.x + (int)point.y * w];
                                    //point.X = points.Data[(int)point.X + (int)point.Y * image_w].X;
                                    //point.Y = points.Data[(int)point.X + (int)point.Y * image_w].Y;
                                    pointsz.Add(point.z);
                                }
                            }
                        }
                    }
                    return parameter;

                }
                catch (Exception)
                {
                    return parameter;
                }
            }

            public bool FilterZRangeID(bool reverse=false)
            {
                string inputstr = Interaction.InputBox("当前区间：" + this.Zmax.ToString() + "," + this.Zmin, "输入Zrange", this.Zmax + "," + this.Zmin);
                string[] inputss = inputstr.Split(new char[] { ',' });
                float f = float.Parse(inputss[0]); float b = float.Parse(inputss[1]);
                float maxf = Math.Max(f, b); float minf = Math.Min(f, b);
                if (!reverse)
                {
                    for (int i = 0; i < this.Spcpoints.pointsz.Count; i++)
                    {
                        if (!(this.Spcpoints.pointsz[i]>=minf&&this.Spcpoints.pointsz[i]<=maxf))
                        {
                            this.Spcpoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.Spcpoints.pointsz.Count; i++)
                    {
                        if ((this.Spcpoints.pointsz[i] >= minf && this.Spcpoints.pointsz[i] <= maxf))
                        {
                            this.Spcpoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                this.Zmax = maxf;
                this.Zmin = minf;
                return reverse;
           }
            public bool FilterZRange(float maxf, float minf,bool reverse = false)
            {
                if (!reverse)
                {
                    for (int i = 0; i < this.Spcpoints.pointsz.Count; i++)
                    {
                        if (!(this.Spcpoints.pointsz[i] >= minf && this.Spcpoints.pointsz[i] <= maxf))
                        {
                            this.Spcpoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < this.Spcpoints.pointsz.Count; i++)
                    {
                        if ((this.Spcpoints.pointsz[i] >= minf && this.Spcpoints.pointsz[i] <= maxf))
                        {
                            this.Spcpoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                this.Zmax = maxf;
                this.Zmin = minf;
                return reverse;
            }
            public Point FilterNoiseID()
            {
                string inputstr = Interaction.InputBox("参数整数：" + 1 + "," + 5, "输入参数", 1 + "," + 5);
                string[] inputss = inputstr.Split(new char[] { ',' });
                int box= int.Parse(inputss[0]);
                int sd = int.Parse(inputss[1]);
                if (box % 2 == 0)
                {
                    box++;
                }
                List<bool> ds = new List<bool>();
                for (int y = 0 + box; y <this.Height - box; y++)
                {
                    for (int x = 0 + box; x < this.Width - box; x++)
                    {
                        if (!float.IsNaN(this.Spcpoints.pointsz[x + y * this.Width]))
                        {
                            for (int m = -box; m < box + 1; m++)
                            {
                                for (int n = -box; n < box + 1; n++)
                                {
                                    if (this.Spcpoints.pointsz[(x + m) + (y + n) *this.Width] - this.Spcpoints.pointsz[x + y * this.Width] < 1) { ds.Add(true); };
                                }
                            }
                            if (ds.Count < sd)
                            {
                                this.Spcpoints.pointsz[x + y * this.Width] = float.NaN;
                            }
                            ds.Clear();
                        }
                    }
                }
                return new Point(box, sd);  
            }
            public void FilterNoise(int box,int sd)
            {
                if (box % 2 == 0)
                {
                    box++;
                }
                List<bool> ds = new List<bool>();
                for (int y = 0 + box; y < this.Height - box; y++)
                {
                    for (int x = 0 + box; x < this.Width - box; x++)
                    {
                        if (!float.IsNaN(this.Spcpoints.pointsz[x + y * this.Width]))
                        {
                            for (int m = -box; m < box + 1; m++)
                            {
                                for (int n = -box; n < box + 1; n++)
                                {
                                    if (this.Spcpoints.pointsz[(x + m) + (y + n) * this.Width] - this.Spcpoints.pointsz[x + y * this.Width] < 1) { ds.Add(true); };
                                }
                            }
                            if (ds.Count < sd)
                            {
                                this.Spcpoints.pointsz[x + y * this.Width] = float.NaN;
                            }
                            ds.Clear();
                        }
                    }
                }
            }
            public void MatLeveling_3points(List<Point> Point_List)
            {
                int w = this.Width; int h =this.Height;
                float[] vsx = this.Spcpoints.pointsx.ToArray();
                float[] vsy = this.Spcpoints.pointsy.ToArray();
                float[] vsz = this.Spcpoints.pointsz.ToArray();
                double[] d =SmartPoints.SP_Translate3D.GetLineKInZPlane(this, 1, Point_List);
                double kkx = d[0]; double kky = d[1];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (float.IsNaN(vsz[x + y * w]))
                        {
                            this.Spcpoints.pointsz[x + y * w] = vsz[x + y * w];
                        }
                        else
                        {
                            this.Spcpoints.pointsz[x + y * w] = (float)(vsz[x + y * w] - kkx * (float)(vsx[x + y * w] - vsx[Point_List[1].X + Point_List[1].Y * w]) - kky * (float)(vsy[x + y * w] - vsy[Point_List[1].X + Point_List[1].Y * w]));
                        }
                    }
                }
            }
            public void FilterHistogram(List<List<float>> hist)
            {
                for (int i = 0; i < hist.Count; i++)
                {
                    float minf = hist[i].First();
                    float maxf = hist[i].Last();
                    float avg = hist[i].Average();
                    for (int n = 0; n < this.Spcpoints.pointsz.Count; n++)
                    {
                        if (this.Spcpoints.pointsz[n] > minf && this.Spcpoints.pointsz[n] < maxf) { this.Spcpoints.pointsz[n] = avg; }
                    }
                }
            }
            public List<string[]>  Proecess(List<string> processlist, List<List<string>> param)
            {
                this.ProcessXml = new TaskXml(this.SpcName, processlist, param,new Point[] {this.TlPoint,this.RbPoint});
                List<string[]> res = new List<string[]>();
                for (int i = 0; i < processlist.Count; i++)
                {
                    switch (processlist[i])
                    {
                        case "FZR":
                            this.FilterZRange(float.Parse(param[i][1]), float.Parse(param[i][2]), bool.Parse(param[i][0]));
                            break;
                        case "FNE":
                            this.FilterNoise(int.Parse(param[i][0]), int.Parse(param[i][1]));
                            break;
                        case "CGCC":
                            List<PointF> fs= SPCV.CvGetContoursCenters(this.GetBitmapGray());
                            string[] vs = new string[fs.Count];
                            for (int n = 0; n < fs.Count; n++)
                            {
                                if (n<fs.Count-1)
                                {
                                    vs[n] = fs[n].X+"\t"+fs[n].Y + "\t"+this.GetValue((int)Math.Round(fs[n].X),(int)Math.Round(fs[n].Y))[2]+ "\t";
                                }
                                else
                                {
                                    vs[n] = fs[n].X + "\t" + fs[n].Y+"\t"+ this.GetValue((int)Math.Round(fs[n].X), (int)Math.Round(fs[n].Y))[2];
                                }
                            }
                            res.Add(vs);
                            break;
                        case "CGC":
                            List<Point[]> points = SPCV.CvGetContours(this.GetBitmapGray());
                            string[] dd = new string[points.Count]; string apoint;string t="";
                            for (int m = 0; m < points.Count; m++)
                            {
                                t = "";
                                for (int c = 0; c < points[m].Length; c++)
                                {
                                    if (c<points[m].Length-1)
                                    {
                                         apoint = points[m][c].X+"\t"+points[m][c].Y + "\t";
                                    }
                                    else
                                    {
                                         apoint = points[m][c].X+"\t"+points[m][c].Y;
                                    }
                                    t += apoint;
                                }
                                dd[m] = t;
                                res.Add(dd);
                            }
                            break;
                        case "CGCP":
                            List<Point[]> pointss = SPCV.CvGetContoursInPoints(this.GetBitmapGray());
                            string[] dds = new string[pointss.Count]; string apoints; string ts = "";
                            for (int m = 0; m < pointss.Count; m++)
                            {
                                ts = "";
                                for (int c = 0; c < pointss[m].Length; c++)
                                {
                                    if (c < pointss[m].Length - 1)
                                    {
                                        apoints = pointss[m][c].X + "\t" + pointss[m][c].Y + "\t";
                                    }
                                    else
                                    {
                                        apoints = pointss[m][c].X + "\t" + pointss[m][c].Y;
                                    }
                                    ts += apoints;
                                }
                                dds[m] = ts;
                                res.Add(dds);
                            }
                            break;
                        default:
                            break;
                    }
                }
                return res;
            }
            public string SpcName { get; set; }
            public string SpcPath { get; set; }
            public string SpcSource { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public float Zmax { get; set; }
            public float Zmin { get; set; }
            public Point TlPoint { get; set; }
            public Point RbPoint { get; set; }
            public SpcPoints Spcpoints { get; set; }
            public RoiRectList rects { get; set; }
            public RoiLineList lines { get; set; }
            public RoiPointtList points { get; set; }
            public RoiCircletList circles { get; set; }
            public TaskXml ProcessXml { get; set; }
            
        }
        public class SmartPointsCloudContour
        {
            public string ControurName { get; set; }
        }
        public class SP_FileReader
        {
            public enum MPsdataindex
            {
                gray = 0,
                mask = 1,
                x = 2,
                y = 3,
                z = 4
            }
            public enum MPsdatat
            {
                Ft_gary = 0,
                Ft_mask = 1,
                Ft_x = 2,
                Ft_y = 3,
                Ft_z = 4,
                Fx_gary = 10,
                Fx_mask = 11,
                Fx_x = 12,
                Fx_y = 13,
                Fx_z = 14,
                Fxz_gray = 20,
                Fxz_mask = 21,
                Fxz_x = 22,
                Fxz_y = 23,
                Fxz_z = 24,
                Fxzs_grap = 34,
                I2D = 40
            }
            public static SmartPointsCloud GetSpcPointsFromTiffFile(string filepath,float XInterval = 0.01f, float YInterval = 0.01f, float ZInterval = 0.0003f, float X0 = 0.0f, float Y0 = 0.0f, float Z0 = 0.0f)
            {
                if (filepath.Length > 0)
                {
                    BitMiracle.LibTiff.Classic.Tiff tiff = BitMiracle.LibTiff.Classic.Tiff.Open(filepath, "r");
                    BitMiracle.LibTiff.Classic.FieldValue[] values_w = tiff.GetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGEWIDTH);
                    BitMiracle.LibTiff.Classic.FieldValue[] values_l = tiff.GetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGELENGTH);
                    BitMiracle.LibTiff.Classic.FieldValue[] values_dt = tiff.GetField(BitMiracle.LibTiff.Classic.TiffTag.DATATYPE);
                    foreach (var item in Enum.GetValues(typeof(BitMiracle.LibTiff.Classic.TiffTag)))
                    {
                        if (tiff.GetField((BitMiracle.LibTiff.Classic.TiffTag)item) != null)
                        {
                            Console.WriteLine(item.ToString() + ": " + tiff.GetField((BitMiracle.LibTiff.Classic.TiffTag)item)[0].ToString());
                        }
                    }
                    int w = (int)values_w[0].Value;int h = (int)values_l[0].Value; int dt;
                    if (values_dt != null)
                    {
                        dt = values_dt[0].ToInt();
                    }
                    else
                    {
                        dt = 0;
                    }
                    float[] data; float minf; float maxf;
                    SmartPointsCloud pointsCloud;
                    switch (dt)
                    {
                        case 3:
                            data = new float[w * h];
                            byte[] data_bs = new byte[4 * w];
                            for (int i = 0; i < h; i++)
                            {
                                tiff.ReadScanline(data_bs, i);
                                Buffer.BlockCopy(data_bs, 0, data, i * w * 4, w * 4);
                            }
                            break;
                        default:
                            data = new float[w * h];
                            ushort[] datas = new ushort[w];
                            byte[] data_bss = new byte[2 * w];
                            for (int i = 0; i < h; i++)
                            {
                                tiff.ReadScanline(data_bss, i);
                                Buffer.BlockCopy(data_bss, 0, datas, 0, w * 2);
                                for (int n = 0; n < w; n++)
                                {
                                    data[n + i * w] = (float)datas[n] * ZInterval + Z0;
                                }
                            }
                            break;
                    }
                    List<float> ds = data.ToList();
                    ds.RemoveAll((a) => Single.IsNaN(a));
                    ds.Sort();
                    maxf = ds[ds.Count - 1];
                    minf = ds[0];
                    pointsCloud = new SmartPointsCloud("TiffFile", filepath, DataSource.MAGEPHASE.ToString(), w, h, maxf, minf, new Point(0, 0), new Point(0, 0));
                    float[] datax = new float[w * h]; float[] datay = new float[w * h];
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            datax[x + y * w] = x * XInterval + X0;
                            datay[x + y * w] = y * YInterval + Y0;
                        }
                    }
                    pointsCloud.Spcpoints.pointsx.AddRange(datax);
                    pointsCloud.Spcpoints.pointsy.AddRange(datay);
                    pointsCloud.Spcpoints.pointsz.AddRange(data);
                    return pointsCloud;
                }
                else
                {
                    return null;
                }
            }
            public static SmartPointsCloud GetSpcPointsFromAscFile(string filepath, string spliter, float XInterval=5, float YInterval=5, float X0 = 0.0f, float Y0 = 0.0f)
            {
                try
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(filepath);
                    if (filepath != null)
                    {
                        string[] filename = filepath.Split(new string[] { "\\" }, StringSplitOptions.None);
                        string[] pointcloud_file = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        string[] data_1 = pointcloud_file[12].Split(new string[] { spliter }, StringSplitOptions.None);
                        List<string> vs = new List<string>();
                        vs = data_1.ToList();
                        vs.RemoveAll(n => n == "");
                        data_1 = vs.ToArray();
                        if (pointcloud_file[11] == "# Start of Data:")
                        {
                            int w = int.Parse(pointcloud_file[2].Split('=')[1]);
                            int h = int.Parse(pointcloud_file[3].Split('=')[1]);
                            SmartPointsCloud spc = new SmartPointsCloud(filename[filename.Length - 1], filepath, DataSource.PRECITEC.ToString(), w, h, 0, 0, new Point(0, 0), new Point(0, 0));
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    string test = data_1[y * w + x];
                                    spc.Spcpoints.pointsz.Add(float.Parse(data_1[y * w + x]));
                                    spc.Spcpoints.pointsx.Add(X0 + XInterval * x);
                                    spc.Spcpoints.pointsy.Add(Y0 + YInterval * y);
                                }
                            }
                            reader.Close();
                            float[] vss = new float[spc.Spcpoints.pointsz.Count]; spc.Spcpoints.pointsz.CopyTo(vss);
                            List<float> ds = vss.ToList();
                            ds.RemoveAll(a => float.IsNaN(a));
                            float minf = ds.Min(); float maxf = ds.Max();
                            spc.Zmax = minf;
                            spc.Zmin = maxf;
                            return spc;
                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static SmartPointsCloud GetSpcPointsFromCsvFile(string filepath, string spliter, float XInterval=0, float YInterval=0, float X0 = 0.0f, float Y0 = 0.0f)
            {
                try
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(filepath);
                    if (filepath != null)
                    {
                        string[] filename = filepath.Split(new string[] { "\\" }, StringSplitOptions.None);
                        string[] pointcloud_file_l = reader.ReadToEnd().Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        if (pointcloud_file_l.Length > 0)
                        {
                            int h = pointcloud_file_l.Length;
                            int w = pointcloud_file_l[0].Split(',').Length;
                            SmartPointsCloud spc = new SmartPointsCloud(filename[filename.Length - 1], filepath, DataSource.PRECITEC.ToString(), w, h, 0, 0, new Point(0, 0), new Point(0, 0));
                            for (int y = 0; y < h; y++)
                            {
                                string[] pointcloud_file_w = pointcloud_file_l[y].Split(',');
                                for (int x = 0; x < w; x++)
                                {
                                    spc.Spcpoints.pointsz.Add(float.Parse(pointcloud_file_w[x]));
                                    spc.Spcpoints.pointsx.Add(XInterval * x);
                                    spc.Spcpoints.pointsy.Add(YInterval * y);
                                }
                            }
                            reader.Close();
                            float[] vs = new float[spc.Spcpoints.pointsz.Count]; spc.Spcpoints.pointsz.CopyTo(vs);
                            List<float> ds = vs.ToList();
                            ds.RemoveAll(a => float.IsNaN(a));
                            float minf = ds.Min(); float maxf = ds.Max();
                            spc.Zmax = minf;
                            spc.Zmin = maxf;
                            return spc;
                        }
                        return null;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception)
                {
                    return null;
                }
            }
            public static SmartPointsCloud GetSpcPointsFromM3dmFile(string filepath)
            {
                string[] filename = filepath.Split(new string[] { "\\" }, StringSplitOptions.None);
                MP3DFrameManaged mP3D = new MP3DFrameManaged(filepath);
                float[] vs = new float[mP3D.SensorResolution];
                float[] vsx = new float[mP3D.SensorResolution];
                float[] vsy = new float[mP3D.SensorResolution];
                int w = (int)mP3D.SensorWidth; int h = (int)mP3D.SensorHeight;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (mP3D.Data[x + y * w].Z > -mP3D.ZMax && mP3D.Data[x + y * w].Z < mP3D.ZMax && mP3D.Data[x + y * w].Mask == 0 && mP3D.Data[x + y * w].Gray > 33)//
                        {
                            vs[x + y * w] = mP3D.Data[x + y * w].Z;
                            vsx[x + y * w] = mP3D.Data[x + y * w].X;
                            vsy[x + y * w] = mP3D.Data[x + y * w].Y;
                        }
                        else
                        {
                            vs[x + y * w] = float.NaN;
                            vsx[x + y * w] = mP3D.Data[x + y * w].X;
                            vsy[x + y * w] = mP3D.Data[x + y * w].Y;
                        }
                    }
                }
                List<float> ds = vs.ToList();
                ds.RemoveAll(a => float.IsNaN(a));
                float minf = ds.Min(); float maxf = ds.Max();
                SmartPointsCloud smartPointsCloud = new SmartPointsCloud(filename[filename.Length - 1], filepath, DataSource.MAGEPHASE.ToString(), w, h, maxf, minf, new Point(0, 0), new Point(0, 0));
                smartPointsCloud.Spcpoints.pointsx.AddRange(vsx);
                smartPointsCloud.Spcpoints.pointsy.AddRange(vsy);
                smartPointsCloud.Spcpoints.pointsz.AddRange(vs);
                return smartPointsCloud;
            }
            public static SmartPointsCloud GetSpcPointsFromM3dSensor(MP3DFrameManaged mP3D,string DeviceName,string path)
            {
                float[] vs = new float[mP3D.SensorResolution];
                float[] vsx = new float[mP3D.SensorResolution];
                float[] vsy = new float[mP3D.SensorResolution];
                int w = (int)mP3D.SensorWidth; int h = (int)mP3D.SensorHeight;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (mP3D.Data[x + y * w].Z > -mP3D.ZMax && mP3D.Data[x + y * w].Z < mP3D.ZMax && mP3D.Data[x + y * w].Mask == 0)
                        {
                            vs[x + y * w] = mP3D.Data[x + y * w].Z;
                            vsx[x + y * w] = mP3D.Data[x + y * w].X;
                            vsy[x + y * w] = mP3D.Data[x + y * w].Y;
                        }
                        else
                        {
                            vs[x + y * w] = float.NaN;
                            vsx[x + y * w] = mP3D.Data[x + y * w].X;
                            vsy[x + y * w] = mP3D.Data[x + y * w].Y;
                        }
                    }
                }
                List<float> ds = vs.ToList();
                ds.RemoveAll(a => float.IsNaN(a));
                float minf = ds.Min(); float maxf = ds.Max();
                SmartPointsCloud smartPointsCloud = new SmartPointsCloud(DeviceName, path, DataSource.MAGEPHASE.ToString(), w, h, maxf, minf, new Point(0, 0), new Point(0, 0));
                smartPointsCloud.Spcpoints.pointsx.AddRange(vsx);
                smartPointsCloud.Spcpoints.pointsy.AddRange(vsy);
                smartPointsCloud.Spcpoints.pointsz.AddRange(vs);
                return smartPointsCloud;
            }         
            public static SmartPointsCloud GetSpcPointsFromMpdataFile(string filepath)
            {
                string[] filename = filepath.Split(new string[] { "\\" }, StringSplitOptions.None);
                int w; int h; float maxf; float minf;
                float[] DataX = GetSpcPointsArray(filepath, MPsdataindex.x, out w, out h, out maxf, out minf);
                float[] DataY = GetSpcPointsArray(filepath, MPsdataindex.y, out w, out h, out maxf, out minf);
                float[] DataZ = GetSpcPointsArray(filepath, MPsdataindex.z, out w, out h, out maxf, out minf);
                SmartPointsCloud smartPointsCloud = new SmartPointsCloud(filename[filename.Length - 1], filepath, DataSource.MAGEPHASE.ToString(), w, h, maxf, minf, new Point(0, 0), new Point(0, 0));
                smartPointsCloud.Spcpoints.pointsx.AddRange(DataX);
                smartPointsCloud.Spcpoints.pointsy.AddRange(DataY);
                smartPointsCloud.Spcpoints.pointsz.AddRange(DataZ);
                return smartPointsCloud;
            }
            public static SmartPointsCloud GetSpcPointsFromMSSensor(UnmanagedDataFrameUndefinedStruct unknowdataframe)
            {
                int w; int h; float maxf; float minf;
                float[] DataX = GetSpcPointsArray_Sensor(unknowdataframe, MPsdataindex.x, out w, out h, out maxf, out minf);
                float[] DataY = GetSpcPointsArray_Sensor(unknowdataframe, MPsdataindex.y, out w, out h, out maxf, out minf);
                float[] DataZ = GetSpcPointsArray_Sensor(unknowdataframe, MPsdataindex.z, out w, out h, out maxf, out minf);
                SmartPointsCloud smartPointsCloud = new SmartPointsCloud(unknowdataframe.FrameInfo.DeviceInfo.DeviceName,"", DataSource.MAGEPHASE.ToString(), w, h, maxf, minf, new Point(0, 0), new Point(0, 0));
                smartPointsCloud.Spcpoints.pointsx.AddRange(DataX);
                smartPointsCloud.Spcpoints.pointsy.AddRange(DataY);
                smartPointsCloud.Spcpoints.pointsz.AddRange(DataZ);
                return smartPointsCloud;
            }
            public static float[] GetSpcPointsArray_Sensor(UnmanagedDataFrameUndefinedStruct unknowdataframe, MPsdataindex mPsdataindex, out int w, out int h, out float max, out float min)
            {
                w = 0; h = 0; float[] pointindex = null; byte[] rawdatabytes; byte[] indexdatabytes; int datat;
                max = unknowdataframe.FrameInfo.DeviceInfo.ZMax;
                min= -max;
                if (true)
                {
                    w = unknowdataframe.FrameInfo.DataInfo.XPixResolution;
                    h = unknowdataframe.FrameInfo.DataInfo.YPixResolution;
                    ConvertMpSdata2rawbytes(unknowdataframe, out rawdatabytes);
                    indexdatabytes = ConvertMpSdataIndexbytes(unknowdataframe.FrameInfo.DataInfo.DataFormat, w, h, rawdatabytes, mPsdataindex, out datat);
                    bool[] valids; bool[] InRanges;
                    switch ((MPsdatat)datat)
                    {
                        case MPsdatat.Ft_x:
                            pointindex = new float[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, pointindex, 0, indexdatabytes.Length);
                            break;
                        case MPsdatat.Ft_y:
                            pointindex = new float[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, pointindex, 0, indexdatabytes.Length);
                            break;
                        case MPsdatat.Ft_z:
                            pointindex = new float[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, pointindex, 0, indexdatabytes.Length);
                            GetMpdataValidPoints(unknowdataframe, out valids);
                            GetMpdataInRangePoints(unknowdataframe, out InRanges);
                            for (int i = 0; i < pointindex.Length; i++)
                            {
                                if (!valids[i] | !InRanges[i])
                                {
                                    pointindex[i] = float.NaN;
                                }
                            }
                            break;
                        case MPsdatat.Fx_x:
                            pointindex = new float[w * h];
                            ushort[] tempx = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempx, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    if (tempx[x + y * w] == ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = float.NegativeInfinity;
                                    }
                                    if (tempx[x + y * w] == ushort.MaxValue)
                                    {
                                        pointindex[x + y * w] = float.PositiveInfinity;
                                    }
                                    if (tempx[x + y * w] != ushort.MaxValue && tempx[x + y * w] != ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.X0Pos + tempx[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.XIncrement;
                                    }
                                }
                            }
                            break;
                        case MPsdatat.Fx_y:
                            pointindex = new float[w * h];
                            ushort[] tempy = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempy, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    if (tempy[x + y * w] == ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = float.NegativeInfinity;
                                    }
                                    if (tempy[x + y * w] == ushort.MaxValue)
                                    {
                                        pointindex[x + y * w] = float.PositiveInfinity;
                                    }
                                    if (tempy[x + y * w] != ushort.MaxValue && tempy[x + y * w] != ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Y0Pos + tempy[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.YIncrement;
                                    }
                                }
                            }
                            break;
                        case MPsdatat.Fx_z:
                            pointindex = new float[w * h];
                            ushort[] temp = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, temp, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Z0Pos + temp[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                }
                            }
                            GetMpdataValidPoints(unknowdataframe, out valids);
                            GetMpdataInRangePoints(unknowdataframe, out InRanges);
                            for (int i = 0; i < pointindex.Length; i++)
                            {
                                if (!valids[i] | !InRanges[i])
                                {
                                    pointindex[i] = float.NaN;
                                }
                            }
                            break;
                        case MPsdatat.Fxz_x:
                            pointindex = new float[w * h];
                            ushort[] tempfzx = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempfzx, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.X0Pos + tempfzx[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                }
                            }
                            break;
                        case MPsdatat.Fxz_y:
                            pointindex = new float[w * h];
                            ushort[] tempfzy = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempfzy, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Y0Pos + tempfzy[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                }
                            }
                            break;
                        case MPsdatat.Fxz_z:
                            pointindex = new float[w * h];
                            ushort[] tempfz = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempfz, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    if (tempfz[x + y * w] == ushort.MinValue)
                                    {

                                        pointindex[x + y * w] = float.NegativeInfinity;
                                    }
                                    if (tempfz[x + y * w] == ushort.MaxValue)
                                    {
                                        pointindex[x + y * w] = float.PositiveInfinity;
                                    }
                                    if (tempfz[x + y * w] != ushort.MaxValue && tempfz[x + y * w] != ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Z0Pos + tempfz[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                    }
                                }
                            }
                            GetMpdataValidPoints(unknowdataframe, out valids);
                            GetMpdataInRangePoints(unknowdataframe, out InRanges);
                            for (int i = 0; i < pointindex.Length; i++)
                            {
                                if (!valids[i] | !InRanges[i])//
                                {
                                    pointindex[i] = float.NaN;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    return pointindex;
                }
            }
            /// <summary>
            /// 将指定信息得字节数组转为浮点数组
            /// </summary>
            /// <param name="filepath">文件路径</param>
            /// <param name="mPsdataindex">数据序列</param>
            /// <param name="mPsdatat">数据序列</param>
            /// <param name="w">点云宽</param>
            /// <param name="h">点云高</param>
            /// <param name="max">点云Z向最大值</param>
            /// <param name="min">点云Z向最小值</param>
            /// <returns></returns>
            public static float[] GetSpcPointsArray(string filepath, MPsdataindex mPsdataindex, out int w, out int h, out float max, out float min)
            {
                w = 0; h = 0; float[] pointindex = null; byte[] rawdatabytes; byte[] indexdatabytes; int datat;
                MPSizectorS_DotNet.UnmanagedDataFrameUndefinedStruct unknowdataframe;
                bool IsLoad = UnmanagedDataFrameUndefinedStruct.LoadDataFrame(out unknowdataframe, filepath, out min, out max);
                if (IsLoad)
                {
                    w = unknowdataframe.FrameInfo.DataInfo.XPixResolution;
                    h = unknowdataframe.FrameInfo.DataInfo.YPixResolution;
                    ConvertMpSdata2rawbytes(unknowdataframe, out rawdatabytes);
                    indexdatabytes = ConvertMpSdataIndexbytes(unknowdataframe.FrameInfo.DataInfo.DataFormat, w, h, rawdatabytes, mPsdataindex, out datat);
                    bool[] valids;bool[] InRanges;
                    switch ((MPsdatat)datat)
                    {
                        case MPsdatat.Ft_x:
                            pointindex = new float[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, pointindex, 0, indexdatabytes.Length);
                            break;
                        case MPsdatat.Ft_y:
                            pointindex = new float[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, pointindex, 0, indexdatabytes.Length);
                            break;
                        case MPsdatat.Ft_z:
                            pointindex = new float[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, pointindex, 0, indexdatabytes.Length);
                            GetMpdataValidPoints(unknowdataframe, out valids);
                            GetMpdataInRangePoints(unknowdataframe, out InRanges);
                            for (int i = 0; i < pointindex.Length; i++)
                            {
                                if (!valids[i]|!InRanges[i])
                                {
                                    pointindex[i] = float.NaN;
                                }
                            }
                            break;
                        case MPsdatat.Fx_x:
                            pointindex = new float[w * h];
                            ushort[] tempx = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempx, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    if (tempx[x + y * w] == ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = float.NegativeInfinity;
                                    }
                                    if (tempx[x + y * w] == ushort.MaxValue)
                                    {
                                        pointindex[x + y * w] = float.PositiveInfinity;
                                    }
                                    if (tempx[x + y * w] != ushort.MaxValue && tempx[x + y * w] != ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.X0Pos + tempx[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.XIncrement;
                                    }
                                }
                            }
                            break;
                        case MPsdatat.Fx_y:
                            pointindex = new float[w * h];
                            ushort[] tempy = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempy, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    if (tempy[x + y * w] == ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = float.NegativeInfinity;
                                    }
                                    if (tempy[x + y * w] == ushort.MaxValue)
                                    {
                                        pointindex[x + y * w] = float.PositiveInfinity;
                                    }
                                    if (tempy[x + y * w] != ushort.MaxValue && tempy[x + y * w] != ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Y0Pos + tempy[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.YIncrement;
                                    }
                                }
                            }
                            break;
                        case MPsdatat.Fx_z:
                            pointindex = new float[w * h];
                            ushort[] temp = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, temp, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Z0Pos + temp[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                }
                            }
                            GetMpdataValidPoints(unknowdataframe, out valids);
                            GetMpdataInRangePoints(unknowdataframe, out InRanges);
                            for (int i = 0; i < pointindex.Length; i++)
                            {
                                if (!valids[i] | !InRanges[i])
                                {
                                    pointindex[i] = float.NaN;
                                }
                            }
                            break;
                        case MPsdatat.Fxz_x:
                            pointindex = new float[w * h];
                            ushort[] tempfzx = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempfzx, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                      pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.X0Pos + tempfzx[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                }
                            }
                            break;
                        case MPsdatat.Fxz_y:
                            pointindex = new float[w * h];
                            ushort[] tempfzy = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempfzy, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Y0Pos + tempfzy[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                }
                            }
                            break;
                        case MPsdatat.Fxz_z:
                            pointindex = new float[w * h];
                            ushort[] tempfz = new ushort[w * h];
                            Buffer.BlockCopy(indexdatabytes, 0, tempfz, 0, indexdatabytes.Length);
                            for (int y = 0; y < h; y++)
                            {
                                for (int x = 0; x < w; x++)
                                {
                                    if (tempfz[x + y * w] == ushort.MinValue)
                                    {

                                        pointindex[x + y * w] = float.NegativeInfinity;
                                    }
                                    if (tempfz[x + y * w] == ushort.MaxValue)
                                    {
                                        pointindex[x + y * w] = float.PositiveInfinity;
                                    }
                                    if (tempfz[x + y * w] != ushort.MaxValue && tempfz[x + y * w] != ushort.MinValue)
                                    {
                                        pointindex[x + y * w] = unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.Z0Pos + tempfz[x + y * w] * unknowdataframe.FrameInfo.PostProcessSettings.PointScaleSetting.ZIncrement;
                                    }
                                }
                            }
                            GetMpdataValidPoints(unknowdataframe, out valids);
                            GetMpdataInRangePoints(unknowdataframe, out InRanges);
                            for (int i = 0; i < pointindex.Length; i++)
                            {
                                if (!valids[i] | !InRanges[i])//
                                {
                                    pointindex[i] = float.NaN;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                    return pointindex;
                }
                else
                {
                    return pointindex;
                }

            }
            /// <summary>
            /// MPS数据转原始Byte数组
            /// </summary>
            /// <param name="unmanagedDataFrameUndefinedStruct"></param>
            /// <param name="rawdatabytes"></param>
            /// <returns></returns>
            public static bool ConvertMpSdata2rawbytes(UnmanagedDataFrameUndefinedStruct unmanagedDataFrameUndefinedStruct, out byte[] rawdatabytes)
            {

                int w = unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.XPixResolution;
                int h = unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.YPixResolution;
                int byteslength = 0;
                unsafe
                {
                    try
                    {
                        switch (unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.DataFormat)
                        {
                            case DataFormatType.FloatPointCloud:
                                byteslength = w * h * (sizeof(byte) * 4 + sizeof(float) * 3);
                                rawdatabytes = new byte[byteslength]; //G - byte,M - byte,XYZ - float
                                Marshal.Copy((IntPtr)unmanagedDataFrameUndefinedStruct.Data, rawdatabytes, 0, rawdatabytes.Length);
                                return true;
                            case DataFormatType.FixPointCloud:
                                byteslength = w * h * (sizeof(byte) * 2 + sizeof(ushort) * 3);
                                rawdatabytes = new byte[byteslength];//G-byte,M-byte,XYZ-ushort
                                Marshal.Copy((IntPtr)unmanagedDataFrameUndefinedStruct.Data, rawdatabytes, 0, rawdatabytes.Length);
                                return true;
                            case DataFormatType.FixZMap:
                                byteslength = w * h * (sizeof(byte) * 2 + sizeof(ushort) * 1);
                                rawdatabytes = new byte[byteslength];//G-byte,M-byte,Z-ushort
                                Marshal.Copy((IntPtr)unmanagedDataFrameUndefinedStruct.Data, rawdatabytes, 0, rawdatabytes.Length);
                                return true;
                            case DataFormatType.FixZMapSimple:
                                byteslength = w * h * sizeof(ushort) * 1;
                                rawdatabytes = new byte[byteslength];//Z-ushort
                                Marshal.Copy((IntPtr)unmanagedDataFrameUndefinedStruct.Data, rawdatabytes, 0, rawdatabytes.Length);
                                return true;
                            case DataFormatType.Image2D:
                                byteslength = w * h;
                                rawdatabytes = new byte[byteslength]; //G-byte
                                Marshal.Copy((IntPtr)unmanagedDataFrameUndefinedStruct.Data, rawdatabytes, 0, rawdatabytes.Length);
                                return true;
                            default:
                                rawdatabytes = null;
                                return false;
                        }
                    }
                    catch (Exception)
                    {
                        rawdatabytes = new byte[0];
                        return false;
                        throw;
                    }
                }
            }
            public static bool GetMpdataValidPoints(UnmanagedDataFrameUndefinedStruct unmanagedDataFrameUndefinedStruct, out bool[] rawvaildarray)
            {
                int w = unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.XPixResolution;
                int h = unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.YPixResolution;
                int l = w * h;
                rawvaildarray = new bool[l];
                switch  (unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.DataFormat)
                {
                    case DataFormatType.FloatPointCloud:
                        ManagedDataFrameFloat3DStruct managedDataFrameFloat = unmanagedDataFrameUndefinedStruct.ToManagedFloat3D();
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                               rawvaildarray[x + w * y] = managedDataFrameFloat.Data[x + w * y].IsValid;
                            }
                        }
                        return true;
                    case DataFormatType.FixPointCloud:
                        ManagedDataFrameFix3DStruct managedDataFrameFix = unmanagedDataFrameUndefinedStruct.ToManagedFix3D();
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                rawvaildarray[x + w * y] = managedDataFrameFix.Data[x + w * y].IsValid;
                            }
                        }
                        return true;
                    case DataFormatType.FixZMap:
                        ManagedDataFrameFixZMapStruct managedDataFrameFixZMap = unmanagedDataFrameUndefinedStruct.ToManagedFixZMap();
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                rawvaildarray[x + y * w] = managedDataFrameFixZMap.Data[x + y * w].IsValid;                              
                            }
                        }
                        return true;
                    case DataFormatType.FixZMapSimple:
                        for (int i = 0; i < rawvaildarray.Length; i++)
                        {
                            rawvaildarray[i] = true;
                        }
                        return true;
                    case DataFormatType.Image2D:
                        for (int i = 0; i < rawvaildarray.Length; i++)
                        {
                            rawvaildarray[i] = true;
                        }
                        return true;
                    default:
                        
                        return false;
                }
            }
            public static bool GetMpdataInRangePoints(UnmanagedDataFrameUndefinedStruct unmanagedDataFrameUndefinedStruct, out bool[] rawrangearray)
            {
                float max = unmanagedDataFrameUndefinedStruct.FrameInfo.DeviceInfo.ZMax;float min = -max;
                int w = unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.XPixResolution;
                int h = unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.YPixResolution;
                int l = w * h;
                rawrangearray = new bool[l];
                switch (unmanagedDataFrameUndefinedStruct.FrameInfo.DataInfo.DataFormat)
                {
                    case DataFormatType.FloatPointCloud:
                        ManagedDataFrameFloat3DStruct managedDataFrameFloat = unmanagedDataFrameUndefinedStruct.ToManagedFloat3D();
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                rawrangearray[x + w * y] = (managedDataFrameFloat.Data[x + w * y].Z > min && managedDataFrameFloat.Data[x + w * y].Z < max);
                            }
                        }
                        return true;
                    case DataFormatType.FixPointCloud:
                        ManagedDataFrameFix3DStruct managedDataFrameFix = unmanagedDataFrameUndefinedStruct.ToManagedFix3D();
                        min = 0;max = 65535;
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                rawrangearray[x + w * y] = (managedDataFrameFix.Data[x + w * y].Z > min && managedDataFrameFix.Data[x + w * y].Z < max);
                            }
                        }
                        return true;
                    case DataFormatType.FixZMap:
                        ManagedDataFrameFixZMapStruct managedDataFrameFixZMap = unmanagedDataFrameUndefinedStruct.ToManagedFixZMap();
                        for (int y = 0; y < h; y++)
                        {
                            for (int x = 0; x < w; x++)
                            {
                                rawrangearray[x + y * w] = (managedDataFrameFixZMap.Data[x + w * y].Z > 0 && managedDataFrameFixZMap.Data[x + w * y].Z < 65535);
                            }
                        }
                        return true;
                    case DataFormatType.FixZMapSimple:
                        for (int i = 0; i < rawrangearray.Length; i++)
                        {
                            rawrangearray[i] = true;
                        }
                        return true;
                    case DataFormatType.Image2D:
                        for (int i = 0; i < rawrangearray.Length; i++)
                        {
                            rawrangearray[i] = true;
                        }
                        return true;
                    default:

                        return false;
                }
            }
            /// <summary>
            /// 从MPS原始Byte数组种，提取指定信息的Bytes数组
            /// </summary>
            /// <param name="dataFormatType"></param>
            /// <param name="w"></param>
            /// <param name="h"></param>
            /// <param name="rawbytes"></param>
            /// <param name="mPsdataindex"></param>
            /// <param name="datat"></param>
            /// <returns></returns>
            public static byte[] ConvertMpSdataIndexbytes(DataFormatType dataFormatType, int w, int h, byte[] rawbytes, MPsdataindex mPsdataindex, out int datat)
            {
                datat = -1;
                try
                {
                    switch (dataFormatType)
                    {
                        case DataFormatType.FloatPointCloud:
                            switch (mPsdataindex)
                            {
                                case MPsdataindex.gray:
                                    byte[] graybytesOfFPC = new byte[w * h];
                                    for (int i = 0; i < graybytesOfFPC.Length; i++)
                                    {
                                        graybytesOfFPC[i] = rawbytes[i * 16];
                                    }
                                    datat = 0;
                                    return graybytesOfFPC;
                                case MPsdataindex.mask:
                                    byte[] maskbytesOfFPC = new byte[w * h];
                                    for (int i = 0; i < maskbytesOfFPC.Length; i++)
                                    {
                                        maskbytesOfFPC[i] = rawbytes[i * 16 + 1];
                                    }
                                    datat = 1;
                                    return maskbytesOfFPC;
                                case MPsdataindex.x:
                                    byte[] floatbytesx = new byte[w * h * sizeof(float)];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        Array.Copy(rawbytes, i * 16 + 4, floatbytesx, i * 4, 4);
                                    }
                                    datat = 2;
                                    return floatbytesx;
                                case MPsdataindex.y:
                                    byte[] floatbytesy = new byte[w * h * sizeof(float)];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        Array.Copy(rawbytes, i * 16 + 8, floatbytesy, i * 4, 4);
                                    }
                                    datat = 3;
                                    return floatbytesy;
                                case MPsdataindex.z:
                                    byte[] floatbytesz = new byte[w * h * sizeof(float)];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        Array.Copy(rawbytes, i * 16 + 12, floatbytesz, i * 4, 4);
                                    }
                                    datat = 4;
                                    return floatbytesz;
                                default:
                                    return null;
                            }
                        case DataFormatType.FixPointCloud:
                            switch (mPsdataindex)
                            {
                                case MPsdataindex.gray:
                                    byte[] graybytesofFXPC = new byte[w * h];
                                    for (int i = 0; i < graybytesofFXPC.Length; i++)
                                    {
                                        graybytesofFXPC[i] = rawbytes[i * 8];
                                    }
                                    datat = 10;
                                    return graybytesofFXPC;
                                case MPsdataindex.mask:
                                    byte[] maskbytesofFXPC = new byte[w * h];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        maskbytesofFXPC[i] = rawbytes[i * 8 + 1];
                                    }
                                    datat = 11;
                                    return maskbytesofFXPC;
                                case MPsdataindex.x:
                                    byte[] ushortx = new byte[w * h * sizeof(ushort)];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        Array.Copy(rawbytes, i * 8 + 2, ushortx, i * 2, 2);
                                    }
                                    datat = 12;
                                    return ushortx;
                                case MPsdataindex.y:
                                    byte[] ushorty = new byte[w * h * sizeof(ushort)];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        Array.Copy(rawbytes, i * 8 + 4, ushorty, i * 2, 2);
                                    }
                                    datat = 13;
                                    return ushorty;
                                case MPsdataindex.z:
                                    byte[] ushortz = new byte[w * h * sizeof(ushort)];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        Array.Copy(rawbytes, i * 8 + 6, ushortz, i * 2, 2);
                                    }
                                    datat = 14;
                                    return ushortz;
                                default:
                                    return null;
                            }
                        case DataFormatType.FixZMap:
                            switch (mPsdataindex)
                            {
                                case MPsdataindex.gray:
                                    byte[] graybytesofFXZ = new byte[w * h];
                                    for (int i = 0; i < graybytesofFXZ.Length; i++)
                                    {
                                        graybytesofFXZ[i] = rawbytes[i * 4];
                                    }
                                    datat = 20;
                                    return graybytesofFXZ;
                                case MPsdataindex.mask:
                                    byte[] maskbytesofFXZ = new byte[w * h];
                                    for (int i = 0; i < maskbytesofFXZ.Length; i++)
                                    {
                                        maskbytesofFXZ[i] = rawbytes[i * 4 + 1];
                                    }
                                    datat = 21;
                                    return maskbytesofFXZ;
                                case MPsdataindex.x:
                                    byte[] xvaluebytesofFXZ = new byte[w * h*sizeof(ushort)];
                                    for (ushort y = 0; y < h; y++)
                                    {
                                        for (ushort x = 0; x < w; x++)
                                        {
                                            Array.Copy(BitConverter.GetBytes(x), 0, xvaluebytesofFXZ, (x + y * w) * sizeof(ushort), 2);
                                        }
                                    }
                                    datat = 22;
                                    return xvaluebytesofFXZ;
                                case MPsdataindex.y:
                                    byte[] yvaluebytesofFXZ = new byte[w * h * sizeof(ushort)];
                                    for (ushort y = 0; y < h; y++)
                                    {
                                        for (ushort x = 0; x < w; x++)
                                        {
                                            Array.Copy(BitConverter.GetBytes(y), 0, yvaluebytesofFXZ, (x + y * w) * sizeof(ushort), 2);
                                        }
                                    }
                                    datat = 23;
                                    return yvaluebytesofFXZ;
                                case MPsdataindex.z:
                                    byte[] ushortz = new byte[w * h * sizeof(ushort)];
                                    for (int i = 0; i < w * h; i++)
                                    {
                                        Array.Copy(rawbytes, i * 4 + 2, ushortz, i * 2, 2);
                                    }
                                    datat = 24;
                                    return ushortz;
                                default:
                                    return null;
                            }
                        case DataFormatType.FixZMapSimple:
                            switch (mPsdataindex)
                            {
                                case MPsdataindex.gray:
                                    return null;
                                case MPsdataindex.mask:
                                    return null;
                                case MPsdataindex.x:
                                    return null;
                                case MPsdataindex.y:
                                    return null;
                                case MPsdataindex.z:
                                    datat = 34;
                                    return rawbytes;
                                default:
                                    return null;
                            }
                        case DataFormatType.Image2D:
                            switch (mPsdataindex)
                            {
                                case MPsdataindex.gray:
                                    datat = 40;
                                    return rawbytes;
                                case MPsdataindex.mask:
                                    return null;
                                case MPsdataindex.x:
                                    return null;
                                case MPsdataindex.y:
                                    return null;
                                case MPsdataindex.z:
                                    return null;
                                default:
                                    return null;
                            }
                        default:
                            return null;
                    }
                }
                catch (Exception)
                {
                    //return null;
                    throw;

                }
            }
        }
        public class SP_Color
        {
            public static Color TransZToRgbColor(float measurevalue, float max_value, float min_value)
            {
                int color;
                Color color_rpg = Color.FromArgb(0, 0, 0);
                measurevalue = measurevalue - min_value;
                max_value = max_value - min_value;
                if (measurevalue / max_value <= 0.125)
                {
                    color = (int)((measurevalue / max_value) / 0.125 * 255);
                    if (color <= 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(color, color, color);
                    return color_rpg;
                }//由黑到白
                if (measurevalue / max_value >= 0.125 && measurevalue / max_value < 0.25)
                {
                    color = (int)(((measurevalue / max_value) - 0.125) / 0.125 * 255);
                    if (color <= 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(255,255-color,255- color);
                    return color_rpg;
                }//由白到红
                if (measurevalue / max_value >= 0.25 && measurevalue / max_value < 0.375)
                {
                    color = (int)(((measurevalue / max_value) - 0.25) / 0.125 * 255);
                    if (color <= 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(255, color, 0);
                    return color_rpg;
                }//由红到黄
                if (measurevalue / max_value >= 0.375 && measurevalue / max_value < 0.5)
                {
                    color = (int)(((measurevalue / max_value) - 0.375) / 0.125 * 255);
                    if (color <= 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(255-color, 255, 0);
                    return color_rpg;
                }//由黄到绿
                if (measurevalue / max_value >= 0.5 && measurevalue / max_value < 0.625)
                {
                    color = (int)(((measurevalue / max_value) - 0.5) / 0.125 * 255);
                    if (color <= 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(0, 255 , color);
                    return color_rpg;
                }//由绿转青
                if (measurevalue / max_value >= 0.625 && measurevalue / max_value < 0.75)
                {
                    color = (int)(((measurevalue / max_value) - 0.625) / 0.125 * 255);
                    if (color <= 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(0, 255-color, 255);
                    return color_rpg;
                }//由青转蓝
                if (measurevalue / max_value >= 0.75 && measurevalue / max_value < 0.875)
                {
                    color = (int)(((measurevalue / max_value) - 0.75) / 0.125 * 255);
                    if (color > 255)
                    {
                        color = 255;
                    }
                    if (color < 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(color,0, 255);
                    return color_rpg;
                }//由蓝转紫
                if (measurevalue / max_value >= 0.875 && measurevalue / max_value < 1)
                {
                    color = (int)(((measurevalue / max_value) - 0.875) / 0.125 * 255);
                    if (color > 255)
                    {
                        color = 255;
                    }
                    if (color < 0)
                    {
                        color = 0;
                    }
                    color_rpg = Color.FromArgb(255, color, 255);
                    return color_rpg;
                }//由紫到白
                return color_rpg;
            }
            public static Color TransZToGrayColor(float measurevalue, float max_value, float min_value)
            {
                Color c;
                if (float.IsNaN(measurevalue))
                {
                    c = Color.FromArgb(0, 0, 0);
                }
                else
                {
                    int a = (int)((measurevalue - min_value) / (max_value - min_value) * 255); if (a < 0 | a > 255)
                    {
                        a = 0;
                    }
                    c = Color.FromArgb(a, a, a);
                }
                return c;
            }
            public static Color TransZToWBColor(float measurevalue, float max_value, float min_value)
            {
                Color c;
                if (float.IsNaN(measurevalue))
                {
                    c = Color.FromArgb(0, 0, 0);
                }
                else
                {
                    int a = (int)((measurevalue - min_value) / (max_value - min_value) * 255);
                    if (a < 0 | a > 255)
                    {
                        a = 0 ;
                    }
                    else
                    {
                        a = 255;
                    }
                    c = Color.FromArgb(a, a, a);
                }
                return c;
            }
        }
        public class SP_Image
        {
            public static Bitmap BitmapRectangleCliping(Bitmap src, Point TLCorner, Point RBCorner)
            {
                int src_w = src.Width;
                int src_h = src.Height;
                int dst_w = RBCorner.X - TLCorner.X;
                int dst_h = RBCorner.Y - TLCorner.Y;
                Bitmap dst = new Bitmap(dst_w, dst_h);

                System.Drawing.Imaging.BitmapData src_data = src.LockBits(new Rectangle(0, 0, src_w, src_h), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                System.Drawing.Imaging.BitmapData dst_data = dst.LockBits(new Rectangle(0, 0, dst_w, dst_h), System.Drawing.Imaging.ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                IntPtr src_ptr = src_data.Scan0;
                IntPtr dst_ptr = dst_data.Scan0;

                byte[] src_rawbytes = new byte[src_data.Stride * src_h];
                byte[] dst_rawbytes = new byte[dst_data.Stride * dst_h];

                Marshal.Copy(src_ptr, src_rawbytes, 0, src_rawbytes.Length);
                Marshal.Copy(dst_ptr, dst_rawbytes, 0, dst_rawbytes.Length);

                for (int y = TLCorner.Y; y < RBCorner.Y; y++)
                {
                    Buffer.BlockCopy(src_rawbytes, y * src_data.Stride + TLCorner.X * 3, dst_rawbytes, (y - TLCorner.Y) * dst_data.Stride, dst_w * 3);
                }

                Marshal.Copy(dst_rawbytes, 0, dst_ptr, dst_rawbytes.Length);

                src.UnlockBits(src_data);
                dst.UnlockBits(dst_data);
                return dst;
            }


            public static Bitmap FloatArray_BMP(float[] vs, int w, int h, float maxf, float minf)
            {
                    Bitmap bitmaps = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    System.Drawing.Imaging.BitmapData bitmapData = bitmaps.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    IntPtr ptr = bitmapData.Scan0;
                    int offset = bitmapData.Stride - w * 3;
                    byte[] bitmaprawbytes = new byte[bitmapData.Stride * h];
                    int posscan = 0; int posreal = 0;
                    byte[] rawvalue = Float2RgbBytes(vs, w, h, maxf, minf);
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                bitmaprawbytes[posscan + i] = rawvalue[posreal + i];
                            }
                            posscan = posscan + 3;
                            posreal = posreal + 3;
                        }
                        posscan = posscan + offset;
                    }
                    Marshal.Copy(bitmaprawbytes, 0, ptr, bitmaprawbytes.Length);
                    bitmaps.UnlockBits(bitmapData);
                    return bitmaps;
            }
            public static Bitmap FloatArray_BMPGray(float[] vs, int w, int h, float maxf, float minf)
            {
                try
                {
                    Bitmap bitmaps = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    System.Drawing.Imaging.BitmapData bitmapData = bitmaps.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    IntPtr ptr = bitmapData.Scan0;
                    int offset = bitmapData.Stride - w * 3;
                    byte[] bitmaprawbytes = new byte[bitmapData.Stride * h];
                    int posscan = 0; int posreal = 0;
                    byte[] rawvalue = Float2GrayBytes(vs, w, h, maxf, minf);
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                bitmaprawbytes[posscan + i] = rawvalue[posreal + i];
                            }
                            posscan = posscan + 3;
                            posreal = posreal + 3;
                        }
                        posscan = posscan + offset;
                    }
                    Marshal.Copy(bitmaprawbytes, 0, ptr, bitmaprawbytes.Length);
                    bitmaps.UnlockBits(bitmapData);
                    return bitmaps;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static Bitmap FloatArray_BMPWB(float[] vs, int w, int h, float maxf, float minf)
            {
                try
                {
                    Bitmap bitmaps = new Bitmap(w, h, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    System.Drawing.Imaging.BitmapData bitmapData = bitmaps.LockBits(new Rectangle(0, 0, w, h), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    IntPtr ptr = bitmapData.Scan0;
                    int offset = bitmapData.Stride - w * 3;
                    byte[] bitmaprawbytes = new byte[bitmapData.Stride * h];
                    int posscan = 0; int posreal = 0;
                    byte[] rawvalue = Float2WBBytes(vs, w, h, maxf, minf);
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                bitmaprawbytes[posscan + i] = rawvalue[posreal + i];
                            }
                            posscan = posscan + 3;
                            posreal = posreal + 3;
                        }
                        posscan = posscan + offset;
                    }
                    Marshal.Copy(bitmaprawbytes, 0, ptr, bitmaprawbytes.Length);
                    bitmaps.UnlockBits(bitmapData);
                    return bitmaps;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            public static BitMiracle.LibTiff.Classic.Tiff WriteFloatListToTiff(List<float> ds, String path, int w, int h)
            {
                BitMiracle.LibTiff.Classic.Tiff tiff = BitMiracle.LibTiff.Classic.Tiff.Open(path, "w");
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGEWIDTH, w);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGELENGTH, h);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.BITSPERSAMPLE, sizeof(float) * 8);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.DATATYPE, 3);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGEDESCRIPTION, "m3dm2tiff");
                for (int i = 0; i < h; i++)
                {
                    byte[] data = new byte[w * sizeof(float)];
                    Buffer.BlockCopy(ds.ToArray(), i * w * sizeof(float), data, 0, data.Length);
                    tiff.WriteScanline(data, i);
                }
                tiff.Close();
                return tiff;
            }
            public static BitMiracle.LibTiff.Classic.Tiff WriteUshortListToTiff(List<ushort> ds, String path, int w, int h)
            {
                BitMiracle.LibTiff.Classic.Tiff tiff = BitMiracle.LibTiff.Classic.Tiff.Open(path, "w");
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGEWIDTH, w);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGELENGTH, h);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.BITSPERSAMPLE, sizeof(ushort) * 8);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGEDEPTH, 1);
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.IMAGEDESCRIPTION, "A Ushort Format Tiff");
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.MAKE, "EdenChen_15721422037");
                tiff.SetField(BitMiracle.LibTiff.Classic.TiffTag.COMPRESSION, BitMiracle.LibTiff.Classic.Compression.PACKBITS);
                for (int i = 0; i < h; i++)
                {
                    byte[] data = new byte[w * sizeof(ushort)];
                    Buffer.BlockCopy(ds.ToArray(), i * w * sizeof(ushort), data, 0, data.Length);
                    tiff.WriteScanline(data, i);
                }
                tiff.Close();
                return tiff;
            }
            public static byte[] Float2RgbBytes(float[] vs, int w, int h, float maxf, float minf)
            {

                byte[] bitmapdatabytes = new byte[w * h * 3];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int i = w * y + x;
                        Color temp = SP_Color.TransZToRgbColor(vs[i], maxf, minf);
                        bitmapdatabytes[i * 3] = temp.B; bitmapdatabytes[i * 3 + 1] = temp.G; bitmapdatabytes[i * 3 + 2] = temp.R;
                    }
                }
                return bitmapdatabytes;
            }
            public static byte[] Float2GrayBytes(float[] vs, int w, int h, float maxf, float minf)
            {

                byte[] bitmapdatabytes = new byte[w * h * 3];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int i = w * y + x;
                        Color temp = SP_Color.TransZToGrayColor(vs[i], maxf, minf);
                        bitmapdatabytes[i * 3] = temp.R; bitmapdatabytes[i * 3 + 1] = temp.G; bitmapdatabytes[i * 3 + 2] = temp.B;
                    }
                }
                return bitmapdatabytes;
            }
            public static byte[] Float2WBBytes(float[] vs, int w, int h, float maxf, float minf)
            {

                byte[] bitmapdatabytes = new byte[w * h * 3];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int i = w * y + x;
                        Color temp = SP_Color.TransZToWBColor(vs[i], maxf, minf);
                        bitmapdatabytes[i * 3] = temp.R; bitmapdatabytes[i * 3 + 1] = temp.G; bitmapdatabytes[i * 3 + 2] = temp.B;
                    }
                }
                return bitmapdatabytes;
            }
        }
        public class SP_Translate
        {
            public static List<ushort> FloatListToUshortList(List<float> fs,out float ZIncrement,out float Zmin)
            {
                float[] s = new float[fs.Count];
                fs.CopyTo(s);
                List<float> fss = s.ToList();
                fss.RemoveAll((a) => float.IsNaN(a));
                List<ushort> ss = new List<ushort>(fs.Count);
                float maxf = fss.Max();
                float minf = fss.Min();
                float ZIncreasement = (maxf - minf) / 65536;
                for (int i = 0; i < fs.Count; i++)
                {
                    if (float.IsNaN(fs[i]))
                    {
                        ss.Add(ushort.MinValue);
                    }
                    else
                    {
                        ss.Add( (ushort)((fs[i] - minf) / (maxf - minf) * ushort.MaxValue));
                    }
                }
                ZIncrement = ZIncreasement;
                Zmin = minf;
                return ss;
            }
            public static List<List<int>> FindPointsCollection(List<float> fs,int filter)
            {
                List<List<int>> pc = new List<List<int>>();
                int i = 0;
                while (i<fs.Count-1)
                {
                    List<int> ps = new List<int>();
                    while (!float.IsNaN(fs[i])&&i<fs.Count-1)
                    {
                        ps.Add(i);
                        i++;
                    }
                    if (ps.Count>filter)
                    {
                        pc.Add(ps);
                    }
                    else
                    {
                        foreach (var item in ps)
                        {
                            fs[item] = float.NaN;
                        }
                    }
                    i++;
                }
                return pc;
            }//FPC
            public static void FillPointsCollection(List<float> fs, string fillmode, int filter=0)
            {
                if (filter==0)
                {
                    filter = fs.Count;
                }
                int i = 0;
                while (i < fs.Count - 1)
                {
                    List<int> ps = new List<int>();
                    while (float.IsNaN(fs[i]) && i < fs.Count - 1)
                    {
                        ps.Add(i);
                        i++;
                    }
                    if (ps.Count< filter&&ps.Count>0)
                    {
                        int s = ps.First() - 1; if (s < 0) { s = 0; }
                        int e = ps.Last() + 1; if (e <= 0) { e = 1; }
                        float sf = fs[s];
                        float ef = fs[e];
                        float inc = (ef - sf) / (e - s);
                        switch (fillmode)
                        {
                            case "l":
                                for (int n = 0; n < ps.Count; n++)
                                {
                                    fs[ps[n]] = sf;
                                }
                                break;
                            case "r":
                                for (int n = 0; n < ps.Count; n++)
                                {
                                    fs[ps[n]] = ef;
                                }
                                break;
                            case "m":
                                for (int n = 0; n < ps.Count; n++)
                                {
                                    fs[ps[n]] = sf+inc*n;
                                }
                                break;
                            case "b":
                                for (int n = 0; n < ps.Count; n++)
                                {
                                    fs[ps[n]] =Math.Min(sf,ef);
                                }
                                break;
                            case "t":
                                for (int n = 0; n < ps.Count; n++)
                                {
                                    fs[ps[n]] = Math.Max(sf,ef);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    i++;
                }
            }//FIPC
            public static void LevelPointsCollection(List<float> fs,int s,int e)
            {
                int l = e - s;
                float h =fs.GetRange(e-2,5).Average() - fs.GetRange(s-2,5).Average();
                float inc = h / l;
                for (int i = 0; i < fs.Count; i++)
                {
                    fs[i] = fs[i] - (i - s) * inc;
                }
            }//LPC
            public static List<float> FindPointsAtValue(List<float> fs,float value)
            {
                List<float> points = new List<float>();
                for (int i = 0; i < fs.Count-1; i++)
                {
                    float maxf = Math.Max(fs[i], fs[i + 1]);
                    float minf = Math.Min(fs[i], fs[i + 1]);
                    if (value<=maxf&&value>=minf)
                    {
                        points.Add(i + Math.Abs(value - fs[i]) * (1 / (maxf - minf)));
                    }
                    
                }
                return points;
            }//FPAV
            public static void FilterPeak(List<float> fs,float offset)
            {
                float[] ds = new float[fs.Count];
                fs.CopyTo(ds);
                for (int i = 0; i < ds.Length-2; i++)
                {
                    float l = ds[i];
                    float m = ds[i + 1];
                    float r = ds[i + 2];
                    if (m - l > offset && m - r > offset)
                    {
                        fs[i + 1] = (l + r) / 2;
                    }
                    else if (m - l < -offset && m-r<-offset)
                    {
                        fs[i + 1] = (l + r) / 2;
                    }
                    else if ((m-l)<=offset&&(m-l)>=-offset&&(r-m>offset|r-m<-offset))
                    {
                        fs[i + 2] = float.NaN;
                    }
                    else if((r-m<=offset&&r-m>=-offset)&&(l-m>offset|l-m<-offset))
                    {
                        fs[i] = float.NaN;
                    }
                }
            }//FRP
            public static void ListFloatProcess(List<float> fs,TreeNodeCollection nodes,out List<string> res_s)
            {
                res_s = new List<string>();
                for (int i = 0; i < nodes.Count; i++)
                {
                    switch (nodes[i].Name)
                    {
                        case "FPC":
                            List<List<int>> res=FindPointsCollection(fs, int.Parse(nodes[i].Text.Split(',')[1]));
                            string fpc = "FPC;";
                            for (int n = 0; n < res.Count; n++)
                            {
                                for (int x = 0; x < res[n].Count; x++)
                                {
                                    if (x==res[n].Count-1)
                                    {
                                        fpc += res[n][x] ;
                                    }
                                    else
                                    {
                                        fpc += res[n][x] + ",";
                                    }
                                }
                                fpc += ";";
                            }
                            res_s.Add(fpc);
                            break;
                        case "FIPC":
                            FillPointsCollection(fs, nodes[i].Text.Split(',')[1], int.Parse(nodes[i].Text.Split(',')[2]));
                            res_s.Add("FIPC;");
                            break;
                        case "LPC":
                            LevelPointsCollection(fs, int.Parse(nodes[i].Text.Split(',')[1]), int.Parse(nodes[i].Text.Split(',')[2]));
                            res_s.Add("LPC;");
                            break;
                        case "FPAV":
                            List<float> res_fpav= FindPointsAtValue(fs, float.Parse(nodes[i].Text.Split(',')[1]));
                            string fpav = "FPAV;";
                            for (int n = 0; n < res_fpav.Count; n++)
                            {
                                if (n==res_fpav.Count-1)
                                {
                                    fpav += res_fpav[n] + ";";
                                }
                                else
                                {
                                    fpav += res_fpav[n] + ",";
                                }
                            }
                            res_s.Add(fpav);
                            break;
                        case "FRP":
                            FilterPeak(fs, float.Parse(nodes[i].Text.Split(',')[1]));
                            res_s.Add("FRP;");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        public class SP_Translate2D
        {
            public static Point TransImageToPicbox(Point e, PictureBox pictureBox1, int offsetx=0,int offsety=0)
            {
                try
                {
                    Rectangle rectangle_curr;
                    //取得当前图像在图片框中的位置和长宽
                    rectangle_curr = (Rectangle)pictureBox1.GetType().GetProperty("ImageRectangle", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(pictureBox1, null);
                    int currentWidth = rectangle_curr.Width;
                    int currentHeight = rectangle_curr.Height;
                    //计算缩放系数
                    double rate_h = (double)currentHeight / ((double)pictureBox1.Image.Height);
                    double rate_w = (double)currentWidth / ((double)pictureBox1.Image.Width);
                    //MessageBox.Show("w比例:" + rate_w + "h比例:" + rate_h);
                    //计算XY偏移
                    int black_left_width = (currentWidth == pictureBox1.Width) ? 0 : (pictureBox1.Width - currentWidth) / 2;
                    int black_top_height = (currentHeight == pictureBox1.Height) ? 0 : (pictureBox1.Height - currentHeight) / 2;
                    float zoom_x = e.X - black_left_width;
                    float zoom_y = e.Y - black_top_height;
                    double original_x = (double)zoom_x / rate_w;
                    double original_y = (double)zoom_y / rate_h;
                    //如果鼠标落在图片区域外则返回原点坐标
                    if (e.X < black_left_width || e.X >= currentWidth + rectangle_curr.Location.X)
                    {
                        return new Point(0, 0);
                    }
                    if (e.Y < black_top_height || e.Y >= currentHeight + rectangle_curr.Location.Y)
                    {
                        return new Point(0, 0);
                    }
                    //返回实际图像坐标
                    return new Point((int)original_x+offsetx, (int)original_y+offsety);
                }
                catch (Exception)
                {
                    return new Point(0, 0);
                }
            }

        }
        public class SP_Translate3D
        {
            public static void TranslatePointsCloud(float x,float y,float z)
            {

            }
            public static void SetCloudsOrigin(SmartPointsCloud cloud, float offx,float offy,float offz)
            {
                GlmNet.mat4 trans = new mat4(1.0f);
                trans = glm.translate(trans, new vec3(-offx, -offy, -offz));
                for (int i = 0; i < cloud.Spcpoints.pointsx.Count; i++)
                {
                    vec4 vec = new vec4(cloud.Spcpoints.pointsx[i], cloud.Spcpoints.pointsy[i], cloud.Spcpoints.pointsz[i],1.0f);
                    vec = trans * vec;
                    cloud.Spcpoints.pointsx[i] = vec.x;
                    cloud.Spcpoints.pointsy[i] = vec.y;
                    cloud.Spcpoints.pointsz[i] = vec.z;
                }
                cloud.Zmax -= offz;
                cloud.Zmin -= offz;
            }
            /// <summary>
            /// 获得所选三点连线在XZ,YZ投影上的斜率
            /// </summary>
            /// <param name="vs">z数组</param>
            /// <param name="vsx">x数组</param>
            /// <param name="vsy">y数组</param>
            /// <param name="box">核距，奇数</param>
            /// <param name="Point_List">点位数组</param>
            /// <returns></returns>
            public static double[] GetLineKInZPlane( SmartPointsCloud pointsCloud ,int box, List<Point> Point_List)
            {
                int w = pointsCloud.Width;
                float[] vsx = pointsCloud.Spcpoints.pointsx.ToArray();
                float[] vsy = pointsCloud.Spcpoints.pointsy.ToArray();
                float[] vsz = pointsCloud.Spcpoints.pointsz.ToArray();
                double[] res = new double[2];
                List<double> kx = new List<double>();
                List<double> ky = new List<double>();
                for (int y = -box; y < box + 1; y++)
                {
                    for (int x = -box; x < box + 1; x++)
                    {
                        for (int i = 0; i < Point_List.Count; i++)
                        {
                            Point_List[i] = new Point(Point_List[i].X + x, Point_List[i].Y + y);
                        }
                        double[] kxy = new double[2]; double[] kz = new double[2];
                        kxy[0] = vsx[Point_List[0].X + Point_List[0].Y * w];
                        kxy[1] = vsx[Point_List[1].X + Point_List[1].Y * w];
                        kz[0] = vsz[Point_List[0].X + Point_List[0].Y * w];
                        kz[1] = vsz[Point_List[1].X + Point_List[1].Y * w];
                        double kkx = Fit.Line(kxy, kz).Item2; kx.Add(kkx);
                        kxy[0] = vsy[Point_List[1].X + Point_List[1].Y * w];
                        kxy[1] = vsy[Point_List[2].X + Point_List[2].Y * w];
                        kz[0] = vsz[Point_List[1].X + Point_List[1].Y * w];
                        kz[1] = vsz[Point_List[2].X + Point_List[2].Y * w];
                        double kky = Fit.Line(kxy, kz).Item2; ky.Add(kky);
                    }
                }
                res[0] = kx.Average(); res[1] = ky.Average();
                return res;
            }
            /// <summary>
            /// 利用3点 校正点云
            /// </summary>
            /// <param name="vs">z数组</param>
            /// <param name="vsx">x数组</param>
            /// <param name="vsy">y数组</param>
            /// <param name="Point_List">点位数组</param>
            /// <returns>返回一个新的z数组</returns>
            public static SmartPointsCloud  MatLeveling_3points(SmartPointsCloud pointsCloud, List<Point> Point_List)
            {
                SmartPointsCloud dst = pointsCloud;
                int w = pointsCloud.Width;int h = pointsCloud.Height;
                float[] vsx = pointsCloud.Spcpoints.pointsx.ToArray();
                float[] vsy = pointsCloud.Spcpoints.pointsy.ToArray();
                float[] vsz = pointsCloud.Spcpoints.pointsz.ToArray();
                double[] d = GetLineKInZPlane(pointsCloud, 1, Point_List);
                double kkx = d[0]; double kky = d[1];
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        if (float.IsNaN(vsz[x + y * w]))
                        {
                            dst.Spcpoints.pointsz[x + y * w] = vsz[x + y * w];
                        }
                        else
                        {
                            dst.Spcpoints.pointsz[x + y * w] = (float)(vsz[x + y * w] - kkx * (float)(vsx[x + y * w] - vsx[Point_List[1].X + Point_List[1].Y * w]) - kky * (float)(vsy[x + y * w] - vsy[Point_List[1].X + Point_List[1].Y * w]));
                        }
                    }
                }
                return dst;
            }
            public static SmartPointsCloud FilterZRangeID(SmartPointsCloud pointsCloud, bool reverse = false)
            {
                SmartPointsCloud dst = pointsCloud;
                string inputstr = Interaction.InputBox("当前区间：" + pointsCloud.Zmax.ToString() + "," + pointsCloud.Zmin, "输入Zrange", pointsCloud.Zmax + "," + pointsCloud.Zmin);
                string[] inputss = inputstr.Split(new char[] { ',' });
                float f = float.Parse(inputss[0]); float b = float.Parse(inputss[1]);
                float maxf = Math.Max(f, b); float minf = Math.Min(f, b);
                if (!reverse)
                {
                    for (int i = 0; i < pointsCloud.Spcpoints.pointsz.Count; i++)
                    {
                        if (!(pointsCloud.Spcpoints.pointsz[i] >= minf && pointsCloud.Spcpoints.pointsz[i] <= maxf))
                        {
                            dst.Spcpoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < pointsCloud.Spcpoints.pointsz.Count; i++)
                    {
                        if ((pointsCloud.Spcpoints.pointsz[i] >= minf && pointsCloud.Spcpoints.pointsz[i] <= maxf))
                        {
                            dst.Spcpoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                dst.Zmax = maxf;
                dst.Zmin = minf;
                return dst;
            }

        }
        public class SPCV
        {
            public static List<PointF> CvGetContoursCenters(Bitmap bitmap,int filter=15)
            {
                Image<Rgb, byte> image = new Image<Rgb, byte>(bitmap);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(bitmap);
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();   
                CvInvoke.FindContours(mat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                
                List<MCvMoments> mu = new List<MCvMoments>();
                List<PointF> mc = new List<PointF>();
                for (int i = 0; i < contours.Size; i++)
                {
                    mu.Add(CvInvoke.Moments(contours[i], false));
                    double l = CvInvoke.ArcLength(contours[i], true);
                    if (l>filter)
                    {
                        mc.Add(new PointF((float)(mu[i].M10 / mu[i].M00), (float)(mu[i].M01 / mu[i].M00)));
                    }
                }
                mc.RemoveAll((a) => float.IsNaN(a.X));
                return mc;
            }
            public static CircleF[] CVFindCycle(Bitmap bitmap)
            {
                
                Image<Rgb, byte> image = new Image<Rgb, byte>(bitmap);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(bitmap);
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                CvInvoke.Canny(mat, mat, 10, 10);
                CvInvoke.GaussianBlur(mat, mat, new Size(5,5), 0);
                CvInvoke.Imshow("Canny", mat);
                CvInvoke.WaitKey(0);
                CircleF[] circles=  CvInvoke.HoughCircles(mat, HoughType.Gradient, 1,10);
                return circles;
            }
            public static List<Point[]> CvGetContoursInPoints(Bitmap bitmap,int filter=15)
            {
                Image<Rgb, byte> image = new Image<Rgb, byte>(bitmap);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(bitmap);
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                CvInvoke.FindContours(mat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                Point[][] contourspoints = contours.ToArrayOfArray();
                List<Point[]> pss = contourspoints.ToList();
                List<Point[]> points = new List<Point[]>();
                for (int i = 0; i < contours.Size; i++)
                {
                    if (pss[i].Length>=filter)
                    {
                        Point[] ps = CvGetPointsInContour(contours[i]).ToArray();
                        points.Add(ps);
                    }
                }
                return points;
            }
            public static List<Point[]> CvGetContours(Bitmap bitmap,int filter=15)
            {
                Image<Rgb, byte> image = new Image<Rgb, byte>(bitmap);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(bitmap);
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                CvInvoke.FindContours(mat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                OutputArray outputArray =  contours.GetOutputArray();
                Point[][] contourspoints = contours.ToArrayOfArray();
                List<Point[]> points= contourspoints.ToList();
                points.RemoveAll((a) => a.Length < filter);
                return points;
            }
            public static List<Point> CvGetPointsInContour(Emgu.CV.Util.VectorOfPoint contour)
            {
                List<Point> PointsInContour = new List<Point>();
                Point[] contourpoints = contour.ToArray();
                List<int> xlist = new List<int>();
                List<int> ylist = new List<int>();
                foreach (var item in contourpoints)
                {
                    xlist.Add(item.X);
                    ylist.Add(item.Y);
                }
                Point tl = new Point(xlist.Min(), ylist.Min());
                Point rb = new Point(xlist.Max(), ylist.Max());
                int w = rb.X - tl.X;
                int h = rb.Y - tl.Y;
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        double d = CvInvoke.PointPolygonTest(contour, new PointF(tl.X + x, tl.Y + y), false);
                        if (d==1)
                        {
                            PointsInContour.Add(new Point(tl.X + x, tl.Y + y));
                        }
                    }
                }
                return PointsInContour;
            }
            public static void CvGetFloodFill(Bitmap bitmap)
            {
                Image<Rgb, byte> image = new Image<Rgb, byte>(bitmap);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(bitmap);
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                
            }
            public static List<List<float>> CvGetHistogram(SmartPointsCloud pointsCloud,float Spliter)
            {
                List<List<float>> historgram = new List<List<float>>();
                float[] vsz = new float[pointsCloud.Spcpoints.pointsz.Count];
                pointsCloud.Spcpoints.pointsz.CopyTo(vsz);
                List<float> vs = vsz.ToList();
                vs.RemoveAll((a) => float.IsNaN(a));
                vs.Sort();
                float range = Math.Abs(vs[0] - vs[vs.Count - 1]);
                int levels = (int)(range / Spliter)+1;
                int s = 0;
                for (int i = 0; i < levels; i++)
                {
                    int e = vs.FindIndex((b) => b > vs[i] + Spliter*i);
                    if (e<0)
                    {
                        break;
                    }
                    historgram.Add(vs.GetRange(s,  e- s));
                    s = e;
                }
                historgram.RemoveAll((a) => a.Count == 0);
                return historgram;
            }
            public static Bitmap CvGetContourAreaAtPoint(Bitmap rsc,Point point)
            {
                Bitmap dst = new Bitmap(rsc.Width,rsc.Height);
                Image<Rgb, byte> image = new Image<Rgb, byte>(rsc);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(rsc);
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                CvInvoke.FindContours(mat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                for (int i = 0; i < contours.Size; i++)
                {
                    double d = CvInvoke.PointPolygonTest(contours[i],point, false);
                    if (d == 1)
                    {
                        for (int n = 0; n <contours[i].Size; n++)
                        {
                            dst.SetPixel(contours[i][n].X, contours[i][n].Y, Color.OrangeRed);
                        }
                    }
                }
                return dst;
            }
            public static VectorOfPoint CvGetContourAreaAtPoint_C(Bitmap rsc, Point point)
            {
                Bitmap dst = new Bitmap(rsc.Width, rsc.Height);
                Image<Rgb, byte> image = new Image<Rgb, byte>(rsc);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(rsc);
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                CvInvoke.FindContours(mat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                for (int i = 0; i < contours.Size; i++)
                {
                    double d = CvInvoke.PointPolygonTest(contours[i], point, false);
                    if (d == 1)
                    {
                        return contours[i];
                    }
                }
                return null;
            }
            public static Bitmap CvGetContourAreaAll(Bitmap rsc)
            {
                Bitmap dst = new Bitmap(rsc.Width, rsc.Height);
                Image<Rgb, byte> image = new Image<Rgb, byte>(rsc);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(rsc);
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                CvInvoke.FindContours(mat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                for (int i = 0; i < contours.Size; i++)
                {
                    if (contours[i].Size > 15)
                    {
                        for (int n = 0; n < contours[i].Size; n++)
                        {
                            dst.SetPixel(contours[i][n].X, contours[i][n].Y, Color.OrangeRed);
                        }
                    }
                }
                return dst;
            }
            public static SmartPointsCloud CvGetNewSPCFromContour(SmartPointsCloud pointsCloud,VectorOfPoint c)
            {
                    List<Point> PointsInContour = new List<Point>();
                    Point[] contourpoints = c.ToArray();
                    List<int> xlist = new List<int>();
                    List<int> ylist = new List<int>();
                    foreach (var item in contourpoints)
                    {
                        xlist.Add(item.X);
                        ylist.Add(item.Y);
                    }
                    Point tl = new Point(xlist.Min(), ylist.Min());
                    Point rb = new Point(xlist.Max(), ylist.Max());
                    int w = rb.X - tl.X;
                    int h = rb.Y - tl.Y;
                SmartPointsCloud dst = new SmartPointsCloud(pointsCloud.SpcName + "NewSpc", pointsCloud.SpcPath, "SmartPoints", w, h, pointsCloud.Zmax, pointsCloud.Zmin, tl, rb);
                unsafe
                {
                    for (int y = 0; y < h; y++)
                    {
                        for (int x = 0; x < w; x++)
                        {
                            Point apoint = new Point(tl.X + x, tl.Y + y);
                            
                            double d = CvInvoke.PointPolygonTest(new VectorOfPoint(contourpoints), apoint, false);
                            if (d > 0)
                            {
                                PointsInContour.Add(apoint);
                                float[] value = pointsCloud.GetValue(apoint.X, apoint.Y);
                                dst.Spcpoints.pointsx.Add(value[0]);
                                dst.Spcpoints.pointsy.Add(value[1]);
                                dst.Spcpoints.pointsz.Add(value[2]);
                            }
                            else
                            {
                                dst.Spcpoints.pointsx.Add(x);
                                dst.Spcpoints.pointsy.Add(y);
                                dst.Spcpoints.pointsz.Add(float.NaN);
                            }
                        }
                    }
                    return dst;

                }
            }
            public static SmartPointsCloud CvErode(SmartPointsCloud pointsCloud)
            {
                Bitmap bitmap = pointsCloud.GetBitmapColor();
                Image<Rgb, byte> image = new Image<Rgb, byte>(bitmap);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(bitmap.Width, bitmap.Height);
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                CvInvoke.Erode(image, mat,CvInvoke.GetStructuringElement(ElementShape.Cross,new Size(3,3),new Point(-1,-1)), new Point(-1, -1), 3, BorderType.Default, new MCvScalar(0, 0, 0)) ;
                bitmap = mat.ToBitmap();
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        int i = y * bitmap.Width + x;
                        Color color= bitmap.GetPixel(x, y);
                        if (color.Name=="ff000000")
                        {
                            pointsCloud.Spcpoints.pointsz[i] = float.NaN;
                        }
                    }
                }
                return pointsCloud;
            }
            public static Bitmap CvFindHoughCircle(Bitmap rsc)
            {
                Image<Rgb, byte> image = new Image<Rgb, byte>(rsc);
                Image<Rgb, byte> mat = new Image<Rgb, byte>(rsc.Width,rsc.Height);
                Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
                CvInvoke.CvtColor(image, mat, ColorConversion.Rgb2Gray);
                CvInvoke.FindContours(mat, contours, null, RetrType.List, ChainApproxMethod.ChainApproxNone);
                List<Point> centers = new List<Point>();
                List<Point> center_res = new List<Point>();
                List<CircleF> circles = new List<CircleF>();
                List<CircleF> circles_res = new List<CircleF>();
                for (int i = 0; i < contours.Size; i++)
                {
                    if (contours[i].Size > 155)
                    {
                        CircleF circle = CvInvoke.MinEnclosingCircle(contours[i]);
                        Point center = new Point((int)Math.Round(circle.Center.X), (int)Math.Round(circle.Center.Y));
                        centers.Add(center);
                        circles.Add(circle);
                    }
                }
                for (int c = 0; c < centers.Count; c++)
                {
                    for (int i = 0; i < centers.Count; i++)
                    {
                        if (((int)Math.Round(Math.Sqrt((centers[c].X - centers[i].X) * (centers[c].X - centers[i].X) + (centers[c].Y - centers[i].Y) * (centers[c].Y - centers[i].Y)))<125)&&i!=c)
                        {
                             circles_res.Add(circles[c]);
                             center_res.Add(centers[c]);
                        } 
                    }
                }
                for (int i = 0; i < circles_res.Count; i++)
                {
                    CvInvoke.Circle(image, center_res[i], (int)Math.Round(circles_res[i].Radius), new MCvScalar(35, 111, 211),3);
                }
                return image.Bitmap;
            }

        }
    }
}
