using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.BatlleScreenButtons
{
    public class SideMenu : MonoBehaviour
    {
        #region EXPOSED IN INSPECTOR

        [SerializeField] Button _open;
        [SerializeField] Button _close;
        [SerializeField] private Animator _animator;

        #endregion

        #region PRIVATE
        private readonly int Anim_State = Animator.StringToHash("IsOpen");
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
        #endregion

        #region PRIVATE METHODS
        private void PlayOpenAnimation() => _animator.SetBool(Anim_State, true);

        private void PlayCloseAnimation() => _animator.SetBool(Anim_State, false);
        #endregion
    }
}