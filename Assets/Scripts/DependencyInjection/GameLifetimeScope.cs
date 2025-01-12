using Core;
using Game;
using Road;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DependencyInjection
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private GameEventContainer m_GameEventContainer;
        [SerializeField] private Player m_PlayerPrefab;
        [SerializeField] private RoadGuardBlock m_RoadGuardBlockPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            m_GameSettings.Configure(Camera.main);
            
            builder.RegisterInstance(m_GameSettings);
            builder.RegisterInstance(m_GameEventContainer);
        
            builder.Register<PlayerFactory>(Lifetime.Scoped).WithParameter(typeof(Player), m_PlayerPrefab);
            builder.Register<RoadGuardBlockRowGroupFactory>(Lifetime.Scoped).WithParameter(typeof(GameSettings), m_GameSettings).WithParameter(typeof(RoadGuardBlock),m_RoadGuardBlockPrefab);
        }
    }
}