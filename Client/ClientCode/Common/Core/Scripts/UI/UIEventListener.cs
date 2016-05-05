
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Splatoon
{
    public class UIEventListener : UnityEngine.EventSystems.EventTrigger
    {
        public delegate void VoidDelegate(GameObject obj);
        public delegate void VoidPointDelegate(GameObject obj, PointerEventData data);
        public VoidDelegate onClick;
        public VoidDelegate onDown;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;
        public VoidPointDelegate onDrag;

        static public UIEventListener Get(GameObject go)
        {
            UIEventListener listener = go.GetComponent<UIEventListener>();
            if (listener == null)
            {
                listener = go.AddComponent<UIEventListener>();
            }

            return listener;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
                onClick(gameObject);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null)
                onDown(gameObject);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null)
                onEnter(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null)
                onExit(gameObject);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null)
                onUp(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null)
                onSelect(gameObject);
        }

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null)
                onUpdateSelect(gameObject);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
                onDrag(gameObject, eventData);
        }
    }
}
