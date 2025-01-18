using UI;
using VContainer;
using VContainer.Unity;

public class UILifetimeScope : LifetimeScope
{
    private UIModel m_UIModel;
    
    protected override void Configure(IContainerBuilder builder)
    {
        m_UIModel = new UIModel();
        builder.RegisterInstance(m_UIModel);
    }
}
