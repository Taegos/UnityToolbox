using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toolbox.EventSystem
{
    public abstract class BaseEvent : ScriptableObject
    {
        public abstract string ArgTypeName { get; }

        public override string ToString()
        {
            return name + " (" + ArgTypeName + " )";
        }
    }

    public abstract class Event<T> : BaseEvent
    {
        private List<EventListener<T>> listeners = new List<EventListener<T>>();

        public override string ArgTypeName => typeof(T).Name;

        public void Raise(T value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
                listeners[i].Raise(value);
        }

        public void Register(EventListener<T> listener)
        {
            listeners.Add(listener);
        }

        public void Unregister(EventListener<T> listener)
        {
            listeners.Remove(listener);
        }
    }
}