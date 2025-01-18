using System;
using System.Collections;
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
        public event Action<RoadBlock> OnPlayerCollision;
        
        public bool IsActive { get; private set; }
        
        [SerializeField] private Color m_StartingColor;
        [SerializeField] private Color m_TargetColor;
        [SerializeField] private LayerMask m_PlayerMask;

        private SpriteRenderer m_SpriteRenderer;
        private BoxCollider2D m_BoxCollider2D;

        public void SetBlockActive(bool isActive)
        {
            if (m_SpriteRenderer != null)
            {
                m_SpriteRenderer.enabled = isActive;
            }

            if (m_BoxCollider2D != null)
            {
                m_BoxCollider2D.enabled = isActive;
            }

            IsActive = isActive;
        }

        public void Reset()
        {
            SetBlockActive(true);
            m_SpriteRenderer.color = m_StartingColor;
        }

        public void Flicker()
        {
            StartCoroutine(Flicker(1, 3));
        }

        private IEnumerator Flicker(float secondPerFlicker, int flickerCount)
        {
            for (int i = 0; i < flickerCount; i++)
            {
                Color startColor = m_StartingColor;
                Color targetColor = m_TargetColor;
                float elapsedTime = 0f;
                
                while (elapsedTime < secondPerFlicker / 2f)
                {
                    elapsedTime += Time.deltaTime;
                    m_SpriteRenderer.color = Color.Lerp(startColor, targetColor, elapsedTime / (secondPerFlicker / 2f));
                    yield return null;
                }

                elapsedTime = 0f;
                
                while (elapsedTime < secondPerFlicker / 2f)
                {
                    elapsedTime += Time.deltaTime;
                    m_SpriteRenderer.color = Color.Lerp(targetColor, startColor, elapsedTime / (secondPerFlicker / 2f));
                    yield return null;
                }
                m_SpriteRenderer.color = startColor;
            }
        }
        
        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            m_BoxCollider2D = GetComponent<BoxCollider2D>();
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((m_PlayerMask & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
            {
                OnPlayerCollision?.Invoke(this);
            }
        }
    }
}