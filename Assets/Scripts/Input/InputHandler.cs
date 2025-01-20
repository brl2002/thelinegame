using Core;
using Game;
using Road;
using Systems;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace Input
{
    public class InputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Inject] private SystemsContainer m_SystemsContainer;
        [Inject] private GameEventContainer m_GameEventContainer;

        private PlayerSpawnSystem m_PlayerSpawnSystem;
        private GameSystem m_GameSystem;
        private bool m_IsMouseDown = false;
        private bool m_IsInputEnabled = true;

        private GameEvent m_GameStartEvent;

        public void Reset()
        {
            m_IsInputEnabled = true;
            m_IsMouseDown = false;
        }

        public void Resume()
        {
            m_IsInputEnabled = true;
        }

        public void Pause()
        {
            m_IsInputEnabled = false;
        }

        private void Awake()
        {
            m_PlayerSpawnSystem = m_SystemsContainer.GetSystem<PlayerSpawnSystem>();
            m_GameSystem = m_SystemsContainer.GetSystem<GameSystem>();

            m_GameStartEvent = m_GameEventContainer.GetGameEvent(GameConstants.GAME_START_EVENT_INDEX);
        }

        private bool m_IsPointerOverGameObject = false;

        void Update()
        {
            if (m_IsInputEnabled)
            {
                if (m_IsPointerOverGameObject && UnityEngine.Input.GetMouseButtonDown(0))
                {
                    m_IsMouseDown = true;
                    if (m_GameSystem.CurrentState != GameState.Playing)
                    {
                        m_GameStartEvent.Raise();
                    }
                }
                else if (m_IsPointerOverGameObject && UnityEngine.Input.GetMouseButtonUp(0))
                {
                    m_IsMouseDown = false;
                }
                
                if (m_IsMouseDown && m_IsPointerOverGameObject)
                {
                    var mousePosition = UnityEngine.Input.mousePosition;
                    mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    m_PlayerSpawnSystem.Player.transform.position = new Vector3(mousePosition.x, m_PlayerSpawnSystem.Player.transform.position.y, 0);
                }   
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_IsPointerOverGameObject = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_IsPointerOverGameObject = false;
        }
    }
}