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
        [SerializeField] private Button tutorialBtn;

        [SerializeField] private GameObject mainPanel;
        [SerializeField] private RectTransform settingsPanel;
        [SerializeField] private RectTransform creditsPanel;

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Button clearProgressBtn;
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TextMeshProUGUI volumeValueTmp;
        [SerializeField] private Slider lookSensSlider;
        [SerializeField] private TextMeshProUGUI lookSensValueTmp;

        [SerializeField] private Button mediumGraphicsBtn;
        [SerializeField] private Button highGraphicsBtn;
        [SerializeField] private Button ultraGraphicsBtn;
        [SerializeField] private TextMeshProUGUI mediumGraphicsValueTmp;
        [SerializeField] private TextMeshProUGUI highGraphicsValueTmp;
        [SerializeField] private TextMeshProUGUI ultraGraphicsValueTmp;

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
            tutorialBtn.onClick.AddListener(StartTutorial);

            volumeSlider.value = PlayerPrefs.GetFloat(Strings.SoundVolumeKey, 1f);
            lookSensSlider.value = PlayerPrefs.GetFloat(Strings.MouseSensitivityKey, .5f);

            volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
            lookSensSlider.onValueChanged.AddListener(OnMouseSensitivitySliderChanged);
            volumeValueTmp.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";

            mediumGraphicsBtn.onClick.AddListener(() => { OnGraphicsQualityChanged(1); });
            highGraphicsBtn.onClick.AddListener(() => { OnGraphicsQualityChanged(2); });
            ultraGraphicsBtn.onClick.AddListener(() => { OnGraphicsQualityChanged(3); });

            OnVolumeSliderChanged(volumeSlider.value);
            OnMouseSensitivitySliderChanged(lookSensSlider.value);
            var currentGraphics = PlayerPrefs.GetInt(Strings.GraphicsQualityKey, 2);
            OnGraphicsQualityChanged(currentGraphics);

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

        private void StartTutorial()
        {
            CustomSceneManager.LoadScene("Tutorial");
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
            volumeValueTmp.text = Mathf.RoundToInt(valuePercent * 100) + "%";
        }

        private void OnMouseSensitivitySliderChanged(float value)
        {
            if (value < 0.03f) value = 0.03f;
            PlayerPrefs.SetFloat(Strings.MouseSensitivityKey, value);
            lookSensValueTmp.text = Mathf.RoundToInt(value * 100) + "%";
        }

        private void OnGraphicsQualityChanged(int value)
        {
            PlayerPrefs.SetInt(Strings.GraphicsQualityKey, value);
            QualitySettings.SetQualityLevel(value);
            mediumGraphicsValueTmp.gameObject.SetActive(value == 1);
            highGraphicsValueTmp.gameObject.SetActive(value == 2);
            ultraGraphicsValueTmp.gameObject.SetActive(value == 3);
        }
    }
}