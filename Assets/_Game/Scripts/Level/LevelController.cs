using System;
using System.Collections.Generic;
using System.Linq;
using Matchstick.Events;
using R3;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private GameObject directionalLight;

        [field: SerializeField] public List<PropSurface> PropSurfaces { get; private set; }
        [field: SerializeField] public List<Prop> ActiveProps { get; private set; }

        [SerializeField] private Material propOutlineMaterial;

        private SignalBus _signalBus;
        private Material _propsOutlineMaterial;

        [Inject]
        private void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<MatchLitEvent>(OnMatchLit);
            _signalBus.Subscribe<MatchWentOutEvent>(OnMatchWentOut);
            SetPropsOutlineMaterial();
            directionalLight.SetActive(true);
        }

        private void OnMatchWentOut()
        {
            directionalLight.SetActive(false);
        }

        private void OnMatchLit()
        {
            directionalLight.SetActive(true);
        }

        public void ResetProps(int count)
        {
            ActiveProps.Clear();
            foreach (var propSurface in PropSurfaces)
            {
                propSurface.SelectProp(PropKind.None);
            }

            if (count == 0)
                return;

            PropSurfaces = PropSurfaces.OrderBy(_ => Guid.NewGuid()).ToList();

            foreach (var propSurface in PropSurfaces)
            {
                if (count <= 0)
                    break;
                var propsToExclude = ActiveProps.ConvertAll(p => p.Kind);
                propSurface.SelectRandomProp(propsToExclude);
                if (propSurface.SelectedProp) ActiveProps.Add(propSurface.SelectedProp);
                count--;
            }
        }

        public Prop MovePropToAnotherSurface(Prop prop)
        {
            var oldSurface = prop.surface;
            var newSurface = PropSurfaces.FirstOrDefault(s =>
                s != oldSurface && !s.SelectedProp && s.IsPropAllowed(prop.Kind));

            if (!newSurface)
            {
                Debug.LogError("No available surface to move prop. Returning the same prop");
                return prop;
            }

            oldSurface.SelectProp(PropKind.None);
            newSurface.SelectProp(prop.Kind);
            return newSurface.SelectedProp;
        }

        private void SetPropsOutlineMaterial()
        {
            _propsOutlineMaterial = new Material(propOutlineMaterial);
            var allProps = PropSurfaces.SelectMany(ps => ps.AllowedProps).ToList();
            foreach (var p in allProps)
                p.SetOutlineMaterial(_propsOutlineMaterial);
        }

#if UNITY_EDITOR
        [Button]
        public void FillDataFields()
        {
            PropSurfaces.Clear();
            var allSurfaces = FindObjectsByType<PropSurface>(FindObjectsSortMode.None);
            foreach (var propSurface in allSurfaces)
            {
                PropSurfaces.Add(propSurface);
                propSurface.FillDataFields();
            }
        }

        [Button]
        public void CheckPropUsages()
        {
            var result = new Dictionary<PropKind, int>();
            foreach (var entry in Enum.GetValues(typeof(PropKind)).Cast<PropKind>())
            {
                result[entry] = PropSurfaces.Count(ps => ps.AllowedProps.FirstOrDefault(p => p.Kind == entry));
            }

            // log everything
            foreach (var entry in result)
            {
                Debug.Log($"{entry.Key}: {entry.Value}");
            }
        }

        [Button]
        public void CheckForDuplicatePropsInOneSurface()
        {
            foreach (var ps in PropSurfaces)
            {
                var duplicates = ps.AllowedProps.GroupBy(p => p.Kind).Where(g => g.Count() > 1).ToList();
                if (duplicates.Count > 0)
                {
                    Debug.LogError($"Duplicate props found on surface {ps.name}");
                    foreach (var duplicate in duplicates)
                    {
                        Debug.LogError($"Duplicate prop kind: {duplicate.Key}");
                    }
                }
            }
        }


        private IDisposable _movingTest;

        [Button]
        public void TestPropMoving()
        {
            _movingTest?.Dispose();
            var iterations = 1000;
            _movingTest = Observable.Interval(TimeSpan.FromSeconds(.05f))
                .Take(iterations)
                .Subscribe(x =>
                {
                    for (var propsCount = 1; propsCount < 10; propsCount++)
                    {
                        ResetProps(propsCount);
                        foreach (var prop in ActiveProps)
                        {
                            var newProp = MovePropToAnotherSurface(prop);
                            if (prop == newProp)
                            {
                                Debug.LogError("Prop was not moved to another surface");
                                break;
                            }
                        }
                    }
                });
        }

#endif
    }
}