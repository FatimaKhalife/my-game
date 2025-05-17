using UnityEngine;

public class ButtonSoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip soundClip;

    public void PlaySound()
    {
        if (audioSource != null && soundClip != null)
        {
            audioSource.clip = soundClip;
            audioSource.Play();
        }
    }
}
