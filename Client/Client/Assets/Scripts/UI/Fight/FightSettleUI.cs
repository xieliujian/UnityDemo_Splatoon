
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightSettleUI : MonoBehaviour
{
    public class ProgressNum
    {
        const int Count = 3;

        Transform mParent;
        Image [] mNum = new Image[Count];

        public void Init(Transform parent)
        {
            mParent = parent;

            for (int i = 0; i < Count; i++)
            {
                string name = "num" + (i + 1).ToString();
                mNum[i] = mParent.Find(name).GetComponent<Image>();
            }
        }

        public void SetValue(float _value)
        {
            string value = string.Format("{0:F3}", ((int)(_value * 1000) / 1000.0f));

            for (int i = 0; i < Count; i++)
            {
                string number = value.Substring(value.Length - (i + 1), 1);
                mNum[Count -1 - i].sprite = Resources.Load(GameDefine.FightClockNumPrefix + number, typeof(Sprite)) as Sprite;
            }
        }
    }

    GameObject mWinGo;
    GameObject mLostGo;
    GameObject mAvatarGo;
    Image mPgPositiveImg;
    Image mPgOppositeImg;
    Image mPgAnimImg;
    ProgressNum mPgPositiveNum;
    ProgressNum mPgOppositeNum;
    float mPositiveVal = 0.7f;
    float mOppositeVal = 0.3f;

    void Awake()
    {
        GameLogic.Instance.UICtrl.SettleUI = this;

        mWinGo = transform.Find("win").gameObject;
        mLostGo = transform.Find("lost").gameObject;
        mPgPositiveImg = transform.Find("progress/positive").GetComponent<Image>();
        mPgOppositeImg = transform.Find("progress/opposite").GetComponent<Image>();
        mPgAnimImg = transform.Find("progress/animation").GetComponent<Image>();
        mPgPositiveNum = new ProgressNum();
        mPgOppositeNum = new ProgressNum();
        mPgPositiveNum.Init(transform.Find("progress/positivenum"));
        mPgOppositeNum.Init(transform.Find("progress/oppositenum"));

        mAvatarGo = transform.Find("avatar").gameObject;

        mWinGo.SetActive(false);
        mLostGo.SetActive(false);
        mPgAnimImg.gameObject.SetActive(false);
        mAvatarGo.SetActive(false);
    }
    
    void Update()
    {
        // 1，预先模拟
        PreSimulate();

        // 2, 计算实际值
        CalcRealProgressValue();

        // 3, 显示结果，倒计时退出
        ExitSettle();

        // 显示数字
        mPgPositiveNum.SetValue(mPgPositiveImg.fillAmount);
        mPgOppositeNum.SetValue(mPgOppositeImg.fillAmount);
    }

    float step1 = 0.0f;
    float step2 = 0.0f;
    float step3 = 0.0f;
    float step4 = 0.0f;
    bool preSimulate = false;
    void PreSimulate()
    {
        if (preSimulate)
            return;

        if (step1 <= 1.0f)
        {
            step1 = Mathf.Clamp01(step1 += Time.deltaTime);
            mPgPositiveImg.fillAmount = Mathf.Lerp(0.0f, 0.5f, step1);
            mPgOppositeImg.fillAmount = Mathf.Lerp(0.0f, 0.5f, step1);
        }

        if (step2 <= 1.0f && step1 >= 1.0f)
        {
            step2 = Mathf.Clamp01(step2 += Time.deltaTime);
            mPgPositiveImg.fillAmount = Mathf.Lerp(0.5f, 0.4f, step2);
            mPgOppositeImg.fillAmount = Mathf.Lerp(0.5f, 0.6f, step2);
        }

        if (step3 <= 1.0f && step2 >= 1.0f)
        {
            step3 = Mathf.Clamp01(step3 += Time.deltaTime);
            mPgPositiveImg.fillAmount = Mathf.Lerp(0.4f, 0.6f, step3);
            mPgOppositeImg.fillAmount = Mathf.Lerp(0.6f, 0.4f, step3);
        }

        if (step4 <= 1.0f && step3 >= 1.0f)
        {
            step4 = Mathf.Clamp01(step4 += Time.deltaTime);
            mPgPositiveImg.fillAmount = Mathf.Lerp(0.6f, 0.5f, step4);
            mPgOppositeImg.fillAmount = Mathf.Lerp(0.4f, 0.5f, step4);
        }

        if (step4 >= 1.0f)
            preSimulate = true;
    }

    float realstep = 0.0f;
    bool calcrealpgvalue = false;
    void CalcRealProgressValue()
    {
        if (!preSimulate)
            return;

        if (calcrealpgvalue)
            return;

        realstep = Mathf.Clamp01(realstep += (Time.deltaTime * 0.5f) );
        mPgPositiveImg.fillAmount = Mathf.Lerp(0.5f, mPositiveVal, realstep);
        mPgOppositeImg.fillAmount = Mathf.Lerp(0.5f, mOppositeVal, realstep);

        if (realstep >= 1.0f)
        {
            calcrealpgvalue = true;
            ShowResult();
        }
    }

    void ShowResult()
    {
        mWinGo.SetActive(true);
        mPgAnimImg.gameObject.SetActive(true);
    }

    float exitsettleuitime = 0.0f;
    bool exitsettleui = false;
    void ExitSettle()
    {
        if (!preSimulate)
            return;

        if (!calcrealpgvalue)
            return;

        if (exitsettleui)
            return;

        exitsettleuitime += Time.deltaTime;

        if (exitsettleuitime >= 3.0f)
        {
            exitsettleui = true;
            GameLogic.Instance.SceneCtrl.LoadScene(GameDefine.eSceneType.eWeaponLoadingScene);
        }
    }
}
