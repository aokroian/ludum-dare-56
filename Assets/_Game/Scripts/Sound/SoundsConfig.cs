using Sirenix.OdinInspector;
using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(menuName = "Game/Sound/SoundsConfig", fileName = "SoundsConfig")]
    public class SoundsConfig : SerializedScriptableObject
    {
        public AudioClip[] playerStepSounds;
        public AudioClip[] gameMusicClips;
        public AudioClip[] menuMusicClips;
    }
}