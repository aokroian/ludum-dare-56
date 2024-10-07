using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cutscenes
{
    public class MenuSceneLampAnim : MonoBehaviour
    {
        [MinMaxSlider(.5f, 4f)]
        [SerializeField] private Vector2 delayRange;
        [SerializeField] private GameObject lamp;

        private float _delay;

        private void Update()
        {
            _delay -= Time.deltaTime;
            if (_delay <= 0)
            {
                _delay = UnityEngine.Random.Range(delayRange.x, delayRange.y);
                StopAllCoroutines();
                StartCoroutine(ToggleCoroutine(UnityEngine.Random.Range(1, 5)));
            }
        }

        private IEnumerator ToggleCoroutine(int numberOfTimes)
        {
            while (numberOfTimes-- > 0)
            {
                lamp.SetActive(!lamp.activeSelf);
                var delay = UnityEngine.Random.Range(.1f, .2f);
                yield return new WaitForSeconds(delay);
            }

            lamp.SetActive(true);
        }
    }
}