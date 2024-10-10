using _GameTemplate.Scripts.SceneManagement;
using GameLoop;
using GameLoop.Events;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Zenject;

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

        [SerializeField] private GameObject mainPanel;
        [SerializeField] private RectTransform settingsPanel;
        [SerializeField] private RectTransform creditsPanel;

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Button clearProgressBtn;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private Slider mouseSensitivitySlider;
        [SerializeField] private TMP_Dropdown graphicsDropdown;

        private SignalBus _signalBus;
        private GameStateProvider _gameStateProvider;

        [Inject]
        private void Initialize(SignalBus signalBus, GameStateProvider gameStateProvider)
        {
            _signalBus = signalBus;
            _gameStateProvider = gameStateProvider;
        }

        private void Start()
        {
            startGameBtn.onClick.AddListener(StartGame);
            quitBtn.onClick.AddListener(QuitGame);
            settingsBtn.onClick.AddListener(OpenSettings);
            backFromSettingsBtn.onClick.AddListener(CloseSettings);
            creditsBtn.onClick.AddListener(OpenCredits);
            backFromCreditsBtn.onClick.AddListener(CloseCredits);
            clearProgressBtn.onClick.AddListener(ClearGameProgress);

            volumeSlider.value = PlayerPrefs.GetFloat(Strings.SoundVolumeKey, 1f);
            mouseSensitivitySlider.value = PlayerPrefs.GetFloat(Strings.MouseSensitivityKey, .5f);

            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
            mouseSensitivitySlider.onValueChanged.AddListener(OnMouseSensitivitySliderChanged);

            var graphicsNames = QualitySettings.names;
            graphicsDropdown.options.Clear();
            foreach (var n in graphicsNames)
            {
                graphicsDropdown.options.Add(new TMP_Dropdown.OptionData(n));
            }

            graphicsDropdown.value = PlayerPrefs.GetInt(Strings.GraphicsQualityKey, QualitySettings.names.Length - 1);
            graphicsDropdown.onValueChanged.AddListener(OnGraphicsQualityChanged);
            OnVolumeSliderChanged(volumeSlider.value);
            OnMouseSensitivitySliderChanged(mouseSensitivitySlider.value);
            OnGraphicsQualityChanged(graphicsDropdown.value);

#if UNITY_WEBGl
            quitBtn.gameObject.SetActive(false);
#endif

            _signalBus.Fire<MenuSceneLoadedEvent>();

            mainPanel.SetActive(true);
        }

        private void ClearGameProgress()
        {
            _gameStateProvider.ClearGameState();
        }

        private void StartGame()
        {
            _signalBus.Fire<GameStartPressedEvent>();
            mainPanel.SetActive(false);
            settingsBtn.interactable = false;
            creditsBtn.interactable = false;
            quitBtn.interactable = false;
            startGameBtn.interactable = false;
            Invoke(nameof(LoadGameScene), 3f);
        }

        private void LoadGameScene()
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

        private void OnVolumeSliderChanged(float valuePercent)
        {
            var minAudio = -40;
            var maxAudio = 0;
            if (valuePercent == 0)
                minAudio = -80;
            var value = minAudio + (maxAudio - minAudio) * valuePercent;

            audioMixer.SetFloat("Volume", value);
            PlayerPrefs.SetFloat(Strings.SoundVolumeKey, valuePercent);
        }

        private void OnMouseSensitivitySliderChanged(float value)
        {
            if (value < 0.03f) value = 0.03f;
            PlayerPrefs.SetFloat(Strings.MouseSensitivityKey, value);
        }

        private void OnGraphicsQualityChanged(int value)
        {
            PlayerPrefs.SetInt(Strings.GraphicsQualityKey, value);
            QualitySettings.SetQualityLevel(value);
        }
    }
}