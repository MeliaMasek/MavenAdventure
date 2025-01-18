using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    public Interactor interactor;
    public bool openCrafting;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Interact);
    }

    public void Interact()
    {
        if (openCrafting)
        {
            //interactor.OpenCraftingPanel();
        }
        else
        {
            //interactor.OpenShopPanel();
        }
    }
}
