namespace SPCWTest
{
    partial class SPTestWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SPTestWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.TSMItem_File = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xyzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.csvToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设备ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.megaPhaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.触发ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.连续ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打开ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工具ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.中值ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SPCTree = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SpcwPanle = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMItem_File,
            this.设备ToolStripMenuItem,
            this.工具ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1344, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // TSMItem_File
            // 
            this.TSMItem_File.BackColor = System.Drawing.Color.LightGray;
            this.TSMItem_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem,
            this.保存ToolStripMenuItem});
            this.TSMItem_File.Name = "TSMItem_File";
            this.TSMItem_File.Size = new System.Drawing.Size(44, 21);
            this.TSMItem_File.Text = "文件";
            // 
            // 打开ToolStripMenuItem
            // 
            this.打开ToolStripMenuItem.Name = "打开ToolStripMenuItem";
            this.打开ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.打开ToolStripMenuItem.Text = "打开";
            this.打开ToolStripMenuItem.Click += new System.EventHandler(this.打开ToolStripMenuItem_Click);
            // 
            // 保存ToolStripMenuItem
            // 
            this.保存ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xyzToolStripMenuItem,
            this.csvToolStripMenuItem});
            this.保存ToolStripMenuItem.Name = "保存ToolStripMenuItem";
            this.保存ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.保存ToolStripMenuItem.Text = "保存";
            // 
            // xyzToolStripMenuItem
            // 
            this.xyzToolStripMenuItem.Name = "xyzToolStripMenuItem";
            this.xyzToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.xyzToolStripMenuItem.Text = ".xyz";
            this.xyzToolStripMenuItem.Click += new System.EventHandler(this.xyzToolStripMenuItem_Click);
            // 
            // csvToolStripMenuItem
            // 
            this.csvToolStripMenuItem.Name = "csvToolStripMenuItem";
            this.csvToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.csvToolStripMenuItem.Text = ".csv";
            this.csvToolStripMenuItem.Click += new System.EventHandler(this.csvToolStripMenuItem_Click);
            // 
            // 设备ToolStripMenuItem
            // 
            this.设备ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.megaPhaseToolStripMenuItem});
            this.设备ToolStripMenuItem.Name = "设备ToolStripMenuItem";
            this.设备ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设备ToolStripMenuItem.Text = "设备";
            // 
            // megaPhaseToolStripMenuItem
            // 
            this.megaPhaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hDToolStripMenuItem});
            this.megaPhaseToolStripMenuItem.Name = "megaPhaseToolStripMenuItem";
            this.megaPhaseToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.megaPhaseToolStripMenuItem.Text = "MegaPhase";
            // 
            // hDToolStripMenuItem
            // 
            this.hDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem1,
            this.触发ToolStripMenuItem,
            this.连续ToolStripMenuItem});
            this.hDToolStripMenuItem.Name = "hDToolStripMenuItem";
            this.hDToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.hDToolStripMenuItem.Text = "HD";
            // 
            // 打开ToolStripMenuItem1
            // 
            this.打开ToolStripMenuItem1.Name = "打开ToolStripMenuItem1";
            this.打开ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.打开ToolStripMenuItem1.Text = "打开";
            this.打开ToolStripMenuItem1.Click += new System.EventHandler(this.打开ToolStripMenuItem1_Click);
            // 
            // 触发ToolStripMenuItem
            // 
            this.触发ToolStripMenuItem.Name = "触发ToolStripMenuItem";
            this.触发ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.触发ToolStripMenuItem.Text = "触发";
            this.触发ToolStripMenuItem.Click += new System.EventHandler(this.触发ToolStripMenuItem_Click);
            // 
            // 连续ToolStripMenuItem
            // 
            this.连续ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开ToolStripMenuItem2,
            this.关闭ToolStripMenuItem});
            this.连续ToolStripMenuItem.Name = "连续ToolStripMenuItem";
            this.连续ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.连续ToolStripMenuItem.Text = "连续";
            // 
            // 打开ToolStripMenuItem2
            // 
            this.打开ToolStripMenuItem2.Name = "打开ToolStripMenuItem2";
            this.打开ToolStripMenuItem2.Size = new System.Drawing.Size(100, 22);
            this.打开ToolStripMenuItem2.Text = "打开";
            this.打开ToolStripMenuItem2.Click += new System.EventHandler(this.打开ToolStripMenuItem2_Click);
            // 
            // 关闭ToolStripMenuItem
            // 
            this.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem";
            this.关闭ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.关闭ToolStripMenuItem.Text = "关闭";
            this.关闭ToolStripMenuItem.Click += new System.EventHandler(this.关闭ToolStripMenuItem_Click);
            // 
            // 工具ToolStripMenuItem
            // 
            this.工具ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dToolStripMenuItem});
            this.工具ToolStripMenuItem.Name = "工具ToolStripMenuItem";
            this.工具ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.工具ToolStripMenuItem.Text = "工具";
            // 
            // dToolStripMenuItem
            // 
            this.dToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fillToolStripMenuItem,
            this.filterToolStripMenuItem,
            this.levelingToolStripMenuItem});
            this.dToolStripMenuItem.Name = "dToolStripMenuItem";
            this.dToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.dToolStripMenuItem.Text = "3D";
            // 
            // fillToolStripMenuItem
            // 
            this.fillToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.中值ToolStripMenuItem});
            this.fillToolStripMenuItem.Name = "fillToolStripMenuItem";
            this.fillToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.fillToolStripMenuItem.Text = "Fill";
            // 
            // 中值ToolStripMenuItem
            // 
            this.中值ToolStripMenuItem.Name = "中值ToolStripMenuItem";
            this.中值ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.中值ToolStripMenuItem.Text = "中值";
            this.中值ToolStripMenuItem.Click += new System.EventHandler(this.中值ToolStripMenuItem_Click);
            // 
            // filterToolStripMenuItem
            // 
            this.filterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zRangeToolStripMenuItem});
            this.filterToolStripMenuItem.Name = "filterToolStripMenuItem";
            this.filterToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.filterToolStripMenuItem.Text = "Filter";
            // 
            // zRangeToolStripMenuItem
            // 
            this.zRangeToolStripMenuItem.Name = "zRangeToolStripMenuItem";
            this.zRangeToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.zRangeToolStripMenuItem.Text = "ZRange";
            this.zRangeToolStripMenuItem.Click += new System.EventHandler(this.zRangeToolStripMenuItem_Click);
            // 
            // levelingToolStripMenuItem
            // 
            this.levelingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointsToolStripMenuItem});
            this.levelingToolStripMenuItem.Name = "levelingToolStripMenuItem";
            this.levelingToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.levelingToolStripMenuItem.Text = "Leveling";
            // 
            // pointsToolStripMenuItem
            // 
            this.pointsToolStripMenuItem.Name = "pointsToolStripMenuItem";
            this.pointsToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.pointsToolStripMenuItem.Text = "3Points";
            this.pointsToolStripMenuItem.Click += new System.EventHandler(this.pointsToolStripMenuItem_Click);
            // 
            // SPCTree
            // 
            this.SPCTree.BackColor = System.Drawing.Color.Linen;
            this.SPCTree.Font = new System.Drawing.Font("宋体", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SPCTree.Location = new System.Drawing.Point(0, 53);
            this.SPCTree.Name = "SPCTree";
            this.SPCTree.Size = new System.Drawing.Size(300, 632);
            this.SPCTree.TabIndex = 1;
            this.SPCTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SPCTree_AfterSelect);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1344, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // SpcwPanle
            // 
            this.SpcwPanle.AutoScroll = true;
            this.SpcwPanle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SpcwPanle.BackgroundImage")));
            this.SpcwPanle.Location = new System.Drawing.Point(297, 53);
            this.SpcwPanle.Name = "SpcwPanle";
            this.SpcwPanle.Size = new System.Drawing.Size(1047, 635);
            this.SpcwPanle.TabIndex = 3;
            // 
            // SPTestWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1344, 683);
            this.Controls.Add(this.SpcwPanle);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.SPCTree);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SPTestWindow";
            this.Text = "SpcWindow";
            this.SizeChanged += new System.EventHandler(this.SPTestWindow_SizeChanged);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem TSMItem_File;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem;
        private System.Windows.Forms.TreeView SPCTree;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel SpcwPanle;
        private System.Windows.Forms.ToolStripMenuItem 设备ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem megaPhaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 触发ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 工具ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fillToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 中值ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xyzToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem csvToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 连续ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打开ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem levelingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pointsToolStripMenuItem;
        //private System.Windows.Forms.TreeView SPCTree;
    }
}

