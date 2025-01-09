using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockGenerator : MonoBehaviour
{
    [SerializeField] private GameObject m_RoadBlockPrefab;
    
    private void Awake()
    {
        SetupRoadBlocks();
    }

    private void SetupRoadBlocks()
    {
        int horizontalBlockCount = 8;
        int verticalBlockCount = 10;
        int extraVerticalBlockCount = 2;

        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Camera.main.pixelWidth / Camera.main.pixelHeight;
        float blockHeight = screenHeight / verticalBlockCount;
        float blockWidth = screenWidth / horizontalBlockCount;
        float xStartPos = screenWidth / 2f * -1f + blockWidth / 2f;
        float yStartPos = Camera.main.orthographicSize - blockHeight / 2f + extraVerticalBlockCount * blockHeight;
        
        float yPos = yStartPos;

        for (int j = 0; j < verticalBlockCount + extraVerticalBlockCount; j++)
        {
            float xPos = xStartPos;
            for (int i = 0; i < horizontalBlockCount; i++)
            {
                GameObject go = Instantiate(m_RoadBlockPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
                go.transform.localScale = new Vector3(blockWidth, go.transform.localScale.y, 1);
                xPos += blockWidth;
            }

            yPos -= blockHeight;
        }
    }
}