using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Level
{
    public class PropSurface : MonoBehaviour
    {
        [field: SerializeField] public BoxCollider BoundsCollider { get; private set; }
        [field: SerializeField] public List<Prop> AllowedProps { get; private set; }

        public Prop SelectedProp { get; private set; }
        private static bool IsDebug => true;

        public bool IsPropAllowed(PropKind kind)
        {
            return AllowedProps.Any(p => p.Kind == kind);
        }

        public void SelectRandomProp(List<PropKind> exclude)
        {
            var filtered = AllowedProps.Where(p => !exclude.Contains(p.Kind)).ToList();
            SelectedProp = null;
            if (filtered.Count == 0)
            {
                Debug.LogWarning("No available props to select after excluding all of them");
                return;
            }

            var randomIndex = Random.Range(0, filtered.Count);
            var selectedProp = filtered[randomIndex];
            SelectProp(selectedProp.Kind);
        }

        public void SelectProp(PropKind kind)
        {
            if (kind == PropKind.None)
            {
                foreach (var prop in AllowedProps)
                    prop.gameObject.SetActive(false);
                SelectedProp = null;
                return;
            }

            var selectedProp = AllowedProps.FirstOrDefault(p => p.Kind == kind);
            if (!selectedProp)
            {
                Debug.LogError($"Prop with kind {kind} is not allowed on this surface");
                return;
            }

            selectedProp.SelectRandomVariation();
            foreach (var prop in AllowedProps)
                prop.gameObject.SetActive(prop == selectedProp);
            SelectedProp = selectedProp;
            SelectedProp.surface = this;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // draw socket size using bounds
            if (!IsDebug || !BoundsCollider)
                return;
            Gizmos.color = Color.yellow * .5f;
            var worldSpaceBoundsCenter = transform.TransformPoint(BoundsCollider.center);
            Gizmos.DrawCube(worldSpaceBoundsCenter, BoundsCollider.size);
        }
#endif
    }
}