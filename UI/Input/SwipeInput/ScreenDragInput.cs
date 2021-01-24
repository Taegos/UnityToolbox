using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Toolbox.UI.Input
{
    public class ScreenDragInput : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public delegate void OnBeginDragDelegate();
        public event OnBeginDragDelegate OnBeginDrag;

        public delegate void OnEndDragDelegate();
        public event OnEndDragDelegate OnEndDrag;

        private Vector2 delta;
        private Vector2 current;
        private Vector2 start;
        private bool holding;

        public bool IsDragging() {
            return delta != Vector2.zero;
        }

        public bool IsHolding() {
            return holding;
        }

        public Vector2 GetDelta() {
            return delta;
        }

        public Vector2 GetDeltaTotal() {
            return current - start;
        }

        void IDragHandler.OnDrag(PointerEventData eventData) {
            delta = eventData.delta;
            current = eventData.position;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
            start = eventData.position;
            current = start;
            holding = true;
            if (OnBeginDrag != null) {
                OnBeginDrag.Invoke();
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
            start = Vector2.zero;
            current = Vector2.zero;
            delta = Vector2.zero;
            holding = false;
            if (OnEndDrag != null) {
                OnEndDrag.Invoke();
            }
        }
    }
}