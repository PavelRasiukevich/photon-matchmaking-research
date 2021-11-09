using System.Collections;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class DotsGroup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _dot_1;
        [SerializeField] private TextMeshProUGUI _dot_2;
        [SerializeField] private TextMeshProUGUI _dot_3;

        private void Awake() => StartCoroutine(nameof(Process));

        private IEnumerator Process()
        {
            while (true)
            {
                ActivateDot_1();
                yield return new WaitForSeconds(.7f);
                ActivateDot_2();
                yield return new WaitForSeconds(.7f);
                ActivateDot_3();
                yield return new WaitForSeconds(.7f);

                DeactivateDot_1();
                DeactivateDot_2();
                DeactivateDot_3();
            }
        }

        private void ActivateDot_1() => _dot_1.gameObject.SetActive(true);

        private void ActivateDot_2() => _dot_2.gameObject.SetActive(true);

        private void ActivateDot_3() => _dot_3.gameObject.SetActive(true);

        private void DeactivateDot_1() => _dot_1.gameObject.SetActive(false);

        private void DeactivateDot_2() => _dot_2.gameObject.SetActive(false);

        private void DeactivateDot_3() => _dot_3.gameObject.SetActive(false);
    }
}