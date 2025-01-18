using System;
using UnityEngine;

namespace Game
{
    public class Player : MonoBehaviour
    {
        public event Action OnRoadBlockCollision;
        public event Action OnStateChangeActive;
        public event Action OnStateChangeInactive;
        
        public bool IsInvincible { get; private set; }
        public bool IsShrink { get; private set; }
        
        [SerializeField] private LayerMask m_RoadBlockLayer;

        private bool m_IsPaused = false;

        private float m_CurrentTimeLeft;
        private Vector2 m_OriginalScale;

        public void Reset()
        {
            m_IsPaused = false;
            IsInvincible = false;
            IsShrink = false;
            m_CurrentTimeLeft = 0;
            transform.localScale = m_OriginalScale;
        }

        public void Resume()
        {
            m_IsPaused = false;
        }

        public void Pause()
        {
            m_IsPaused = true;
        }

        public void MakeInvincible(float time)
        {
            IsInvincible = true;
            m_CurrentTimeLeft = time;
            OnStateChangeActive?.Invoke();
        }

        public void Shrink(float shrinkFactor, float time)
        {
            IsShrink = true;
            m_CurrentTimeLeft = time;
            transform.localScale = m_OriginalScale * shrinkFactor;
            OnStateChangeActive?.Invoke();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!IsInvincible && (m_RoadBlockLayer & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
            {
                OnRoadBlockCollision?.Invoke();
            }
        }

        private void Start()
        {
            m_OriginalScale = transform.localScale;
        }

        private void Update()
        {
            if (!m_IsPaused && (IsInvincible || IsShrink))
            {
                m_CurrentTimeLeft -= Time.deltaTime;
                if (m_CurrentTimeLeft <= 0)
                {
                    if (IsInvincible)
                    {
                        IsInvincible = false;
                    }

                    if (IsShrink)
                    {
                        IsShrink = false;
                        transform.localScale = m_OriginalScale;
                    }
                    
                    m_CurrentTimeLeft = 0;
                    OnStateChangeInactive?.Invoke();
                }
            }
        }
    }
}