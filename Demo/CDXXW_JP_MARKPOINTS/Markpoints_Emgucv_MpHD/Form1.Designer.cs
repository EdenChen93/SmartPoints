
namespace Markpoints_Emgucv_MpHD
{
    partial class HD_MaskPoints_Form
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
            this.DeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.触发ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.结束ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单次ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.后处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.特征ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.blobToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeToolStripMenuItem,
            this.后处理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(789, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // DeToolStripMenuItem
            // 
            this.DeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpToolStripMenuItem});
            this.DeToolStripMenuItem.Name = "DeToolStripMenuItem";
            this.DeToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.DeToolStripMenuItem.Text = "设备";
            // 
            // OpToolStripMenuItem
            // 
            this.OpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hDToolStripMenuItem,
            this.触发ToolStripMenuItem});
            this.OpToolStripMenuItem.Name = "OpToolStripMenuItem";
            this.OpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.OpToolStripMenuItem.Text = "打开";
            // 
            // hDToolStripMenuItem
            // 
            this.hDToolStripMenuItem.Name = "hDToolStripMenuItem";
            this.hDToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.hDToolStripMenuItem.Text = "HD";
            this.hDToolStripMenuItem.Click += new System.EventHandler(this.hDToolStripMenuItem_Click);
            // 
            // 触发ToolStripMenuItem
            // 
            this.触发ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.开始ToolStripMenuItem,
            this.结束ToolStripMenuItem,
            this.单次ToolStripMenuItem});
            this.触发ToolStripMenuItem.Name = "触发ToolStripMenuItem";
            this.触发ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.触发ToolStripMenuItem.Text = "触发";
            // 
            // 开始ToolStripMenuItem
            // 
            this.开始ToolStripMenuItem.Name = "开始ToolStripMenuItem";
            this.开始ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.开始ToolStripMenuItem.Text = "开始";
            this.开始ToolStripMenuItem.Click += new System.EventHandler(this.开始ToolStripMenuItem_Click);
            // 
            // 结束ToolStripMenuItem
            // 
            this.结束ToolStripMenuItem.Name = "结束ToolStripMenuItem";
            this.结束ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.结束ToolStripMenuItem.Text = "结束";
            this.结束ToolStripMenuItem.Click += new System.EventHandler(this.结束ToolStripMenuItem_Click);
            // 
            // 单次ToolStripMenuItem
            // 
            this.单次ToolStripMenuItem.Name = "单次ToolStripMenuItem";
            this.单次ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.单次ToolStripMenuItem.Text = "单次";
            this.单次ToolStripMenuItem.Click += new System.EventHandler(this.单次ToolStripMenuItem_Click);
            // 
            // 后处理ToolStripMenuItem
            // 
            this.后处理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.特征ToolStripMenuItem});
            this.后处理ToolStripMenuItem.Name = "后处理ToolStripMenuItem";
            this.后处理ToolStripMenuItem.Size = new System.Drawing.Size(56, 21);
            this.后处理ToolStripMenuItem.Text = "后处理";
            // 
            // 特征ToolStripMenuItem
            // 
            this.特征ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blobToolStripMenuItem});
            this.特征ToolStripMenuItem.Name = "特征ToolStripMenuItem";
            this.特征ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.特征ToolStripMenuItem.Text = "特征";
            // 
            // blobToolStripMenuItem
            // 
            this.blobToolStripMenuItem.Name = "blobToolStripMenuItem";
            this.blobToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.blobToolStripMenuItem.Text = "Blob";
            this.blobToolStripMenuItem.Click += new System.EventHandler(this.blobToolStripMenuItem_Click);
            // 
            // HD_MaskPoints_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 524);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "HD_MaskPoints_Form";
            this.Text = "HD";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 触发ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开始ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 结束ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 单次ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 后处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 特征ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem blobToolStripMenuItem;
    }
}

