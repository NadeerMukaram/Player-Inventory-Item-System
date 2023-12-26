using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Spawn : MonoBehaviour
{
    [SerializeField] private List<RectTransform> uibuttonPrefab;
    [SerializeField] private List<Transform> slots;
    [SerializeField] private List<string> spawnedPrefabNames = new List<string>();
    [SerializeField] private ItemsEffects effects;

    private void Start()
    {
        effects = FindObjectOfType<ItemsEffects>();
    }


    // Method to spawn the UI button prefab
    public void SpawnPrefab(int prefabIndex)
    {
        SpawnButton(prefabIndex);
    }


    private bool SpawnButton(int prefabIndex)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "spawnedPrefabNames.json");

        if (uibuttonPrefab != null && prefabIndex >= 0 && prefabIndex < uibuttonPrefab.Count && slots != null && slots.Count > 0)
        {
            // Find the first slot with the same name
            Transform existingSlot = FindSameNameSlot(prefabIndex);

            if (existingSlot != null)
            {
                // Increment the count in the existing slot
                RectTransform existingButton = existingSlot.GetChild(0) as RectTransform;
                if (existingButton != null)
                {
                    // Update the count or perform any other actions needed
                    // For example, you might want to update a text component on the button.
                    existingButton.GetComponentInChildren<Text>().text = "New Count";
                }

                return true;
            }
            else
            {
                // Find the first empty slot
                Transform emptySlot = FindEmptySlot();

                if (emptySlot != null)
                {
                    // Spawn a new button in the empty slot
                    RectTransform buttonInstance = Instantiate(uibuttonPrefab[prefabIndex], emptySlot);

                    buttonInstance.localPosition = Vector3.zero;
                    buttonInstance.localScale = Vector3.one;

                    // Get the name of the prefab and add it to the list
                    string prefabName = uibuttonPrefab[prefabIndex].name;
                    spawnedPrefabNames.Add(prefabName);

                    Button button = buttonInstance.GetComponent<Button>();

                    // Check if a button component is present
                    if (button != null)
                    {
                        // Add a listener to the button to handle the delete action
                        button.onClick.AddListener(() => OnPrefabButtonClick(prefabName, filePath));
                        // Add a listener to the button to handle the delete action
                        button.onClick.AddListener(() => DeletePrefab(buttonInstance, prefabName, filePath));
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab with name '{prefabName}' is missing a Button component.");
                    }

                    return true;
                }
                else
                {
                    Debug.LogWarning("All slots are occupied. Unable to spawn a new button.");
                }
            }
        }

        return false;
    }

    private Transform FindSameNameSlot(int prefabIndex)
    {
        // Iterate through the slots to find the first slot with the same name
        for (int i = 0; i < slots.Count; i++)
        {
            if (IsSlotSameName(slots[i], prefabIndex))
            {
                return slots[i];
            }
        }

        return null;
    }

    private bool IsSlotSameName(Transform slot, int prefabIndex)
    {
        // Check if the slot has a child with the same name
        if (slot.childCount > 0)
        {
            RectTransform child = slot.GetChild(0) as RectTransform;
            if (child != null && child.name == uibuttonPrefab[prefabIndex].name)
            {
                return true;
            }
        }

        return false;
    }

    private Transform FindEmptySlot()
    {
        // Iterate through the slots to find the first empty one
        for (int i = 0; i < slots.Count; i++)
        {
            if (IsSlotEmpty(slots[i]))
            {
                return slots[i];
            }
        }

        return null;
    }

    private bool IsSlotEmpty(Transform slot)
    {
        // Check if the slot is empty
        return slot.childCount == 0;
    }



    private void OnPrefabButtonClick(string prefabName, string jsonFilePath)
    {
        // Call the HandleButtonClick method from PrefabButtonClickHandler
        effects.HandleButtonClick(prefabName);

        //// Remove the prefab name from the JSON data
        //RemovePrefabNameFromJson(prefabName, jsonFilePath);
    }



    public void DeletePrefab(RectTransform prefabToDelete, string prefabName, string jsonFilePath)
    {
        // Remove the listener to prevent multiple calls
        Button button = prefabToDelete.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
        }

        // Remove the instantiated prefab
        Destroy(prefabToDelete.gameObject);

        //// Remove the prefab name from the JSON data
        //RemovePrefabNameFromJson(prefabName, jsonFilePath);
    }


    //private void SavePrefabNamesToJson()
    //{
    //    // Load existing JSON data
    //    string filePath = Application.persistentDataPath + "/spawnedPrefabNames.json";
    //    string existingJson = File.Exists(filePath) ? File.ReadAllText(filePath) : "";

    //    // Deserialize existing JSON into PrefabNames object
    //    PrefabNames existingPrefabNames = JsonUtility.FromJson<PrefabNames>(existingJson);

    //    // If the existingPrefabNames is null, create a new instance
    //    if (existingPrefabNames == null)
    //    {
    //        existingPrefabNames = new PrefabNames();
    //        existingPrefabNames.Names = new List<string>();
    //    }

    //    // Add the newly spawned prefab name to the list
    //    existingPrefabNames.Names.Add(spawnedPrefabNames[spawnedPrefabNames.Count - 1]);

    //    // Convert the updated list of prefab names to JSON
    //    string updatedJson = JsonUtility.ToJson(existingPrefabNames);

    //    // Overwrite the entire file with the updated JSON array
    //    File.WriteAllText(filePath, updatedJson);

    //    // Log the file path to the console
    //    Debug.Log("Prefab names appended to JSON: " + filePath);
    //}


    // Method to remove the prefab name from JSON data
    //void RemovePrefabNameFromJson(string prefabName, string jsonFilePath)
    //{
    //    // Read the JSON file content
    //    string jsonContent = File.ReadAllText(jsonFilePath);

    //    // Deserialize the JSON data
    //    SpawnData spawnData = JsonUtility.FromJson<SpawnData>(jsonContent);

    //    // Check if the deserialization was successful
    //    if (spawnData != null && spawnData.Names != null)
    //    {
    //        // Remove the prefab name from the list
    //        spawnData.Names.Remove(prefabName);

    //        // Serialize the updated JSON data
    //        string updatedJsonContent = JsonUtility.ToJson(spawnData);

    //        // Write the updated JSON data back to the file
    //        File.WriteAllText(jsonFilePath, updatedJsonContent);
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to update JSON data while removing prefab name.");
    //    }
    //}



    [System.Serializable]
    private class PrefabNames
    {
        public List<string> Names;
    }
}