using System;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_ScoreText;
        [SerializeField] private TextMeshProUGUI m_BestScoreText;

        [Inject] private UIModel m_UIModel;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            m_ScoreText.text = m_UIModel.Score.ToString();
            m_BestScoreText.text = m_UIModel.BestScore.ToString();
        }
    }
}