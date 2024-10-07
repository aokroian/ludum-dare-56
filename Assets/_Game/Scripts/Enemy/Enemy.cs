using System;
using Level;
using Matchstick.Events;
using MimicSpace;
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
        private Mimic _mimicInstance;


        public void Init(Prop prop, SignalBus signalBus, Mimic enemyObject, Action<Prop> moveProp, Action<Enemy> onEnemyDied)
        {
            _mimicInstance = enemyObject;
            _onEnemyDied = onEnemyDied;
            _moveProp = moveProp;
            _signalBus = signalBus;
            Prop = prop;
            _signalBus.Subscribe<MatchWentOutEvent>(OnMatchWentOut);
        }

        public void Kill()
        {
            _onEnemyDied?.Invoke(this);
            Prop.GetComponent<MeshRenderer>().enabled = false;
            _mimicInstance.gameObject.SetActive(false);
            _mimicInstance.transform.position = Prop.transform.position;
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