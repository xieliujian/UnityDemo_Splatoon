
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FightScoreUI : MonoBehaviour 
{
    Image mNum1Img;
    Image mNum2Img;
    Image mNum3Img;
    Image mNum4Img;

    void Awake()
    {
        mNum1Img = transform.Find("bg1/num").GetComponent<Image>();
        mNum2Img = transform.Find("bg2/num").GetComponent<Image>();
        mNum3Img = transform.Find("bg3/num").GetComponent<Image>();
        mNum4Img = transform.Find("bg4/num").GetComponent<Image>();
    }
}
