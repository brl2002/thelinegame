// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        public virtual void OnEventRaised(object data = null)
        {
            Response.Invoke();
        }
        
        protected virtual void OnEnable()
        {
            Event.RegisterListener(this);
        }

        protected virtual void OnDisable()
        {
            Event.UnregisterListener(this);
        }
    }
}