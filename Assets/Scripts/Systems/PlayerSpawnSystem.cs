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
        
        public Player Player => m_Player;

        public void SpawnPlayer()
        {
            m_Player = m_PlayerFactory.Create(m_PlayerSpawnTransform);
        }

        public override void Initialize() {}
    }
}