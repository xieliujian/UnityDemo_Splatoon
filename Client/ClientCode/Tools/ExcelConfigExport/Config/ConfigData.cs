
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class ConfigData
{
    ColumnInfo[] mColumnInfos;
    Hashtable mDataTypeMap;
    List<object[]> mLoadDataList;

    public ConfigData()
    {
        mDataTypeMap = new Hashtable();
        foreach (ConfigDataType dt in ConfigDefine.ALL_DATA_TYPE)
        {
            mDataTypeMap.Add(dt.ConfigName, dt);
        }

        mLoadDataList = new List<object[]>();
    }

    public void Clear()
    {
        mLoadDataList.Clear();

        if (mColumnInfos != null)
            Array.Clear(mColumnInfos, 0, mColumnInfos.Length);
        mColumnInfos = null;
    }

    public void Init(object [,] head)
    {
        if (head == null)
            return;

        if (head.GetLength(0) <= (int)EConfigHeadType.Count)
            return;

        mColumnInfos = new ColumnInfo[head.GetLength(1)];
        int rowBase = head.GetLowerBound(0);
        int colBase = head.GetLowerBound(1);
        for (int col = 0; col < mColumnInfos.Length; col++)
        {
            SetColumnInfo(col,
                    head[rowBase + (int)EConfigHeadType.Desc, col + colBase],
                    head[rowBase + (int)EConfigHeadType.CodeName, col + colBase],
                    head[rowBase + (int)EConfigHeadType.DataType, col + colBase],
                    head[rowBase + (int)EConfigHeadType.ShareType, col + colBase]);
        }
    }

    void SetColumnInfo(int col, object desc, object name, object dt, object share)
    {
        ConfigDataType datatype = GetDataType(dt);
        ColumnInfo info = new ColumnInfo();
        info.SetColumnInfo(desc, name, datatype);
        mColumnInfos[col] = info;
    }

    ConfigDataType GetDataType(object dt)
    {
        if (mDataTypeMap.Contains(dt.ToString().Trim()))
        {
            return mDataTypeMap[dt] as ConfigDataType;
        }

        return null;
    }

    public void ExportConfigFile(string apppath, string name)
    {
        string configname = apppath + "\\" + name + ".csv";
        CreateFile(configname);

        StreamWriter fs = new StreamWriter(configname, false, new UTF8Encoding(false));
        string oLine = string.Empty;

        // 写表头 
        for (int i = 0; i < mColumnInfos.Length; i++)
        {
            if (i > 0)
            {
                oLine += ConfigDefine.SPLIT_STRING;
            }
            oLine += mColumnInfos[i].ConfigName;
        }
        fs.WriteLine(oLine);

        // 写数据
        foreach (object[] line in mLoadDataList)
        {
            oLine = string.Empty;
            for (int i = 0; i < mColumnInfos.Length; i++)
            {
                if (i > 0)
                {
                    oLine += ConfigDefine.SPLIT_STRING;
                }

                oLine += line[i].ToString();
            }

            fs.WriteLine(oLine);
        }

        fs.Close();
    }

    public void ExportSrcFile(string apppath, string name)
    {
        ExportCShapeSource(apppath, name);
    }

    void CreateFile(string name)
    {
        if (File.Exists(name))
        {
            File.Delete(name);
        }

        FileStream fs = File.Create(name);
        fs.Close();
    }

    public void ExportCShapeSource(string apppath, string name)
    {
        string srcname = apppath + "\\" + name + "Config.cs";
        string classname = name + "Config";
        CreateFile(srcname);

        StreamWriter fs = new StreamWriter(srcname, false, new UTF8Encoding(false));

        // 头部数据 
        WriteCode(fs, 0, ConfigDefine.SOURCE_FILE_HEAD);
        fs.WriteLine();
        WriteCode(fs, 0, ConfigDefine.CSHARP_INCLUDE);
        fs.WriteLine();

        // 命名空间
        WriteCode(fs, 0, ConfigDefine.CSHARP_NAMESPACE);
        WriteCode(fs, 0, "{");

        // 管理器定义
        WriteCsMgrDef(fs, classname);

        // 类定义
        WriteCsClassDef(fs, classname);
        WriteCode(fs, 1, "{");

        // 导出列的字符串索引
        for (int col = 0; col < mColumnInfos.Length; col++ )
        {
            ColumnInfo info = mColumnInfos[col];
            WriteCode(fs, 2, "public static readonly string {0}{1} = \"{2}\";",
                ConfigDefine.CSHARP_KEY_PREFIX, info.ConfigName, info.ConfigName);
        }
        fs.WriteLine();

        // 成员变量定义
        for (int col = 0; col < mColumnInfos.Length; col++)
        {
            ColumnInfo info = mColumnInfos[col];
            // 数组只有下标为 0 的需要定义
                WriteCode(fs, 2, "public {0} {{ get; private set; }}\t\t\t\t// {1}", info.GetVarDefineStr(ECodeLanguageType.CS), info.ColDesc);
        }
        fs.WriteLine();

        // 构造函数定义(用于初始化数组变量)
        WriteCode(fs, 2, "public {0}()", classname);
        WriteCode(fs, 2, "{");
        for (int col = 0; col < mColumnInfos.Length; col++)
        {
            ColumnInfo info = mColumnInfos[col];
            //if (info.IsArray && 0 == info.ArrayIndex)
            //{
            //    writeCode(fs, 2, "{0} = new {1}[{2}];", info.DefName, info.DataType.CsName, info.ArraySize);
            //}
        }
        WriteCode(fs, 2, "}");
        fs.WriteLine();

        WriteCsKeyInterface(fs);

        // 数据读取函数
        WriteCode(fs, 2, ConfigDefine.CSHARP_READ_METHOD);
        WriteCode(fs, 2, "{");
        for (int col = 0; col < mColumnInfos.Length; col++)
        {
            ColumnInfo info = mColumnInfos[col];
            WriteCode(fs, 3, "{0} = {1}.Get<{2}>({3}{4});", info.VarName, ConfigDefine.CSHARP_READ_READER,
                info.DataType.CsName, ConfigDefine.CSHARP_KEY_PREFIX, info.ConfigName);
        }

        // 函数末尾
        WriteCode(fs, 3, "return true;");
        WriteCode(fs, 2, "}");

        WriteCode(fs, 1, "}");
        fs.WriteLine();
        WriteCode(fs, 0, "}");
        fs.WriteLine();
        fs.Close();
    }

    public void ClearData()
    {
        mLoadDataList.Clear();
    }

    public void AddMultiLine(object[,] multiLine, int addRowBegin)
    {
        int rowend = multiLine.GetUpperBound(0);
        int colbegin = multiLine.GetLowerBound(1);

        List<object[]> allDataLine = new List<object[]>();
        for (int row = addRowBegin; row <= rowend; row++)
        {
            int colnum = mColumnInfos.Length;
            object[] dateLine = new object[colnum];
            for (int col = 0; col < colnum; col++)
            {
                dateLine[col] = GetColumnData(col, multiLine[row, col + colbegin]);
            }

            allDataLine.Add(dateLine);
        }

        mLoadDataList.AddRange(allDataLine);
    }

    object GetColumnData(int col, object data)
    {
        if (col < 0 || col > (mColumnInfos.Length - 1))
            return null;

        ColumnInfo info = mColumnInfos[col];
        return info.DataType.GetData(data);
    }

    void WriteCode(StreamWriter fs, int tabNum, string data)
    {
        WriteCode(fs, tabNum, data, null);
    }

    void WriteCode(StreamWriter fs, int tabNum, string format, params object[] args)
    {
        if (tabNum < 0 || tabNum > ConfigDefine.TAB_STRING_MAX.Length)
        {
            tabNum = 0;
        }

        if (null == args)
        {
            fs.WriteLine(ConfigDefine.TAB_STRING_MAX.Substring(0, tabNum) + format);
        }
        else
        {
            fs.WriteLine(ConfigDefine.TAB_STRING_MAX.Substring(0, tabNum) + string.Format(format, args));
        }
    }

    void WriteCsMgrDef(StreamWriter fs, string name)
    {
        string mgrName = name + "Mgr";
        WriteCode(fs, 1, "public class {0} : {1}<{2}> {{ }};", mgrName, "CfgListMgrTemplate", name);
        fs.WriteLine();
    }

    void WriteCsClassDef(StreamWriter fs, string name)
    {
        WriteCode(fs, 1, "public class {0} : {1}", name, ConfigDefine.CSHARP_CLASS_BASE_0KEY);
    }

    void WriteCsKeyInterface(StreamWriter fs)
    {

    }
}
