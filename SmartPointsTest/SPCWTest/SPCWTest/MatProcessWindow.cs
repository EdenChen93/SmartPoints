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
    public partial class MatProcessWindow : Form
    {
        public SmartPoints.SmartPoints.ProcessCmd cmd = new SmartPoints.SmartPoints.ProcessCmd();
        public MatProcessWindow()
        {
            InitializeComponent();
        }

        private void MatProcessWindow_Load(object sender, EventArgs e)
        {

        }
        public void MLP_UIUpdatei(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            switch (cmd)
            {
                case SmartPoints.SmartPoints.ProcessCmd.FZR:
                    break;
                case SmartPoints.SmartPoints.ProcessCmd.FZRI:
                    break;
                case SmartPoints.SmartPoints.ProcessCmd.FIN:
                    break;
                case SmartPoints.SmartPoints.ProcessCmd.MLP:
                    Cmd_MLP_WindowUi(cloud);
                    break;
                default:
                    break;
            }
        }
        private void Cmd_MLP_WindowUi(SmartPoints.SmartPoints.SmartPointsCloud cloud)
        {
            Label cloudnamelable = new Label();
            cloudnamelable.Text = "cloudnamelable";
            cloudnamelable.Text = cloud.SpcName;
            cloudnamelable.Location = new Point(0,0);

            ListView pointslistview = new ListView();
            pointslistview.Name= "pointslistview";
            pointslistview.Size = new Size(80, 240);
            
            pointslistview.View = View.List;
            pointslistview.Scrollable = true;
            for (int i = 0; i < cloud.points.Count; i++)
            {
                pointslistview.Items.Add("Point" + i + ":" + cloud.points[i].rectangle.Location.ToString());
            }

            Button Addbutton = new Button();
            Addbutton.Name = "Addbutton";
            Addbutton.Location = new Point(0, this.Height / 2 - 25);
            Addbutton.Size = new Size(25, 25);
            Addbutton.Text = "+";
            
            Button Removebutton = new Button();
            Removebutton.Name = "Removebutton";
            Removebutton.Location = new Point(this.Width / 2 - 25, this.Height / 2 +25);
            Removebutton.Size = new Size(25, 25);
            Removebutton.Text = "-";

            ListView respointslistview = new ListView();
            respointslistview.Name = "respointslistview";
            respointslistview.Size = new Size(80, 240);
            respointslistview.Location = new Point(20 + this.Width / 2, 60);
            respointslistview.View = View.List;
            respointslistview.Scrollable = true;

            Button Canclebutton = new Button();
            Canclebutton.Name = "Canclebutton";
            Canclebutton.Location = new Point( 25, this.Height - 80);
            Canclebutton.Size = new Size(50, 25);
            Canclebutton.Text = "Canclebutton";

            Button Okbutton = new Button();
            Okbutton.Name = "Okbutton";
            Okbutton.Location = new Point(this.Width / 2 + 25, this.Height - 80);
            Okbutton.Size = new Size(50, 25);
            Okbutton.Text = "OK";

            this.Controls.Add(cloudnamelable);
            this.Controls.Add(pointslistview);
            this.Controls.Add(Addbutton);
            this.Controls.Add(Removebutton);
            this.Controls.Add(respointslistview);
            this.Controls.Add(Canclebutton);
            this.Controls.Add(Okbutton);
        }
    }
}
