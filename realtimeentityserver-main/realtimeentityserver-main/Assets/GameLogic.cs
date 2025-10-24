using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    float timer = 1f;
    int nextID = 1;
    Dictionary<int, (float x, float y)> balloons = new();

    void Start() => NetworkServerProcessing.SetGameLogic(this);

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 1f;
            float x = Random.value, y = Random.value;
            int id = nextID++;
            balloons[id] = (x, y);
            NetworkServerProcessing.BroadcastMessageToAllClients($"1,{id},{x},{y}", TransportPipeline.ReliableAndInOrder);
        }
    }

    public void HandlePop(int id)
    {
        if (!balloons.Remove(id)) return;
        NetworkServerProcessing.BroadcastMessageToAllClients($"2,{id}", TransportPipeline.ReliableAndInOrder);
    }

    public void SendFullState(int clientID)
    {
        List<string> msg = new() { "3", balloons.Count.ToString() };
        foreach (var b in balloons)
            msg.AddRange(new[] { b.Key.ToString(), b.Value.x.ToString(), b.Value.y.ToString() });
        NetworkServerProcessing.SendMessageToClient(string.Join(",", msg), clientID, TransportPipeline.ReliableAndInOrder);
    }
}
