using Toolbox.EventSystem.Events;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class SwipeInput : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    //Decide when to run (fixed, regular update)
    public Vector2Event OnPressedFixed;

    private Vector2 delta;
    private bool pressed;

    void IDragHandler.OnDrag(PointerEventData eventData) {
        delta = eventData.delta;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        pressed = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        pressed = false;
    }

    void FixedUpdate()
    {
        if (!pressed) return;
        OnPressedFixed?.Raise(delta);
    }
}