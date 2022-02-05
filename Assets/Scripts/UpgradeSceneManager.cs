using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeSceneManager : NetworkBehaviour
{
    [SerializeField] private double sceneDuration;
    [SerializeField] TextMeshProUGUI timerText;
    [SyncVar] private double startTime;

    private CustomNetworkManager nm;
    private void Start()
    {
        if (isServer)
        {
            nm = CustomNetworkManager.singleton;
            startTime = NetworkTime.time;
        }
    }
    private void Update()
    {
        var timeRemaining = (startTime + sceneDuration) - NetworkTime.time;
        if(timeRemaining >= 0)
        {
            timerText.text = Mathf.FloorToInt((float)timeRemaining).ToString();
        }
        else if(timeRemaining <= 0 && isServer)
        {
            nm.StartLevel();
        }
    }
}
