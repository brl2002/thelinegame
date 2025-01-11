using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "GameEventContainer", menuName = "GameEvent/GameEventContainer")]
    public class GameEventContainer : ScriptableObject
    {
        [SerializeField] private GameEvent[] m_GameEvent;

        public int GameEventCount => m_GameEvent.Length;
        
        public GameEvent GetGameEvent(int eventIndex)
        {
            return m_GameEvent[eventIndex];
        }
    }
}