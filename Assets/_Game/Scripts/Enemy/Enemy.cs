using System;
using Level;
using Matchstick.Events;
using UnityEngine;
using Zenject;

namespace Enemy
{
    public class Enemy: MonoBehaviour
    {
        public Prop Prop { get; private set; }
        
        private SignalBus _signalBus;
        private Action<Prop> _moveProp;


        public void Init(Prop prop, SignalBus signalBus, Action<Prop> moveProp)
        {
            _moveProp = moveProp;
            _signalBus = signalBus;
            Prop = prop;
            _signalBus.Subscribe<MatchWentOutEvent>(OnMatchWentOut);
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
    }
}