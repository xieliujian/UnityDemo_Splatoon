using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour 
{
    Camera mCamera;
    Transform mPlayer;
    Vector3 mOffset;

    void Awake()
    {
        mCamera = Camera.main;
        mPlayer = GameObject.FindGameObjectWithTag(Tags.PlayerTag).transform;
        mOffset = mCamera.transform.position - mPlayer.position;
    }

    void LateUpdate()
    {
        mCamera.transform.position = Vector3.Lerp(mCamera.transform.position, mPlayer.position + mOffset, Time.deltaTime * 3.0f);
        //mCamera.transform.LookAt(mPlayer.position);
    }
}
