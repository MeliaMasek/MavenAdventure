using UnityEngine;

//Background Music "Game Comedy Interesting Playful Sweet Bright Childish Music" by REDproductions from Pixabay
//Code borrowed and modified from NuffuruDevelopment on Unity Answers https://answers.unity.com/questions/1260393/make-music-continue-playing-through-scenes.html
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private GameObject[] other;
    private bool NotFirst = false;
    private void Awake()
    {
        other = GameObject.FindGameObjectsWithTag("Music");
 
        foreach (GameObject oneOther in other)
        {
            if (oneOther.scene.buildIndex == -1)
            {
                NotFirst = true;
            }
        }
 
        if (NotFirst == true)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
        audioSource = GetComponent<AudioSource>();
    }
 
    public void PlayMusic()
    {
        if (audioSource.isPlaying) return;
        audioSource.Play();
    }
 
    public void StopMusic()
    {
        audioSource.Stop();
    }
}
