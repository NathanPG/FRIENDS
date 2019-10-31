using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Newtonsoft.Json;

public class NetCore : MonoBehaviour
{
    public GameObject player_pre;
    GameObject spawned_player;
    NetworkClient myClient;
    public bool isPlayer;
    public string user_name;
    public PlayerIndicator playerIndicator;
    public ProfileSys profileSys;
    public LoginUI loginui;
    public SQLHandler sql;

    public Text debugtext;
    public GameObject red;

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

    #region TASK_TRANSMISSION
    //3333
    public void ReadTaskRequest()
    {

    }

    //3333
    public void ServerRecvTaskRQ()
    {

    }
    #endregion

    #region LOGIN_TRANSMISSION
    //Client send username and pwd through 1111
    public void ClientSendLogIn(string loginMsg)
    {
        //Client send Accound and Password to the server
        //随便想一个ID NetworkManager.singleton.client.Send(想的ID, 包含账户密码的信息);
        Debug.Log("Client sent login user name and pwd");
        NetworkManager.singleton.client.Send(1111, new StringMessage(loginMsg));
    }


    //AFTER RECV 1111 FROM CLIENT
    public void ServerRecvLogin(NetworkMessage logInMsg)
    {
        red.SetActive(red);
        Debug.Log("Server Received Login Info");
        string clientLogIn = logInMsg.ReadMessage<StringMessage>().value;


        SQLHandler tmp = new SQLHandler();
        outputMessage optMsg = new outputMessage();
        optMsg.success = false;
        optMsg.ErrorMessage = "Unable to deserailze";
        string LogInOutPut = JsonConvert.SerializeObject(optMsg);

        //debugtext.text += LogInOutPut;
        //Debug.Log(LogInOutPut);

        NetworkServer.SendToAll(2222, new StringMessage(LogInOutPut));
        
    }
    //CLIENT RECV 2222 FROM CLIENT
    public void OnClientReceiveFB(NetworkMessage FBMsg)
    {
        Debug.Log("Client Received Server Feedback!");
        //Deserialize message
        string Fbjson = FBMsg.ReadMessage<StringMessage>().value;
        outputMessage outputFBMsg = JsonConvert.DeserializeObject<outputMessage>(Fbjson);
        Debug.Log(Fbjson);
        /*
        
        if (outputFBMsg.success)
        {
            //outputFBMsg.lst["result"]["name"];
            //outputFBMsg.lst["result"]["pwd"];
            profileSys.exp = Convert.ToInt32(outputFBMsg.lst["result"]["exp"]);
            profileSys.gold = Convert.ToInt32(outputFBMsg.lst["result"]["coin"]);
            Debug.Log("CLIENT RECEIVED INFO");
            loginui.loginUI.SetActive(false);
        }
        else
        {
            //REPORT ERROR
            Debug.Log("ERROR");
        }
        */
    }
    #endregion

    const short NameChannelId = 8888;
    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
        red.SetActive(false);
        //Client
        if (playerIndicator.isPlayer)
        {
            Debug.Log("This is client");

            NetworkManager.singleton.networkPort = 9999;
            NetworkClient netc = NetworkManager.singleton.StartClient();

            /*
             * CLIENT CONNECTION ID
            NetworkConnection netconnect = netc.connection;
            int connectionID = netconnect.connectionId;
            */

            NetworkManager.singleton.client.Connect("localhost", 9999);
            NetworkManager.singleton.client.RegisterHandler(2222,OnClientReceiveFB);

            //SEND USERNAME TO SERVER
            //NetworkManager.singleton.client.RegisterHandler(4321, TestClientReceive);
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
            NetworkServer.RegisterHandler(1111, ServerRecvLogin);


            //NetworkServer.RegisterHandler(1234, TestServerReceive);

            //NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
        }
    }


    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }
}
