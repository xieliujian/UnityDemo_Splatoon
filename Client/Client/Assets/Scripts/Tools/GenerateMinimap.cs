using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GenerateMinimap : MonoBehaviour 
{
    public int mMinimapSize;

    void Update()
    {
        Camera camera = GetComponent<Camera>();
        if (camera == null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        RenderTexture rt = new RenderTexture(mMinimapSize, mMinimapSize, 24);
        camera.targetTexture = rt;
        camera.Render();

        RenderTexture.active = rt;
        Texture2D texture = new Texture2D(mMinimapSize, mMinimapSize, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, mMinimapSize, mMinimapSize), 0, 0);

        string name = gameObject.name.Remove(gameObject.name.LastIndexOf("(Clone)"));
        byte[] bytes = texture.EncodeToPNG();
        string filename = Application.dataPath + "/Resources/UI/Minimap/SnapShots/" + name + ".png";
        System.IO.File.WriteAllBytes(filename, bytes);

        RenderTexture.active = null;
        DestroyImmediate(rt);
        DestroyImmediate(texture);
        DestroyImmediate(gameObject);
    }
}
