using _GameTemplate.Scripts.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button startGameBtn;
        [SerializeField] private Button settingsBtn;
        [SerializeField] private Button backFromSettingsBtn;
        [SerializeField] private Button quitBtn;
        [SerializeField] private Button creditsBtn;
        [SerializeField] private Button backFromCreditsBtn;

        [SerializeField] private RectTransform settingsPanel;
        [SerializeField] private RectTransform creditsPanel;


        private void Start()
        {
            startGameBtn.onClick.AddListener(StartGame);
            quitBtn.onClick.AddListener(QuitGame);
            settingsBtn.onClick.AddListener(OpenSettings);
            backFromSettingsBtn.onClick.AddListener(CloseSettings);
            creditsBtn.onClick.AddListener(OpenCredits);
            backFromCreditsBtn.onClick.AddListener(CloseCredits);

#if UNITY_WEBGl
            quitBtn.gameObject.SetActive(false);
#endif
        }

        private void StartGame()
        {
            CustomSceneManager.LoadScene("Game");
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void OpenSettings()
        {
            settingsPanel.gameObject.SetActive(true);
        }

        private void CloseSettings()
        {
            settingsPanel.gameObject.SetActive(false);
        }

        private void OpenCredits()
        {
            creditsPanel.gameObject.SetActive(true);
        }

        private void CloseCredits()
        {
            creditsPanel.gameObject.SetActive(false);
        }
    }
}