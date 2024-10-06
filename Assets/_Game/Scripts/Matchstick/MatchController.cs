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
            
            if (!_matchService.TryLight())
                return;
            
            Debug.Log("Matchstick lit");
            
            _currentMatch = Instantiate(matchPrefab, transform);
            _currentMatch.transform.localPosition = Vector3.zero;
            _currentMatch.transform.localRotation = Quaternion.identity;
            _currentMatch.Light(5f, () =>
            {
                Debug.Log("Matchstick burned out");
                Destroy(_currentMatch.gameObject);
                _currentMatch = null;
            });
        }
    }
}