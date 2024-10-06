using UnityEngine;

namespace Level
{
    public class PropSpawnSurface : MonoBehaviour
    {
        [field: SerializeField] public BoxCollider Bounds { get; private set; }
        private static bool IsDebug => true;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // draw socket size using bounds
            if (!IsDebug || !Bounds)
                return;
            Gizmos.color = Color.yellow * .5f;
            var worldSpaceBoundsCenter = transform.TransformPoint(Bounds.center);
            Gizmos.DrawCube(worldSpaceBoundsCenter, Bounds.size);
        }
#endif
    }
}