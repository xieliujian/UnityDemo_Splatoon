
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
	public class MinimapConfigMgr : CfgListMgrTemplate<MinimapConfig> { };

	public class MinimapConfig : ICsvItem
	{
		public static readonly string _KEY_MapName = "MapName";
		public static readonly string _KEY_MinimapName = "MinimapName";
		public static readonly string _KEY_CenterX = "CenterX";
		public static readonly string _KEY_CenterY = "CenterY";
		public static readonly string _KEY_Size = "Size";

		public string MapName { get; private set; }				// 地图名字
		public string MinimapName { get; private set; }				// Minimap的名字
		public float CenterX { get; private set; }				// Minimap中心点X
		public float CenterY { get; private set; }				// Minimap中心点Y
		public float Size { get; private set; }				// Minimap的投影尺寸

		public MinimapConfig()
		{
		}

		public bool ReadItem(CsvFile tf)
		{
			MapName = tf.Get<string>(_KEY_MapName);
			MinimapName = tf.Get<string>(_KEY_MinimapName);
			CenterX = tf.Get<float>(_KEY_CenterX);
			CenterY = tf.Get<float>(_KEY_CenterY);
			Size = tf.Get<float>(_KEY_Size);
			return true;
		}
	}

}

