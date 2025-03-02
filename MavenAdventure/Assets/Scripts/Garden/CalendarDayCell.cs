using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarDayCell : MonoBehaviour
{
    public Image backgroundImage; // Background color
    public Image harvestIcon; // Icon to display for harvested plants
    
    public void ShowMultipleIcons(List<Sprite> icons)
    {
        // Clear previous icons
        foreach (Transform child in transform)
        {
            if (child.name.Contains("HarvestIcon"))
            {
                Destroy(child.gameObject);
            }
        }

        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(icons.Count)); // Creates a square grid (e.g., 2x2, 3x3)

        float iconSize = 20f;  // Adjust icon size as needed
        float spacing = 5f;    // Space between icons

        for (int i = 0; i < icons.Count; i++)
        {
            GameObject newIcon = new GameObject("HarvestIcon" + i, typeof(RectTransform), typeof(Image));
            newIcon.transform.SetParent(transform, false);

            Image iconImage = newIcon.GetComponent<Image>();
            iconImage.sprite = icons[i];
            iconImage.preserveAspect = true;

            RectTransform rect = newIcon.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(iconSize, iconSize); 

            // Position in a grid pattern
            int row = i / gridSize;
            int col = i % gridSize;
            rect.anchoredPosition = new Vector2(col * (iconSize + spacing), -row * (iconSize + spacing));
        }
    }

}