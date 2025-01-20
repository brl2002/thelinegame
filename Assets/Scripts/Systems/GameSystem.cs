using System;
using System.Collections;
using Core;
using Game;
using Road;
using UnityEngine;
using VContainer;

namespace Systems
{
    public class GameSystem : ASystem
    {
        [Inject] private SystemsContainer m_SystemsContainer;
        [Inject] private GameEventContainer m_GameEventContainer;
        [Inject] private GameSettings m_GameSettings;

        private GameEvent m_GameStartEvent;
        private GameEvent m_GameResetEvent;
        private GameEvent m_GameEndEvent;
        private GameEvent m_GamePauseEvent;
        private GameEvent m_GameResumeEvent;
        private GameEvent m_BoosterActiveEvent;
        private GameEvent m_BoosterInactiveEvent;

        private GameState m_CurrentState;
        private GameState m_LastState;

        private RoadSystem m_RoadSystem;
        private BoosterSystem m_BoosterSystem;

        private WaitForSeconds m_PlayerCollisionWait;
        private WaitForSeconds m_PreGameOverWait;
        private float m_PlayerCollisionWaitInterval = 0.15f;
        private float m_PreGameOverWaitInterval = 3f;
        private Coroutine m_PlayerCollisionWaitCoroutine;
        private Coroutine m_PreGameOverWaitCoroutine;

        private float m_NextBoosterScrollDistance;

        public GameState CurrentState => m_CurrentState;
        public GameState LastState => m_LastState;

        public void Reset()
        {
            SetGameState(GameState.Standby);
        }
        
        public void Pause()
        {
            SetGameState(GameState.Pause);
        }

        public void Resume()
        {
            SetGameState(m_LastState);
        }

        public void GameStart()
        {
            SetGameState(GameState.Playing);
        }

        public void SetGameState(GameState newState)
        {
            m_LastState = m_CurrentState;
            m_CurrentState = newState;

            switch (m_CurrentState)
            {
                case GameState.Standby:
                    Standby();
                    break;
                case GameState.Pause:
                    PauseGame();
                    break;
                case GameState.Playing:
                    StartGame();
                    break;
                case GameState.PreGameOver:
                    PreGameOver();
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
                m_GameStartEvent = m_GameEventContainer.GetGameEvent(GameConstants.GAME_START_EVENT_INDEX);
                m_GameResetEvent = m_GameEventContainer.GetGameEvent(GameConstants.GAME_RESET_EVENT_INDEX);
                m_GameEndEvent = m_GameEventContainer.GetGameEvent(GameConstants.GAME_END_EVENT_INDEX);
                m_GamePauseEvent = m_GameEventContainer.GetGameEvent(GameConstants.GAME_PAUSE_EVENT_INDEX);
                m_GameResumeEvent = m_GameEventContainer.GetGameEvent(GameConstants.GAME_RESUME_EVENT_INDEX);
                m_BoosterActiveEvent = m_GameEventContainer.GetGameEvent(GameConstants.BOOSTER_ACTIVE_EVENT_INDEX);
                m_BoosterInactiveEvent = m_GameEventContainer.GetGameEvent(GameConstants.BOOSTER_INACTIVE_EVENT_INDEX);

                m_RoadSystem = m_SystemsContainer.GetSystem<RoadSystem>();
                m_BoosterSystem = m_SystemsContainer.GetSystem<BoosterSystem>();

                m_PlayerCollisionWait = new WaitForSeconds(m_PlayerCollisionWaitInterval);
                m_PreGameOverWait = new WaitForSeconds(m_PreGameOverWaitInterval);

                Player player = m_SystemsContainer.GetSystem<PlayerSpawnSystem>().SpawnPlayer();
                player.OnRoadBlockCollision += () =>
                {
                    m_PlayerCollisionWaitCoroutine = StartCoroutine(WaitAndCall(m_PlayerCollisionWait, () =>
                    {
                        m_GamePauseEvent.Raise();
                        m_RoadSystem.StopRunning();
                        SetGameState(GameState.PreGameOver);
                        m_PlayerCollisionWaitCoroutine = null;
                    }));
                };
                player.OnStateChangeActive += () => m_BoosterActiveEvent.Raise();
                player.OnStateChangeInactive += () => m_BoosterInactiveEvent.Raise();
            }
            catch (Exception e)
            {
                Debug.LogException(e, gameObject);
            }
        }

        private void Standby()
        {
            
        }

        private void PauseGame()
        {
            
        }

        private void StartGame()
        {
            SetNextBoosterScrollDistance();
        }

        private void PreGameOver()
        {
            m_PreGameOverWaitCoroutine = StartCoroutine(WaitAndCall(m_PreGameOverWait, () =>
            {
                SetGameState(GameState.GameOver);
                m_PreGameOverWaitCoroutine = null;
            }));
        }

        private void GameOver()
        {
            m_GameEndEvent.Raise();
        }

        private void SetNextBoosterScrollDistance()
        {
            m_NextBoosterScrollDistance = m_RoadSystem.ScrollDistance + m_GameSettings.BoosterSpawningRate;
        }

        private IEnumerator WaitAndCall(WaitForSeconds waitForSeconds, Action callback)
        {
            yield return waitForSeconds;
            callback?.Invoke();
        }

        private void Start()
        {
            SetGameState(GameState.Standby);
            SetNextBoosterScrollDistance();
            m_GameResetEvent.Raise();
        }

        private void Update()
        {
            if (m_CurrentState == GameState.Playing)
            {
                float frameScrollDistance = m_GameSettings.ScrollSpeed * Time.deltaTime;
                m_RoadSystem.ScrollRows(frameScrollDistance);
                m_BoosterSystem.ScrollBoosters(frameScrollDistance);
            
                if (m_RoadSystem.ScrollDistance >= m_NextBoosterScrollDistance)
                {
                    RoadBlock roadBlock = m_RoadSystem.GetRandomDisabledRoadBlock();
                    m_BoosterSystem.SpawnRandomBooster(roadBlock.transform.position);
                    SetNextBoosterScrollDistance();
                }
            }
        }
    }
}