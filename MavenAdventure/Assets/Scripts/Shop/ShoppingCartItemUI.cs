using UnityEngine;
using UnityEngine.UI;

//Code borrowed and Modified by Dan Pos off of the inventory system series from youtube https://www.youtube.com/playlist?list=PL-hj540P5Q1hLK7NS5fTSNYoNJpPWSL24
public class ShoppingCartItemUI : MonoBehaviour
{
    [SerializeField] private Text itemText;
    
    public void SetItemText(string newString)
    {
        itemText.text = newString;
    }
}
