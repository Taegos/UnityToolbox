using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Vector3 targetScale = Vector3.one;
    
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;            
    }


    void Update()
    {
        Vector3 dt = targetScale - startScale;
        float value = (Mathf.Sin(Time.unscaledTime * speed) + 1.0f) / 2.0f;
        transform.localScale = startScale + dt * value;
    }
}
