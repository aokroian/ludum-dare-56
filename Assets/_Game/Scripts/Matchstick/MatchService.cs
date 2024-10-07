using System;
using Matchstick.Events;
using UnityEngine;
using Zenject;

namespace Matchstick
{
    public class MatchService
    {
        private Config _config;
        
        public int Matches { get; private set; }
        
        private SignalBus _signalBus;
        private float _nextLightTime;
        
        [Inject]
        private void Initialize(Config config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            
            Matches = _config.startMatches;
        }
        
        public float TryLight()
        {
            if (Matches <= 0)
            {
                return 0;
            }
            
            if (_nextLightTime > Time.time)
            {
                return 0;
            }
            _nextLightTime = Time.time + _config.duration + _config.delay;
            
            Matches--;
            _signalBus.Fire(new MatchLitEvent());
            return _config.duration;
        }

        [Serializable]
        public class Config
        {
            public int startMatches;
            public float duration;
            public float delay;
        }
    }
}