using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTracker : NetworkBehaviour
{
    public int RoundsToWin { get; set; } = 10;

    private static Dictionary<uint, int> points;
    private void Start()
    {
        GameSceneManager.OnPlayerWin += OnPlayerWin;
    }
    private void OnPlayerWin(uint winnerID)
    {
        if (points.ContainsKey(winnerID))
        {
            points[winnerID]++;
        } 
        else
        {
            points.Add(winnerID, 1);
        }
        if (isServer && points[winnerID] >= RoundsToWin)
        {
            StartWin();
        }
    }
    [Server]
    private void StartWin()
    {

    }
}
