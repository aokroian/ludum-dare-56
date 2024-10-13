using System.Collections;
using MimicSpace;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cutscenes
{
    public class MainMenuHouseAnimation : MonoBehaviour
    {
        [MinMaxSlider(1f, 6f)]
        [SerializeField] private Vector2 delayRange;
        [SerializeField] private GameObject lightObj;
        [SerializeField] private GameObject mimic;
        [SerializeField] private GameObject[] objects;
        [SerializeField] private Movement mimicMovement;

        private float _delay;

        private bool _isMimicLeft;
        private bool _isMimicForward;

        private void Awake()
        {
            ToggleThings(true);
        }

        private void Update()
        {
            _delay -= Time.deltaTime;
            if (_delay <= 0)
            {
                _delay = Random.Range(delayRange.x, delayRange.y);
                StopAllCoroutines();
                StartCoroutine(ToggleThingsCoroutine());
            }

            var x = _isMimicLeft ? -.2f : .2f;
            var z = _isMimicForward ? .2f : -.2f;
            mimicMovement.input = new Vector2(x, z);
            _isMimicForward = !_isMimicForward;
            _isMimicLeft = !_isMimicLeft;
        }

        private IEnumerator ToggleThingsCoroutine()
        {
            var numberOfTimes = Random.Range(1, 5);
            while (numberOfTimes-- > 0)
            {
                ToggleThings();
                yield return new WaitForSeconds(Random.Range(.35f, .55f));
            }

            ToggleThings(true);
        }

        private void ToggleThings(bool forceLightOn = false)
        {
            var prevLightState = lightObj.activeSelf;
            var lightState = forceLightOn || !lightObj.activeSelf;
            lightObj.SetActive(lightState);
            var mimicState = !lightState && Random.value > .5f;
            mimic.SetActive(mimicState);

            if (prevLightState && lightState)
                return;
            var randomObject = objects[Random.Range(0, objects.Length)];
            foreach (var obj in objects)
                obj.SetActive(obj == randomObject && lightState);
        }
    }
}