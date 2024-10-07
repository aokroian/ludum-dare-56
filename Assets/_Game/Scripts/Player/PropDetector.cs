using System.Linq;
using Level;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player
{
    public class PropDetector : MonoBehaviour
    {
        [MinMaxSlider(0, 1, true)]
        public Vector2 detectionX = new(.3f, .7f);
        [MinMaxSlider(0, 1, true)]
        public Vector2 detectionY = new(.3f, .7f);

        public LayerMask detectionLayerMask;
        public Prop Detected { get; private set; }
        private const float MaxDetectionDistance = 100f;
        private Camera _cam;

        private Prop _prevDetected;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void Update()
        {
            var detected = TryDetectWithRaycast();
            if (!detected)
                detected = TryDetectWithViewportPoint();
            Detected = detected;

            if (_prevDetected || Detected != _prevDetected)
                _prevDetected.SetOutline(false);

            if (Detected && Detected != _prevDetected)
                Detected.SetOutline(true);

            _prevDetected = Detected;
        }

        private Prop TryDetectWithRaycast()
        {
            var ray = _cam.ScreenPointToRay(new Vector3(Screen.width * .5f, Screen.height * .5f, 0));
            Physics.Raycast(ray, out var hit, MaxDetectionDistance, detectionLayerMask);
            var prop = hit.collider ? Prop.AllActiveProps.FirstOrDefault(p => p.Bounds == hit.collider) : null;
            return prop;
        }

        private Prop TryDetectWithViewportPoint()
        {
            var eligibleItems = Prop.AllActiveProps.Where(IsEligibleForDetectionByViewportPoint)
                .ToList();
            if (eligibleItems.Count == 0)
                return null;
            eligibleItems.Sort(
                (elem1, elem2) =>
                {
                    var camTransform = _cam.transform;
                    var camForward = camTransform.forward;
                    var camPosition = camTransform.position;
                    var angle1 = Vector3.Angle(camForward, elem1.BoundsCenter - camPosition);
                    var angle2 = Vector3.Angle(camForward, elem2.BoundsCenter - camPosition);
                    return angle1.CompareTo(angle2);
                });
            return eligibleItems[0];
        }

        private bool IsEligibleForDetectionByViewportPoint(Prop detectable)
        {
            // check if the object is close enough to the center of the screen
            var point = _cam.WorldToViewportPoint(detectable.BoundsCenter);
            return point.z > 0f && point.x > detectionX.x && point.x < detectionX.y && point.y > detectionY.x &&
                   point.y < detectionY.y;
        }
    }
}