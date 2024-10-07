using System;
using System.Collections;
using MimicSpace;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Cutscenes
{
    public class MenuSceneLampAnim : MonoBehaviour
    {
        [MinMaxSlider(1f, 6f)]
        [SerializeField] private Vector2 delayRange;
        [SerializeField] private GameObject lamp;
        [SerializeField] private GameObject mimic;
        [SerializeField] private Movement mimicMovement;

        private float _delay;

        private bool isMimicLeft = false;
        private bool isMimicForward = false;

        private void Update()
        {
            _delay -= Time.deltaTime;
            if (_delay <= 0)
            {
                _delay = UnityEngine.Random.Range(delayRange.x, delayRange.y);
                StopAllCoroutines();
                StartCoroutine(ToggleCoroutine(UnityEngine.Random.Range(1, 5)));
            }

            var x = isMimicLeft ? -.2f : .2f;
            var z = isMimicForward ? .2f : -.2f;
            mimicMovement.input = new Vector2(x, z);
            isMimicForward = !isMimicForward;
            isMimicLeft = !isMimicLeft;
        }

        private IEnumerator ToggleCoroutine(int numberOfTimes)
        {
            while (numberOfTimes-- > 0)
            {
                lamp.SetActive(!lamp.activeSelf);
                if (!lamp.activeSelf)
                {
                    var isMimicActive = UnityEngine.Random.value > .5f;
                    mimic.SetActive(isMimicActive);
                }
                else
                {
                    mimic.SetActive(false);
                }

                var delay = UnityEngine.Random.Range(.35f, .55f);
                yield return new WaitForSeconds(delay);
            }

            lamp.SetActive(true);
            mimic.SetActive(false);
        }
    }
}