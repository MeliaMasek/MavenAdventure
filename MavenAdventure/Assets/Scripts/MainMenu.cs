using UnityEngine;
using UnityEngine.SceneManagement;

//code borrowed and modified by Hooson on youtube https://www.youtube.com/watch?v=tfzwyNS1LUY
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject indexMenu;

    private int sceneToContinue;
    private int currentSceneIndex;

    public void Resume()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void Index()
    {
        mainMenu.SetActive(false);
        indexMenu.SetActive(true);
        Time.timeScale = 1f;
    }
    
    public void IndexResume()
    {
        indexMenu.SetActive(false);
        Time.timeScale = 1f;
    }
}