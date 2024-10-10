using _GameTemplate.Scripts.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue
{
    public class EscManager : MonoBehaviour
    {
        [SerializeField] private Button escButton;
        [SerializeField] private GameObject escText;

        private float delay = 2f;

        private void Start()
        {
#if UNITY_ANDROID || UNITY_IOS
            escButton.gameObject.SetActive(true);
            escText.SetActive(false);
            escButton.onClick.AddListener(ExitToMenu);
#else
            escButton.gameObject.SetActive(false);
            escText.SetActive(true);
#endif
        }

        private void Update()
        {
            delay -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ExitToMenu();
            }
        }

        private void ExitToMenu()
        {
            if (delay > 0)
                return;
            CustomSceneManager.LoadScene("Menu");
        }
    }
}