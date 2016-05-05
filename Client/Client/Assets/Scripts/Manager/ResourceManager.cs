
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IResourceManager
{
    IEnumerator Init();
}

public class ResourceManager : MonoBehaviour
{
    static ResourceManager mInstance;
    ConfigManager mConfigManager;
    AudioManager mAudioManager;
    Dictionary<string, Texture2D> mMinimapDict = new Dictionary<string, Texture2D>();
    Dictionary<string, GameObject> mUIPrefabDict = new Dictionary<string, GameObject>();

    public static ResourceManager Instance { get { return mInstance; } }
    public ConfigManager ConfigManager { get { return mConfigManager; } }
    public AudioManager AudioManager { get { return mAudioManager; } }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mInstance = this;

        mConfigManager = new ConfigManager();
        mAudioManager = new AudioManager();
    }

    public IEnumerator PreLoadResource(UIController controller)
    {
        yield return StartCoroutine(controller.StartUI.UpdateDesc("开始加载配置文件"));
        yield return StartCoroutine(mConfigManager.Init());

        yield return StartCoroutine(controller.StartUI.UpdateDesc("开始加载音乐文件"));
        yield return StartCoroutine(mAudioManager.Init());
    }

    public Texture2D GetMinimap(string scenename, string minimapname)
    {
        if (mMinimapDict.ContainsKey(scenename))
            return mMinimapDict[scenename];

        Texture2D texture = Resources.Load(GameDefine.MinimapPathPrefix + minimapname, typeof(Texture2D)) as Texture2D;
        mMinimapDict[scenename] = texture;
        return texture;
    }

    public GameObject GetUIPrefab(string name)
    {
        if (mUIPrefabDict.ContainsKey(name))
        {
            return mUIPrefabDict[name];
        }

        GameObject prefab = Resources.Load(name, typeof(GameObject)) as GameObject;
        mUIPrefabDict.Add(name, prefab);
        return prefab;
    }
}
