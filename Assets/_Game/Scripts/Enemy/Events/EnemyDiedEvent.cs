using UnityEngine;

namespace Enemy.Events
{
    public class EnemyDiedEvent
    {
        public Vector3 Pos { get; private set; }
        
        public EnemyDiedEvent(Vector3 pos)
        {
            Pos = pos;
        }
    }
}