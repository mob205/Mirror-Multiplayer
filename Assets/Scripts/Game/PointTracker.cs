using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointTracker : NetworkBehaviour
{
    public int RoundsToWin { get; set; } = 10;

    private static Dictionary<NetworkConnection, int> serverPoints = new Dictionary<NetworkConnection, int>();
    private Dictionary<uint, int> localPoints = new Dictionary<uint, int>();

    public override void OnStartServer()
    {
        GameSceneManager.OnPlayerWin += OnPlayerWin;
    }
    public override void OnStartClient()
    {
        CmdRequestPoints();
    }
    [Server]
    private void OnPlayerWin(uint winnerID)
    {
        var conn = NetworkServer.spawned[winnerID].connectionToClient;
        AddPoints(conn, 1);
        if (isServer && serverPoints[conn] >= RoundsToWin)
        {
            StartWin();
        }
    }
    [Server]
    public void AddPoints(NetworkConnection conn, int amount)
    {
        if (serverPoints.ContainsKey(conn))
        {
            serverPoints[conn] += amount;
        }
        else
        {
            serverPoints.Add(conn, amount);
        }
        SendPoints();
    }
    [Server]
    private void SendPoints()
    {
        var ids = new uint[serverPoints.Count];
        int counter = 0;
        foreach(var conn in serverPoints)
        {
            ids[counter] = conn.Key.identity.netId;
            counter++;
        }
        var pointValues = serverPoints.Values.ToArray();
        RpcSendPoints(ids, pointValues);
    }
    [ClientRpc]
    private void RpcSendPoints(uint[] ids, int[] points)
    {
        var temp = new Dictionary<uint, int>();
        for (int i = 0; i < ids.Length; i++)
        {
            temp.Add(ids[i], points[i]);
        }
        localPoints = temp;
    }
    [Command(requiresAuthority = false)]
    public void CmdRequestPoints()
    {
        SendPoints();
    }
    [Client]
    public int GetPoints(uint id)
    {
        if (localPoints.ContainsKey(id))
        {
            return localPoints[id];
        }
        else
        {
            return 0;
        }
    }

    [Server]
    private void StartWin()
    {
        Debug.Log("major dubbleyoos");
    }
    [Server]
    public static void ResetStatics()
    {
        serverPoints.Clear();
    }
    private void OnDestroy()
    {
        GameSceneManager.OnPlayerWin -= OnPlayerWin;
    }
}
