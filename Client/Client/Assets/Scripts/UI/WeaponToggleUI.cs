
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Splatoon;

public class WeaponToggleUI : MonoBehaviour 
{
    public interface Item
    {
        void Selected();
    }

    public class EquipItem : Item
    {
        Toggle mToggle;
        int mIndex;

        public delegate void EquipItemClickDel(int index);
        public EquipItemClickDel mEquipItemClick;

        public void Init(Toggle toggle, int index)
        {
            mToggle = toggle;
            mIndex = index;

            UIEventListener.Get(mToggle.gameObject).onClick += OnClick;
        }

        public void Selected()
        {
            if (!mToggle.interactable)
                return;

            mToggle.isOn = true;
            OnClick(mToggle.gameObject);
        }

        void OnClick(GameObject go)
        {
            if (mEquipItemClick != null)
                mEquipItemClick(mIndex);
        }
    }

    public class AvatarItem : Item
    {
        Toggle mToggle;
        int mIndex;
        public delegate void AvatarItemClickDel(int index);
        public AvatarItemClickDel mAvatarItemClick;

        public void Init(Toggle toggle, int index)
        {
            mToggle = toggle;
            mIndex = index;

            UIEventListener.Get(mToggle.gameObject).onClick += OnClick;
        }

        public void Selected()
        {
            OnClick(mToggle.gameObject);
        }

        void OnClick(GameObject go)
        {
            if (!mToggle.interactable)
                return;

            mToggle.isOn = true;

            if (mAvatarItemClick != null)
                mAvatarItemClick(mIndex);
        }
    }

    public class WeaponItem : Item
    {
        GunConfig mConfig;
        public GameObject mGo;

        public delegate void delWeaponClick(GunConfig cfg);
        public delWeaponClick mWeaponClickEvent;

        public void Init(GridLayoutGroup grid, ToggleGroup group, GunConfig config)
        {
            GameObject prefab = (GameObject)Resources.Load(GameDefine.WeaponTypeTemplatePath);
            GameObject weapon = (GameObject)Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
            weapon.transform.SetParent(grid.transform);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            weapon.GetComponent<Toggle>().group = group;
            weapon.transform.Find("icon").GetComponent<Image>().sprite =
                (Sprite)Resources.Load(GameDefine.GunIconPathPrefix + config.Icon, typeof(Sprite));

            UIEventListener.Get(weapon).onClick += OnClick;

            mConfig = config;
            mGo = weapon;
        }

        public void Selected()
        {
            mGo.GetComponent<Toggle>().isOn = true;
            OnClick(mGo.gameObject);
        }

        void OnClick(GameObject go)
        {
            if (mWeaponClickEvent != null)
                mWeaponClickEvent(mConfig);
        }
    }

    public enum eAvatarType
    {
        Lady,
        Girl
    }

    public enum eEquipType
    {
        Weapon,
        Helmet,
        Armor,
        Shoes
    }

    RawImage mAvatarImg;
    Transform mAvatarSpawnPos;
    GridLayoutGroup mWeaponGrid;
    ToggleGroup mWeaponTogGroup;
    Button mEnterBtn;
    Button mReturnBtn;

    Dictionary<int, GameObject> mAvatarDict = new Dictionary<int, GameObject>();
    List<AvatarItem> mAvatarList = new List<AvatarItem>();
    List<EquipItem> mEquipList = new List<EquipItem>();
    List<WeaponItem> mWeaponList = new List<WeaponItem>();
    GameObject mCurAvatar;
    int mCurAvatarType;

    void Awake()
    {
        mAvatarImg = transform.Find("avatar").GetComponent<RawImage>();
        mAvatarSpawnPos = GameObject.FindGameObjectWithTag(Tags.AvatarCamTag).transform.Find("SpawnPos");
        mWeaponTogGroup = transform.Find("weapon").GetComponent<ToggleGroup>();
        mWeaponGrid = transform.Find("weapon/scroll/gridlayout").GetComponent<GridLayoutGroup>();
        mEnterBtn = transform.Find("enter").GetComponent<Button>();
        mReturnBtn = transform.Find("return").GetComponent<Button>();
        UIEventListener.Get(mAvatarImg.gameObject).onDrag += OnDragAvatarImage;
        UIEventListener.Get(mEnterBtn.gameObject).onClick += OnEnterBtnClick;
        UIEventListener.Get(mReturnBtn.gameObject).onClick += OnReturnBtnClick;

        ToggleGroup group = transform.Find("herolist").GetComponent<ToggleGroup>();
        Toggle[] toglist = group.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toglist.Length; i++)
        {
            AvatarItem item = new AvatarItem();
            item.Init(toglist[i], i);
            item.mAvatarItemClick += OnRoleToggleClick;
            mAvatarList.Add(item);
        }

        group = transform.Find("equipment").GetComponent<ToggleGroup>();
        toglist = group.GetComponentsInChildren<Toggle>();
        for (int i = 0; i < toglist.Length; i++)
        {
            EquipItem item = new EquipItem();
            item.Init(toglist[i], i);
            item.mEquipItemClick += OnEquipToggleClick;
            mEquipList.Add(item);
        }

        if (mAvatarList.Count > 0)
            mAvatarList[0].Selected();
    }
    
    void OnDragAvatarImage(GameObject go, PointerEventData data)
    {
        if (mCurAvatar != null)
            mCurAvatar.transform.Rotate(Vector3.up, -data.delta.x);
    }

    void OnEnterBtnClick(GameObject go)
    {
        GameLogic.Instance.SceneCtrl.LoadScene(GameDefine.eSceneType.eFightScene);
    }

    void OnReturnBtnClick(GameObject go)
    {
        GameLogic.Instance.UICtrl.EscUI.gameObject.SetActive(true);
    }

    void OnRoleToggleClick(int index)
    {
        SelectAvatar((eAvatarType)index);
    }

    void OnEquipToggleClick(int index)
    {
        SelectEquip((eEquipType)index);
    }

    void OnWeaponToggleClick(GunConfig config)
    {
        if (mCurAvatar == null)
            return;

        Transform weaponPoint = CommonFunction.FindObject(mCurAvatar.transform, "Weapon");
        if (weaponPoint == null)
            return;

        foreach (Transform childs in weaponPoint)
        {
            Destroy(childs.gameObject);
        }

        GameObject prefab = Resources.Load(GameDefine.GunPrefabPrefix + config.GunPrefab) as GameObject;
        GameObject gun = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
        gun.transform.parent = weaponPoint;
        if (config.GunId == 1000)
            gun.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        else
            gun.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gun.transform.localPosition = Vector3.zero;
        gun.transform.localRotation = Quaternion.identity;
    }

    void SelectAvatar(eAvatarType avatartype)
    {
        UnSelectAvatar();

        if (avatartype == eAvatarType.Girl)
            SelectGirl();
        else if (avatartype == eAvatarType.Lady)
            SelectLady();

        if (mEquipList.Count > 0)
            mEquipList[0].Selected();
    }

    void SelectEquip(eEquipType type)
    {
        if (type == eEquipType.Weapon)
            SelectWeapon();
    }

    void UnSelectAvatar()
    {
        foreach ( KeyValuePair<int, GameObject> key in mAvatarDict )
        {
            key.Value.SetActive(false);
        }
    }

    void SelectLady()
    {
        GameObject lady = null;
        if (!mAvatarDict.ContainsKey((int)eAvatarType.Lady))
        {
            GameObject instance = (GameObject)Resources.Load(GameDefine.LadyPrefabPath);
            lady = (GameObject)Object.Instantiate(instance, Vector3.zero, Quaternion.identity);
            mAvatarDict[(int)eAvatarType.Lady] = lady;
        }
        else
        {
            lady = mAvatarDict[(int)eAvatarType.Lady];
        }

        lady.transform.parent = mAvatarSpawnPos;
        lady.transform.localPosition = Vector3.zero;
        lady.transform.localRotation = Quaternion.identity;
        mCurAvatar = lady;
        mCurAvatar.SetActive(true);
        mCurAvatarType = (int)eAvatarType.Lady;
    }

    void SelectGirl()
    {
        GameObject girl = null;
        if (!mAvatarDict.ContainsKey((int)eAvatarType.Girl))
        {
            GameObject instance = (GameObject)Resources.Load(GameDefine.GirlPrefabPath);
            girl = (GameObject)Object.Instantiate(instance, Vector3.zero, Quaternion.identity);
            mAvatarDict[(int)eAvatarType.Girl] = girl;
        }
        else
        {
            girl = mAvatarDict[(int)eAvatarType.Girl];
        }

        girl.transform.parent = mAvatarSpawnPos;
        girl.transform.localPosition = Vector3.zero;
        girl.transform.localRotation = Quaternion.identity;
        mCurAvatar = girl;
        mCurAvatar.SetActive(true);
        mCurAvatarType = (int)eAvatarType.Girl;
    }

    void SelectWeapon()
    {
        GunConfigMgr gunmgr = ResourceManager.Instance.ConfigManager.Gun;
        if (gunmgr == null)
            return;

        for (int i = 0; i < mWeaponList.Count; i++ )
        {
            Destroy(mWeaponList[i].mGo);
        }
        mWeaponList.Clear();

        for (int i = 0; i < gunmgr.Count; i++)
        {
            GunConfig config = (GunConfig)gunmgr.Value(i);
            if (config == null)
                continue;

            if (mCurAvatarType == config.UserType)
            {
                WeaponItem item = new WeaponItem();
                item.Init(mWeaponGrid, mWeaponTogGroup, config);
                item.mWeaponClickEvent += OnWeaponToggleClick;
                mWeaponList.Add(item);
            }
        }

        if (mWeaponList.Count > 0)
            mWeaponList[0].Selected();
    }
}
