using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.BatlleScreenButtons
{
    public class SideMenu : MonoBehaviour
    {
        #region EXPOSED IN INSPECTOR

        [SerializeField] private Image _statsPanel;
        [SerializeField] Button _open;
        [SerializeField] Button _close;
        [SerializeField] private Animator _animator;

        #endregion

        #region PRIVATE
        private readonly int Anim_State = Animator.StringToHash("State");
        #endregion

        #region PROPERTIES

        public bool IsStatsPanelActive;

        #endregion

        #region PUBLIC METHODS
        public void OpenSideMenu()
        {
            PlayOpenAnimation();

            _open.gameObject.SetActive(false);
            _close.gameObject.SetActive(true);
        }

        public void CloseSideMenu()
        {
            PlayCloseAnimation();

            _open.gameObject.SetActive(true);
            _close.gameObject.SetActive(false);
        }

        public void OpenStatsPanel() => _statsPanel.gameObject.SetActive(IsStatsPanelActive = !IsStatsPanelActive);

        #endregion

        #region PRIVATE METHODS
        private void PlayOpenAnimation() => _animator.SetInteger(Anim_State, 1);

        private void PlayCloseAnimation() => _animator.SetInteger(Anim_State, 2);
        #endregion
    }
}