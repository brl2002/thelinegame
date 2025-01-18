using Game;
using Road;
using Systems;
using UI;
using UnityEditor;
using UnityEngine;
using VContainer;

namespace Score
{
    public class ScoreManager : MonoBehaviour
    {
        [Inject] private SystemsContainer m_SystemsContainer;
        [Inject] private GameSettings m_GameSettings;
        [Inject] private UIModel m_UIModel;

        private RoadSystem m_RoadSystem;

        public void Reset()
        {
            TrySetBestScore();
            m_UIModel.SetScore(0);
        }

        public void Pause()
        {
            TrySetBestScore();
        }

        private void TrySetBestScore()
        {
            if (m_UIModel.Score > m_UIModel.BestScore)
            {
                m_UIModel.SetBestScore(m_UIModel.Score);
                PlayerPrefs.SetInt(GameConstants.BEST_SCORE_PLAYER_PREFS_KEY, m_UIModel.Score);
            }
        }

        private void Awake()
        {
            m_RoadSystem = m_SystemsContainer.GetSystem<RoadSystem>();
            m_UIModel.SetBestScore(PlayerPrefs.GetInt(GameConstants.BEST_SCORE_PLAYER_PREFS_KEY, 0));
        }

        private void Update()
        {
            m_UIModel.SetScore((int)(m_RoadSystem.ScrollDistance * m_GameSettings.ScoringFactor));
        }

#if UNITY_EDITOR
        [MenuItem("Game/Reset Best Score")]
        private static void ResetBestScore()
        {
            PlayerPrefs.DeleteKey(GameConstants.BEST_SCORE_PLAYER_PREFS_KEY);
            Debug.Log("Successfully deleted best score.");
        }
#endif
    }
}
