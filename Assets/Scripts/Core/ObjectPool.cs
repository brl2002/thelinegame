using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ObjectPool<T> where T : Component
    {
        private readonly List<T> m_Pool;
        private readonly Transform m_Parent;

        public ObjectPool(Transform parent)
        {
            m_Pool = new List<T>();
            this.m_Parent = parent;
        }

        public void AddToPool(T item)
        {
            item.gameObject.SetActive(false);
            m_Pool.Add(item);
        }

        public T GetFromPool()
        {
            if (m_Pool.Count == 0)
            {
                Debug.LogError("Object pool is empty! Ensure sufficient objects are pre-allocated.");
                return null;
            }

            T item = m_Pool[0];
            m_Pool.RemoveAt(0);
            item.gameObject.SetActive(true);
            return item;
        }

        public void ReturnToPool(T item)
        {
            item.gameObject.SetActive(false);
            m_Pool.Add(item);
        }

        public T GetLast()
        {
            if (m_Pool.Count == 0)
            {
                Debug.LogError("Object pool is empty! Cannot retrieve the last object.");
                return null;
            }

            return m_Pool[^1];
        }
    }
}