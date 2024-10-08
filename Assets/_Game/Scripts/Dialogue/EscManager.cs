using _GameTemplate.Scripts.SceneManagement;
using UnityEngine;

namespace Dialogue
{
    public class EscManager : MonoBehaviour
    {
        private float delay = 2f;

        private void Update()
        {
            delay -= Time.deltaTime;
            if (delay > 0)
                return;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                CustomSceneManager.LoadScene("Menu");
            }
        }
    }
}