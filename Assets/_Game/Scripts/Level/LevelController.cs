using System.Collections.Generic;
using System.Linq;
using Matchstick.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        [field: SerializeField] public List<PropSurface> PropSurfaces { get; private set; }
        [field: SerializeField] public List<Prop> ActiveProps { get; private set; }

        [SerializeField] private Material propOutlineMaterial;

        private SignalBus _signalBus;
        private Material _propsOutlineMaterial;
        private Color _targetOutlineColor;

        private void Update()
        {
            if (_propsOutlineMaterial) _propsOutlineMaterial.color = _targetOutlineColor;
        }

        [Inject]
        private void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<MatchLitEvent>(OnMatchLit);
            _signalBus.Subscribe<MatchWentOutEvent>(OnMatchWentOut);

            _propsOutlineMaterial = new Material(propOutlineMaterial);
            foreach (var ap in PropSurfaces.SelectMany(ps => ps.AllowedProps))
                ap.SetOutlineMaterial(_propsOutlineMaterial);
        }

        private void OnMatchWentOut()
        {
            _targetOutlineColor = Color.black;
        }

        private void OnMatchLit()
        {
            _targetOutlineColor = Color.white;
        }

        [Button]
        public void ResetProps()
        {
            ActiveProps.Clear();
            foreach (var propSurface in PropSurfaces)
            {
                var exclude = ActiveProps.ConvertAll(p => p.Kind);
                propSurface.SelectRandomProp(exclude);
                if (propSurface.SelectedProp) ActiveProps.Add(propSurface.SelectedProp);
            }
        }

        [Button]
        public void MovePropToAnotherSurface(Prop prop)
        {
            var oldSurface = prop.surface;
            if (!oldSurface)
            {
                Debug.LogError("Prop is not on any surface");
                return;
            }

            var newSurface = PropSurfaces.FirstOrDefault(s => !s.SelectedProp && s.IsPropAllowed(prop.Kind));

            if (!newSurface)
            {
                Debug.LogError("No available surface to move prop");
                return;
            }

            oldSurface.SelectProp(PropKind.None);
            newSurface.SelectProp(prop.Kind);
        }

#if UNITY_EDITOR
        [Button]
        public void FillPropSurfaces()
        {
            PropSurfaces.Clear();
            var allSurfaces = FindObjectsByType<PropSurface>(FindObjectsSortMode.None);
            foreach (var propSurface in allSurfaces)
                PropSurfaces.Add(propSurface);
        }

#endif
    }
}