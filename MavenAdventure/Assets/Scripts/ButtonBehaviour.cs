using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public IntData value;
    public Button shuffleButton;
    
    public void OffButton()
    {
        // Check if the IntData value is zero and toggle the button state accordingly
        if (value.value == 0)
        {
            shuffleButton.gameObject.SetActive(false); // Turn off the button
        }
    }

    public void OnButton()
    {
        // Check if the IntData value is zero and toggle the button state accordingly
        if (value.value >= 1)
        {
            shuffleButton.gameObject.SetActive(true); // Turn off the button
        }
    }
}
