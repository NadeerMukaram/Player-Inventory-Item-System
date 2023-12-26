using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class Incre : MonoBehaviour
{
    private int health = 0;
    private int mana = 0;

    [SerializeField] private TextMeshProUGUI healthresultText;
    [SerializeField] private TextMeshProUGUI manaresultText;

    private string saveFilePath;




    public void DecreHP()
    {
        DecrementHP(1);
    }

    public void DecreMana()
    {
        DecrementMana(1);
    }




    void Start()
    {
        LoadData();
        HealthResultText();
        ManaResultText();
    }

    public int IncrementHP(int num)
    {
        health += num;
        HealthResultText();
        SaveData();
        return health;
    }

    public int DecrementHP(int num)
    {
        health -= num;
        HealthResultText();
        SaveData();
        return health;
    }

    public int IncrementMana(int num)
    {
        mana += num;
        ManaResultText();
        SaveData();
        return mana;
    }

    public int DecrementMana(int num)
    {
        mana -= num;
        ManaResultText();
        SaveData();
        return mana;
    }

    void HealthResultText()
    {
        if (healthresultText != null)
        {
            healthresultText.text = health.ToString();
        }
    }

    void ManaResultText()
    {
        if (manaresultText != null)
        {
            manaresultText.text = mana.ToString();
        }
    }

    void SaveData()
    {
        SaveData saveData = new SaveData
        {
            Health = health,
            Mana = mana
        };

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(saveFilePath, json);
    }

    void LoadData()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savedData.json");

        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            health = saveData.Health;
            mana = saveData.Mana;
        }
    }

    public void ResetSavedData()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        health = 0;
        mana = 0;
    }
}

[System.Serializable]
public class SaveData
{
    public int Health;
    public int Mana;
}
