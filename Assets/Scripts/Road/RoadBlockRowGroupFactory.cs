using System;
using Game;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Road
{
    public class RoadBlockRowGroupFactory
    {
        private const string ROAD_GUARD_BLOCK_ROW_GROUP_NAME = "RoadGuardBlockRowGroup";
        
        private readonly IObjectResolver m_Container;
        private readonly GameSettings m_GameSettings;
        private readonly RoadBlock m_RoadBlockPrefab;

        public RoadBlockRowGroupFactory(IObjectResolver container, GameSettings gameSettings, RoadBlock roadBlockPrefab)
        {
            m_Container = container ?? throw new ArgumentNullException(nameof(container));
            m_GameSettings = gameSettings ?? throw new ArgumentNullException(nameof(gameSettings));
            m_RoadBlockPrefab = roadBlockPrefab ?? throw new ArgumentNullException(nameof(roadBlockPrefab));
        }

        public RoadBlockRowGroup CreateRowGroup(Transform parent, float rowYPosition)
        {
            GameObject rowGroupObject = new GameObject(ROAD_GUARD_BLOCK_ROW_GROUP_NAME);
            rowGroupObject.transform.parent = parent;
            rowGroupObject.transform.position = new Vector3(0, rowYPosition, 0);

            RoadBlockRowGroup rowGroup = rowGroupObject.AddComponent<RoadBlockRowGroup>();

            for (int i = 0; i < m_GameSettings.BlockCountPerRow; i++)
            {
                float xPosition = -m_GameSettings.BlockWidth * (m_GameSettings.BlockCountPerRow - 1) / 2f + i * m_GameSettings.BlockWidth;
                RoadBlock block = CreateRoadBlock(rowGroupObject.transform, xPosition);
                block.transform.localScale = new Vector3(m_GameSettings.BlockWidth, m_GameSettings.RowHeight, 1);
            }
            
            rowGroup.Setup();

            return rowGroup;
        }

        private RoadBlock CreateRoadBlock(Transform parent, float xPosition)
        {
            RoadBlock block = m_Container.Instantiate(m_RoadBlockPrefab, parent);
            block.transform.localPosition = new Vector3(xPosition, 0, 0);
            return block;
        }
    }
}