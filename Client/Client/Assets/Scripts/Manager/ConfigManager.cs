
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Splatoon;

public class ConfigManager : IResourceManager
{
    GunConfigMgr mGun = new GunConfigMgr();
    LoadingDescConfigMgr mLoadingDesc = new LoadingDescConfigMgr();
    MinimapConfigMgr mMinimap = new MinimapConfigMgr();

    public GunConfigMgr Gun { get { return mGun; } }
    public LoadingDescConfigMgr LoadingDesc { get { return mLoadingDesc; } }
    public MinimapConfigMgr Minimap { get { return mMinimap; } }

    public IEnumerator Init()
    {
        TextAsset asset = (TextAsset)Resources.Load(CsvDefine.GunCfgPath, typeof(TextAsset));
        mGun.Init(asset);

        asset = (TextAsset)Resources.Load(CsvDefine.LoadingDescCfgPath, typeof(TextAsset));
        mLoadingDesc.Init(asset);

        asset = (TextAsset)Resources.Load(CsvDefine.MinimapCfgPath, typeof(TextAsset));
        mMinimap.Init(asset);

        yield return null;
    }
}


