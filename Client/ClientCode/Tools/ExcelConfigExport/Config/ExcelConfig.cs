using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

public class ExcelConfig
{
    Excel.Application mExcelApp;
    Excel.Workbook mWorkbook;
    Excel.Worksheet mWorksheet;
    ConfigData mConfigData;
    object[,] mAllData;

    public ExcelConfig()
    {
        mConfigData = new ConfigData();
        mAllData = null;
    }

    void Clear()
    {
        if (mWorkbook != null)
            mWorkbook.Close(false, false, Missing.Value);
        mWorksheet = null;
        mWorkbook = null;

        if (mExcelApp != null)
            mExcelApp.Quit();
        mExcelApp = null;

        mAllData = null;
        mConfigData.Clear();
    }

    public void LoadExcel(string path)
    {
        Clear();

        try
        {
            mExcelApp = new Excel.Application();
            object miss = System.Reflection.Missing.Value;
            mWorkbook = (Excel.Workbook)mExcelApp.Workbooks.Open(path,
                    miss, true, miss, miss, miss, true, miss, miss, true, miss, miss, miss, miss, miss);
        }
        catch (System.Exception)
        {
            mWorkbook = null;
            mExcelApp.Quit();
        }

        if (mWorkbook == null)
            return;

        foreach (Excel.Worksheet sheet in mWorkbook.Worksheets)
        {
            mWorksheet = sheet;
            break;
        }

        Parse();
    }

    public void Parse()
    {
        if (mWorksheet == null)
            return;

        Excel.Range range = mWorksheet.UsedRange.CurrentRegion;
        int row = range.Rows.Count;
        int column = range.Columns.Count;

        if (row < 4 || column < 1)
            return;

        mAllData = range.Value2 as object[,];
        mConfigData.Init(mAllData);
    }

    public void Export(string apppath, string name)
    {
        if (mConfigData == null || mAllData == null)
            return;

        mConfigData.ClearData();
        mConfigData.AddMultiLine(mAllData, mAllData.GetLowerBound(0) + (int)EConfigHeadType.Count);

        mConfigData.ExportConfigFile(apppath, name);
        mConfigData.ExportSrcFile(apppath, name);
    }
}
