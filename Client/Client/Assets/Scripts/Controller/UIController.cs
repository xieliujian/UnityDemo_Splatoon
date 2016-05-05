
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : IController
{
    StartUI mStartUI;
    JoystickUI mJoystickUI;
    EscUI mEscUI;
    FightSettleUI mSettleUI;

    public StartUI StartUI { get { return mStartUI; } set { mStartUI = value; } }
    public JoystickUI JoystickUI { get { return mJoystickUI; } set { mJoystickUI = value; } }
    public EscUI EscUI { get { return mEscUI; } set { mEscUI = value; } }
    public FightSettleUI SettleUI { get { return mSettleUI; } set { mSettleUI = value; } }

    public void Tick(float deltaTime)
    {
    }

    public void LoadPrefab(GameDefine.eSceneType type)
    {
        Transform canvas = GameObject.Find("Canvas").transform;
        
        if (type == GameDefine.eSceneType.eWeaponLoadingScene)
        {
            GameObject prefab = ResourceManager.Instance.GetUIPrefab(GameDefine.WeaponToggleUIPath);
            GameObject ui = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            ui.transform.SetParent(canvas, false);

            prefab = Resources.Load(GameDefine.EscUIPath) as GameObject;
            ui = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            ui.transform.SetParent(canvas, false);
            ui.SetActive(false);
        }
        else if (type == GameDefine.eSceneType.eFightScene)
        {
            GameObject prefab = ResourceManager.Instance.GetUIPrefab(GameDefine.FightUIPath);
            GameObject ui = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            ui.transform.SetParent(canvas, false);

            prefab = ResourceManager.Instance.GetUIPrefab(GameDefine.JoystickUIPath);
            ui = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            ui.transform.SetParent(canvas, false);

            prefab = ResourceManager.Instance.GetUIPrefab(GameDefine.EscUIPath);
            ui = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            ui.transform.SetParent(canvas, false);
            ui.gameObject.SetActive(false);

            prefab = ResourceManager.Instance.GetUIPrefab(GameDefine.FightSettleUIPath);
            ui = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            ui.transform.SetParent(canvas, false);
            ui.gameObject.SetActive(false);
        }
    }
}

