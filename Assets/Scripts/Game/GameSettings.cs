using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 1)]
    public class GameSettings : ScriptableObject
    {
        [Header("RoadBlock Settings")]
        [SerializeField] private int m_VisibleRowCount = 10;
        [SerializeField] private int m_BlockCountPerRow = 9;
        [SerializeField] private float m_ScrollSpeed = 2.0f;

        [Header("Player Settings")]
        [SerializeField] private Vector2 m_PlayerLocalScale;

        public int VisibleRowCount => m_VisibleRowCount;
        public int BlockCountPerRow => m_BlockCountPerRow;
        public float ScrollSpeed => m_ScrollSpeed;
        public Vector2 PlayerLocalScale => m_PlayerLocalScale;

        public float RowHeight { get; private set; }
        public float BlockWidth { get; private set; }

        public void Configure(float screenHeight, float screenWidth)
        {
            RowHeight = screenHeight / m_VisibleRowCount;
            BlockWidth = screenWidth / m_BlockCountPerRow;
        }
    }
}