using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private Sprite itemIcon;

    [SerializeField] private TextMeshProUGUI itemNameTooltip;
    [SerializeField] private TextMeshProUGUI itemDescriptionTooltip;
    [SerializeField] private GameObject tooltipPanel;

    [SerializeField] private Button itemToDropButton;
    [SerializeField] private GameObject itemToDrop;
    [SerializeField] private GameObject itemDestroy;

    private void Start()
    {
        // Ensure that the tooltip panel is initially hidden
        HideTooltip();

        itemToDropButton.onClick.AddListener(OnItemToDropButtonClick);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ShowTooltip();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            HideTooltip();
        }
    }

    private void Update()
    {
        // Check for left mouse button click anywhere
        if (Input.GetMouseButtonDown(1))
        {
            HideTooltip();
        }
    }

    void ShowTooltip()
    {
        // Set the text of the tooltip
        itemNameTooltip.text = $"{itemName}";
        itemDescriptionTooltip.text = $"{itemDescription}";

        // Set the parent of the tooltip panel to the canvas
        tooltipPanel.transform.SetParent(transform.root, false);

        // Set the position of the tooltip
        tooltipPanel.transform.position = Input.mousePosition + new Vector3(20f, 20f, 0f); // Offset to prevent blocking the mouse

        tooltipPanel.SetActive(true);
    }

    void HideTooltip()
    {
        // Hide the tooltip
        tooltipPanel.SetActive(false);
    }

    void OnItemToDropButtonClick()
    {
        // Find the "ItemDropLocation" GameObject in the scene
        GameObject itemDropLocation = GameObject.Find("ItemDropLocation");

        if (itemDropLocation != null)
        {
            // Instantiate the itemToDrop prefab at the position of ItemDropLocation
            GameObject newItem = Instantiate(itemToDrop, itemDropLocation.transform.position, Quaternion.identity);

            // Optionally, you can parent the new item to the itemDropLocation
            newItem.transform.parent = itemDropLocation.transform;

            // You might want to activate/deactivate or perform additional actions based on your requirements
            Destroy(itemDestroy);
        }
        else
        {
            Debug.LogError("ItemDropLocation not found in the scene.");
        }
    }

}
