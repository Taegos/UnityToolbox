using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.EventSystem
{
    public class ActionInfo
    {
        public Object TargetObject { get; set; }
        public string TargetMethod { get; set; }
        public string TargetArgType { get; set; }

        public override string ToString()
        {
            if (TargetObject == null)
                return "NoTarget";
            string method = TargetMethod == "" ? "NoTargetMethod" : TargetMethod;
            return TargetObject.GetType().Name + "." + method;
        }
    }

    public abstract class BaseEventListener : MonoBehaviour
    {
        public abstract BaseEvent BaseEvent { get; }
        public abstract List<ActionInfo> ActionInfos { get; }

        public override string ToString()
        {
            return gameObject.name + "." + GetType().Name;
        }
    }

    public abstract class EventListener<T> : BaseEventListener
    {
        [SerializeField] private Event<T> Event;

        public override BaseEvent BaseEvent => Event;

        public abstract UnityEvent<T> Actions { get; }


        public override List<ActionInfo> ActionInfos {
            get {
                List<ActionInfo> actionInfos = new List<ActionInfo>();
                int i = 0;

                while (i < Actions.GetPersistentEventCount())
                {
                    Object targetObject = Actions.GetPersistentTarget(i);
                    ActionInfo actionInfo = new ActionInfo
                    {
                        TargetObject = targetObject,
                        TargetMethod = Actions.GetPersistentMethodName(i)
                    };
                    actionInfos.Add(actionInfo);
                    i++;
                }

                return actionInfos;
            }
        }

        private void OnEnable()
        {
            Event.Register(this);
        }

        private void OnDisable()
        {
            Event.Unregister(this);
        }

        public void Raise(T value)
        {
            Actions.Invoke(value);
        }
    }
}