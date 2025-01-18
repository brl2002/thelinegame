using System;
using UnityEngine;
using VContainer;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private MainView m_MainView;
        [SerializeField] private GameView m_GameView;
        [SerializeField] private GameOverView m_GameOverView;
        
        [Inject] private UIModel m_UIModel;
        
        public void TransitionToGameOver()
        {
            m_UIModel.SetState(UIModel.UIState.GameOver);
            UpdateViews();
        }

        public void ShowMenu()
        {
            m_UIModel.SetState(UIModel.UIState.Main);
            UpdateViews();
        }

        public void TryAgain()
        {
            m_UIModel.SetState(UIModel.UIState.Game);
            UpdateViews();
        }

        public void KeepGoing()
        {
            m_UIModel.SetState(UIModel.UIState.Game);
            UpdateViews();
        }

        private void UpdateViews()
        {
            m_MainView.SetActive(m_UIModel.CurrentState == UIModel.UIState.Main);
            m_GameView.SetActive(m_UIModel.CurrentState == UIModel.UIState.Game);
            m_GameOverView.SetActive(m_UIModel.CurrentState == UIModel.UIState.GameOver);
        }
        
        private void Awake()
        {
            m_UIModel.SetState(UIModel.UIState.Game);
        }

        private void Start()
        {
            m_MainView.SetActive(false);
            m_GameView.SetActive(true);
            m_GameOverView.SetActive(false);
        }
    }
}