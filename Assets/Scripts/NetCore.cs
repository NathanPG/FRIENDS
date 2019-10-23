using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetCore : MonoBehaviour
{
    public GameObject player_pre;
    GameObject spawned_player;
    NetworkClient myClient;
    public bool isPlayer;
    const short MESSAGE_ID = 1002;
    const short MESSAGE_ID0 = 1003;

    public void OnClientConnected(NetworkMessage netMsg)
    {
        Debug.Log("CLINET CONNECTED!");
        NetworkManager.singleton.client.Send(MESSAGE_ID, new StringMessage("MIAO"));
    }
    
    public void OnServerReceive(NetworkMessage netMsg)
    {
        Debug.Log("Server Received MSG!" + netMsg);
        NetworkServer.SendToAll(MESSAGE_ID0, new StringMessage("I HEARED YOUR MIAO"));
    }

    public void OnClientReceive(NetworkMessage netMsg)
    {
        Debug.Log("Client Received MSG!" + netMsg);
    }

    bool msg_sent = false;
    private void Update()
    {
        if (NetworkServer.active)
        {
            //Debug.Log("Server Active!");
        }
        if(GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>().isPlayer && 
            NetworkServer.active && !msg_sent && NetworkManager.singleton.client.isConnected)
        {
            Debug.Log("CLIENT MSG SENT");
            msg_sent = true;
            NetworkManager.singleton.client.Send(MESSAGE_ID, new StringMessage("MIAO"));
        }
    }

    private void Start()
    {
        
        if (GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>().isPlayer)
        {
            Debug.Log("This is client");
            //SetupClient();
            NetworkManager.singleton.networkPort = 9999;
            //NetworkManager.singleton.networkAddress = "localhost";
            NetworkManager.singleton.StartClient();
            NetworkManager.singleton.client.Connect("localhost", 9999);
            NetworkManager.singleton.client.RegisterHandler(MESSAGE_ID0, OnClientReceive);
        }
        else
        {
            //SetupServer();
            NetworkManager.singleton.networkPort = 9999;

            //NetworkManager.singleton.networkPort = 10000;
            //var config = new ConnectionConfig();
            // There are different types of channels you can use, check the official documentation
            //config.AddChannel(QosType.ReliableFragmented);
            //config.AddChannel(QosType.UnreliableFragmented);

            //THIS START A NetworkServer
            NetworkManager.singleton.StartHost();
            //NetworkServer.Listen(9999);
            NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
            NetworkServer.RegisterHandler(MESSAGE_ID, OnServerReceive);

            
            //spawned_player = Instantiate(player_pre);
            //spawned_player.GetComponent<Player>().isPlayer = false;
            //NetworkServer.Spawn(spawned_player);
        }
    }

    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }
}
