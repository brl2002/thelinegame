using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game
{
    public class PlayerFactory
    {
        private IObjectResolver m_Container;
        private Player m_PlayerPrefab;
        private GameSettings m_GameSettings;

        public PlayerFactory(IObjectResolver container, Player playerPrefab, GameSettings gameSettings)
        {
            m_Container = container;
            m_PlayerPrefab = playerPrefab;
            m_GameSettings = gameSettings;
        }

        public Player Create(Transform spawnTransform)
        {
            var instance = m_Container.Instantiate(m_PlayerPrefab, spawnTransform);
            instance.transform.localScale = m_GameSettings.PlayerLocalScale;
            return instance;
        }
    }
}