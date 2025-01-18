using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Force landscape orientation
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
