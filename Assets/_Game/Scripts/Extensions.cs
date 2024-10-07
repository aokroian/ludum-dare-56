using UnityEngine;
using UnityEngine.Audio;

public static class Extensions
{
    public static T UnityRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public static void CustomPlayClipAtPoint(
        AudioClip clip,
        Vector3 position,
        AudioMixerGroup group = null,
        float volume = 1.0f
    )
    {
        if (!clip) return;
        var gameObject = new GameObject("One shot audio");
        gameObject.transform.position = position;
        var audioSource = (AudioSource)gameObject.AddComponent(typeof(AudioSource));
        if (group) audioSource.outputAudioMixerGroup = group;
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.volume = volume;
        audioSource.Play();
        Object.Destroy(gameObject, clip.length * (Time.timeScale < 0.009999999776482582 ? 0.01f : Time.timeScale));
    }
}