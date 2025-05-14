using UnityEngine;
using System.Collections;

public class StatueActivator : MonoBehaviour
{
    public Animator animator;
    public AudioClip[] statueSounds;
    public float[] startDelays;
    public float[] durations;

    private bool playerIsNear = false;
    private bool isPlaying = false;

    void Update()
    {
        if (playerIsNear && Input.GetKeyDown(KeyCode.P) && !isPlaying)
        {
            StartCoroutine(PlayStatueSequence());
        }
    }

    IEnumerator PlayStatueSequence()
    {
        isPlaying = true;
        animator.SetTrigger("StartPlay");

        float maxTime = 0f;

        for (int i = 0; i < statueSounds.Length; i++)
        {
            StartCoroutine(PlaySoundWithDelay(i));
            float soundEndTime = startDelays[i] + durations[i];
            if (soundEndTime > maxTime) maxTime = soundEndTime;
        }

        yield return new WaitForSeconds(maxTime);

        animator.SetTrigger("ReturnToIdle"); // You must set this in Animator
        isPlaying = false;
    }

    IEnumerator PlaySoundWithDelay(int index)
    {
        yield return new WaitForSeconds(startDelays[index]);

        GameObject tempAudio = new GameObject("TempAudio" + index);
        AudioSource source = tempAudio.AddComponent<AudioSource>();
        source.clip = statueSounds[index];
        source.Play();

        yield return new WaitForSeconds(durations[index]);

        source.Stop();
        Destroy(tempAudio);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsNear = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerIsNear = false;
    }
}
