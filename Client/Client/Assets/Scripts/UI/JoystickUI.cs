
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Splatoon;

public class JoystickUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    Image mTouchImg;
    SpButton mShootBtn;

    int mRadius;
    bool mDrag = false;
    Vector2 mBeginPos;

    public SpButton ShootBtn { get { return mShootBtn; } }

    void Awake()
    {
        mTouchImg = transform.Find("position/bg/touch").GetComponent<Image>();
        mShootBtn = transform.Find("shoot").GetComponent<SpButton>();
        UIEventListener.Get(mShootBtn.gameObject).onDown += OnShootBtnDown;
        UIEventListener.Get(mShootBtn.gameObject).onUp += OnShootBtnUp;

        RectTransform rect = transform.Find("position/bg").GetComponent<RectTransform>();
        mRadius = (int)((rect.rect.bottom - rect.rect.top) / 2);
    }

    void Start()
    {
        GameLogic.Instance.UICtrl.JoystickUI = this;
    }

    public void OnDrag(PointerEventData data)
    {
        if (mDrag)
        {
            Vector3 delta = data.position - mBeginPos;
            mTouchImg.transform.localPosition = Vector3.ClampMagnitude(delta, mRadius);
        }
    }

    public void OnBeginDrag(PointerEventData data)
    {
        mDrag = true;
        mBeginPos = data.position;
    }

    public void OnEndDrag(PointerEventData data)
    {
        mDrag = false;
        mTouchImg.transform.localPosition = Vector3.zero;
    }

    void OnShootBtnDown(GameObject go)
    {
        Player player = GameObject.FindGameObjectWithTag(Tags.PlayerTag).GetComponent<Player>();
        if (player == null)
            return;

        player.Anim.SetBool("Shoot", true);
    }

    void OnShootBtnUp(GameObject go)
    {
        Player player = GameObject.FindGameObjectWithTag(Tags.PlayerTag).GetComponent<Player>();
        if (player == null)
            return;

        player.Anim.SetBool("Shoot", false);
    }

    public bool IsDrag()
    {
        return mDrag;  
    }

    public Vector3 GetDir()
    {
        return mTouchImg.transform.localPosition.normalized;
    }

    public void UpdateJoystick(Vector3 dir)
    {
        mTouchImg.transform.localPosition = mRadius * dir;
    }
}
