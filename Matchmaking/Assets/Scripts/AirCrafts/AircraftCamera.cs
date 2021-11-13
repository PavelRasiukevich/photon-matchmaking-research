using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.AirCrafts
{
    [RequireComponent(typeof(UniversalAdditionalCameraData), typeof(Camera))]
    public class AircraftCamera : MonoBehaviour
    {
        private UniversalAdditionalCameraData _airCraftCamera;

        private void Awake()
        {
            _airCraftCamera = GetComponent<UniversalAdditionalCameraData>();
            var overlayCamera = GameObject.FindWithTag(UtilsConst.UICameraBattle).GetComponent<Camera>();
            _airCraftCamera.cameraStack.Add(overlayCamera);
        }
    }
}