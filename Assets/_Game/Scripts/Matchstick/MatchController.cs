using InputUtils;
using UnityEngine;
using Zenject;

namespace Matchstick
{
    public class MatchController : MonoBehaviour
    {
        [SerializeField] private Matchstick matchPrefab;
        
        [Inject]
        private PlayerInputsService _playerInputService;
        
        [Inject]
        private MatchService _matchService;
        
        
        private Matchstick _currentMatch;
        
        private void Update()
        {
            Light();
        }

        public void Light()
        {
            if (!_playerInputService.CurrentState.matchstick) 
                return;
        
            _playerInputService.CurrentState.matchstick = false;
            
            if (_currentMatch is not null)
                return;

            var duration = _matchService.TryLight(PutOut);
            if (duration == 0)
                return;

            _currentMatch = Instantiate(matchPrefab, transform);
            _currentMatch.transform.localPosition = Vector3.zero;
            _currentMatch.transform.localRotation = Quaternion.identity;
            _currentMatch.Light(duration, () =>
            {
                Destroy(_currentMatch.gameObject);
                _currentMatch = null;
            });
        }

        private void PutOut()
        {
            if (_currentMatch is not null)
            {
                Destroy(_currentMatch.gameObject);
                _currentMatch = null;
            }
        }
    }
}