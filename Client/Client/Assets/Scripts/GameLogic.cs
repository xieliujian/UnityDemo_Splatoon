using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour 
{
    static GameLogic mInstance;
    UIController mUICtrl;
    SceneController mSceneCtrl;
    SceneManager mSceneManager;

    public static GameLogic Instance { get { return mInstance; } }
    public UIController UICtrl { get { return mUICtrl; } }
    public SceneController SceneCtrl { get { return mSceneCtrl; } }
    public SceneManager SceneManager { get { return mSceneManager; } }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mInstance = this;
        mUICtrl = new UIController();
        mSceneCtrl = new SceneController();
        mSceneManager = new SceneManager();

        mSceneManager.Init();
    }

    void Start()
    {
        StartCoroutine(StartGame());
    }
    
    // Update is called once per frame
	void Update ()
    {
        float deltaTime = Time.deltaTime;
        mUICtrl.Tick(deltaTime);
        mSceneCtrl.Tick(deltaTime);
        mSceneManager.Tick(deltaTime);
	}

    IEnumerator StartGame()
    {
        yield return StartCoroutine(ResourceManager.Instance.PreLoadResource(mUICtrl));
        mSceneCtrl.LoadFirstScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
