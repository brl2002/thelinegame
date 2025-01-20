using System;
using Booster;
using Core;
using Game;
using Systems;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

public class BoosterSystem : ASystem
{
    [SerializeField] private float m_BoosterDuration = 5f;

    [Inject] private SystemsContainer m_SystemsContainer;
    [Inject] private GameSettings m_GameSettings;

    [Inject] private Func<Transform, BoosterType, Action<BoosterPickUp>, float, BoosterPickUp> m_BoosterFactory;

    private BoosterPickUp[] m_Boosters;
    private Transform m_BoosterParentTransform;
    private PlayerSpawnSystem m_PlayerSpawnSystem;
    private BoosterPickUp m_SpawnedBoosterPickUp;

    public void Reset()
    {
        if (m_SpawnedBoosterPickUp)
        {
            m_SpawnedBoosterPickUp.gameObject.SetActive(false);
            m_SpawnedBoosterPickUp = null;
        }
    }

    public void SpawnRandomBooster(Vector2 spawnPosition)
    {
        BoosterPickUp boosterPickUp = GetRandomBooster();
        boosterPickUp.transform.position = spawnPosition;
        boosterPickUp.gameObject.SetActive(true);
        m_SpawnedBoosterPickUp = boosterPickUp;
    }

    public void ScrollBoosters(float scrollDistance)
    {
        if (m_SpawnedBoosterPickUp)
        {
            m_SpawnedBoosterPickUp.transform.Translate(0, -scrollDistance, 0);
        }
    }

    public override void Initialize()
    {
        m_PlayerSpawnSystem = m_SystemsContainer.GetSystem<PlayerSpawnSystem>();
        
        m_BoosterParentTransform = new GameObject("BoosterParent").transform;
        m_Boosters = new BoosterPickUp[6];
        m_Boosters[0] = m_BoosterFactory(m_BoosterParentTransform, BoosterType.Invincibility, OnPickUpBooster, m_GameSettings.InvincibilityBoosterTime);
        m_Boosters[1] = m_BoosterFactory(m_BoosterParentTransform, BoosterType.Invincibility, OnPickUpBooster, m_GameSettings.InvincibilityBoosterTime);
        m_Boosters[2] = m_BoosterFactory(m_BoosterParentTransform, BoosterType.Invincibility, OnPickUpBooster, m_GameSettings.InvincibilityBoosterTime);
        m_Boosters[3] = m_BoosterFactory(m_BoosterParentTransform, BoosterType.Shrinker, OnPickUpBooster, m_GameSettings.ShrinkerBoosterTime);
        m_Boosters[4] = m_BoosterFactory(m_BoosterParentTransform, BoosterType.Shrinker, OnPickUpBooster, m_GameSettings.ShrinkerBoosterTime);
        m_Boosters[5] = m_BoosterFactory(m_BoosterParentTransform, BoosterType.Shrinker, OnPickUpBooster, m_GameSettings.ShrinkerBoosterTime);
        m_Boosters[0].gameObject.SetActive(false);
        m_Boosters[1].gameObject.SetActive(false);
        m_Boosters[2].gameObject.SetActive(false);
        m_Boosters[3].gameObject.SetActive(false);
        m_Boosters[4].gameObject.SetActive(false);
        m_Boosters[5].gameObject.SetActive(false);
    }

    private BoosterPickUp GetRandomBooster()
    {
        int randIndex = Random.Range(0, 2);
        BoosterPickUp boosterPickUp = m_Boosters[randIndex];
        boosterPickUp.gameObject.SetActive(true);
        return boosterPickUp;
    }

    private void OnPickUpBooster(BoosterPickUp boosterPickUp)
    {
        switch (boosterPickUp.BoosterType)
        {
            case BoosterType.Invincibility:
                m_PlayerSpawnSystem.Player.MakeInvincible(boosterPickUp.BoosterTime);
                break;
            case BoosterType.Shrinker:
                m_PlayerSpawnSystem.Player.Shrink(m_GameSettings.ShrinkerPlayerScaleFactor, boosterPickUp.BoosterTime);
                break;
        }
        
        boosterPickUp.gameObject.SetActive(false);
        m_SpawnedBoosterPickUp = null;
    }
}
