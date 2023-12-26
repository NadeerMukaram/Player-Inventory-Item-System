using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHP : MonoBehaviour
{
    private Spawn spawn;
    void Start()
    {
        spawn = FindObjectOfType<Spawn>();
    }

    // Method to spawn the HP BUTTON in inventory
    // Put inside a trigger method or something that is player can interact
    public void HPSpawn()
    {
        spawn.SpawnPrefab(0);
    }


    // Method to spawn the MANA BUTTON in inventory
    // Put inside a trigger method or something that is player can interact
    public void ManaSpawn()
    {
        spawn.SpawnPrefab(1);
    }

}
