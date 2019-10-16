using Stylet;

namespace MTGApro
{
    public class Bootstrapper : Bootstrapper<MainViewModel>
    {
        protected override void Configure()
        {
            var viewManager = Container.Get<ViewManager>();
            viewManager.ViewNameSuffix = "Window";
        }
    }
}