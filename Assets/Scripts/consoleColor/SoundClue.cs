using UnityEngine;

public class SoundCluePlayer : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] possibleClips;
    public SequenceManager sequenceManager;

    // This is called from ColorButton when player steps on it
    public void PlayVoiceClue()
    {
        if (sequenceManager == null || possibleClips.Length == 0) return;

        int index = Random.Range(0, possibleClips.Length);
        AudioClip clip = possibleClips[index];
        source.PlayOneShot(clip);

        string clipName = clip.name;
        sequenceManager.AddNoteToSequence(clipName);
    }
}
