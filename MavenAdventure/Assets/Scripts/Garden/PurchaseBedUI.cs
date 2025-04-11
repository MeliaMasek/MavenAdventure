using UnityEngine;
using UnityEngine.UI;

public class PurchaseBedUI : MonoBehaviour
{
    public GameObject UIPanel;
    private PurchaseBed currentPlot;

    public Button yesButton;
    public Button noButton;

    private void Start()
    {
        UIPanel.SetActive(false);

        yesButton.onClick.AddListener(() =>
        {
            //currentPlot?.ConfirmPurchase();
            Hide();
        });

        noButton.onClick.AddListener(Hide);
    }

    public void Show(PurchaseBed plot)
    {
        UIPanel.SetActive(true);
        currentPlot = plot;
    }

    public void Hide()
    {
        UIPanel.SetActive(false);
        currentPlot = null;
    }
}