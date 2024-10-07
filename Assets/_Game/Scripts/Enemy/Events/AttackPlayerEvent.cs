using UnityEngine;

namespace Enemy.Events
{
    public class AttackPlayerEvent
    {
        public Transform EnemyTransform { get; private set; }
        
        public AttackPlayerEvent(Transform enemyTransform)
        {
            EnemyTransform = enemyTransform;
        }
    }
}