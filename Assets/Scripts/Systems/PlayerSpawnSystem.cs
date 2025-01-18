using Game;
using UnityEngine;
using VContainer;

namespace Systems
{
    public class PlayerSpawnSystem : ASystem
    {
        public Player Player => m_Player;
        public Vector2 PlayerSpawnPosition => m_PlayerSpawnPosition;
        
        [SerializeField] private Transform m_PlayerSpawnTransform;
        
        [Inject] private PlayerFactory m_PlayerFactory;
        
        private Player m_Player;
        private Vector2 m_PlayerSpawnPosition;

        public void Reset()
        {
            m_Player.transform.position = m_PlayerSpawnPosition;
        }

        public Player SpawnPlayer()
        {
            m_Player = m_PlayerFactory.Create(m_PlayerSpawnTransform);
            m_PlayerSpawnPosition = m_Player.transform.position;
            return m_Player;
        }

        public override void Initialize() {}
    }
}