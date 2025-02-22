using UnityEngine;
using UnityEngine.SceneManagement;

//code borrowed and modified by Hooson on youtube https://www.youtube.com/watch?v=tfzwyNS1LUY
public class ShopMenu : MonoBehaviour
{
    [SerializeField] private GameObject shopMenu;
    private int sceneToContinue;
    private int currentSceneIndex;
    public void Shop()
    {
        shopMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        shopMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }
}