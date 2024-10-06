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

        private static async void LoadSceneInner(string sceneName, bool additive, bool showLoadingScreen,
            bool waitLoadingScreenAnimation, Action onLoad)
        {
            if (showLoadingScreen)
            {
                var loadingScreenTask = ShowLoadingScreen();
                if (waitLoadingScreenAnimation)
                    await loadingScreenTask;
            }

            var mode = additive ? LoadSceneMode.Additive : LoadSceneMode.Single;
            await SceneManager.LoadSceneAsync(sceneName, mode);
            if (showLoadingScreen)
            {
                var hideLoadingScreenTask = HideLoadingScreen();
                if (waitLoadingScreenAnimation)
                    await hideLoadingScreenTask;
            }
            onLoad?.Invoke();
        }

        private static Awaitable ShowLoadingScreen()
        {
            var task = Awaitable.AwaitableAsyncMethodBuilder.Create().Task;
            if (LoadingScreen != null)
            {
                task = LoadingScreen.Show();
            }
            else
                Debug.LogWarning("Loading screen is not set");

            return task;
        }
        
        private static Awaitable HideLoadingScreen()
        {
            var task = Awaitable.AwaitableAsyncMethodBuilder.Create().Task;
            if (LoadingScreen != null)
            {
                task = LoadingScreen.Hide();
            }
            else
                Debug.LogWarning("Loading screen is not set");

            return task;
        }
    }
}