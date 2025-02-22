using UnityEngine;

//code borrowed and modified by Hooson on youtube https://www.youtube.com/watch?v=tfzwyNS1LUY

public class MenusBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject pauseMenu;

    private int sceneToContinue;
    private int currentSceneIndex;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    
    public void Resume()
    {
        mainMenu.SetActive(false);
        shopMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void Home()
    {
        mainMenu.SetActive(true);
        Time.timeScale = 1f;
    }
    
    public void Shop()
    {
        shopMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
