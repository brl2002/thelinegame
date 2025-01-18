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

        [Header("Score Settings")]
        [SerializeField] private float m_ScoringFactor = 2f;

        [Header("Player Settings")]
        [SerializeField] private Vector2 m_PlayerLocalScale;
        
        [Header("ObjectPool Settings")]
        [SerializeField] private int m_RoadBlockPoolCount;

        [Header("Booster Settings")]
        [SerializeField] private float m_BoosterSpawningRate;
        [SerializeField] private float m_InvincibilityBoosterTime;
        [SerializeField] private float m_ShrinkerBoosterTime;
        [SerializeField] private float m_ShrinkerPlayerScaleFactor;

        public int VisibleRowCount => m_VisibleRowCount;
        public int BlockCountPerRow => m_BlockCountPerRow;
        public float ScrollSpeed => m_ScrollSpeed;
        public float ScoringFactor => m_ScoringFactor;
        public Vector2 PlayerLocalScale => m_PlayerLocalScale;
        public int RoadBlockPoolCount => m_RoadBlockPoolCount;
        public float BoosterSpawningRate => m_BoosterSpawningRate;
        public float InvincibilityBoosterTime => m_InvincibilityBoosterTime;
        public float ShrinkerBoosterTime => m_ShrinkerBoosterTime;
        public float ShrinkerPlayerScaleFactor => m_ShrinkerPlayerScaleFactor;

        public float RowHeight { get; private set; }
        public float BlockWidth { get; private set; }

        public void Configure(float screenHeight, float screenWidth)
        {
            RowHeight = screenHeight / m_VisibleRowCount;
            BlockWidth = screenWidth / m_BlockCountPerRow;
        }
    }
}