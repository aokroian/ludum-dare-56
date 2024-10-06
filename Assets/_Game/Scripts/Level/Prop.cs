using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Level
{
    public class Prop : MonoBehaviour
    {
        [field: SerializeField] public PropKind Kind { get; private set; }
        [field: SerializeField] public SphereCollider Bounds { get; private set; }
        [SerializeField] private List<GameObject> allVariations;

        [HideInInspector] public PropSurface surface;

        private GameObject _currentVariation;

        private static bool IsDebug => true;

        private void Awake()
        {
            if (!_currentVariation)
                SelectRandomVariation();
        }

        public void SelectRandomVariation()
        {
            var exceptCurrent = _currentVariation
                ? allVariations.Where(v => v != _currentVariation).ToList()
                : allVariations;
            var randomIndex = Random.Range(0, exceptCurrent.Count);
            _currentVariation = exceptCurrent[randomIndex];

            foreach (var variation in allVariations)
                variation.SetActive(variation == _currentVariation);
        }

#if UNITY_EDITOR
        [Button]
        public void FillVariations()
        {
            allVariations.Clear();
            foreach (Transform variation in transform)
                allVariations.Add(variation.gameObject);
        }

        private void OnDrawGizmos()
        {
            // draw socket size using bounds
            if (!IsDebug || !Bounds)
                return;
            Gizmos.color = Color.green;
            var worldSpaceBoundsCenter = transform.TransformPoint(Bounds.center);
            Gizmos.DrawWireSphere(worldSpaceBoundsCenter, Bounds.radius);
        }
#endif
    }
}