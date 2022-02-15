using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics;
namespace SmartPoints
{
    class Temp
    {
        public struct Point
        {
            public int X;
            public int Y;
            public Point(int x,int y)
            {
                X = x;
                Y = y;
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
            public SmartPointsCloud(string spcnme, string spcpath, string spcsource, int width, int height, float zmax, float zmin)
            {
                this.SpcName = spcnme;
                this.SpcPath = spcpath;
                this.SpcSource = spcsource;
                this.Width = width;
                this.Height = height;
                this.Zmax = zmax;
                this.Zmin = zmin;
                this.Spcpoints = new SpcPoints(width, height);
            }

            public string SpcName { get; set; }
            public string SpcPath { get; set; }
            public string SpcSource { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            public float Zmax { get; set; }
            public float Zmin { get; set; }
            public SpcPoints Spcpoints { get; set; }
            /// <summary>
            /// 高度滤波
            /// </summary>
            /// <param name="maxf"></param>
            /// <param name="minf"></param>
            /// <param name="reverse"></param>
            /// <returns></returns>
            public bool FilterZRange(float maxf, float minf, bool reverse = false)
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
            /// <summary>
            /// 获得所选三点连线在XZ,YZ投影上的斜率
            /// </summary>
            /// <param name="vs">z数组</param>
            /// <param name="vsx">x数组</param>
            /// <param name="vsy">y数组</param>
            /// <param name="box">核距，奇数</param>
            /// <param name="Point_List">点位数组</param>
            /// <returns></returns>
            public double[] GetLineKInZPlane(int box, List<Point> Point_List)
            {
                int w = this.Width;
                float[] vsx = this.Spcpoints.pointsx.ToArray();
                float[] vsy = this.Spcpoints.pointsy.ToArray();
                float[] vsz = this.Spcpoints.pointsz.ToArray();
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
            public void MatLeveling_3points(List<Point> Point_List)
            {
                int w = this.Width; int h = this.Height;
                float[] vsx = this.Spcpoints.pointsx.ToArray();
                float[] vsy = this.Spcpoints.pointsy.ToArray();
                float[] vsz = this.Spcpoints.pointsz.ToArray();
                double[] d = GetLineKInZPlane( 1, Point_List);
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

        }
    }
}
