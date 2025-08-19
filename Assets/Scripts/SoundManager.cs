using UnityEngine;

namespace Mtaka
{
    [RequireComponent (typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        public static SoundManager instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<SoundManager>();
                return _instance;
            }
        }

        private AudioSource aud;

        private void Awake()
        {
            aud = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip sound, float min = 0.5f, float max = 1f)
        {
            float volume = Random.Range (min, max);
            aud.PlayOneShot(sound, volume);
        }
    }
}