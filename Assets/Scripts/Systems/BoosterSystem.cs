using Systems;
using UnityEngine;
using VContainer;

public class BoosterSystem : ASystem
{
    [SerializeField] private float m_BoosterDuration = 5f;

    [Inject] private SystemsContainer m_SystemsContainer;

    private PlayerSpawnSystem m_PlayerSpawnSystem;

    public override void Initialize()
    {
        m_PlayerSpawnSystem = m_SystemsContainer.GetSystem<PlayerSpawnSystem>();
    }

    
}
