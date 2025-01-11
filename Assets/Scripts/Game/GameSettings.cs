using UnityEngine;

namespace Game
{
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private int m_HorizontalBlockCount;
        [SerializeField] private int m_VerticalBlockCount;
        [SerializeField] private int m_ExtraVerticalBlockCount;

        public int HorizontalBlockCount => m_HorizontalBlockCount;
        public int VerticalBlockCount => m_VerticalBlockCount;
        public int ExtraVerticalBlockCount => m_ExtraVerticalBlockCount;
        
        public float ScreenHeight { get; private set; }
        public float ScreenWidth { get; private set; }
        public float BlockHeight { get; private set; }
        public float BlockWidth { get; private set; }

        public void Configure(Camera camera)
        {
            ScreenHeight = Camera.main.orthographicSize * 2;
            ScreenWidth = ScreenHeight * Camera.main.pixelWidth / Camera.main.pixelHeight;
            BlockHeight = ScreenHeight / VerticalBlockCount;
            BlockWidth = ScreenWidth / HorizontalBlockCount;
        }
    }
}