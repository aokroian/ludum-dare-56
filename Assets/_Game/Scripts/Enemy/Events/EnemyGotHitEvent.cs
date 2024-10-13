using UnityEngine;

namespace Enemy.Events
{
    public class EnemyGotHitEvent
    {
        public Vector3 Pos { get; private set; }
        
        public EnemyGotHitEvent(Vector3 pos)
        {
            Pos = pos;
        }
    }
}