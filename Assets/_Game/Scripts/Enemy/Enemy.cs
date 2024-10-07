using System;
using Level;
using Matchstick.Events;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Enemy: MonoBehaviour
    {
        public Prop Prop { get; private set; }
        
        private SignalBus _signalBus;
        private Action<Prop> _moveProp;
        private Action<Enemy> _onEnemyDied;

        public bool Alive { get; private set; } = true;

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<MatchWentOutEvent>(OnMatchWentOut);
        }

        public void Init(Prop prop, SignalBus signalBus, Action<Prop> moveProp, Action<Enemy> onEnemyDied)
        {
            _onEnemyDied = onEnemyDied;
            _moveProp = moveProp;
            _signalBus = signalBus;
            Prop = prop;
            _signalBus.Subscribe<MatchWentOutEvent>(OnMatchWentOut);
        }

        [Button]
        public void Kill()
        {
            if (!Alive)
                return;
            
            Alive = false;
            _onEnemyDied?.Invoke(this);
        }
        
        private void OnMatchWentOut()
        {
            Reposition();
        }

        private void Reposition()
        {
            _moveProp?.Invoke(Prop);
            // TODO: Move prop to a new position
        }
        
#if UNITY_EDITOR
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.5f);
        }
#endif
    }
}