
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightClockUI : MonoBehaviour 
{
    const int NumCount = 10;

    Image mMinImg;
    Image mSec1Img;
    Image mSec2Img;
    Sprite[] mNumSprList = new Sprite[NumCount];

#if DEBUG
    int mCurMin = 3;
    int mCurSec1 = 0;
#else
    int mCurMin = 0;
    int mCurSec1 = 1;
#endif
    int mCurSec2 = 0;
    float mTime = 0.0f;

    void Awake()
    {
        mMinImg = transform.Find("nummin").GetComponent<Image>();
        mSec1Img = transform.Find("numsec1").GetComponent<Image>();
        mSec2Img = transform.Find("numsec2").GetComponent<Image>();

        for (int i = 0; i < NumCount; i++)
        {
            string path = GameDefine.FightClockNumPrefix + i.ToString();
            mNumSprList[i] = Resources.Load(path, typeof(Sprite)) as Sprite;
        }

        SetTime(mCurMin, mCurSec1, mCurSec2);
    }

    void SetTime(int min, int sec1, int sec2)
    {
        if (min < 0 || min >= 10)
            return;

        if (sec1 < 0 || sec1 >= 10)
            return;

        if (sec2 < 0 || sec2 >= 10)
            return;

        mMinImg.sprite = mNumSprList[min];
        mSec1Img.sprite = mNumSprList[sec1];
        mSec2Img.sprite = mNumSprList[sec2];

        if (min <= 0 && sec1 <= 0 && sec2 <= 0)
        {
            transform.parent.gameObject.SetActive(false);
            GameLogic.Instance.UICtrl.JoystickUI.gameObject.SetActive(false);
            GameLogic.Instance.UICtrl.SettleUI.gameObject.SetActive(true);
        }
    }

    public void Tick(float deltatime)
    {
        mTime += deltatime;
        if (mTime >= 1.0f)
        {
            mCurSec2--;
            if (mCurSec2 < 0)
            {
                mCurSec2 = 9;
                mCurSec1--;
                if (mCurSec1 < 0)
                {
                    mCurSec2 = 9;
                    mCurSec1 = 5;
                    mCurMin--;
                    if (mCurMin < 0)
                    {
                        mCurSec2 = 0;
                        mCurSec1 = 0;
                        mCurMin = 0;
                    }
                }
            }

            mTime = 0.0f;
        }

        SetTime(mCurMin, mCurSec1, mCurSec2);
    }
}
