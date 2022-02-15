
namespace SPCWTest
{
    partial class ChartWindowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.分割点集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.插补NANToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.左值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.右值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.低值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.校平ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.两点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.寻点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.指定高度值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.过滤ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.噪音峰ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.LineTree = new System.Windows.Forms.TreeView();
            this.processToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.工具ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1150, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.processToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.分割点集ToolStripMenuItem,
            this.插补NANToolStripMenuItem,
            this.校平ToolStripMenuItem,
            this.寻点ToolStripMenuItem,
            this.过滤ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 分割点集ToolStripMenuItem
            // 
            this.分割点集ToolStripMenuItem.Name = "分割点集ToolStripMenuItem";
            this.分割点集ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.分割点集ToolStripMenuItem.Text = "分割点集";
            this.分割点集ToolStripMenuItem.Click += new System.EventHandler(this.分割点集ToolStripMenuItem_Click);
            // 
            // 插补NANToolStripMenuItem
            // 
            this.插补NANToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.左值ToolStripMenuItem,
            this.右值ToolStripMenuItem,
            this.中值ToolStripMenuItem,
            this.高值ToolStripMenuItem,
            this.低值ToolStripMenuItem});
            this.插补NANToolStripMenuItem.Name = "插补NANToolStripMenuItem";
            this.插补NANToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.插补NANToolStripMenuItem.Text = "插补NAN";
            // 
            // 左值ToolStripMenuItem
            // 
            this.左值ToolStripMenuItem.Name = "左值ToolStripMenuItem";
            this.左值ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.左值ToolStripMenuItem.Text = "左值";
            this.左值ToolStripMenuItem.Click += new System.EventHandler(this.左值ToolStripMenuItem_Click);
            // 
            // 右值ToolStripMenuItem
            // 
            this.右值ToolStripMenuItem.Name = "右值ToolStripMenuItem";
            this.右值ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.右值ToolStripMenuItem.Text = "右值";
            this.右值ToolStripMenuItem.Click += new System.EventHandler(this.右值ToolStripMenuItem_Click);
            // 
            // 中值ToolStripMenuItem
            // 
            this.中值ToolStripMenuItem.Name = "中值ToolStripMenuItem";
            this.中值ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.中值ToolStripMenuItem.Text = "中值";
            this.中值ToolStripMenuItem.Click += new System.EventHandler(this.中值ToolStripMenuItem_Click);
            // 
            // 高值ToolStripMenuItem
            // 
            this.高值ToolStripMenuItem.Name = "高值ToolStripMenuItem";
            this.高值ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.高值ToolStripMenuItem.Text = "高值";
            this.高值ToolStripMenuItem.Click += new System.EventHandler(this.高值ToolStripMenuItem_Click);
            // 
            // 低值ToolStripMenuItem
            // 
            this.低值ToolStripMenuItem.Name = "低值ToolStripMenuItem";
            this.低值ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.低值ToolStripMenuItem.Text = "低值";
            this.低值ToolStripMenuItem.Click += new System.EventHandler(this.低值ToolStripMenuItem_Click);
            // 
            // 校平ToolStripMenuItem
            // 
            this.校平ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.两点ToolStripMenuItem});
            this.校平ToolStripMenuItem.Name = "校平ToolStripMenuItem";
            this.校平ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.校平ToolStripMenuItem.Text = "校平";
            // 
            // 两点ToolStripMenuItem
            // 
            this.两点ToolStripMenuItem.Name = "两点ToolStripMenuItem";
            this.两点ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.两点ToolStripMenuItem.Text = "两点";
            this.两点ToolStripMenuItem.Click += new System.EventHandler(this.两点ToolStripMenuItem_Click);
            // 
            // 寻点ToolStripMenuItem
            // 
            this.寻点ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.指定高度值ToolStripMenuItem});
            this.寻点ToolStripMenuItem.Name = "寻点ToolStripMenuItem";
            this.寻点ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.寻点ToolStripMenuItem.Text = "寻点";
            // 
            // 指定高度值ToolStripMenuItem
            // 
            this.指定高度值ToolStripMenuItem.Name = "指定高度值ToolStripMenuItem";
            this.指定高度值ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.指定高度值ToolStripMenuItem.Text = "指定高度值";
            this.指定高度值ToolStripMenuItem.Click += new System.EventHandler(this.指定高度值ToolStripMenuItem_Click);
            // 
            // 过滤ToolStripMenuItem
            // 
            this.过滤ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.噪音峰ToolStripMenuItem});
            this.过滤ToolStripMenuItem.Name = "过滤ToolStripMenuItem";
            this.过滤ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.过滤ToolStripMenuItem.Text = "过滤";
            // 
            // 噪音峰ToolStripMenuItem
            // 
            this.噪音峰ToolStripMenuItem.Name = "噪音峰ToolStripMenuItem";
            this.噪音峰ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.噪音峰ToolStripMenuItem.Text = "噪音峰";
            this.噪音峰ToolStripMenuItem.Click += new System.EventHandler(this.噪音峰ToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1150, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Right;
            this.chart1.Location = new System.Drawing.Point(220, 50);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(930, 458);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // LineTree
            // 
            this.LineTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.LineTree.Location = new System.Drawing.Point(0, 50);
            this.LineTree.Name = "LineTree";
            this.LineTree.Size = new System.Drawing.Size(221, 458);
            this.LineTree.TabIndex = 3;
            // 
            // processToolStripMenuItem
            // 
            this.processToolStripMenuItem.Name = "processToolStripMenuItem";
            this.processToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.processToolStripMenuItem.Text = "Process";
            this.processToolStripMenuItem.Click += new System.EventHandler(this.processToolStripMenuItem_Click);
            // 
            // ChartWindowForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 508);
            this.Controls.Add(this.LineTree);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ChartWindowForm";
            this.Text = "ChartWindowForm";
            this.SizeChanged += new System.EventHandler(this.ChartWindowForm_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 分割点集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 插补NANToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 左值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 右值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 中值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 高值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 低值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 校平ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 两点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 寻点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 指定高度值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 过滤ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 噪音峰ToolStripMenuItem;
        private System.Windows.Forms.TreeView LineTree;
        private System.Windows.Forms.ToolStripMenuItem processToolStripMenuItem;
    }
}