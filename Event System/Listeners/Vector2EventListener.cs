using UnityEngine;
using UnityEngine.Events;

namespace Toolbox.EventSystem.Listeners
{
    public class Vector2EventListener : EventListener<Vector2>
    {
        [SerializeField] private Vector2UnityEvent actions;
        public override UnityEvent<Vector2> Actions => actions;
     }
}