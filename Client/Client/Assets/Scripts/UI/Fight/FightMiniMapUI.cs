
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Splatoon;

public class FightMiniMapUI : MonoBehaviour
{
    static Vector2 RadiusLimit = new Vector2(0.1f, 0.3f);
    static float RadiusDelta = 0.05f;

    string mMinimapName;
    float mMinimapCenterX = 0.0f;
    float mMinimapCenterY = 0.0f;
    float mMinimapSize = 10.0f;
    Image mMinimapImg;
    Image mFocusImg;
    Button mPlusBtn;
    Button mMinusBtn;
    Button mExitGameBtn;
    Material mMinimapMat;
    float mRadius = 0.2f;
    Transform mPlayer;

    void Awake()
    {
        mMinimapImg = GetComponent<Image>();
        mFocusImg = transform.Find("player/focus").GetComponent<Image>();
        mPlusBtn = transform.Find("plus").GetComponent<Button>();
        mMinusBtn = transform.Find("minus").GetComponent<Button>();
        mExitGameBtn = transform.Find("exitgame").GetComponent<Button>();

        UIEventListener.Get(mPlusBtn.gameObject).onClick += OnPlusBtnClick;
        UIEventListener.Get(mMinusBtn.gameObject).onClick += OnMinusBtnClick;
        UIEventListener.Get(mExitGameBtn.gameObject).onClick += OnExitGameBtnClick;
    }

    void Start()
    {
        MinimapConfigMgr mgr = ResourceManager.Instance.ConfigManager.Minimap;
        string scenename = Application.loadedLevelName;
        for (int i = 0; i < mgr.Count; i++)
        {
            MinimapConfig config = mgr.Value(i);
            if (config == null)
                continue;

            if (config.MapName.ToLower() == scenename.ToLower())          
            {
                mMinimapName = config.MinimapName;
                mMinimapCenterX = config.CenterX;
                mMinimapCenterY = config.CenterY;
                mMinimapSize = config.Size;    
                break;
            }
        }

        mMinimapMat = new Material(Shader.Find(GameDefine.MinimapShaderName));
        mMinimapImg.material = mMinimapMat;
        mMinimapMat.SetTexture(0, ResourceManager.Instance.GetMinimap(scenename.ToLower(), mMinimapName));

        Texture2D masktex = Resources.Load("UI/Minimap/Resource/GradMask", typeof(Texture2D)) as Texture2D;
        mMinimapMat.SetTexture("_MaskTex", masktex);

        mPlayer = GameObject.FindGameObjectWithTag(Tags.PlayerTag).transform;
    }

    public void Tick(float elapsedTime)
    {
        Vector2 playerpos = new Vector2(mPlayer.position.x, mPlayer.position.z);
        Vector2 centerpos = new Vector2(mMinimapCenterX, mMinimapCenterY);
        Vector2 offset = (playerpos - centerpos) / (mMinimapSize * 2);

        //if (!Application.isEditor)
        //    offset.y = -offset.y;

        mMinimapMat.SetFloat("_TexScale", mRadius);
        mMinimapMat.SetFloat("_TexOffsetX", offset.x);
        mMinimapMat.SetFloat("_TexOffsetY", offset.y);

        mFocusImg.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -mPlayer.eulerAngles.y);
    }

    public void SetRadius(float radius)
    {
        mRadius = radius;
    }

    void OnPlusBtnClick(GameObject go)
    {
        mRadius += RadiusDelta;
        mRadius = Mathf.Clamp(mRadius, RadiusLimit.x, RadiusLimit.y);
    }

    void OnMinusBtnClick(GameObject go)
    {
        mRadius -= RadiusDelta;
        mRadius = Mathf.Clamp(mRadius, RadiusLimit.x, RadiusLimit.y);
    }

    void OnExitGameBtnClick(GameObject go)
    {
        GameLogic.Instance.SceneCtrl.LoadScene(GameDefine.eSceneType.eWeaponLoadingScene);
    }
}
