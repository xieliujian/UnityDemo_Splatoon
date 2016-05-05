
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Splatoon;

public class LoadingSceneUI : MonoBehaviour 
{
    Image mFillPer;
    Text mDescTex;
    
    void Awake()
    {
        mFillPer = transform.Find("bg/fillbg/fill").GetComponent<Image>();
        mDescTex = transform.Find("bg/desc").GetComponent<Text>();
    }

    void Start()
    {
        GameLogic.Instance.SceneCtrl.LoadingSceneUI = this;

        LoadingDescConfigMgr mgr = ResourceManager.Instance.ConfigManager.LoadingDesc;
        LoadingDescConfig cfg = mgr.Value(Random.Range(0, mgr.Count));
        mDescTex.text = cfg.Desc;

        GameLogic.Instance.StartCoroutine(
            GameLogic.Instance.SceneCtrl.Load(GameLogic.Instance.SceneCtrl.CurSceneType));
    }

    public void RefreshProgressPar(float percent)
    {
        mFillPer.fillAmount = percent;
    }
}
