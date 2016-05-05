
using UnityEngine;
using System.Collections;

public class ShowFps : MonoBehaviour
{
    const float IntervalTime = 1.0f;
    float mTime = 0.0f;
    float mAccum = 0.0f;
    int mFrame = 0;
    float mFps = 0.0f;

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 100, Screen.height - 100, 100, 100), string.Format("{0:F2} FPS", mFps));
    }

    void Update()
    {
        mTime += Time.deltaTime;
        mAccum += Time.timeScale / Time.deltaTime;
        mFrame++;

        if (mTime >= IntervalTime)
        {
            mFps = mAccum / mFrame;
            mTime = 0.0f;
            mAccum = 0.0f;
            mFrame = 0;
        }
    }
}
