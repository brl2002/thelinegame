namespace UI
{
    public class UIModel
    {
        public enum UIState
        {
            Main,
            Game,
            GameOver
        }

        public UIState CurrentState { get; private set; }
        public int Score { get; private set; }
        public int BestScore { get; private set; }

        public void SetState(UIState newState)
        {
            CurrentState = newState;
        }

        public void SetScore(int score)
        {
            Score = score;
        }

        public void SetBestScore(int bestScore)
        {
            BestScore = bestScore;
        }
    }
}