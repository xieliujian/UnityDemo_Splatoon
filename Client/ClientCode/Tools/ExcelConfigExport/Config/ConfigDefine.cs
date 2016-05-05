
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum EConfigHeadType
{
    CodeName = 0,
    DataType,
    ShareType,
    Desc,
    Count
}

enum ECfgMgrType
{
    None = 0,
    OneKeyMgr,      // 第一列键值唯一索引
    TwoKeyMgr,      // 前两列键值组唯一索引
    NoKeyList,      // 无键值配置数据列表
    OneGroupMgr,    // 根据第一列分组
}

enum ECodeLanguageType
{
    CS = 0,
    CPP = 1,
}

public class ColumnInfo
{
    public string ColDesc;
    public string ConfigName;
    public ConfigDataType DataType;
    public string DefName;

    public void SetColumnInfo(object desc, object name, ConfigDataType dt)
    {
        string sName = (null == name) ? string.Empty : name.ToString().Trim();
        string sDesc = (null == desc) ? string.Empty : desc.ToString().Trim();

        ColDesc = sDesc;
        ConfigName = sName;
        DataType = dt;
        DefName = sName;
    }

    internal string VarName
    {
        get { return DefName; }
    }

    internal string GetVarDefineStr(ECodeLanguageType e)
    {
        switch (e)
        {
            case ECodeLanguageType.CS:
                return string.Format("{0} {1}", DataType.CsName, DefName);
            case ECodeLanguageType.CPP:
                return string.Format("{0} {1}", DataType.CppName, DefName);
            default:
                break;
        }
        return string.Empty;
    }
}

public class ConfigDataType
{
    public string ConfigName;
    public string CppName;
    public string CsName;
    public Type CsType;

    public ConfigDataType(string config, string cpp, string cs, Type type)
    {
        ConfigName = config;
        CppName = cpp;
        CsName = cs;
        CsType = type;
    }

    public object GetData(object data)
    {
        if (data == null)
            return null;

        if (data.ToString().Length <= 0)
            return null;

        return Convert.ChangeType(data, CsType);
    }
}

class CfgMgrType
{
    internal ECfgMgrType MgrEnum { get; private set; }
    internal string MgrDesc { get; private set; }
    internal string CsMgrBaseName { get; private set; }
    internal string CppMgrBaseName { get; private set; }
    internal string CppMgrHead { get; private set; }    // 头文件包含语句

    internal bool IsCreateMgr { get { return MgrEnum != ECfgMgrType.None; } }

    internal CfgMgrType(ECfgMgrType e, string desc, string csbase, string cppbase, string cpphead)
    {
        MgrEnum = e;
        MgrDesc = desc;
        CsMgrBaseName = csbase;
        CppMgrBaseName = cppbase;
        CppMgrHead = cpphead;
    }

    public override string ToString()
    {
        return MgrDesc;
    }
}

class ConfigDefine
{
    public static readonly string EXCEL_FILTER = "Excel文件(*.xlsx)|*.xlsx";
    public static readonly string SPLIT_STRING = ",";

    public static readonly ConfigDataType[] ALL_DATA_TYPE = new ConfigDataType[] 
    {
        new ConfigDataType("int", "INT", "int", typeof(int)),
        new ConfigDataType("uint", "DWORD", "uint", typeof(uint)),
        new ConfigDataType("short", "SHORT", "short", typeof(short)),
        new ConfigDataType("ushort", "USHORT", "ushort", typeof(ushort)),
        new ConfigDataType("word", "WORD", "ushort", typeof(ushort)),
        new ConfigDataType("byte", "BYTE", "byte", typeof(byte)),
        new ConfigDataType("char", "CHAR", "char", typeof(char)),
        new ConfigDataType("float", "FLOAT", "float", typeof(float)),
        new ConfigDataType("string", "KSTRING", "string", typeof(string)),
    };

    public static readonly string SOURCE_FILE_HEAD = @"
//============================================
//--4>:
//    Exported by ExcelConfigExport
//
//    此代码为工具根据配置自动生成, 建议不要修改
//
//============================================";

    internal static readonly string CSHARP_NAMESPACE = "namespace Splatoon";

    internal static readonly string CSHARP_CLASS_BASE_0KEY = "ICsvItem";
    internal static readonly string CSHARP_KEY_PREFIX = "_KEY_";

    internal static readonly string CSHARP_READ_READER = "tf";
    internal static readonly string CSHARP_READ_METHOD = "public bool ReadItem(CsvFile " + CSHARP_READ_READER + ")";

    public static readonly string TAB_STRING_MAX = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t";

    public static readonly string CSHARP_INCLUDE = "using System;" + Environment.NewLine + "using UnityEngine;";

    public static readonly CfgMgrType[] ALL_MGR_TYPE = new CfgMgrType[] {
        new CfgMgrType(ECfgMgrType.None, "不创建管理器", string.Empty, string.Empty, string.Empty),

        new CfgMgrType(ECfgMgrType.OneKeyMgr, "第一列索引管理器", "Cfg1KeyMgrTemplate",
            "Cfg1KeyMgrTemplate", @"#include ""Config1KeyMgrTemplate.h"""),

        new CfgMgrType(ECfgMgrType.TwoKeyMgr, "前两列索引管理器", "Cfg2KeyMgrTemplate",
            "Cfg2KeyMgrTemplate", @"#include ""Config2KeyMgrTemplate.h"""),
                    
        new CfgMgrType(ECfgMgrType.NoKeyList, "无索引链表管理器", "CfgListMgrTemplate",
            "CfgListMgrTemplate", @"#include ""ConfigListMgrTemplate.h"""),

        new CfgMgrType(ECfgMgrType.OneGroupMgr, "第一列分组管理器", "Cfg1GroupMgrTemplate",
            "Cfg1GroupMgrTemplate", @"#include ""Config1GroupMgrTemplate.h"""),
    };
}