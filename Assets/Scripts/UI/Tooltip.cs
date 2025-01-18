using System.Collections;
using Game;
using Systems;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TooltipText;
        [SerializeField] private float m_ShowYPosition;
        [SerializeField] private float m_HideYPosition;
        [SerializeField] private float m_TimeToMove;

        [Inject] private SystemsContainer m_SystemsContainer;
        
        private bool m_IsShowing = false;
        private RectTransform m_RectTransform;
        private PlayerSpawnSystem m_PlayerSpawnSystem;

        public void Reset()
        {
            m_RectTransform.anchoredPosition = new Vector2(0, m_HideYPosition);
        }

        public void SetTooltipText(string text)
        {
            m_TooltipText.text = text;
        }

        public void Show()
        {
            m_IsShowing = true;
            StartCoroutine(ShowOrHideCoroutine());
        }

        public void Hide()
        {
            m_IsShowing = false;
            StartCoroutine(ShowOrHideCoroutine());
        }

        private IEnumerator ShowOrHideCoroutine()
        {
            float targetYPos = m_IsShowing ? m_ShowYPosition : m_HideYPosition;
            float startYPos = m_RectTransform.anchoredPosition.y;
            float elapsedTime = 0f;

            while (elapsedTime < m_TimeToMove)
            {
                float newY = Mathf.Lerp(startYPos, targetYPos, elapsedTime / m_TimeToMove);
                m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x, newY);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            m_RectTransform.anchoredPosition = new Vector2(m_RectTransform.anchoredPosition.x, targetYPos);
        }

        private void Update()
        {
            if (m_IsShowing)
            {
                Vector3 playerWorldPosition = m_PlayerSpawnSystem.Player.transform.position;
                
                Vector2 playerScreenPosition = Camera.main.WorldToScreenPoint(playerWorldPosition);
                Vector2 playerUIPosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    m_TooltipText.rectTransform.parent as RectTransform,
                    playerScreenPosition,
                    null,
                    out playerUIPosition
                );
                
                m_TooltipText.rectTransform.anchoredPosition = new Vector2(
                    playerUIPosition.x,
                    m_TooltipText.rectTransform.anchoredPosition.y
                );
            }
        }

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_PlayerSpawnSystem = m_SystemsContainer.GetSystem<PlayerSpawnSystem>();
        }
    }
}
