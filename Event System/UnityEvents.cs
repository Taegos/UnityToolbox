using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.EventSystem
{
    [System.Serializable]
    public class FloatUnityEvent : UnityEvent<float> { }

    [System.Serializable]
    public class Vector2UnityEvent : UnityEvent<Vector2> { }
}