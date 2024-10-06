using System;
using _GameTemplate.Scripts.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button quitBtn;
        
        [SerializeField] private RectTransform settingsPanel;

        private void Start()
        {
            startGameBtn.onClick.AddListener(StartGame);
            quitBtn.onClick.AddListener(QuitGame);
        }
        
        private void StartGame()
        {
            CustomSceneManager.LoadScene("Game");
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
    }
}