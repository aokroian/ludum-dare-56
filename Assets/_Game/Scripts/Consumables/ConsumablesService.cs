using System;
using Zenject;

namespace Consumables
{
    public class ConsumablesService
    {
        public int Rounds { get; private set; }
        public int Matches { get; private set; }
        
        private Config _config;
        
        [Inject]
        public void Initialize(Config config)
        {
            _config = config;
            Rounds = _config.Rounds;
            Matches = _config.Matches;
        }
        
        public void Reset()
        {
            Rounds = _config.Rounds;
            Matches = _config.Matches;
        }
        
        public void UseMatch()
        {
            Matches--;
        }
        
        public void UseRound()
        {
            Rounds--;
        }
        
        [Serializable]
        public class Config
        {
            public int Rounds;
            public int Matches;
        }
    }
}