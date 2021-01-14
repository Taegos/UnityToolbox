using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.ExecuteEvents;
using UnityEngine.EventSystems;

public class TutorialIndicator : MonoBehaviour, IPointerDownHandler
{
    void OnEnable() {
        Time.timeScale = 0.0f;
    }
    public void OnPointerDown(PointerEventData ev) {
        Time.timeScale = 1.0f;

        transform.GetChild(0).gameObject.SetActive(false);
        ExecuteHierarchy(GetComponentInParent<Canvas>().gameObject, ev, pointerDownHandler);
    }
}