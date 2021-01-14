using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletons : MonoBehaviour { }

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance {
        get {
            if (instance != null) 
                return instance;

            Singletons container = Object.FindObjectOfType<Singletons>();
            if (container == null) {
                GameObject containerGO = new GameObject("Singletons");
                container = containerGO.AddComponent<Singletons>();
            }

            GameObject singleton = new GameObject(typeof(T).Name);
            singleton.transform.SetParent(container.transform);
            instance = singleton.AddComponent<T>();
            return instance;
        }
    }
}