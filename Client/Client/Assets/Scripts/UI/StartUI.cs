
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartUI : MonoBehaviour 
{
    Text mDescText;

    void Awake()
    {
        mDescText = transform.Find("desc").GetComponent<Text>();
        GameLogic.Instance.UICtrl.StartUI = this;
    }

    public IEnumerator UpdateDesc(string desc)
    {
        mDescText.text = desc;
        yield return null;
    }
}
