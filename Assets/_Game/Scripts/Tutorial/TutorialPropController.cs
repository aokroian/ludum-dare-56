using Enemy;
using Level;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialPropController : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private EnemyService _enemyService;
        [Inject] private LevelController _levelController;

        private void OnDestroy()
        {
            ResetAll();
        }

        public void ResetAll()
        {
            _levelController.ResetProps(0);
        }

        public void SpawnFirstProp()
        {
            _levelController.ResetProps(1);
            _enemyService.ResetEnemies();
        }

        public void SpawnSecondProp()
        {
        }
    }
}