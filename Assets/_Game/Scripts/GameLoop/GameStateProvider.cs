using UnityEngine;

namespace GameLoop
{
    public class GameStateProvider
    {
        private const string Key = "MainData";

        public GameStateData GameState { get; private set; }

        public void LoadGameState()
        {
            if (PlayerPrefs.HasKey(Key) && !string.IsNullOrEmpty(PlayerPrefs.GetString(Key)))
                GameState = JsonUtility.FromJson<GameStateData>(PlayerPrefs.GetString(Key));
            else
                GameState = CreateNewState();
        }

        public void SaveGameState()
        {
            PlayerPrefs.SetString(Key, JsonUtility.ToJson(GameState));
        }

        private GameStateData CreateNewState()
        {
            return new GameStateData
            {
                night = 1
            };
        }
    }
}