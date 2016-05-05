using UnityEngine;
using System.Collections;

public class SceneManager 
{
    PlayerManager mPlayerManager;

    PlayerManager PlayerManager { get { return mPlayerManager; } }

    public void Init()
    {
        mPlayerManager = new PlayerManager();
        mPlayerManager.Init();
    }

    public void Tick(float deltaTime)
    {
        mPlayerManager.Tick(deltaTime);
    }
}
