using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;

public class SpawnFromJson : MonoBehaviour
{
    [SerializeField] private List<RectTransform> uibuttonPrefab;
    [SerializeField] private List<Transform> slots;

    private ItemsEffects effects;

    [SerializeField] private TextMeshProUGUI saveIndicator; // Add this variable
    [SerializeField] private Button saveData; // New public Button variable

    void Start()
    {

        // Assign the method to the saveData button click event
        saveData.onClick.AddListener(SaveDataButtonClick);


        effects = FindObjectOfType<ItemsEffects>();

        string filePath = Path.Combine(Application.persistentDataPath, "spawnedPrefabNames.json");

        // Check if the JSON file exists
        if (File.Exists(filePath))
        {
            // Read the JSON file content
            string jsonContent = File.ReadAllText(filePath);

            // Deserialize the JSON data
            SpawnData spawnData = JsonUtility.FromJson<SpawnData>(jsonContent);

            // Check if the deserialization was successful
            if (spawnData != null && spawnData.Names != null)
            {
                // Iterate through the names in the JSON data
                int slotIndex = 0;  // Start with the first slot
                foreach (string prefabName in spawnData.Names)
                {
                    if (prefabName == "Empty")
                    {
                        // Handle Empty slot (skip instantiation or perform specific logic)
                        slotIndex++;
                        continue;
                    }

                    RectTransform prefabToSpawn = uibuttonPrefab.Find(prefab => prefab.name == prefabName);

                    if (prefabToSpawn != null && slotIndex < slots.Count)
                    {
                        // Instantiate the prefab in the current slot
                        RectTransform instantiatedPrefab = Instantiate(prefabToSpawn, slots[slotIndex].position, Quaternion.identity, slots[slotIndex]);

                        // Get the Button component from the instantiated prefab
                        Button button = instantiatedPrefab.GetComponent<Button>();

                        if (button != null)
                        {
                            // Add a listener to the button to handle the delete action
                            button.onClick.AddListener(() => OnPrefabButtonClick(prefabName, filePath));
                            // Add a listener to the button to handle the delete action
                            button.onClick.AddListener(() => DeletePrefab(instantiatedPrefab, prefabName, filePath));
                        }
                        else
                        {
                            Debug.LogWarning($"Prefab with name '{prefabName}' is missing a Button component.");
                        }

                        // Increment the slot index for the next prefab
                        slotIndex++;
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab with name '{prefabName}' not found or not enough slots.");
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to deserialize JSON data.");
            }
        }
        else
        {
            Debug.LogError("JSON file not found at path: " + filePath);
            InitializeEmptyJsonData(); // Initialize with empty slots if the file doesn't exist
        }

    }


    private void InitializeEmptyJsonData()
    {
        SpawnData emptySpawnData = new SpawnData
        {
            Names = Enumerable.Repeat("Empty", slots.Count).ToList()
        };

        string filePath = Path.Combine(Application.persistentDataPath, "spawnedPrefabNames.json");
        string jsonData = JsonUtility.ToJson(emptySpawnData);

        File.WriteAllText(filePath, jsonData);
    }

    public void SaveDataButtonClick()
    {
        // Load existing JSON data
        string filePath = Path.Combine(Application.persistentDataPath, "spawnedPrefabNames.json");
        string jsonContent = File.ReadAllText(filePath);
        SpawnData spawnData = JsonUtility.FromJson<SpawnData>(jsonContent);

        // If spawnData is null, initialize it
        if (spawnData == null)
        {
            spawnData = new SpawnData();
        }

        // Iterate through slots
        for (int i = 0; i < slots.Count; i++)
        {
            Transform currentSlot = slots[i];
            bool foundPrefab = false;

            // Iterate through the child objects of the slot
            foreach (Transform child in currentSlot)
            {
                RectTransform prefabInSlot = child.GetComponent<RectTransform>();

                if (prefabInSlot != null)
                {
                    // Get the name of the prefab
                    string prefabName = prefabInSlot.name.Replace("(Clone)", "").Trim();

                    // Update the prefab name in the spawnData
                    if (spawnData.Names.Count > i)
                    {
                        spawnData.Names[i] = prefabName;
                    }
                    else
                    {
                        spawnData.Names.Add(prefabName);
                    }

                    foundPrefab = true;
                    break; // Break out of the loop once a prefab is found in the slot
                }
            }

            // If no prefab is found and it's not already marked as "Empty", update to "Empty" in spawnData
            if (!foundPrefab)
            {
                if (spawnData.Names.Count > i)
                {
                    spawnData.Names[i] = "Empty";
                }
                else
                {
                    spawnData.Names.Add("Empty");
                }
            }
        }

        // Trim the spawnData.Names list to match the number of slots
        spawnData.Names = spawnData.Names.Take(slots.Count).ToList();

        // Convert spawnData to JSON and save it to the file
        string jsonData = JsonUtility.ToJson(spawnData);
        File.WriteAllText(filePath, jsonData);

        // Display the save indicator
        if (saveIndicator != null)
        {
            saveIndicator.text = "Data Saved!";
        }
    }








    private void OnPrefabButtonClick(string prefabName, string jsonFilePath)
    {
        // Call the HandleButtonClick method from PrefabButtonClickHandler
        effects.HandleButtonClick(prefabName);

        // Remove the prefab name from the JSON data
        RemovePrefabNameFromJson(prefabName, jsonFilePath);
    }

    // Method to delete the prefab and its corresponding name from JSON data
    public void DeletePrefab(RectTransform prefabToDelete, string prefabName, string jsonFilePath)
    {
        // Remove the instantiated prefab
        Destroy(prefabToDelete.gameObject);

        // Remove the prefab name from the JSON data
        RemovePrefabNameFromJson(prefabName, jsonFilePath);
    }



    // Method to remove the prefab name from JSON data
    void RemovePrefabNameFromJson(string prefabName, string jsonFilePath)
    {
        // Read the JSON file content
        string jsonContent = File.ReadAllText(jsonFilePath);

        // Deserialize the JSON data
        SpawnData spawnData = JsonUtility.FromJson<SpawnData>(jsonContent);

        // Check if the deserialization was successful
        if (spawnData != null && spawnData.Names != null)
        {
            // Remove the prefab name from the list
            spawnData.Names.Remove(prefabName);

            // Serialize the updated JSON data
            string updatedJsonContent = JsonUtility.ToJson(spawnData);

            // Write the updated JSON data back to the file
            File.WriteAllText(jsonFilePath, updatedJsonContent);
        }
        else
        {
            Debug.LogError("Failed to update JSON data while removing prefab name.");
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public List<string> Names;
}
