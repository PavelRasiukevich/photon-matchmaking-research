using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenuButtons
{
    public class MainMenuButton : Button
    {
        public void Activate() => gameObject.SetActive(true);

        public void Deactivate() => gameObject.SetActive(false);
    
    }
}