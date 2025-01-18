using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEventContainer", menuName = "GameEvent/GameEventContainer")]
    public class GameEventContainer : ScriptableObject
    {
        public int GameEventCount => m_GameEvent.Length;
        
        [SerializeField] private GameEvent[] m_GameEvent;

        public GameEvent GetGameEvent(int eventIndex)
        {
            return m_GameEvent[eventIndex];
        }
    }
}