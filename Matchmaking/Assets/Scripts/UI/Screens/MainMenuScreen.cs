using Assets.Scripts.Core;
using Assets.Scripts.Utils;

namespace Assets.Scripts.UI.Screens
{
    public class MainMenuScreen : BaseScreen
    {
        public override ScreenType Type => ScreenType.MainMenu;

        public void SwitchToSearchScreen() => ScreenHolder.SetCurrentScreen(ScreenType.Search).ShowScreen();
    }
}