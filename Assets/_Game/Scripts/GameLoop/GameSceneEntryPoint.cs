using Enemy;
using Level;
using UnityEngine;
using Zenject;

namespace GameLoop
{
    public class GameSceneEntryPoint : MonoBehaviour
    {
        [Inject]
        private LevelController _levelController;
        
        [Inject]
        private EnemyService _enemyService;
        
        private void Start()
        {
            _levelController.ResetProps();
            
        }
    }
}