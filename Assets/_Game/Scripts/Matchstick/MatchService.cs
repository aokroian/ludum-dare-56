using System;
using Zenject;

namespace Matchstick
{
    public class MatchService
    {
        private Config _config;
        
        public int Matches { get; private set; }
        
        [Inject]
        private void Initialize(Config config)
        {
            _config = config;
            
            Matches = _config.startMatches;
        }
        
        public bool TryLight()
        {
            if (Matches <= 0)
            {
                return false;
            }
            
            Matches--;
            return true;
        }

        [Serializable]
        public class Config
        {
            public int startMatches;
        }
    }
}