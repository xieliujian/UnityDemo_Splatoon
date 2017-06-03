
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PaintResourceManager : MonoBehaviour 
{
    public struct DecalInfo
    {
        public Texture texture;
        public int row;
        public int column;
    };

    private Dictionary<string, DecalInfo> mDecalArray = new Dictionary<string, DecalInfo>();

    private Dictionary<string, Texture> mDecalBumpArray = new Dictionary<string, Texture>();

    private Dictionary<string, Cubemap> mCubeArray = new Dictionary<string, Cubemap>();

    public List<StaticObj_Render> mRenderList = new List<StaticObj_Render>();

    public int mDecalWidth = 1024;
    public int mDecalHeight = 1024;

	// Use this for initialization
	void Start () 
    {
        ManagerResolver.Register<PaintResourceManager>(this);

        ReadCsv();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void ReadCsv()
    {
        DecalInfo info;
        info.texture = Resources.Load("Decal/Gun_H100") as Texture;
        info.row = 4;
        info.column = 4;
        mDecalArray["Gun_H100"] = info;

        info.texture = Resources.Load("Decal/Roller_001") as Texture;
        info.row = 3;
        info.column = 3;
        mDecalArray["Roller_001"] = info;

        mDecalBumpArray["flowera01_N"] = Resources.Load("Decal/flowera01_N") as Texture;
        mDecalBumpArray["T_Detail_Rocky_C"] = Resources.Load("Decal/T_Detail_Rocky_C") as Texture;
        mDecalBumpArray["T_Detail_Rocky_N"] = Resources.Load("Decal/T_Detail_Rocky_N") as Texture;

        mCubeArray["SkyboxCube"] = Resources.Load("Decal/Skybox") as Cubemap;
    }

    public DecalInfo GetDecalTex(string name)
    {
        return mDecalArray[name];
    }

    public Texture GetDecalBumpTex(string name)
    {
        return mDecalBumpArray[name];
    }

    public Cubemap GetCubemap(string name)
    {
        return mCubeArray[name];
    }
}
