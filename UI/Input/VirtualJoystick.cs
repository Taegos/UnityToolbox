using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[System.Serializable]
public class BeginDragEvent : UnityEvent { }

[System.Serializable]
public class DraggingEvent : UnityEvent<Vector2> { }

[System.Serializable]
public class EndDragEvent : UnityEvent { }


public class VirtualJoystick : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler{
   
    [SerializeField] private RectTransform knob;
    
    public BeginDragEvent OnBeginDrag;
    public DraggingEvent OnDragging;
    public EndDragEvent OnEndDrag;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {
        if (OnBeginDrag != null) {
            OnBeginDrag.Invoke();
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        RectTransform trans = GetComponent<RectTransform>();
        Vector2 point;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(trans, eventData.position, null, out point);
        Vector2 dir = point.normalized;
        float radius = trans.rect.width / 2;
        float length = Mathf.Clamp(point.magnitude / radius, 0.0f, 1.0f);
        knob.anchoredPosition = dir * length * radius;
        if (OnDragging != null) {
            OnDragging.Invoke(dir * length);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {
        knob.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        if (OnEndDrag != null) {
            OnEndDrag.Invoke();
        }
    }
}
