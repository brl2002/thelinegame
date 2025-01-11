using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Systems
{
    public class SystemsContainer : MonoBehaviour
    {
        [SerializeField] private ASystem[] m_Systems;

        public T GetSystem<T>() where T : ASystem
        {
            foreach (var system in m_Systems)
            {
                if (system is T) return system as T;
            }
            return null;
        }

        private void Awake()
        {
            foreach (var system in m_Systems)
            {
                system.Initialize();
            }
        }
    }
}