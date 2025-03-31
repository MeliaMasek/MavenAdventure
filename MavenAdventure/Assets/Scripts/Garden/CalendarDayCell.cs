using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarDayCell : MonoBehaviour
{
    public Image backgroundImage; // Background color

    [Header("Harvest Icon Settings")]
    public float defaultIconSize = 40f;  // Default size of each icon
    public float maxIconSize = 50f; // Max size for fewer icons (single or small grid)
    public float minIconSize = 30f; // Min size for large grid (many icons)

    [Header("Grid Specific Settings")]
    public float iconSize3x3 = 35f;  // Icon size for a 3x3 grid
    public float iconSize2x2 = 40f;  // Icon size for a 2x2 grid

    public float spacing = 2.5f;  // Space between icons
    public float offsetX = 30f;   // Offset to shift icons to the right
    public float offsetY = 10f;   // Offset to shift icons vertically

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

        // Determine the grid size (square grid)
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(icons.Count));
        
        // Calculate the available space in the cell
        float totalWidth = backgroundImage.rectTransform.rect.width;
        float totalHeight = backgroundImage.rectTransform.rect.height;

        // Calculate the maximum icon size that fits within the grid, considering spacing
        float maxIconsInRow = Mathf.Floor(totalWidth / (defaultIconSize + spacing));
        float maxIconsInColumn = Mathf.Floor(totalHeight / (defaultIconSize + spacing));

        // Adjust icon size based on the number of icons, ensuring it fits in the grid
        float iconSize = defaultIconSize;

        // Apply custom sizes for 2x2 and 3x3 grids
        if (gridSize == 3)
        {
            iconSize = iconSize3x3;  // Use the specific size for 3x3 grid
        }
        else if (gridSize == 2)
        {
            iconSize = iconSize2x2;  // Use the specific size for 2x2 grid
        }
        else if (icons.Count > maxIconsInRow * maxIconsInColumn)
        {
            iconSize = Mathf.Min(minIconSize, defaultIconSize); // If too many icons, shrink to min size
        }
        else
        {
            // Adjust icon size depending on the grid size
            iconSize = Mathf.Min(defaultIconSize, totalWidth / (maxIconsInRow + spacing));
        }

        // If only one icon, center it
        if (icons.Count == 1)
        {
            GameObject newIcon = new GameObject("HarvestIcon", typeof(RectTransform), typeof(Image));
            newIcon.transform.SetParent(transform, false);

            Image iconImage = newIcon.GetComponent<Image>();
            iconImage.sprite = icons[0];
            iconImage.preserveAspect = true;

            RectTransform rect = newIcon.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(iconSize, iconSize);
            rect.anchoredPosition = new Vector2(offsetX, offsetY);
        }
        else
        {
            // If multiple icons, arrange them in a grid
            for (int i = 0; i < icons.Count; i++)
            {
                GameObject newIcon = new GameObject("HarvestIcon" + i, typeof(RectTransform), typeof(Image));
                newIcon.transform.SetParent(transform, false);

                Image iconImage = newIcon.GetComponent<Image>();
                iconImage.sprite = icons[i];
                iconImage.preserveAspect = true;

                RectTransform rect = newIcon.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(iconSize, iconSize);

                // Position each icon in the grid
                int row = i / gridSize;
                int col = i % gridSize;

                // Adjust positions to fit the grid within the cell
                float xPos = (col * (iconSize + spacing)) - ((gridSize - 1) * (iconSize + spacing) / 2) + offsetX;
                float yPos = -(row * (iconSize + spacing)) + ((gridSize - 1) * (iconSize + spacing) / 2) - offsetY;

                rect.anchoredPosition = new Vector2(xPos, yPos);
            }
        }
    }
}