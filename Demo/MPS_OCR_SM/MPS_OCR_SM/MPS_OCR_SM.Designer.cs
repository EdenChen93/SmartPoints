
namespace Double_Mps_Thickness
{
    partial class MPS_OCR_SM
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
            this.ConDev1Button = new System.Windows.Forms.Button();
            this.TriDev1Button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.计算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.平面方程ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConDev1Button
            // 
            this.ConDev1Button.Location = new System.Drawing.Point(14, 7);
            this.ConDev1Button.Name = "ConDev1Button";
            this.ConDev1Button.Size = new System.Drawing.Size(71, 40);
            this.ConDev1Button.TabIndex = 0;
            this.ConDev1Button.Text = "ConDev1";
            this.ConDev1Button.UseVisualStyleBackColor = true;
            this.ConDev1Button.Click += new System.EventHandler(this.ConDev1Button_Click);
            // 
            // TriDev1Button
            // 
            this.TriDev1Button.Location = new System.Drawing.Point(14, 81);
            this.TriDev1Button.Name = "TriDev1Button";
            this.TriDev1Button.Size = new System.Drawing.Size(71, 40);
            this.TriDev1Button.TabIndex = 1;
            this.TriDev1Button.Text = "TriDev1";
            this.TriDev1Button.UseVisualStyleBackColor = true;
            this.TriDev1Button.Click += new System.EventHandler(this.TriDev1Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TriDev1Button);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.ConDev1Button);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 472);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(741, 136);
            this.panel1.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.计算ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(741, 25);
            this.menuStrip1.TabIndex = 8;
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
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 计算ToolStripMenuItem
            // 
            this.计算ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.平面方程ToolStripMenuItem});
            this.计算ToolStripMenuItem.Name = "计算ToolStripMenuItem";
            this.计算ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.计算ToolStripMenuItem.Text = "计算";
            // 
            // 平面方程ToolStripMenuItem
            // 
            this.平面方程ToolStripMenuItem.Name = "平面方程ToolStripMenuItem";
            this.平面方程ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.平面方程ToolStripMenuItem.Text = "平面方程";
            this.平面方程ToolStripMenuItem.Click += new System.EventHandler(this.平面方程ToolStripMenuItem_Click);
            // 
            // MPS_OCR_SM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 608);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MPS_OCR_SM";
            this.Text = "WinForm";
            this.Load += new System.EventHandler(this.Double_Mps_Thickness_Demo_Load);
            this.Resize += new System.EventHandler(this.MPS_OCR_SM_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConDev1Button;
        private System.Windows.Forms.Button TriDev1Button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 计算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 平面方程ToolStripMenuItem;
    }
}

