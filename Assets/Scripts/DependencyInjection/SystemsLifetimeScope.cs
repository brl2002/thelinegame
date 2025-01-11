using Systems;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DependencyInjection
{
    public class SystemsLifetimeScope : LifetimeScope
    {
        [SerializeField] private SystemsContainer m_SystemsContainer;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(m_SystemsContainer);
        }
    }
}