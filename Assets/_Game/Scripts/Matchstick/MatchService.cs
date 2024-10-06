using System;

namespace Matchstick
{
    public class MatchService
    {
        private readonly Config _config;
        
        

        [Serializable]
        public class Config
        {
            public int startMatches;
        }
    }
}