using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Game;
using Systems;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Road
{
    public class RoadSystem : ASystem
    {
        public bool IsRunning => m_IsRunning;
        public float ScrollDistance => m_ScrollDistance;
        
        [Header("Debug")] 
        [SerializeField] private bool m_IsDebugging = false;

        [Inject] private SystemsContainer m_SystemsContainer;
        [Inject] private GameSettings m_GameSettings;
        [Inject] private RoadBlockRowGroupFactory m_RoadBlockRowGroupFactory;

        private ObjectPool<RoadBlockRowGroup> m_RoadBlockRowGroupObjectPool;
        private LinkedList<RoadBlockRowGroup> m_RoadBlockRowGroups;
        private Transform m_RoadParentTransform;
        private Camera m_MainCamera;
        private PlayerSpawnSystem m_PlayerSpawnSystem;

        private int m_TotalRowCount;

        // Road generation state
        private bool m_IsHorizontalSegment = true;
        private int m_CurrentSegmentLength = 0;
        private int m_SegmentCounter = 0;
        private int m_VerticalColumnIndex = 0;

        // Horizontal road pattern properties
        private int m_HorizontalStartIndex;
        private int m_HorizontalEndIndex;

        // Toggle for horizontal road direction
        private bool m_NextHorizontalMovesRight = true;
        private bool m_ConnectToRightEnd = true;
        
        private float m_LastRowYPosition;

        private float m_ScrollDistance;

        private bool m_IsRunning = false;

        public void Reset()
        {
            ClearRoadBlocks();
            SetupRoad();
        }

        public void StartRunning()
        {
            if (!m_IsRunning)
            {
                m_IsRunning = true;
            }
        }

        public void StopRunning()
        {
            if (m_IsRunning)
            {
                m_IsRunning = false;   
            }
        }

        public RoadBlock GetRandomDisabledRoadBlock()
        {
            return m_RoadBlockRowGroups.Last.Value.GetDisabledRoadBlock();
        }

        public RoadBlock GetRandomDisabledRoadBlock(int rowGroupIndex, int roadBlockIndex)
        {
            return m_RoadBlockRowGroups.ElementAt(rowGroupIndex).GetRoadBlock(roadBlockIndex);
        }
        
        public void ScrollRows(float scrollDistance)
        {
            m_ScrollDistance += scrollDistance;
            foreach (var rowGroup in m_RoadBlockRowGroups)
            {
                rowGroup.transform.Translate(0, -scrollDistance, 0);
            }
        }

        public override void Initialize()
        {
            m_MainCamera = Camera.main;

            m_PlayerSpawnSystem = m_SystemsContainer.GetSystem<PlayerSpawnSystem>();

            m_RoadParentTransform = new GameObject("RoadParent").transform;
            m_RoadBlockRowGroupObjectPool = new ObjectPool<RoadBlockRowGroup>(m_RoadParentTransform);
            for (int i = 0; i < m_GameSettings.RoadBlockPoolCount; i++)
            {
                m_RoadBlockRowGroupObjectPool.AddToPool(m_RoadBlockRowGroupFactory.CreateRowGroup(m_RoadParentTransform, 0, OnRoadBlockCollidedWithPlayer));
            }
            
            m_RoadBlockRowGroups = new LinkedList<RoadBlockRowGroup>();
        }

        private void OnRoadBlockCollidedWithPlayer(RoadBlock roadBlock)
        {
            if (m_PlayerSpawnSystem.Player.IsInvincible)
            {
                roadBlock.SetBlockActive(false);
            }
            else
            {
                roadBlock.Flicker();
            }
        }

        private void ClearRoadBlocks()
        {
            foreach (var roadBlockRowGroup in m_RoadBlockRowGroups)
            {
                m_RoadBlockRowGroupObjectPool.ReturnToPool(roadBlockRowGroup);
            }
            m_RoadBlockRowGroups.Clear();
        }

        private void SetupRoad()
        {
            m_LastRowYPosition = m_MainCamera.transform.position.y - m_MainCamera.orthographicSize;

            m_ScrollDistance = 0;
            
            InitializeStartingRoadGuardBlock();
            ConfigureNextSegment();
        }

        private void TryRecycleRows()
        {
            RoadBlockRowGroup firstRowGroup = m_RoadBlockRowGroups.First.Value;

            // Check if the first row has moved completely off the bottom of the screen
            if (firstRowGroup.transform.position.y < m_MainCamera.transform.position.y - m_MainCamera.orthographicSize - m_GameSettings.RowHeight)
            {
                RoadBlockRowGroup newLastRowGroup = m_RoadBlockRowGroupObjectPool.GetFromPool();
                
                // Get the Y position of the last row group
                RoadBlockRowGroup lastRowGroup = m_RoadBlockRowGroups.Last.Value;
                float newYPosition = lastRowGroup.transform.position.y + m_GameSettings.RowHeight;
                
                newLastRowGroup.transform.position = new Vector3(0, newYPosition, 0);

                ApplyRoadPattern(newLastRowGroup);
                
                m_RoadBlockRowGroups.RemoveFirst();
                m_RoadBlockRowGroupObjectPool.ReturnToPool(firstRowGroup);
                
                m_RoadBlockRowGroups.AddLast(newLastRowGroup);
            }
        }
        
        private void ConfigureNextSegment()
        {
            if (m_IsHorizontalSegment)
            {
                // Transition to a vertical segment
                m_IsHorizontalSegment = false;

                // If this is the first vertical segment, connect with the middle column
                if (m_HorizontalStartIndex == 0 && m_HorizontalEndIndex == 0)
                {
                    m_VerticalColumnIndex = m_GameSettings.BlockCountPerRow / 2; // Center column
                }
                else
                {
                    // Use horizontal indices for subsequent vertical segments
                    m_VerticalColumnIndex = m_ConnectToRightEnd ? m_HorizontalEndIndex : m_HorizontalStartIndex;
                }

                // Vertical segment spans multiple rows
                m_CurrentSegmentLength = Random.Range(3, 7);

                if (m_IsDebugging)
                {
                    Debug.Log($"Vertical Segment - Connecting at Column: {m_VerticalColumnIndex}");
                }
            }
            else
            {
                // Transition to a horizontal segment
                m_IsHorizontalSegment = true;

                // Horizontal segment spans two rows
                m_CurrentSegmentLength = 2; // Two row groups for horizontal segments
                m_LastRowYPosition += m_GameSettings.RowHeight;

                // Ensure horizontal segments align with the vertical segment
                if (m_NextHorizontalMovesRight)
                {
                    m_HorizontalStartIndex = m_VerticalColumnIndex;
                    m_HorizontalEndIndex = Mathf.Min(
                        m_HorizontalStartIndex + Random.Range(2, m_GameSettings.BlockCountPerRow - m_HorizontalStartIndex),
                        m_GameSettings.BlockCountPerRow - 2
                    );
                }
                else
                {
                    m_HorizontalEndIndex = m_VerticalColumnIndex;
                    m_HorizontalStartIndex = Mathf.Max(
                        m_HorizontalEndIndex - Random.Range(2, m_HorizontalEndIndex + 1),
                        1
                    );
                }

                // Toggle the direction for the next horizontal pattern
                m_ConnectToRightEnd = m_NextHorizontalMovesRight;
                m_NextHorizontalMovesRight = !m_NextHorizontalMovesRight;

                if (m_IsDebugging)
                {
                    Debug.Log($"Horizontal Segment - Start: {m_HorizontalStartIndex}, End: {m_HorizontalEndIndex}");
                }
            }

            m_SegmentCounter = 0;
        }

        private void ApplyRoadPattern(RoadBlockRowGroup rowGroup)
        {
            if (m_IsHorizontalSegment)
            {
                rowGroup.DisableRoadBlocksInRange(m_HorizontalStartIndex, m_HorizontalEndIndex);
            }
            else
            {
                rowGroup.DisableRoadBlockAtIndex(m_VerticalColumnIndex);
            }

            m_SegmentCounter++;

            if (m_IsDebugging)
            {
                Debug.Log($"Applied pattern: {(m_IsHorizontalSegment ? "Horizontal" : "Vertical")}, RowGroup: {rowGroup.name}, SegmentCounter: {m_SegmentCounter}");
            }

            // End the segment after the required length
            if (m_SegmentCounter >= m_CurrentSegmentLength)
            {
                ConfigureNextSegment();
            }
        }

        private void InitializeStartingRoadGuardBlock()
        {
            int totalStepsForPyramid = Mathf.Min(5, m_GameSettings.VisibleRowCount + 1);
        
            // First two rows are open/road
            for (int i = 0; i < 2; i++)
            {
                RoadBlockRowGroup rowGroup = m_RoadBlockRowGroupObjectPool.GetFromPool();
                rowGroup.DisableAllRoadBlocks();
                rowGroup.transform.position = new Vector3(0, m_LastRowYPosition, 0);
                m_RoadBlockRowGroups.AddLast(rowGroup);
                m_LastRowYPosition += m_GameSettings.RowHeight;
            }
        
            // Gradually narrow down to a single vertical segment like pyramid fashion and each step consists of two row groups
            int currentStartIndex = 0;
            int currentEndIndex = m_GameSettings.BlockCountPerRow - 1;
        
            for (int step = 0; step < totalStepsForPyramid; step++)
            {
                for (int row = 0; row < 2; row++)
                {
                    RoadBlockRowGroup rowGroup = m_RoadBlockRowGroupObjectPool.GetFromPool();
                    rowGroup.transform.position = new Vector3(0, m_LastRowYPosition, 0);
        
                    // Narrow the void by reducing the start and end indices
                    rowGroup.DisableRoadBlocksInRange(currentStartIndex, currentEndIndex);
        
                    m_RoadBlockRowGroups.AddLast(rowGroup);
                    m_LastRowYPosition += m_GameSettings.RowHeight;
                }
                
                currentStartIndex++;
                currentEndIndex--;
        
                // Ensure the road doesn't narrow beyond a single column
                if (currentStartIndex >= currentEndIndex)
                {
                    currentStartIndex = currentEndIndex = m_GameSettings.BlockCountPerRow / 2; // Center column
                }
            }
        
            // Add the first vertical segment to connect with the pyramid's top
            m_VerticalColumnIndex = m_GameSettings.BlockCountPerRow / 2;

            if (m_IsDebugging)
            {
                Debug.Log($"First vertical column index set to {m_VerticalColumnIndex}");
            }
        
            // Align the first vertical segment with the top of the pyramid
            RoadBlockRowGroup verticalRowGroup = m_RoadBlockRowGroupObjectPool.GetFromPool();
            verticalRowGroup.transform.position = new Vector3(0, m_LastRowYPosition, 0);
            verticalRowGroup.DisableRoadBlockAtIndex(m_VerticalColumnIndex);
            m_RoadBlockRowGroups.AddLast(verticalRowGroup);
        
            // Update the last row position for further road generation
            m_LastRowYPosition += m_GameSettings.RowHeight;
        
            // Explicitly configure the state for the first vertical segment
            m_IsHorizontalSegment = false; // next segment is vertical
            m_SegmentCounter = 0;
        }

        private void Update()
        {
            if (m_IsRunning)
            {
                TryRecycleRows();
            }
        }
    }
}