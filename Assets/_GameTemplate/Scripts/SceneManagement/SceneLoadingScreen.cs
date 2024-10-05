using System;
using System.Threading.Tasks;
using _GameTemplate.Scripts.Common;
using DG.Tweening;
using UnityEngine;

namespace _GameTemplate.Scripts.SceneManagement
{
    public class SceneLoadingScreen : SingletonGlobal<SceneLoadingScreen>
    {
        [SerializeField] private Canvas loadingScreen;
        [SerializeField] private CanvasGroup loadingScreenGroup;
        

        private async void Start()
        {
            CustomSceneManager.LoadingScreen = this;
            await Hide();
        }

        public async Awaitable Show(float fadeTime = 0f)
        {
            loadingScreen.gameObject.SetActive(true);
            loadingScreenGroup.alpha = 0f;
            await loadingScreenGroup.DOFade(1f, fadeTime).SetUpdate(true).AsyncWaitForCompletion();
        }
        
        public async Awaitable Hide(float fadeTime = 0f)
        {
            loadingScreenGroup.alpha = 1f;
            await loadingScreenGroup.DOFade(0f, fadeTime).SetUpdate(true).AsyncWaitForCompletion();
            loadingScreen.gameObject.SetActive(false);
        }
        
        public static async Awaitable ShowIfPresent()
        {
            if (Instance != null)
                await Instance.Show();
            else
                Debug.LogWarning("SceneLoadingScreen is not present in the scene");
        }
        
        public static async Awaitable HideIfPresent()
        {
            if (Instance != null)
                await Instance.Hide();
        }
    }
}