using UnityEngine;

public class OpenAudioTask : MonoBehaviour
{
    [SerializeField] private AudioSource audio1;
    [SerializeField] private AudioSource audio2;

    // TODO: Switch to SoundManager
    void Start()
    {
        audio1.Play();
        audio2.PlayDelayed(3.0f);
    }
}