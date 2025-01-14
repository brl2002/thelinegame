using UnityEngine;

namespace Road
{
    public class RoadBlockRowGroup : MonoBehaviour
    {
        private RoadBlock[] m_RoadBlocks;

        public void Setup()
        {
            m_RoadBlocks = GetComponentsInChildren<RoadBlock>(true);
        }
        
        public void DisableAllRoadBlocks()
        {
            foreach (var block in m_RoadBlocks)
            {
                block.SetBlockActive(false);
            }
        }

        public void DisableRoadBlocksInRange(int startIndex, int endIndex)
        {
            for (int i = 0; i < m_RoadBlocks.Length; i++)
            {
                bool isRoad = i < startIndex || i > endIndex;
                m_RoadBlocks[i].SetIsEnabled(isRoad);
            }
        }

        public void DisableRoadBlockAtIndex(int columnIndex)
        {
            for (int i = 0; i < m_RoadBlocks.Length; i++)
            {
                // Make sure all blocks in other columns are disabled
                m_RoadBlocks[i].SetIsEnabled(i != columnIndex);
            }
        }

        public void Reset()
        {
            foreach (var block in m_RoadBlocks)
            {
                block.SetIsEnabled(true);
            }
        }
    }
}