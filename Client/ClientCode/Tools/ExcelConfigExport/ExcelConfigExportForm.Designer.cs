namespace ExcelConfigExport
{
    partial class ExcelConfigExportForm
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mOpenExcelPathBtn = new System.Windows.Forms.Button();
            this.mExcelPathLbl = new System.Windows.Forms.Label();
            this.mExportBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mOpenExcelPathBtn
            // 
            this.mOpenExcelPathBtn.Location = new System.Drawing.Point(1, 29);
            this.mOpenExcelPathBtn.Name = "mOpenExcelPathBtn";
            this.mOpenExcelPathBtn.Size = new System.Drawing.Size(128, 37);
            this.mOpenExcelPathBtn.TabIndex = 0;
            this.mOpenExcelPathBtn.Text = "打开Excel文件：";
            this.mOpenExcelPathBtn.UseVisualStyleBackColor = true;
            this.mOpenExcelPathBtn.Click += new System.EventHandler(this.mOpenExcelPathBtn_Click);
            // 
            // mExcelPathLbl
            // 
            this.mExcelPathLbl.AutoSize = true;
            this.mExcelPathLbl.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mExcelPathLbl.Location = new System.Drawing.Point(180, 42);
            this.mExcelPathLbl.Name = "mExcelPathLbl";
            this.mExcelPathLbl.Size = new System.Drawing.Size(0, 24);
            this.mExcelPathLbl.TabIndex = 1;
            // 
            // mExportBtn
            // 
            this.mExportBtn.Location = new System.Drawing.Point(356, 80);
            this.mExportBtn.Name = "mExportBtn";
            this.mExportBtn.Size = new System.Drawing.Size(184, 39);
            this.mExportBtn.TabIndex = 2;
            this.mExportBtn.Text = "导出";
            this.mExportBtn.UseVisualStyleBackColor = true;
            this.mExportBtn.Click += new System.EventHandler(this.mExportBtn_Click);
            // 
            // ExcelConfigExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 131);
            this.Controls.Add(this.mExportBtn);
            this.Controls.Add(this.mExcelPathLbl);
            this.Controls.Add(this.mOpenExcelPathBtn);
            this.Name = "ExcelConfigExportForm";
            this.Text = "ExcelConfigExport";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button mOpenExcelPathBtn;
        private System.Windows.Forms.Label mExcelPathLbl;
        private System.Windows.Forms.Button mExportBtn;


    }
}

