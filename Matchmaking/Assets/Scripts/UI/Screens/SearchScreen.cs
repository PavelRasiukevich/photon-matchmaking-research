using Assets.Scripts.Core;
using Assets.Scripts.Utils;

namespace Assets.Scripts.UI.Screens
{
    public class SearchScreen : BaseScreen
    {
        public override ScreenType Type => ScreenType.Search;

        public void SwitchToMainMenu() => ScreenHolder.SetCurrentScreen(ScreenType.MainMenu).ShowScreen();
    }
}