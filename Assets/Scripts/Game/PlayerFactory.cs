using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class PlayerFactory
    {
        private IObjectResolver m_Container;
        private Player m_PlayerPrefab;

        public PlayerFactory(IObjectResolver container, Player playerPrefab)
        {
            m_Container = container;
            m_PlayerPrefab = playerPrefab;
        }

        public Player Create(Transform spawnTransform)
        {
            return m_Container.Instantiate(m_PlayerPrefab, spawnTransform);
        }
    }
}