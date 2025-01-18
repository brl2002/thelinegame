using System;
using Game;
using Systems;
using UnityEngine;
using VContainer;

namespace UI
{
    [RequireComponent(typeof(Tooltip))]
    public class PlayerStateUI : MonoBehaviour
    {
        [Inject] private SystemsContainer m_SystemsContainer;
        [Inject] private GameSettings m_GameSettings;

        private PlayerSpawnSystem m_PlayerSpawnSystem;
        
        private Tooltip m_Tooltip;

        private bool m_IsPaused = false;
        private bool m_IsShowingPlayerState = false;
        private float m_PlayerStateTime;
        private float m_LastPlayerStateTime;

        private string m_InvincibleStateString = "Break the walls!";

        public void Pause()
        {
            m_IsPaused = true;
        }

        public void Resume()
        {
            m_IsPaused = false;
        }

        public void ShowPlayerState()
        {
            if (m_PlayerSpawnSystem.Player.IsInvincible)
            {
                m_PlayerStateTime = m_GameSettings.InvincibilityBoosterTime;
                m_Tooltip.SetTooltipText(m_InvincibleStateString);
            }
            else if (m_PlayerSpawnSystem.Player.IsShrink)
            {
                m_PlayerStateTime = m_GameSettings.ShrinkerBoosterTime;
                m_Tooltip.SetTooltipText($"{m_PlayerStateTime:0}sec");
            }

            m_LastPlayerStateTime = m_PlayerStateTime;
            m_IsShowingPlayerState = true;
            m_Tooltip.Show();
        }

        public void HidePlayerState()
        {
            m_IsShowingPlayerState = false;
            m_Tooltip.Hide();
        }

        private void Update()
        {
            if (!m_IsPaused && m_IsShowingPlayerState)
            {
                if (m_PlayerSpawnSystem.Player.IsShrink && m_LastPlayerStateTime - m_PlayerStateTime >= 1)
                {
                    m_Tooltip.SetTooltipText($"{m_PlayerStateTime:0}sec");
                    m_LastPlayerStateTime = m_PlayerStateTime;
                }

                m_PlayerStateTime -= Time.deltaTime;
            }
        }
        
        private void Awake()
        {
            m_PlayerSpawnSystem = m_SystemsContainer.GetSystem<PlayerSpawnSystem>();
            m_Tooltip = GetComponent<Tooltip>();
        }
    }
}
