using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class Prop : SerializedMonoBehaviour
    {
        [field: SerializeField] public PropKind Kind { get; private set; }
        [field: SerializeField] public SphereCollider Bounds { get; private set; }

        [ShowInInspector] public static Material outlineMaterial;

        [SerializeField] private List<GameObject> allVariations;

        public Vector3 BoundsCenter => transform.TransformPoint(Bounds.center);

        [HideInInspector] public PropSurface surface;

        private GameObject _currentVariation;
        public static List<Prop> AllActiveProps;

        private Material _defaultMaterial;

        private static bool IsDebug => true;

        private void Awake()
        {
            if (!_currentVariation)
                SelectRandomVariation();

            _defaultMaterial = _currentVariation.GetComponent<MeshRenderer>().material;
        }

        private void OnEnable()
        {
            AllActiveProps ??= new List<Prop>();
            AllActiveProps.Add(this);
        }

        private void OnDisable()
        {
            AllActiveProps.Remove(this);
        }
        
        public void SetOutline(bool value)
        {
            // if (value)
                // _currentVariation.GetComponent<MeshRenderer>().material = outlineMaterial;
            // else
                // _currentVariation.GetComponent<MeshRenderer>().material = _defaultMaterial;
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