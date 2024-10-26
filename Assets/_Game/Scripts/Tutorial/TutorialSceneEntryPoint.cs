using UnityEngine;

namespace Tutorial
{
    public class TutorialSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] private TutorialController tutorialController;

        private void Start()
        {
            Debug.Log("Tutorial scene loaded");
            tutorialController.StartTutorial();
        }
    }
}