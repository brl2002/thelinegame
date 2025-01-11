using System;
using UnityEngine;

namespace Road
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class RoadGuardBlock : MonoBehaviour
    {
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;
        
        public bool IsEnabled { get; private set; }

        public void SetIsEnabled(bool isEnabled)
        {
            if (isEnabled != IsEnabled)
            {
                m_SpriteRenderer.enabled = isEnabled;
                m_BoxCollider2D.enabled = isEnabled;
            }
            IsEnabled = isEnabled;
        }
        
        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
        }
    }
}