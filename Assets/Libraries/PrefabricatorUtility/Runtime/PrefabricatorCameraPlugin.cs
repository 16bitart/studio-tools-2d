using UnityEngine;

namespace PrefabricatorUtility.Runtime
{
    public class PrefabricatorCameraPlugin : MonoBehaviour
    {
        private Camera _cam;
        private float _nearClipPlane => _cam ? _cam.nearClipPlane : 0f;
        [field: SerializeField] public BoundsInt CameraBounds { get; private set; }
        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void OnEnable()
        {
            _cam = GetComponent<Camera>();
        }

        private void GetBounds()
        {
            var minViewport = new Vector3(0, 0, 0);
            var maxViewport = new Vector3(1, 1, 0);

            var minWorld = _cam.ViewportToWorldPoint(minViewport);
            var maxWorld = _cam.ViewportToWorldPoint(maxViewport);
            var center = (minWorld + maxWorld) / 2f;
            var extents = (maxWorld - minWorld) / 2f;
        }
    }
}