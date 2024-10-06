using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _GameTemplate.Scripts.SceneManagement
{
    public static class CustomSceneManager
    {
        public static SceneLoadingScreen LoadingScreen;

        public static void LoadScene(string sceneName, bool showLoadingScreen = true,
            bool waitLoadingScreenAnimation = true, Action onLoad = null)
        {
            LoadSceneInner(sceneName, false, showLoadingScreen, waitLoadingScreenAnimation, onLoad);
        }

        public static void LoadSceneAdditive(string sceneName, Action onLoad = null)
        {
            LoadSceneInner(sceneName, true, false, false, onLoad);
        }

        private static void LoadSceneInner(string sceneName, bool additive, bool showLoadingScreen,
            bool waitLoadingScreenAnimation, Action onLoad)
        {
            if (showLoadingScreen)
                ShowLoadingScreen();

            var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            var task = SceneManager.LoadSceneAsync(sceneName, mode);
            task.completed += (op) =>
            {
                Debug.Log(op.isDone + " " + op.progress);
                if (showLoadingScreen)
                    HideLoadingScreen();
                onLoad?.Invoke();
            };
        }

        private static void ShowLoadingScreen()
        {
            if (LoadingScreen != null)
            {
                var task = LoadingScreen.Show();
            }
            else
                Debug.LogWarning("Loading screen is not set");
        }
        
        private static void HideLoadingScreen()
        {
            if (LoadingScreen != null)
            {
                var task = LoadingScreen.Hide();
            }
        }
    }
}