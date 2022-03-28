using Mirror;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : NetworkBehaviour
{
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] private GameObject[] spawnableItems;
    [SerializeField] private int totalSpawns;

    public override void OnStartServer()
    {
        if(spawnLocations.Length > 0 && spawnableItems.Length > 0)
        {
            var locations = spawnLocations.ToList();
            for (int i = 0; i < totalSpawns; i++)
            {
                var locIndex = Random.Range(0, locations.Count - 1);
                var itemIndex = Random.Range(0, spawnableItems.Length - 1);
                var spawnedItem = Instantiate(spawnableItems[itemIndex], locations[locIndex]);
                NetworkServer.Spawn(spawnedItem);
            }
        }
    }
}
