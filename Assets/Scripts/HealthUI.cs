using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameObject template;
    [SerializeField] private Vector2 offset;

    private Dictionary<GameObject, GameObject> playerTemplates = new Dictionary<GameObject, GameObject>();

    public void AddPlayerUI(GameObject player, string playerName)
    {
        var playerTemplate = Instantiate(template, transform);
        playerTemplates.Add(player, playerTemplate);

        playerTemplate.GetComponentInChildren<TextMeshProUGUI>().text = playerName;
        playerTemplate.GetComponentInChildren<HealthTracker>().target = player.GetComponent<Health>();

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
        foreach (var value in playerTemplates.Values)
        {
            value.transform.localPosition = offset * counter;
            counter++;
        }
    }
}
