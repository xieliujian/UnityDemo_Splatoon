
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
	public class GunConfigMgr : CfgListMgrTemplate<GunConfig> { };

	public class GunConfig : ICsvItem
	{
		public static readonly string _KEY_UserType = "UserType";
		public static readonly string _KEY_GunId = "GunId";
		public static readonly string _KEY_GunPrefab = "GunPrefab";
		public static readonly string _KEY_Icon = "Icon";

		public ushort UserType { get; private set; }				// 玩家使用类型
		public ushort GunId { get; private set; }				// 枪ID
		public string GunPrefab { get; private set; }				// 枪预设
		public string Icon { get; private set; }				// 图标

		public GunConfig()
		{
		}

		public bool ReadItem(CsvFile tf)
		{
			UserType = tf.Get<ushort>(_KEY_UserType);
			GunId = tf.Get<ushort>(_KEY_GunId);
			GunPrefab = tf.Get<string>(_KEY_GunPrefab);
			Icon = tf.Get<string>(_KEY_Icon);
			return true;
		}
	}

}

