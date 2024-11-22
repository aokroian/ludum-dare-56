using Sirenix.OdinInspector;
using UnityEngine;

namespace Sound
{
    [CreateAssetMenu(menuName = "Game/Sound/SoundsConfig", fileName = "SoundsConfig")]
    public class SoundsConfig : SerializedScriptableObject
    {
        public AudioClip gameStartPressedSound;
        public AudioClip winSound;
        public AudioClip[] getUpFromBedSounds;

        public AudioClip[] newNightSounds;
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

        // enemy sounds
        public AudioClip[] enemyDiedSounds;
        public AudioClip[] enemyPrepareToAttackSounds;
        public AudioClip[] enemyAttackSounds;
        public AudioClip[] enemyRepositionedSounds;
    }
}