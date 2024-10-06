using System;
using Matchstick.Events;
using Zenject;

namespace Matchstick
{
    public class MatchService
    {
        private Config _config;
        
        public int Matches { get; private set; }
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Initialize(Config config, SignalBus signalBus)
        {
            _config = config;
            _signalBus = signalBus;
            
            Matches = _config.startMatches;
        }
        
        public bool TryLight()
        {
            if (Matches <= 0)
            {
                return false;
            }
            
            Matches--;
            _signalBus.Fire(new MatchLitEvent());
            return true;
        }

        [Serializable]
        public class Config
        {
            public int startMatches;
        }
    }
}