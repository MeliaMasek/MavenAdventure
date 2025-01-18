using UnityEngine;

//code borrowed and modified by Hooson on youtube https://www.youtube.com/watch?v=tfzwyNS1LUY
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject shopMenu; 
    [SerializeField] private GameObject creditsMenu; 

    private int sceneToContinue;
    private int currentSceneIndex;
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void HomeOn()
    {
        mainMenu.SetActive(true);
        Time.timeScale = 1f;
    }
   
    public void HomeOff()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    
    public void ShopOn()
    {
        shopMenu.SetActive(true);
        Time.timeScale = 1f;
    }
    
    public void ShopOff()
    {
        shopMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void CreditsOn()
    {
        creditsMenu.SetActive(true);
        Time.timeScale = 1f;
    }
    
    public void CreditsOff()
    {
        creditsMenu.SetActive(false);
        Time.timeScale = 0f;
    }
}
