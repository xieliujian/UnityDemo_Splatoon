
//============================================
//--4>:
//    Exported by ExcelConfigExport
//
//    此代码为工具根据配置自动生成, 建议不要修改
//
//============================================

using System;
using UnityEngine;

namespace Splatoon
{
	public class LoadingDescConfigMgr : CfgListMgrTemplate<LoadingDescConfig> { };

	public class LoadingDescConfig : ICsvItem
	{
		public static readonly string _KEY_Desc = "Desc";

		public string Desc { get; private set; }				// Loading条的描述

		public LoadingDescConfig()
		{
		}

		public bool ReadItem(CsvFile tf)
		{
			Desc = tf.Get<string>(_KEY_Desc);
			return true;
		}
	}

}

