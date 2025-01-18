using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private GameObject m_PlayUIGroupObject;
        [SerializeField] private TextMeshProUGUI m_ScoreText;
        [SerializeField] private TextMeshProUGUI m_StartHelpText;
        [SerializeField] private Button m_MenuButton;
        [SerializeField] private float m_FadeDuration = 0.5f;

        [Inject] private UIModel m_UIModel;
        
        private Coroutine m_FadeCoroutine;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Reset()
        {
            Color tooltipTextColor = m_StartHelpText.color;
            tooltipTextColor.a = 1.0f;
            m_StartHelpText.color = tooltipTextColor;
            m_MenuButton.interactable = true;
        }

        public void Pause()
        {
            m_MenuButton.interactable = false;
        }
        
        public void ShowStartHelpText()
        {
            if (m_FadeCoroutine != null) StopCoroutine(m_FadeCoroutine);
            m_FadeCoroutine = StartCoroutine(FadeTextAlpha(1f));
        }

        public void HideStartHelpText()
        {
            if (m_FadeCoroutine != null) StopCoroutine(m_FadeCoroutine);
            m_FadeCoroutine = StartCoroutine(FadeTextAlpha(0f));
        }

        private IEnumerator FadeTextAlpha(float targetAlpha)
        {
            Color currentColor = m_StartHelpText.color;
            float startAlpha = currentColor.a;
            float elapsedTime = 0f;

            while (elapsedTime < m_FadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / m_FadeDuration);
                m_StartHelpText.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
                yield return null;
            }
            
            m_StartHelpText.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        }

        private void Update()
        {
            m_ScoreText.text = m_UIModel.Score.ToString();
        }
    }
}