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
        
        
        public void Init(Prop prop, SignalBus signalBus)
        {
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
            
            // TODO: Move prop to a new position
        }
    }
}