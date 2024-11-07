using System.Collections.Generic;
using Level;
using Tutorial.States;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialPropController : MonoBehaviour
    {
        [SerializeField] private Prop propPrefab;
        [SerializeField] private Transform spawnPoint1;
        [SerializeField] private Transform spawnPoint2;

        private bool _isInit;
        private TutorialController _controller;
        private TutorialState _prevState;
        private readonly List<GameObject> _spawnedObjects = new();
        [Inject] private SignalBus _signalBus;

        private void OnDestroy()
        {
            UnInit();
        }

        public void Init(TutorialController controller)
        {
            if (_isInit) UnInit();
            _controller = controller;
            _isInit = true;
        }

        public void UnInit()
        {
            if (!_isInit) return;
            _controller = null;
            _prevState = null;
            _spawnedObjects.ForEach(Destroy);
            _spawnedObjects.Clear();
            _isInit = false;
        }

        private void Update()
        {
            if (!_isInit) return;
            if (_prevState is not MimicTutorialState && _controller.CurrentState is MimicTutorialState)
            {
                // Show the mimic
                var prop = Instantiate(propPrefab, spawnPoint1.position, spawnPoint1.rotation);
                _spawnedObjects.Add(prop.gameObject);
            }

            if (_prevState is MimicTutorialState && _controller.CurrentState is not MimicTutorialState)
            {
                UnInit();
            }
        }
    }
}