using Core;
using Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    [SerializeField] private GameSettings m_GameSettings;
    [SerializeField] private GameEventContainer m_GameEventContainer;
    [SerializeField] private Player m_PlayerPrefab;

    protected override void Configure(IContainerBuilder builder)
    {
        m_GameSettings.Configure(Camera.main);
        
        builder.RegisterInstance(m_GameSettings);
        builder.RegisterInstance(m_GameEventContainer);
        
        builder.Register<PlayerFactory>(Lifetime.Scoped).WithParameter(typeof(Player), m_PlayerPrefab);
    }
}
