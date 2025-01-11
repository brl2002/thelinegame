using System;
using System.Linq;
using UnityEngine;

namespace Road
{
    public class RoadGuardBlockRowGroup : MonoBehaviour
    {
        private RoadGuardBlock[] m_RoadGuardBlocks;

        public void Setup()
        {
            m_RoadGuardBlocks = GetComponentsInChildren<RoadGuardBlock>(true);
        }
    }
}