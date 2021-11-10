using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core
{
    public class ScreenHolder : MonoBehaviour
    {
        [SerializeField] private ScreenType _default;

        private static Dictionary<ScreenType, BaseScreen> _dictionary;
        private static BaseScreen _currentScreen;

        private BaseScreen[] _children;

        private void Awake()
        {
            _dictionary = new Dictionary<ScreenType, BaseScreen>();
            _children = GetComponentsInChildren<BaseScreen>(true);

            PopulateDictionary(_dictionary, _children);
            _currentScreen = _dictionary[_default];

            _currentScreen.ShowScreen();
        }

        public static BaseScreen SetCurrentScreen(ScreenType screenType)
        {
            var nextScreen = _dictionary[screenType];

            if (_currentScreen)
                _currentScreen.HideScreen();

            _currentScreen = nextScreen;

            return _currentScreen;
        }

        private void PopulateDictionary(Dictionary<ScreenType, BaseScreen> dictionary, BaseScreen[] array)
        {
            foreach (var screen in array)
                dictionary.Add(screen.Type, screen);
        }
    }
}