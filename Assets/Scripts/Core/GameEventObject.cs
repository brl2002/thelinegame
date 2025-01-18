using UnityEngine;

namespace Core
{
    public class GameEventObject : MonoBehaviour
    {
        [SerializeField] private GameEvent m_GameEvent;

        public void TriggerGameEvent()
        {
            m_GameEvent.Raise();
        }
    }
}
