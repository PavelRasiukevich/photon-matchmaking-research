using TMPro;

namespace Assets.Scripts.UI.MainMenuButtons
{
    public class LoadingLabel : TextMeshProUGUI
    {
        public void Activate() => gameObject.SetActive(true);

        public void Deactivate() => gameObject.SetActive(false);
    }
}