using Assets.Scripts.Core;
using Assets.Scripts.Utils;

namespace Assets.Scripts.UI.Screens
{
    public class OptionsScreen : BaseScreen
    {
        public override ScreenType Type => ScreenType.Options;

        public void SwitchToMainMenu() => ScreenHolder.SetCurrentScreen(ScreenType.MainMenu).ShowScreen();
    }
}