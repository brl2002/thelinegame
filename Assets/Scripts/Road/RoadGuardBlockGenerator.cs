using Game;
using UnityEngine;
using VContainer;

namespace Road
{
    public class RoadGuardBlockGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject m_RoadGuardBlockPrefab;

        [Inject] private GameSettings m_GameSettings;
    
        public void SetupRoadGuardBlocks()
        {
            GenerateRoadGuardBlocks();
        }

        private void GenerateRoadGuardBlocks()
        {
            int horizontalBlockCount = m_GameSettings.HorizontalBlockCount;
            int verticalBlockCount = m_GameSettings.VerticalBlockCount;
            int extraVerticalBlockCount = m_GameSettings.ExtraVerticalBlockCount;

            float screenHeight = Camera.main.orthographicSize * 2;
            float screenWidth = screenHeight * Camera.main.pixelWidth / Camera.main.pixelHeight;
            float blockHeight = screenHeight / verticalBlockCount;
            float blockWidth = screenWidth / horizontalBlockCount;
            float xStartPos = screenWidth / 2f * -1f + blockWidth / 2f;
            float yStartPos = Camera.main.orthographicSize - blockHeight / 2f + extraVerticalBlockCount * blockHeight;
        
            float yPos = yStartPos;

            GameObject root = new GameObject("RoadGuardBlockGroup");
            for (int j = 0; j < verticalBlockCount + extraVerticalBlockCount; j++)
            {
                GameObject rowGroupObject = new GameObject("RoadGuardBlockRowGroup");
                RoadGuardBlockRowGroup rowGroup = rowGroupObject.AddComponent<RoadGuardBlockRowGroup>();
                rowGroupObject.transform.parent = root.transform;
                float xPos = xStartPos;
                for (int i = 0; i < horizontalBlockCount; i++)
                {
                    GameObject go = Instantiate(m_RoadGuardBlockPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                    go.transform.parent = rowGroupObject.transform;
                    go.transform.localScale = new Vector3(blockWidth, go.transform.localScale.y, 1);
                    xPos += blockWidth;
                }
                
                rowGroup.Setup();

                yPos -= blockHeight;
            }
        }
    }
}