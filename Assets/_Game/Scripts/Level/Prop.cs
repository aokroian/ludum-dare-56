using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Level
{
    public class Prop : MonoBehaviour
    {
        [field: SerializeField] public PropKind Kind { get; private set; }
        [field: SerializeField] public SphereCollider Bounds { get; private set; }

        [SerializeField] private List<GameObject> allVariations;

        public bool IsDeadEnemy
        {
            get => _isDeadEnemy;
            set
            {
                if (value)
                    SetOutline(false);
                _isDeadEnemy = value;
            }
        }
        private bool _isDeadEnemy;

        public Vector3 BoundsCenter => transform.TransformPoint(Bounds.center);

        [HideInInspector] public PropSurface surface;

        public static List<Prop> AllActiveProps = new();

        public GameObject CurrentVariation { get; private set; }

        private Material _outlineMaterial;
        private Material _defaultMaterial;

        private void Awake()
        {
            if (!CurrentVariation)
                SelectRandomVariation();

            _defaultMaterial = CurrentVariation.GetComponent<MeshRenderer>().material;
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

        public void SetOutlineMaterial(Material mat) => _outlineMaterial = mat;

        public void SetOutline(bool value)
        {
            if (IsDeadEnemy)
                return;
            CurrentVariation.GetComponent<MeshRenderer>().material = value ? _outlineMaterial : _defaultMaterial;
        }

        public void SelectRandomVariation()
        {
            IsDeadEnemy = false;
            var exceptCurrent = CurrentVariation
                ? allVariations.Where(v => v != CurrentVariation).ToList()
                : allVariations;
            var randomIndex = Random.Range(0, exceptCurrent.Count);
            CurrentVariation = exceptCurrent[randomIndex];

            foreach (var variation in allVariations)
                variation.SetActive(variation == CurrentVariation);
        }

#if UNITY_EDITOR
        public void FillDataFields()
        {
            allVariations.Clear();
            foreach (Transform variation in transform)
                allVariations.Add(variation.gameObject);
        }

        private void OnDrawGizmos()
        {
            // draw socket size using bounds
            if (!Bounds)
                return;
            Gizmos.color = Color.green;
            var worldSpaceBoundsCenter = transform.TransformPoint(Bounds.center);
            Gizmos.DrawWireSphere(worldSpaceBoundsCenter, Bounds.radius);
        }
#endif
    }
}