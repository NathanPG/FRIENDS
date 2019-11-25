using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using Newtonsoft.Json;

/// <summary>
/// NetCore: 
/// This is the Core Package for server and clients to communiate,
/// </summary>
public class NetCore : MonoBehaviour
{
    public OLScene oLScene;
    public LoginUI loginUI;
    GameObject spawned_player;
    NetworkClient myClient;
    public bool isPlayer;
    public string user_name;
    public PlayerIndicator playerIndicator;
    public ProfileSys profileSys;
    public LoginUI loginui;
    public GameObject host_c;
    public GameObject client_c;
    public GameObject red;

    /// <summary>
    /// function for Client to send message to the server
    /// </summary>
    /// <param name="Msg">Input Requirement</param>
    public void ClientSendMsg(string Msg)
    {
        //Client send Accound and Password to the server
        NetworkManager.singleton.client.Send(3333, new StringMessage(Msg));
        Debug.Log("CLIENT SENT REQUEST: " + Msg);
    }

    /// <summary>
    /// function for Server to receive Message from the client.
    /// HANDLE 3333, SEND 4444
    /// </summary>
    /// <param name="Msg"></param>
    public void ServerRecvMsg(NetworkMessage Msg)
    {
        SQLHandler sql = new SQLHandler();
        string msg = Msg.ReadMessage<StringMessage>().value;
        Debug.Log("SERVER RECV REQUEST: " + msg);
        string fb = sql.recvMsg(msg);
        NetworkServer.SendToAll(4444, new StringMessage(fb));
        Debug.Log("SERVER SENT FEEDBACK: " + fb);
    }

    
    /// <summary>
    /// function for Client to recive Message from the server, and also the function
    /// and also load the function form the server.
    /// </summary>
    /// <param name="Msg"></param>
    public void ClientRecvMsg(NetworkMessage Msg)
    {
        string msg = Msg.ReadMessage<StringMessage>().value;
        Debug.Log("CLIENT RECV FEEDBACK: " + msg);
        outputMessage tskOpt = new outputMessage(msg);
        //USER INFO
        if (tskOpt.getWay().Equals("getDetailsUsr"))
        {
            //TODO: MATCH NAME, else error
            oLScene.UPDATEINFO(msg);
        }
        //REG USER
        else if (tskOpt.getWay().Equals("addUsr"))
        {
            loginUI.REG_USER(msg);
        }
        //LOBBY TASK LIST
        else if (tskOpt.getWay().Equals("getallTsk"))
        {
            oLScene.UPDATETSK(msg);
        }
        //PUBLISH TASK
        else if (tskOpt.getWay().Equals("addTsk"))
        {
            oLScene.ADDTASK(msg);
        }
        //FININSH TASK
        else if (tskOpt.getWay().Equals("finishTsk"))
        {
            profileSys.UpdateAccepted(tskOpt.getResult());
        }
        //ACCEPT TASK
        else if (tskOpt.getWay().Equals("takeTsk"))
        {
            profileSys.UpdateAccepted(tskOpt.getResult());
        }
        //PERSONAL TASK LIST
        else if (tskOpt.getWay().Equals("getAcceptedTsk"))
        {
            oLScene.UPDATEACCEPTED(msg);
        }
        else
        {
            Debug.Log("Unable to recognize the Way, Please check the database infomation");
        }

    }

    #region old_transmission
    public void ClientSendLogIn(string loginMsg)
    {
        //Client send Accound and Password to the server
        Debug.Log("Client sent login user name and pwd");
        NetworkManager.singleton.client.Send(1111, new StringMessage(loginMsg));
    }
    //AFTER RECV 1111 FROM CLIENT
    public void ServerRecvLogin(NetworkMessage logInMsg)
    {
        Debug.Log("Server Received Login Info");
        string clientLogIn = logInMsg.ReadMessage<StringMessage>().value;

        SQLHandler sql = new SQLHandler();
        string LogInOutPut = sql.recvMsg(clientLogIn);
        Debug.Log(LogInOutPut);
        bool return_value = NetworkServer.SendToAll(2222, new StringMessage(LogInOutPut));      
    }
    //CLIENT RECV 2222 FROM Server
    public void OnClientReceiveFB(NetworkMessage FBMsg)
    {
        //Deserialize message
        string Fbjson = FBMsg.ReadMessage<StringMessage>().value;
        Debug.Log(Fbjson);
        outputMessage outputFBMsg = new outputMessage(Fbjson);


        if (outputFBMsg.getSuccess())
        {

            Dictionary<string, Dictionary<string, string>> outDic = outputFBMsg.getResult();
            profileSys.exp = Convert.ToInt32(outDic["0"]["exp"]);
            profileSys.gold = Convert.ToInt32(outDic["0"]["coin"]);
            profileSys.username = outDic["0"]["name"].ToString();
            playerIndicator.UserName = outDic["0"]["name"].ToString();
            Debug.Log("CLIENT RECEIVED INFO");
            loginui.loginUI.SetActive(false);
        }
        else
        {
            //REPORT ERROR
            Debug.Log("ERROR" + outputFBMsg.getErrorMsg());
        }

    }
    #endregion



    /// <summary>
    /// Indicate one client is connected. 
    /// </summary>
    /// <param name="netMsg"></param>    
    public void OnClientConnected(NetworkMessage netMsg)
    {
        Debug.Log("A CLIENT HAS CONNECTED");

    }

    const short NameChannelId = 8888;
    /// <summary>
    /// Start to connect 
    /// Which is separated into two parts which means: 
    /// Host:start to make up the base 
    /// Client: Try to connect the base written in the base
    /// </summary>
    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
        red.SetActive(false);
        //Client
        if (playerIndicator.isPlayer)
        {
            Debug.Log("This is client");

            NetworkManager.singleton.playerPrefab = client_c;
            NetworkClient netc = NetworkManager.singleton.StartClient();
            //CLIENT CONNECTION ID
            NetworkConnection netconnect = netc.connection;
            //int connectionID = netconnect.connectionId;

            ///////////////////////////////////////////////////////////////////////////
            NetworkManager.singleton.client.Connect("129.161.54.218", 8888);
            ///////////////////////////////////////////////////////////////////////////
            ///
            NetworkManager.singleton.client.RegisterHandler(2222 , OnClientReceiveFB);
            NetworkManager.singleton.client.RegisterHandler(4444, ClientRecvMsg);

            //SEND USERNAME TO SERVER
            //NetworkManager.singleton.client.RegisterHandler(4321, TestClientReceive);
        }

        
        //Host
        else
        {
            ///////////////////////////////////////////////////////////////////////////
            NetworkManager.singleton.networkPort = 8888;
            NetworkManager.singleton.networkAddress = "129.161.54.218";
            ///////////////////////////////////////////////////////////////////////////
            //THIS START A NetworkServer
            NetworkManager.singleton.playerPrefab = host_c;
            NetworkManager.singleton.StartHost();
            

            //HANDLERS
            NetworkServer.RegisterHandler(1111, ServerRecvLogin);
            NetworkServer.RegisterHandler(3333, ServerRecvMsg);
            NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
        }
    }

    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }
}
