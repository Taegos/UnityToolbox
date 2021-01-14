using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.EventSystem.Listeners
{
    public class FloatEventListener : EventListener<float>
    {
        [SerializeField] private FloatUnityEvent action;
        public override UnityEvent<float> Actions => action;
    }
}