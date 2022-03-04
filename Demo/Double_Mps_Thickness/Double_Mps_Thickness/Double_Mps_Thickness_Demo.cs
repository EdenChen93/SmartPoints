using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using SPCwindowUI;

namespace Double_Mps_Thickness
{
    public partial class Double_Mps_Thickness_Demo : Form
    {
        SPCwindow spcw1 = new SPCwindow();
        SPCwindow spcw2 = new SPCwindow();
        SPCwindow spcw12 = new SPCwindow();
        MpsSensor sensor1;
        MpsSensor sensor2;
        Rectangle Roi_Measure_Area;
        float[] calb_thickness1; float[] calb_thickness2;float[] calb_parameter;
        public Double_Mps_Thickness_Demo()
        {
            InitializeComponent();
            
        }

        private void Double_Mps_Thickness_Demo_Load(object sender, EventArgs e)
        {
            AddSpcWindowToControls();
        }
        private void AddSpcWindowToControls()
        {
            spcw1.Location = Point.Empty;
            spcw1.Size = new Size(400, 400);
            spcw2.Location = new Point(400, 0);
            spcw2.Size = new Size(400, 400);
            spcw12.Location = new Point(800, 0);
            spcw12.Size = new Size(400, 400);
            spcw1.Inilize(); spcw2.Inilize(); spcw12.Inilize();
            spcw1.GetInfoEvent += Spcw1_GetInfoEvent;
            spcw2.GetInfoEvent += Spcw2_GetInfoEvent;
            spcw12.GetInfoEvent += Spcw12_GetInfoEvent;
            this.Controls.Add(spcw1);
            this.Controls.Add(spcw2);
            this.Controls.Add(spcw12);
            this.Update();
        }

        private void Spcw1_GetInfoEvent(string datainfo)
        {
            label1.Text = datainfo;
        }
        private void Spcw2_GetInfoEvent(string datainfo)
        {
            label2.Text = datainfo;
        }
        private void Spcw12_GetInfoEvent(string datainfo)
        {
            label3.Text = datainfo;
        }

        private void ConDev1Button_Click(object sender, EventArgs e)
        {
             sensor1 = new MpsSensor();
             sensor1.DataProcessEvent += Sensor1_DataProcessEvent;
        }
        private void ConDev2Button_Click(object sender, EventArgs e)
        {
            sensor2 = new MpsSensor();
            sensor2.DataProcessEvent += Sensor2_DataProcessEvent;
        }

        private void Sensor1_DataProcessEvent(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            if (checkBox1.Checked)
            {
                cloud.FilterZRange(10f, -10f);
                SPCV_Filp(cloud);
                //calb_parameter = CalibAffChangeProcess(cloud,spcw2.pointsCloud);
                spcw1.pointsCloud = AfflineChange(cloud, calb_parameter[0], calb_parameter[1], calb_parameter[2], calb_parameter[3], calb_parameter[4], new PointF(calb_parameter[5], calb_parameter[6]));
                spcw1.Inilize();
            }
            else
            {

                cloud.FilterZRange(10f, -10f);
                spcw1.pointsCloud = cloud;
                spcw1.Inilize();
            }
        }
        private void Sensor2_DataProcessEvent(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            cloud.FilterZRange(20f, -2f);
            spcw2.pointsCloud = cloud;
            spcw2.Inilize();
        }



        private void TriDev1Button_Click(object sender, EventArgs e)
        {
            sensor1.mps.FireSoftwareTrigger();
        }
        private void TriDev2Button_Click(object sender, EventArgs e)
        {
            sensor2.mps.FireSoftwareTrigger();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calb_parameter = CalibAffChange();
        }
        public static void ImgTranslate(Image<Gray, float> srcImg, Image<Gray, float> dstImg, int xOffset, int yOffset)
        {
            for (int i = 0; i < srcImg.Rows; i++)
            {
                for (int j = 0; j < srcImg.Cols; j++)
                {
                    int x = j + xOffset;
                    int y = i + yOffset;
                    if (x >= 0 && x < dstImg.Cols && y >= 0 && y < dstImg.Rows)
                    {
                        if (!float.IsNaN(srcImg.Data[i, j, 0]))
                        {
                            dstImg.Data[y, x, 0] = srcImg.Data[i, j, 0];

                        }
                        else
                        {
                            dstImg.Data[y, x, 0] = float.NaN;
                        }
                    }
                }
            }
        }
        private float[] CalibAffChange()
        {
            Image<Rgb, byte> image1 = new Image<Rgb, byte>((Bitmap)spcw1.SPCWPictureBox.Image);
            Image<Rgb, byte> image2 = new Image<Rgb, byte>(spcw2.SPCWPictureBox.Image.Width, spcw2.SPCWPictureBox.Image.Height);
            Image<Gray, Single> mat1 = new Image<Gray, Single>(spcw1.SPCWPictureBox.Image.Width, spcw1.SPCWPictureBox.Image.Height);
            Image<Gray, Single> mat2 = new Image<Gray, Single>(spcw1.SPCWPictureBox.Image.Width, spcw1.SPCWPictureBox.Image.Height);
            for (int y = 0; y < mat2.Height; y++)
            {
                for (int x = 0; x < mat2.Width; x++)
                {
                    mat2.Data[y, x, 0] = float.NaN;
                }
            }
            //spcw1.pointsCloud.LineClipingFloatA(new Point[] { new Point(0, i), new Point(spcw1.pointsCloud.Width,i ) }, out temp);
            Buffer.BlockCopy(spcw1.pointsCloud.Spcpoints.pointsz.ToArray(), 0, mat1.Data, 0, sizeof(float) * spcw1.SPCWPictureBox.Image.Width * spcw1.SPCWPictureBox.Image.Height);
            Emgu.CV.Util.VectorOfPoint points = SmartPoints.SmartPoints.SPCV.CvGetContourAreaAtPoint_C(image1.ToBitmap(),new Point(972,736));
            RotatedRect rotatedRect1 = CvInvoke.MinAreaRect(points);
            image1.Draw(points.ToArray(), new Rgb(255, 0, 0));

            points = SmartPoints.SmartPoints.SPCV.CvGetContourAreaAtPoint_C((Bitmap)spcw2.SPCWPictureBox.Image, new Point(972, 736));
            RotatedRect rotatedRect2 = CvInvoke.MinAreaRect(points);
            image2.Draw(points.ToArray(), new Rgb(255, 0, 0));

            label1.Text = "Center:" + rotatedRect1.Center + "\r\n" + "Angle:" + rotatedRect1.Angle + "\r\n" + "Size:" + rotatedRect1.Size;
            label2.Text = "Center:" + rotatedRect2.Center + "\r\n" + "Angle:" + rotatedRect2.Angle + "\r\n" + "Size:" + rotatedRect2.Size;
            float offset_angle = - rotatedRect1.Angle + rotatedRect2.Angle;
            float offset_x = rotatedRect2.Center.X - rotatedRect1.Center.X;
            float offset_y = rotatedRect2.Center.Y - rotatedRect1.Center.Y;

            float w1 = Math.Max(rotatedRect1.Size.Width, rotatedRect1.Size.Height);
            float h1 = Math.Min(rotatedRect1.Size.Width, rotatedRect1.Size.Height);
            float w2 = Math.Max(rotatedRect2.Size.Width, rotatedRect2.Size.Height);
            float h2 = Math.Min(rotatedRect2.Size.Width, rotatedRect2.Size.Height);


            float offset_scalx = w2 / w1;
            float offset_scaly = h2 / h1;
            image1 = image1.Rotate(offset_angle, rotatedRect1.Center, Emgu.CV.CvEnum.Inter.Area, new Rgb(0, 0, 0), true);
            mat1 = mat1.Rotate(offset_angle, rotatedRect1.Center, Emgu.CV.CvEnum.Inter.Area, new Gray(0), true);
            ImgTranslate(mat1, mat2, (int)offset_x, (int)offset_y);

            spcw1.SPCWPictureBox.Image = image1.ToBitmap();
            spcw2.SPCWPictureBox.Image = image2.ToBitmap();

            label3.Text = "offset_angle:" + offset_angle + "\r\n" + "offset_y:" + offset_x + "\r\n" + "offest_x:" + offset_y + "\r\n" + "scalx:" + offset_scalx + "\r\n" + "scaly:" + offset_scaly;



            Roi_Measure_Area = rotatedRect2.MinAreaRect();
            calb_thickness1 = new float[Roi_Measure_Area.Width * Roi_Measure_Area.Height];
            calb_thickness2 = new float[Roi_Measure_Area.Width * Roi_Measure_Area.Height];

            for (int y = 0; y < Roi_Measure_Area.Height; y++)
            {
                for (int x = 0; x < Roi_Measure_Area.Width; x++)
                {
                    int rx = Roi_Measure_Area.Location.X + x;
                    int ry = Roi_Measure_Area.Location.Y + y;
                    int i = rx + ry * spcw2.pointsCloud.Width;
                    int rx1 = (int)(rx * offset_scalx);
                    int ry1 = (int)(ry * offset_scaly);
                    calb_thickness1[x + y * Roi_Measure_Area.Width] = spcw2.pointsCloud.Spcpoints.pointsz[i];
                    calb_thickness2[x + y * Roi_Measure_Area.Width] = mat2.Data[ry1, rx1, 0];
                }
            }
            spcw12.pointsCloud = new SmartPoints.SmartPoints.SmartPointsCloud("Mat2", "", "SmartPoints", spcw1.pointsCloud.Width, spcw1.pointsCloud.Height, -5, 5, Point.Empty, new Point(spcw1.pointsCloud.Width, spcw1.pointsCloud.Height));
            for (int y = 0; y < spcw1.pointsCloud.Height; y++)
            {
                for (int x = 0; x < spcw1.pointsCloud.Width; x++)
                {
                    spcw12.pointsCloud.Spcpoints.pointsz.Add(mat2.Data[y, x, 0]);
                    spcw12.pointsCloud.Spcpoints.pointsx.Add(x);
                    spcw12.pointsCloud.Spcpoints.pointsy.Add(y);
                }
            }
            spcw12.Inilize();
            return new float[] { offset_x, offset_y, offset_scalx, offset_scaly, offset_angle , rotatedRect1.Center.X,rotatedRect1.Center.Y};
        }
        private float[] CalibAffChangeProcess(SmartPoints.SmartPoints.SmartPointsCloud cloud1,SmartPoints.SmartPoints.SmartPointsCloud cloud2)
        {
            Image<Gray, Single> mat1 = new Image<Gray, Single>(cloud1.Width, cloud1.Height);
            Image<Gray, Single> mat2 = new Image<Gray, Single>(cloud1.Width, cloud1.Height);
            for (int y = 0; y < mat2.Height; y++)
            {
                for (int x = 0; x < mat2.Width; x++)
                {
                    mat2.Data[y, x, 0] = float.NaN;
                }
            }
            //spcw1.pointsCloud.LineClipingFloatA(new Point[] { new Point(0, i), new Point(spcw1.pointsCloud.Width,i ) }, out temp);
            Buffer.BlockCopy(cloud1.Spcpoints.pointsz.ToArray(), 0, mat1.Data, 0, sizeof(float) * cloud1.Width * cloud1.Height);
            Emgu.CV.Util.VectorOfPoint points = SmartPoints.SmartPoints.SPCV.CvGetContourAreaAtPoint_C((Bitmap)spcw1.SPCWPictureBox.Image, new Point(972, 736));
            RotatedRect rotatedRect1 = CvInvoke.MinAreaRect(points);

            points = SmartPoints.SmartPoints.SPCV.CvGetMaxContour(cloud2.GetBitmapGray());
            RotatedRect rotatedRect2 = CvInvoke.MinAreaRect(points);

            float offset_angle =-90- rotatedRect1.Angle + rotatedRect2.Angle;
            float offset_x = rotatedRect2.Center.X - rotatedRect1.Center.X;
            float offset_y = rotatedRect2.Center.Y - rotatedRect1.Center.Y;

            float w1 = Math.Max(rotatedRect1.Size.Width, rotatedRect1.Size.Height);
            float h1 = Math.Min(rotatedRect1.Size.Width, rotatedRect1.Size.Height);
            float w2 = Math.Max(rotatedRect2.Size.Width, rotatedRect2.Size.Height);
            float h2 = Math.Min(rotatedRect2.Size.Width, rotatedRect2.Size.Height);


            float offset_scalx = w2 / w1;
            float offset_scaly = h2 / h1;
            return new float[] { offset_x, offset_y, offset_scalx, offset_scaly, offset_angle, rotatedRect1.Center.X, rotatedRect1.Center.Y };
        }

        private SmartPoints.SmartPoints.SmartPointsCloud AfflineChange(SmartPoints.SmartPoints.SmartPointsCloud cloud,float offsetx,float offsety,float scalx,float scaly,float angle,PointF center)
        {
            SmartPoints.SmartPoints.SmartPointsCloud pointsCloud = new SmartPoints.SmartPoints.SmartPointsCloud("ChangeSpc1", "", "SmartPoints", cloud.Width, cloud.Height, -5, 5, Point.Empty, new Point(cloud.Width, cloud.Height));

            for (int y = 0; y < cloud.Height; y++)
            {
                for (int x = 0; x < cloud.Width; x++)
                {
                    pointsCloud.Spcpoints.pointsz.Add(float.NaN);
                    pointsCloud.Spcpoints.pointsx.Add(cloud.Spcpoints.pointsx[x + y * cloud.Width]);
                    pointsCloud.Spcpoints.pointsy.Add(cloud.Spcpoints.pointsy[x + y * cloud.Width]);
                }
            }
            Image<Gray, Single> mat1 = new Image<Gray, Single>(cloud.Width, cloud.Height);
            Image<Gray, Single> mat2 = new Image<Gray, Single>(cloud.Width, cloud.Height);
            for (int y = 0; y < mat2.Height; y++)
            {
                for (int x = 0; x < mat2.Width; x++)
                {
                    mat2.Data[y, x, 0] = float.NaN;
                }
            }
            Buffer.BlockCopy(cloud.Spcpoints.pointsz.ToArray(), 0, mat1.Data, 0, sizeof(float) *cloud.Width *cloud.Height);
            mat1 = mat1.Rotate(angle, center, Emgu.CV.CvEnum.Inter.Area,new Gray(0), true);
            ImgTranslate(mat1, mat2, (int)offsetx, (int)offsety);
            for (int y = 0; y < Roi_Measure_Area.Height; y++)
            {
                for (int x = 0; x < Roi_Measure_Area.Width; x++)
                {
                    int rx = Roi_Measure_Area.Location.X + x;
                    int ry = Roi_Measure_Area.Location.Y + y;
                    int i = rx + ry * cloud.Width;
                    int rx1 = (int)(rx * scalx);
                    int ry1 = (int)(ry * scaly);
                    pointsCloud.Spcpoints.pointsz[i] = mat2.Data[ry1, rx1, 0];
                    pointsCloud.Spcpoints.pointsx[i] = rx1;
                    pointsCloud.Spcpoints.pointsy[i] = ry1;
                }
            }
            return pointsCloud;
        }
        private void SPCV_Filp(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            Image<Gray, Single> Imgmat = new Image<Gray, Single>(cloud.Width, cloud.Height);
            Buffer.BlockCopy(cloud.Spcpoints.pointsz.ToArray(), 0, Imgmat.Data, 0, sizeof(float) * cloud.Width * cloud.Height);
            Imgmat= Imgmat.Flip(Emgu.CV.CvEnum.FlipType.Vertical);
            for (int y = 0; y < cloud.Height; y++)
            {
                for (int x = 0; x < cloud.Width; x++)
                {
                    cloud.Spcpoints.pointsz[x+y*cloud.Width] = Imgmat.Data[y,x,0];
                }
            }
        }

        private void ConDev3Button_Click(object sender, EventArgs e)
        {
            SmartPoints.SmartPoints.SmartPointsCloud cloud1 = spcw1.pointsCloud.RectangleCliping(Roi_Measure_Area);
            SmartPoints.SmartPoints.SmartPointsCloud cloud2 = spcw2.pointsCloud.RectangleCliping(Roi_Measure_Area);
            SmartPoints.SmartPoints.SmartPointsCloud pointsCloud = new SmartPoints.SmartPoints.SmartPointsCloud("Compose", "", "SmartPoints", cloud2.Width, cloud2.Height, -2, 15, Point.Empty, new Point(cloud2.Width, cloud2.Height));
            for (int y = 0; y < cloud2.Height; y++)
            {
                for (int x = 0; x < cloud2.Width; x++)
                {
                    int i = x + y * cloud2.Width;
                    float change1 = cloud1.Spcpoints.pointsz[i] - calb_thickness1[i];
                    float change2 = cloud2.Spcpoints.pointsz[i] - calb_thickness2[i];

                    pointsCloud.Spcpoints.pointsz.Add(1.0f + change1 + change2);
                    pointsCloud.Spcpoints.pointsx.Add(cloud2.Spcpoints.pointsx[i]);
                    pointsCloud.Spcpoints.pointsy.Add(cloud2.Spcpoints.pointsy[i]);
                }
            }
            spcw12.pointsCloud = pointsCloud;
            spcw12.Inilize();
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void FindMaxContourSB1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SPCV_Filp(spcw1.pointsCloud);
            spcw1.Inilize();
        }
    }
}
