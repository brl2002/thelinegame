using System;
using System.Collections;
using UnityEngine;

namespace Booster
{
    public enum BoosterType
    {
        Invincibility,
        Shrinker
    }
    
    public class BoosterPickUp : MonoBehaviour
    {
        [SerializeField] private Color m_StartingColor;
        [SerializeField] private Color m_TargetColor;
        [SerializeField] private LayerMask m_PlayerMask;

        public event Action<BoosterPickUp> OnPickUpBooster;

        public BoosterType BoosterType { get; private set; }
        public float BoosterTime { get; private set; }

        private SpriteRenderer m_SpriteRenderer;
        private Coroutine m_FlickerCoroutine;

        public void SetBoosterType(BoosterType boosterType)
        {
            BoosterType = boosterType;
        }

        public void SetBoosterTime(float time)
        {
            BoosterTime = time;
        }

        public void StartFlicker()
        {
            m_FlickerCoroutine = StartCoroutine(Flicker(1, 3));
        }

        public void StopFlicker()
        {
            if (m_FlickerCoroutine != null)
            {
                StopCoroutine(m_FlickerCoroutine);
                m_FlickerCoroutine = null;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if ((m_PlayerMask & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
            {
                OnPickUpBooster?.Invoke(this);
            }
        }

        private IEnumerator Flicker(float secondPerFlicker, int flickerCount)
        {
            while (true)
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
        }
        
        private void Awake()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartFlicker();
        }
    }
}
