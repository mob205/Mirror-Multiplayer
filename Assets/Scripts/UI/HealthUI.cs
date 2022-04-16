using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject template;
    [SerializeField] private Vector2 offset;
    [SerializeField] private Transform startingLocation;

    private Dictionary<GameObject, GameObject> playerTemplates = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        PlayerDisplayer.OnRemovePlayerUI += RemovePlayerUI;
        PlayerDisplayer.OnStartPlayerUI += AddPlayerUI;
    }
    public void AddPlayerUI(GameObject player)
    {
        var playerTemplate = Instantiate(template, transform);
        playerTemplates.Add(player, playerTemplate);

        playerTemplate.GetComponentInChildren<TextMeshProUGUI>().text = player.GetComponent<PlayerDisplayer>().playerName;
        playerTemplate.GetComponentInChildren<HealthTracker>().target = player.GetComponent<Health>();
        playerTemplate.GetComponentInChildren<PointDisplay>().target = player.GetComponent<NetworkIdentity>().netId;

        RepositionUI();
    }
    public void RemovePlayerUI(GameObject player)
    {
        if (playerTemplates.ContainsKey(player))
        {
            Destroy(playerTemplates[player]);
            playerTemplates.Remove(player);
        }
        else
        {
            Debug.LogError("UI not found for player.");
        }
        RepositionUI();
    }
    void RepositionUI()
    {
        var counter = 0;
        foreach (var template in playerTemplates.Values)
        {
            if (!template) { continue; }
            template.transform.localPosition = (Vector2) startingLocation.localPosition + offset * counter;
            counter++;
        }
    }
    private void OnDestroy()
    {
        PlayerDisplayer.OnRemovePlayerUI -= RemovePlayerUI;
        PlayerDisplayer.OnStartPlayerUI -= AddPlayerUI;
    }
}
