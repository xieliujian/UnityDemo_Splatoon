using UnityEngine;
using System.Collections;

public class SceneController
{
    LoadingSceneUI mLoadingSceneUI;
    AsyncOperation mAsyncOp;
    GameDefine.eSceneType mCurSceneType;
    public GameDefine.eSceneType CurSceneType { get { return mCurSceneType; } }
    public LoadingSceneUI LoadingSceneUI { get { return mLoadingSceneUI; } set { mLoadingSceneUI = value; } }

    public void Tick(float deltaTime)
    {
        if (mAsyncOp != null && !mAsyncOp.isDone)
        {
            if (mLoadingSceneUI != null)
                mLoadingSceneUI.RefreshProgressPar(mAsyncOp.progress + 0.1f);
        }
    }

    public void LoadFirstScene()
    {
        LoadScene(GameDefine.eSceneType.eWeaponLoadingScene);
    }

    public void LoadScene(GameDefine.eSceneType type)
    {
        mCurSceneType = type;
        Application.LoadLevelAsync("LoadingScene");
    }

    public IEnumerator Load(GameDefine.eSceneType type)
    {
        string scenename = "";
        if (type == GameDefine.eSceneType.eWeaponLoadingScene)
            scenename = "WeaponLoading";
        else if (type == GameDefine.eSceneType.eFightScene)
            scenename = "Fighting";

        mAsyncOp = Application.LoadLevelAsync(scenename);
        mAsyncOp.allowSceneActivation = true;
        yield return mAsyncOp;

        GameLogic.Instance.UICtrl.LoadPrefab(type);
    }
}
