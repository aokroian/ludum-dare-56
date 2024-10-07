using UnityEngine;

namespace Enemy.Events
{
    public class EnemyRepositionEvent
    {
        public Vector3 Pos { get; private set; }
        
        public EnemyRepositionEvent(Vector3 pos)
        {
            Pos = pos;
        }
    }
}