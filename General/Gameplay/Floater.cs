using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private float floatHeight;
    [SerializeField] private float floatSpeed;
    [SerializeField] private float rotateSpeed;

    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatHeight + startY;
        Vector3 pos = transform.position;
        pos.y = newY;
        transform.position = pos;
        transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed, Space.World);
    }
}
