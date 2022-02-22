namespace SYQL_MPS_EmguCV_Demo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mpdatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.md3mToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.色谱ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置色谱范围ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.腐蚀ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.找圆ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.三点校平ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.灰度图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.工具ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(793, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mpdatToolStripMenuItem,
            this.md3mToolStripMenuItem});
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // mpdatToolStripMenuItem
            // 
            this.mpdatToolStripMenuItem.Name = "mpdatToolStripMenuItem";
            this.mpdatToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.mpdatToolStripMenuItem.Text = "mpdat";
            this.mpdatToolStripMenuItem.Click += new System.EventHandler(this.mpdatToolStripMenuItem_Click);
            // 
            // md3mToolStripMenuItem
            // 
            this.md3mToolStripMenuItem.Name = "md3mToolStripMenuItem";
            this.md3mToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.md3mToolStripMenuItem.Text = "md3m";
            this.md3mToolStripMenuItem.Click += new System.EventHandler(this.md3mToolStripMenuItem_Click);
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.色谱ToolStripMenuItem,
            this.cVToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.视图ToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // 色谱ToolStripMenuItem
            // 
            this.色谱ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置色谱范围ToolStripMenuItem});
            this.色谱ToolStripMenuItem.Name = "色谱ToolStripMenuItem";
            this.色谱ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.色谱ToolStripMenuItem.Text = "过滤";
            // 
            // 设置色谱范围ToolStripMenuItem
            // 
            this.设置色谱范围ToolStripMenuItem.Name = "设置色谱范围ToolStripMenuItem";
            this.设置色谱范围ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.设置色谱范围ToolStripMenuItem.Text = "范围过滤";
            this.设置色谱范围ToolStripMenuItem.Click += new System.EventHandler(this.设置色谱范围ToolStripMenuItem_Click);
            // 
            // cVToolStripMenuItem
            // 
            this.cVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.腐蚀ToolStripMenuItem,
            this.找圆ToolStripMenuItem});
            this.cVToolStripMenuItem.Name = "cVToolStripMenuItem";
            this.cVToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.cVToolStripMenuItem.Text = "CV";
            // 
            // 腐蚀ToolStripMenuItem
            // 
            this.腐蚀ToolStripMenuItem.Name = "腐蚀ToolStripMenuItem";
            this.腐蚀ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.腐蚀ToolStripMenuItem.Text = "腐蚀";
            this.腐蚀ToolStripMenuItem.Click += new System.EventHandler(this.腐蚀ToolStripMenuItem_Click);
            // 
            // 找圆ToolStripMenuItem
            // 
            this.找圆ToolStripMenuItem.Name = "找圆ToolStripMenuItem";
            this.找圆ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.找圆ToolStripMenuItem.Text = "找圆";
            this.找圆ToolStripMenuItem.Click += new System.EventHandler(this.找圆ToolStripMenuItem_Click);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.三点校平ToolStripMenuItem});
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.修改ToolStripMenuItem.Text = "修改";
            // 
            // 三点校平ToolStripMenuItem
            // 
            this.三点校平ToolStripMenuItem.Name = "三点校平ToolStripMenuItem";
            this.三点校平ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.三点校平ToolStripMenuItem.Text = "三点校平";
            this.三点校平ToolStripMenuItem.Click += new System.EventHandler(this.三点校平ToolStripMenuItem_Click);
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.灰度图ToolStripMenuItem});
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            this.视图ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.视图ToolStripMenuItem.Text = "视图";
            // 
            // 灰度图ToolStripMenuItem
            // 
            this.灰度图ToolStripMenuItem.Name = "灰度图ToolStripMenuItem";
            this.灰度图ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.灰度图ToolStripMenuItem.Text = "灰度图";
            this.灰度图ToolStripMenuItem.Click += new System.EventHandler(this.灰度图ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 626);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Demo";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 色谱ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置色谱范围ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 腐蚀ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 找圆ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 三点校平ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mpdatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem md3mToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 灰度图ToolStripMenuItem;
    }
}

