namespace SPCwindowUI
{
    partial class SPCwindow
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SPCWPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.SPCWPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // SPCWPictureBox
            // 
            this.SPCWPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SPCWPictureBox.Location = new System.Drawing.Point(0, 0);
            this.SPCWPictureBox.Name = "SPCWPictureBox";
            this.SPCWPictureBox.Size = new System.Drawing.Size(792, 635);
            this.SPCWPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SPCWPictureBox.TabIndex = 0;
            this.SPCWPictureBox.TabStop = false;
            this.SPCWPictureBox.MouseEnter += new System.EventHandler(this.SPCWPictureBox_MouseEnter);
            this.SPCWPictureBox.MouseLeave += new System.EventHandler(this.SPCWPictureBox_MouseLeave);
            this.SPCWPictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.SPCWPictureBox_PreviewKeyDown);
            // 
            // SPCwindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SPCWPictureBox);
            this.Name = "SPCwindow";
            this.Size = new System.Drawing.Size(792, 635);
            ((System.ComponentModel.ISupportInitialize)(this.SPCWPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox SPCWPictureBox;
    }
}
