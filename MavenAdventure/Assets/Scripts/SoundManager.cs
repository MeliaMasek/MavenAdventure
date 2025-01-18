using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource matchingSound;
    public AudioSource noMatchingSound;
    
    public void PlayMatchingSound()
    {
        matchingSound.Play();    
    }

    public void PlayNoMatchSound()
    {
        noMatchingSound.Play();
    }
}
