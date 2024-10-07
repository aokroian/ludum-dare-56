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
        public AudioClip[] matchStickSounds;

        // weapon sounds
        public AudioClip[] shootSounds;
        public AudioClip[] cockSounds;
        public AudioClip[] shootNoAmmoSounds;
        public AudioClip[] bulletCasingsSounds;
        //
        
    }
}