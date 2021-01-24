using System.Collections;
using System.Collections.Generic;
using Toolbox.EventSystem.Listeners;
using UnityEngine;

namespace Toolbox.EventSystem.Events
{
    [CreateAssetMenu]
    public class VoidEvent : ScriptableObject
    {
        private List<VoidEventListener> listeners = new List<VoidEventListener>();

        public void Raise()
        {
            if (listeners.Count == 0) return;
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].Raise();
        }

        public void Register(VoidEventListener listener)
        {
            listeners.Add(listener);
        }

        public void Unregister(VoidEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}