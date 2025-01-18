using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Button button;
    public AudioSource audioSource;
    public AudioClip audioClip;

    void Start()
    {
        // Add a listener to the button's click event
        button.onClick.AddListener(PlayAudioOnClick);
    }

    void PlayAudioOnClick()
    {
        // Check if an audio clip is assigned and the audio source is not already playing
        if (audioClip != null && !audioSource.isPlaying)
        {
            // Assign the audio clip to the audio source and play it
            audioSource.clip = audioClip;
            audioSource.Play();
        }
    }
}