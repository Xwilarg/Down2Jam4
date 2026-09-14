using UnityEngine;

namespace Down2Jam.Manager
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { private set; get; }

        [SerializeField]
        private AudioSource _sfxAudioSource;

        [SerializeField]
        private AudioClip _hitSfx, _explosionSfx, _validateSfx;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayHit()
        {
            _sfxAudioSource.PlayOneShot(_hitSfx);
        }

        public void PlayExplosion()
        {
            _sfxAudioSource.PlayOneShot(_explosionSfx, .5f);
        }

        public void PlayValidate()
        {
            _sfxAudioSource.PlayOneShot(_validateSfx, .5f);
        }
    }
}
