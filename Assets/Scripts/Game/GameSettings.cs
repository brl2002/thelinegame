using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [Header("Game Settings")]
        [SerializeField] private int m_VisibleRowCount = 10;
        [SerializeField] private int m_BlockCountPerRow = 9;
        [SerializeField] private float m_ScrollSpeed = 2.0f;

        public int VisibleRowCount => m_VisibleRowCount;
        public int BlockCountPerRow => m_BlockCountPerRow;
        public float ScrollSpeed => m_ScrollSpeed;

        public float RowHeight { get; private set; }
        public float BlockWidth { get; private set; }

        public void Configure(Camera camera)
        {
            if (camera == null)
            {
                Debug.LogError("Camera is null. Please ensure a camera is provided.");
                return;
            }

            float screenHeight = camera.orthographicSize * 2;
            float screenWidth = screenHeight * camera.pixelWidth / camera.pixelHeight;

            RowHeight = screenHeight / m_VisibleRowCount;
            BlockWidth = screenWidth / m_BlockCountPerRow;
        }
    }
}