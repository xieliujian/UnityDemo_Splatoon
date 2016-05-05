using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Splatoon
{
    public interface ICsvItem
    {
        bool ReadItem(CsvFile file);
    }

    public interface IConfigManager
    {
        bool Init(TextAsset asset);
    }
}
