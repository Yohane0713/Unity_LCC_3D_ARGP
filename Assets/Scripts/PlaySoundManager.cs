using UnityEngine;

namespace Mtaka
{
    public class PlaySoundManager : MonoBehaviour
    {
        private void PlaySound(AudioClip clip)
        {
            SoundManager.instance.PlaySound(clip);
        }
    }
}