using System;
using Booster;
using Core;
using Game;
using Road;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

namespace DependencyInjection
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameSettings m_GameSettings;
        [SerializeField] private GameEventContainer m_GameEventContainer;
        [SerializeField] private Player m_PlayerPrefab;
        [SerializeField] private RoadBlock m_RoadBlockPrefab;
        [SerializeField] private BoosterPickUp m_BoosterPickUpPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            Camera camera = Camera.main;
            float screenHeight = camera.orthographicSize * 2;
            float screenWidth = screenHeight * camera.pixelWidth / camera.pixelHeight;
            m_GameSettings.Configure(screenHeight, screenWidth);
            
            builder.RegisterInstance(m_GameSettings);
            builder.RegisterInstance(m_GameEventContainer);
        
            builder.Register<PlayerFactory>(Lifetime.Scoped).WithParameter(typeof(Player), m_PlayerPrefab).WithParameter(typeof(GameSettings), m_GameSettings);
            builder.Register<RoadBlockRowGroupFactory>(Lifetime.Scoped).WithParameter(typeof(GameSettings), m_GameSettings).WithParameter(typeof(RoadBlock),m_RoadBlockPrefab);
            builder.Register<Func<Transform, BoosterType, Action<BoosterPickUp>, float, BoosterPickUp>>(container => (parent, boosterType, callback, boosterTime) =>
            {
                BoosterPickUp instance = container.Instantiate(m_BoosterPickUpPrefab, parent);
                instance.SetBoosterType(boosterType);
                instance.OnPickUpBooster += callback;
                instance.SetBoosterTime(boosterTime);
                return instance;
            }, Lifetime.Scoped);
        }
    }
}