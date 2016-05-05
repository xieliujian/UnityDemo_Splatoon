
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelConfigExport
{
    public partial class ExcelConfigExportForm : Form
    {
        ExcelConfig mExcelCfg;
        string mExcelPath;
        string mExcelFileName;
        string mAppPath;

        public ExcelConfigExportForm()
        {
            InitializeComponent();
            Init();
        }

        void Init()
        {
            mExcelCfg = new ExcelConfig();
        }

        private void mOpenExcelPathBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = ConfigDefine.EXCEL_FILTER;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                mExcelPathLbl.Text = dialog.FileName;
                mExcelPath = Path.GetDirectoryName(dialog.FileName);
                mExcelFileName = Path.GetFileNameWithoutExtension(dialog.FileName);
                mAppPath = System.Environment.CurrentDirectory;

                mExcelCfg.LoadExcel(dialog.FileName);
            }
        }

        private void mExportBtn_Click(object sender, EventArgs e)
        {
            mExcelCfg.Export(mAppPath, mExcelFileName);
        }
    }
}
