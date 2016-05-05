
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class GenerateMinimapEd : EditorWindow 
{
    [MenuItem("SpraySoldier/GenerateMiniMap")]

    static void Init()
    {
        GenerateMinimapEd window = (GenerateMinimapEd)EditorWindow.GetWindow(typeof(GenerateMinimapEd));
        window.Show();
    }

    GameObject mCamPrefab;
    int mMinimapSize = 1024;

    void OnGUI()
    {
        mCamPrefab = EditorGUILayout.ObjectField("MiniMap摄像机", mCamPrefab, typeof(GameObject)) as GameObject;

        if (GUILayout.Button("生成Minimap"))
        {      
            if (mCamPrefab == null)
                return;

            Camera mCamera = mCamPrefab.GetComponent<Camera>();
            if (mCamera == null)
                return;

            GameObject obj = Object.Instantiate(mCamPrefab) as GameObject;

            GenerateMinimap script = obj.GetComponent<GenerateMinimap>();
            if (script == null)
                script = obj.AddComponent<GenerateMinimap>();

            script.mMinimapSize = mMinimapSize;
        }
    }
}
