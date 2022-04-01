using Mirror;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private Spawnable[] spawnableItems;
    [SerializeField] private int totalSpawns;

    public override void OnStartServer()
    {
        if(spawnLocations.Length > 0 && spawnableItems.Length > 0)
        {
            var locations = spawnLocations.ToList();
            for (int i = 0; i < totalSpawns; i++)
            {
                // Spawn a random item at a random location. 
                var locIndex = Random.Range(0, locations.Count);
                var spawnedItem = Instantiate(GetRandomItem(), locations[locIndex].position, Quaternion.identity);
                NetworkServer.Spawn(spawnedItem.gameObject);

                // One location should not have two items.
                locations.Remove(locations[locIndex]);
            }
        }
    }
    private Item GetRandomItem()
    {
        var totalWeight = spawnableItems.Sum(p => p.weight);
        var randomWeight = Random.Range(0, totalWeight) + 1;
        foreach(var spawnable in spawnableItems)
        {
            randomWeight -= spawnable.weight;
            if(randomWeight <= 0)
            {
                return spawnable.item;
            }
        }
        Debug.LogError("Item was not randomly selected. Selecting last item.");
        return spawnableItems[spawnableItems.Length - 1].item;
    }
}
[System.Serializable]
public struct Spawnable 
{
    [SerializeField] public Item item;
    [SerializeField] public int weight;
}

