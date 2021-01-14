using Toolbox.EventSystem.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.EventSystem.Listeners
{
    public class VoidEventListener : MonoBehaviour
    {

        [SerializeField] private UnityEvent actions;
        [SerializeField] private VoidEvent Event;

        private void OnEnable()
        {
            Event.Register(this);
        }

        private void OnDisable()
        {
            Event.Unregister(this);
        }

        public void Raise()
        {
            actions.Invoke();
        }
    }
}