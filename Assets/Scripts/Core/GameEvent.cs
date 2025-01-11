// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "GameEvent/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<GameEventListener> m_EventListeners = new List<GameEventListener>();

        public void Raise()
        {
            for (int i = m_EventListeners.Count - 1; i >= 0; i--)
            {
                m_EventListeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            if (!m_EventListeners.Contains(listener))
            {
                m_EventListeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (m_EventListeners.Contains(listener))
            {
                m_EventListeners.Remove(listener);
            }
        }
    }
}