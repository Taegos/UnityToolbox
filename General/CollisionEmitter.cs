using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollisionEvent : UnityEvent<Collision> { }


[RequireComponent(typeof(Collider))]
public class CollisionEmitter : MonoBehaviour
{
    public CollisionEvent OnCollisionEntered;
    public CollisionEvent OnCollisionExited;

    void OnCollisionEnter(Collision collision) {
        if (OnCollisionEntered != null) {
            OnCollisionEntered.Invoke(collision);
        }
    }

    void OnCollisionExit(Collision collision) {
        if (OnCollisionExited != null) {
            OnCollisionExited.Invoke(collision);
        }
    }
}