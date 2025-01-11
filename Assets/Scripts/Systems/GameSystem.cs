using System;
using Core;
using Game;
using UnityEngine;
using VContainer;

namespace Systems
{
    public class GameSystem : ASystem
    {
        [Inject] private SystemsContainer m_SystemsContainer;
        [Inject] private GameEventContainer m_GameEventContainer;

        private GameEvent m_GameStartEvent;
        private GameEvent m_GameResetEvent;
        private GameEvent m_GameEndEvent;
        
        private GameState m_CurrentState;

        public void SetGameState(GameState newState)
        {
            m_CurrentState = newState;

            switch (m_CurrentState)
            {
                case GameState.Standby:
                    Standby();
                    break;
                case GameState.Menu:
                    ShowMenu();
                    break;
                case GameState.Playing:
                    StartGame();
                    break;
                case GameState.GameOver:
                    GameOver();
                    break;
            }
        }

        public override void Initialize()
        {
            try
            {
                m_GameStartEvent = m_GameEventContainer.GetGameEvent(0);
                m_GameResetEvent = m_GameEventContainer.GetGameEvent(1);
                m_GameEndEvent = m_GameEventContainer.GetGameEvent(2);
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
            }
        }

        private void Standby()
        {
            m_GameResetEvent?.Raise();
        }

        private void ShowMenu()
        {
            
        }

        private void StartGame()
        {
            
        }

        private void GameOver()
        {
            
        }

        private void Start()
        {
            SetGameState(GameState.Standby);
        }
    }
}