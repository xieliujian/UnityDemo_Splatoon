
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Splatoon
{
    public abstract class CfgListMgrTemplate<TItem> : IConfigManager
        where TItem : ICsvItem, new()
    {
        protected List<TItem> mItemList = new List<TItem>();

        public bool Init(TextAsset asset)
        {
            if (asset == null)
            {
                LogSystem.Write(LogLevel.Error, "[error] failed to init csvmanager : {0}, text is null", ToString());
                return false;
            }

            if (mItemList != null && mItemList.Count > 0)
            {
                LogSystem.Write(LogLevel.Error, "[error] failed to init csvmanager : {0}, already inited", ToString());
                return false;
            }

            mItemList.Clear();

            CsvFile file = new CsvFile(asset.name, asset.text);
            while (file.Next())
            {
                TItem item = new TItem();
                if (item.ReadItem(file) == false)
                {
                    LogSystem.Write(LogLevel.Error, "[error] failed to init csvmanager : {0}, read line error, line : {1}", ToString(), file.CurrentLine);
                    continue;
                }

                mItemList.Add(item);
            }

            return true;
        }

        public int Count { get {return mItemList.Count; }}

        public TItem Value(int index)
        {
             return mItemList[index];
        }
    }
}
