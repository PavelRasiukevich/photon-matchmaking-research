using Assets.Scripts.Core;
using Assets.Scripts.Utils;

namespace Assets.Scripts.UI.Screens
{
    public class ShopScreen : BaseScreen
    {
        public override ScreenType Type => ScreenType.Shop;

        public void Exit() => ScreenHolder.SetCurrentScreen(ScreenType.MainMenu).ShowScreen();
    }
}