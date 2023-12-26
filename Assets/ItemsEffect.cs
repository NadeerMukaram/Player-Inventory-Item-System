using UnityEngine;

public class ItemsEffects : MonoBehaviour
{

    private Incre incre;

    private void Start()
    {
        incre = FindObjectOfType<Incre>();
    }
    public void HandleButtonClick(string prefabName)
    {
        
        switch (prefabName)
        {
            case "HP":
                HandleHPClick();
                break;
            case "Mana":
                HandleManaClick();
                break;
            // Add more cases as needed for other prefab types

            default:
                Debug.LogWarning($"Unknown prefab type: {prefabName}");
                break;
        }

    }

    private void HandleHPClick()
    {
        Debug.Log("HP prefab clicked. Implement your HP-specific functionality here.");
        incre.IncrementHP(1);
    }

    private void HandleManaClick()
    {
        Debug.Log("Mana prefab clicked. Implement your Mana-specific functionality here.");
        incre.IncrementMana(1);
    }
}
