using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public abstract ScreenType Type { get; }

        public void ShowScreen() => gameObject.SetActive(true);

        public void HideScreen() => gameObject.SetActive(false);
    }
}