using Game;
using UnityEngine;
using VContainer;

namespace Systems
{
    public class PlayerSpawnSystem : ASystem
    {
        [SerializeField] private Transform m_PlayerSpawnTransform;
        
        [Inject] private PlayerFactory m_PlayerFactory;
        
        private Player m_Player;
        private Vector2 m_PlayerSpawnPosition;
        
        public Player Player => m_Player;
        public Vector2 PlayerSpawnPosition => m_PlayerSpawnPosition;

        public void SpawnPlayer()
        {
            m_Player = m_PlayerFactory.Create(m_PlayerSpawnTransform);
            m_PlayerSpawnPosition = m_Player.transform.position;
        }

        public override void Initialize() {}
    }
}