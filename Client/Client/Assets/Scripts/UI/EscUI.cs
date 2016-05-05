
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Splatoon;

public class EscUI : MonoBehaviour 
{
    Button mConfirmBtn;
    Button mCancelBtn;

    void Awake()
    {
        mConfirmBtn = transform.Find("confirm").GetComponent<Button>();
        mCancelBtn = transform.Find("cancel").GetComponent<Button>();

        UIEventListener.Get(mConfirmBtn.gameObject).onClick += OnConfirmBtnClick;
        UIEventListener.Get(mCancelBtn.gameObject).onClick += OnCancelBtnClick;

        GameLogic.Instance.UICtrl.EscUI = this;
    }

    void OnDestroy()
    {
        GameLogic.Instance.UICtrl.EscUI = null;
    }

    void OnConfirmBtnClick(GameObject go)
    {
        GameLogic.Instance.QuitGame();
    }
    
    void OnCancelBtnClick(GameObject go)
    {
        gameObject.SetActive(false);
    }
}
