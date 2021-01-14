using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Fader : MonoBehaviour
{
    [SerializeField] private float speed;

    void OnEnable() {    
        Color c = GetComponent<Image>().color;
        c.a = 0.0f;
        GetComponent<Image>().color = c;
    }

    void Update() {
        Color c = GetComponent<Image>().color;
        c.a += Time.deltaTime * speed;;
        GetComponent<Image>().color = c;
    }
}