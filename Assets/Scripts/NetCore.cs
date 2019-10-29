using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;

public class NetCore : MonoBehaviour
{
    public GameObject player_pre;
    GameObject spawned_player;
    NetworkClient myClient;
    public bool isPlayer;
    public string user_name;
    public PlayerIndicator playerIndicator;
    public SQLHandler sql;

    /*
    public class ProfileMsg : MessageBase
    {
        public string username;
        public int exp;
        public int gold;
        public List<Task> accepted_task;
    }
    */

    #region MsgTEST
    public InputField Testinput;
    public void TestServerReceive(NetworkMessage netMsg)
    {
        Debug.Log("SERVER RECEIVED THE MSG" + netMsg.ReadMessage<StringMessage>().value);
    }
    public void TestClientReceive(NetworkMessage netMsg)
    {
        Debug.Log("CLINET RECEIVED THE MSG" + netMsg.ReadMessage<StringMessage>().value);
    }

    public void SendStringOnClick()
    {
        //Client
        if (playerIndicator.isPlayer)
        {
            NetworkManager.singleton.client.Send(1234, new StringMessage(Testinput.text));
        }
        //Servers
        else
        {
            NetworkServer.SendToAll(4321, new StringMessage(Testinput.text));
        }
    }
    #endregion

    //client get profile
    public void OnClientReceiveProfile(NetworkMessage netMsg) { 
        Debug.Log("Client Received Player Profile!");
        //if(user name matches)
            //CLIENT STORE ALL DATA 
    }

    public void ClientSendName(string loginAccount, string loginPassword)
    {
        //Client send Accound and Password to the server
        //随便想一个ID NetworkManager.singleton.client.Send(想的ID, 包含账户密码的信息);
    }

    bool msg_sent = false;

    private void Update()
    {
        if (NetworkServer.active)
        {
            //Debug.Log("Server Active!");
        }
    }

    const short NameChannelId = 8888;
    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
        //SEND USERNAME TO SERVER

        //SERVER RETURN PLAYER PROFILE

        //Client
        if (playerIndicator.isPlayer)
        {
            Debug.Log("This is client");

            NetworkManager.singleton.networkPort = 9999;
            NetworkManager.singleton.StartClient();
            NetworkManager.singleton.client.Connect("localhost", 9999);

            //SEND USERNAME TO SERVER
            NetworkManager.singleton.client.RegisterHandler(4321, TestClientReceive);
        }

        //Host
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
            NetworkServer.RegisterHandler(1234, TestServerReceive);

            //NetworkServer.RegisterHandler(你想的ID, ServerReceiveName);

            //NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
        }
    }

    //Server got client name, send profile
    public void ServerReceiveName(NetworkMessage netMsg)
    {
        //sql.Check User
        //sql.GET USER PROFILE
        //SEND USER INFORMATION TO THE CLIENT

        //NetworkServer.SendToAll(8889, ProfileMsg);
    }

    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }
}
