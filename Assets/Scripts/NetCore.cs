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
    public OLScene oLScene;
    public LoginUI loginUI;
    GameObject spawned_player;
    NetworkClient myClient;
    public bool isPlayer;
    public string user_name;
    public PlayerIndicator playerIndicator;
    public ProfileSys profileSys;
    public LoginUI loginui;

    public Text debugtext;
    public GameObject red;

    public Text testText;

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

    

    
    //SEND 3333
    public void ClientSendMsg(string Msg)
    {
        //Client send Accound and Password to the server
        NetworkManager.singleton.client.Send(3333, new StringMessage(Msg));
        Debug.Log("CLIENT SENT REQUEST: " + Msg);
    }

    //HANDLE 3333, SEND 4444
    public void ServerRecvMsg(NetworkMessage Msg)
    {
        SQLHandler sql = new SQLHandler();
        string msg = Msg.ReadMessage<StringMessage>().value;
        Debug.Log("SERVER RECV REQUEST: " + msg);
        string fb = sql.recvMsg(msg);
        NetworkServer.SendToAll(4444, new StringMessage(fb));
        Debug.Log("SERVER SENT FEEDBACK: " + fb);
    }

    //HANDLE, 4444
    public void ACCEPT(string json)
    {

    }
    public void FINISH(string json)
    {

    }

    public void ClientRecvMsg(NetworkMessage Msg)
    {
        string msg = Msg.ReadMessage<StringMessage>().value;
        Debug.Log("CLIENT RECV FEEDBACK: " + msg);
        outputMessage tskOpt = new outputMessage(msg);
        if (tskOpt.getWay().Equals("getDetailsUsr"))
        {
            oLScene.UPDATEINFO(msg);
        }else if (tskOpt.getWay().Equals("addUsr"))
        {
            loginUI.REG_USER(msg);
        }
        else if (tskOpt.getWay().Equals("getallTsk"))
        {

        }
        else if (tskOpt.getWay().Equals("addTsk"))
        {
            oLScene.ADDTASK(msg);
        }
        else if (tskOpt.getWay().Equals("finishTsk"))
        {
            profileSys.UpdateAccepted(tskOpt.getResult());
        }else if (tskOpt.getWay().Equals("takeTsk"))
        {
            profileSys.UpdateAccepted(tskOpt.getResult());
        }
        else if (tskOpt.getWay().Equals("getAcceptedTsk"))
        {
            
        }
        else if (tskOpt.getWay().Equals("getDetailsUsr"))
        {
            
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
            testText.text = "ERROR" + outputFBMsg.getErrorMsg();
        }

    }
    #endregion


    private void Update()
    {
        if (NetworkServer.active)
        {
            //Debug.Log("Server Active!");
        }
    }
    
    public void OnClientConnected(NetworkMessage netMsg)
    {
        Debug.Log("A CLIENT HAS CONNECTED");

    }

    const short NameChannelId = 8888;
    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
        red.SetActive(false);
        //Client
        if (playerIndicator.isPlayer)
        {
            Debug.Log("This is client");


            NetworkClient netc = NetworkManager.singleton.StartClient();
            //CLIENT CONNECTION ID
            NetworkConnection netconnect = netc.connection;
            //int connectionID = netconnect.connectionId;

            ///////////////////////////////////////////////////////////////////////////
            NetworkManager.singleton.client.Connect("192.168.31.165", 8888);
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
            //SetupServer();


            ///////////////////////////////////////////////////////////////////////////
            NetworkManager.singleton.networkPort = 8888;
            NetworkManager.singleton.networkAddress = "192.168.31.165";
            ///////////////////////////////////////////////////////////////////////////
            ///


            //var config = new ConnectionConfig();
            // There are different types of channels you can use, check the official documentation
            //config.AddChannel(QosType.ReliableFragmented);
            //config.AddChannel(QosType.UnreliableFragmented);

            //THIS START A NetworkServer

            NetworkManager.singleton.StartHost();

            //HANDLERS
            NetworkServer.RegisterHandler(1111, ServerRecvLogin);
            NetworkServer.RegisterHandler(3333, ServerRecvMsg);
            //NetworkServer.RegisterHandler(1234, TestServerReceive);
            NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
        }
    }

    /*
    [Command]
    public void CmdMoveClient()
    {

    }
    [ClientRpc]
    public void RpcSendInt()
    {

    }
    */

    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }
}
