using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xEnumDefine//枚举定义类
{
    public enum TeamFlag
    {
        Invalid = -1,
        Team_0 = 0,
        Team_1 = 1,
    }
}

public class CustomColorData_0
{
    public static Color Color_Team_0 = new Color(42 / 255.0f, 212 / 255.0f, 211 / 255.0f);
    public static Color Color_Team_1 = new Color(233 / 255.0f, 106 / 255.0f, 173 / 255.0f);
}

public class CameraRender : MonoBehaviour
{
    public static CameraRender Instance;

    public class HitObj
    {
        public GameObject obj;
        public Vector2 texcoord;
        public Color color;
        public int coltype;
        public string decalname = "Gun_H100";
        public float decalwidth = 1.0f;
        public float decalheight = 1.0f;
        public float decalrot = 0.0f;
    };

    public List<HitObj> M_StaticObjArray
    {
        set
        {
            mStaticObjArray = value;
        }

        get
        {
            return mStaticObjArray;
        }
    }
    private List<HitObj> mStaticObjArray = new List<HitObj>();

    private Material mQuadMaterial;

    //private Material mHostQuadMat;
    //private Material mGuestQuadMat;

    public bool M_OnceRender
    {
        get
        {
            return mOnceRender;
        }
        set
        {
            mOnceRender = value;
        }
    }

    private bool mOnceRender = false;
    public  bool M_OpenOnceRender
    {
        get
        {
            return mOpenOnceRender;
        }
        set
        {
            mOpenOnceRender=value;
        }
    }
    private bool mOpenOnceRender = false;

    //public Color mQuadColor;

    //public int mQuadColType;

	// Use this for initialization
	void Start ()
    {
#if UNITY_ANDROID
        if (Application.isEditor)
            mQuadMaterial = new Material(Shader.Find("SpraySoldier/Function/DrawDecal"));
        else
            mQuadMaterial = new Material(Shader.Find("SpraySoldier/Function/MobileDrawDecal"));
#else
        mQuadMaterial = new Material(Shader.Find("SpraySoldier/Function/DrawDecal"));
#endif

        //mQuadColor = Color.blue;

        //mHostQuadMat = new Material(Shader.Find("Custom/DrawQuad"));
        //mGuestQuadMat = new Material(Shader.Find("Custom/DrawQuad"));

        Instance = this;
    }

    //public void SetColor(Color c )
    //{
    //    mQuadColor = c;
    //}

    //public void SetColType(int type)
    //{
    //    mQuadColType = type;
    //}

	// Update is called once per frame
	void Update ()
    {
        //if (mOpenOnceRender)
        //{
        //    if (mOnceRender)
        //        return;
        //}
        //else 
        //    mStaticObjArray.Clear();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (Camera.main != null)
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;

        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            HitObj obj = new HitObj();
        //            obj.obj = hit.collider ? hit.collider.gameObject : null;
        //            obj.texcoord = hit.textureCoord1;

        //            mStaticObjArray.Add(obj);

        //            if (mOpenOnceRender)
        //            {
        //                StaticObj_Render render = obj.obj.GetComponent<StaticObj_Render>();
        //                if (render != null)
        //                    mOnceRender = true;
        //                else
        //                    mStaticObjArray.Clear();
        //            }
        //        }
        //    }
        //}

        //if (!mOnceRender)
        //if (ManagerResolver.Resolve<ResourceManager>() != null)
        //{
        //    Texture texture = ManagerResolver.Resolve<ResourceManager>().GetDecalTex("Gun_H100");
        //    mHostQuadMat.SetTexture("_MainTex", texture);
        //    mHostQuadMat.SetColor("_Color", Color.blue);

        //    mGuestQuadMat.SetTexture("_MainTex", texture);
        //    mGuestQuadMat.SetColor("_Color", Color.yellow);

        //    mOnceRender = true;
        //}
	}

    void OnPostRender()
    {
        for (int i = 0; i < mStaticObjArray.Count; i++ )
        {
            HitObj obj = mStaticObjArray[i];
            if (obj == null)
                continue;

            StaticObj_Render render = obj.obj.GetComponent<StaticObj_Render>();
            if (render == null)
                continue;

            //obj.decalwidth = 0.25f;
            //obj.decalheight = 0.25f;
            if (obj.coltype == (int)xEnumDefine.TeamFlag.Team_0)
                render.Render(obj.texcoord, mQuadMaterial, obj.color, obj.coltype, obj.decalname, obj.decalwidth, obj.decalheight, obj.decalrot);
            else
                render.Render(obj.texcoord, mQuadMaterial, obj.color, obj.coltype, obj.decalname, obj.decalwidth, obj.decalheight, obj.decalrot);
        }

        mStaticObjArray.Clear();
    }
}
