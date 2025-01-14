using UnityEngine;

namespace Road
{
    /// <summary>
    /// Each unit representing a block segment that the player isn't supposed to travel on.
    /// Any area/section that is void of RoadBlock-s would be considered road that the player can travel on.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D))]
    public class RoadBlock : MonoBehaviour
    {
        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
        }
        
        public void SetBlockActive(bool isActive)
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            var collider = GetComponent<BoxCollider2D>();

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = isActive;
            }

            if (collider != null)
            {
                collider.enabled = isActive;
            }
        }

        public void SetIsEnabled(bool isEnabled)
        {
            m_SpriteRenderer.enabled = isEnabled;
            m_BoxCollider2D.enabled = isEnabled;
        }

        public void Reset()
        {
            SetIsEnabled(true);
        }
    }
}