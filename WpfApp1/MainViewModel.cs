using Stylet;

namespace MTGApro
{
    public class MainViewModel : Screen
    {
        private readonly IWindowManager _windowManager;

        public MainViewModel(IWindowManager windowManager)
        {
            _windowManager = windowManager;
        }

    }
}