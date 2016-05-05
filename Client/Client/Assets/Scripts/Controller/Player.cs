using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    Animator mAnimator;
    CharacterController mCharCtrl;
    SkinnedMeshRenderer[] mSkinMeshs;
    MeshRenderer mWeaponMesh;
    Camera mCamera;
    Transform mTemp;

    public Animator Anim { get { return mAnimator; } }

    void Awake()
    {
        mAnimator = GetComponentInChildren<Animator>();
        mCharCtrl = GetComponent<CharacterController>();
        mSkinMeshs = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        mWeaponMesh = GameObject.FindGameObjectWithTag(Tags.WeaponTag).GetComponent<MeshRenderer>();
        mCamera = Camera.main;

        GameObject go = new GameObject("TempJoystick");
        mTemp = go.transform;
    }

    void Update()
    {
        MoveProcess();

        RaycastHit hit;
        if (Physics.Linecast(mCamera.transform.position, transform.position, out hit))
        {
            GameObject obj = hit.collider.gameObject;
            foreach (SkinnedMeshRenderer render in mSkinMeshs)
            {
                Material mat = render.material;
                if (obj.tag == Tags.PlayerTag)
                    mat.shader = Shader.Find("Unlit/Texture");
                else
                    mat.shader = Shader.Find("SpraySoldier/Function/CompoundBlocked");
            }

            Material mat1 = mWeaponMesh.material;
            if (obj.tag == Tags.PlayerTag)
                mat1.shader = Shader.Find("Unlit/Texture");
            else
                mat1.shader = Shader.Find("SpraySoldier/Function/CompoundBlocked");
        }
    }

    void MoveProcess()
    {
        if (GameLogic.Instance.UICtrl.JoystickUI == null)
            return;

        Vector3 dir = Vector3.zero;
        bool drag = GameLogic.Instance.UICtrl.JoystickUI.IsDrag();
        if (drag)
            dir = GameLogic.Instance.UICtrl.JoystickUI.GetDir();
        else
            dir = KeyProcess();

        KeyButtonProcess();

        if (dir == Vector3.zero)
            mAnimator.SetBool("Speed", false);
        else
        {
            Vector3 realdir = new Vector3(dir.x, 0.0f, dir.y);
            mTemp.eulerAngles = new Vector3(0.0f, mCamera.transform.eulerAngles.y, 0.0f);
            realdir = mTemp.TransformDirection(realdir);

            float angle = Vector3.Angle(transform.forward, realdir);
            realdir = Vector3.Slerp(transform.forward, realdir, Mathf.Clamp01(180 * Time.deltaTime * 5 / angle));
            transform.LookAt(transform.position + realdir);

            mCharCtrl.SimpleMove(realdir * 3);
            mAnimator.SetBool("Speed", true);
        }
    }

    Vector3 KeyProcess()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            dir += Vector3.up;

        if (Input.GetKey(KeyCode.S))
            dir += -Vector3.up;

        if (Input.GetKey(KeyCode.A))
            dir += -Vector3.right;

        if (Input.GetKey(KeyCode.D))
            dir += Vector3.right;

        dir.Normalize();
        GameLogic.Instance.UICtrl.JoystickUI.UpdateJoystick(dir);
        return dir;
    }

    void KeyButtonProcess()
    {
        if (Input.GetKey(KeyCode.J))
        {
            mAnimator.SetBool("Shoot", true);
            GameLogic.Instance.UICtrl.JoystickUI.ShootBtn.Press();
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            mAnimator.SetBool("Shoot", false);
            GameLogic.Instance.UICtrl.JoystickUI.ShootBtn.Normal();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject escgo = GameLogic.Instance.UICtrl.EscUI.gameObject;
            if (escgo.activeSelf)
                escgo.SetActive(false);
            else
                escgo.SetActive(true);
        }
    }
}
