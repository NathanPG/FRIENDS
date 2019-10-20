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

    void OnClinetConnected(NetworkMessage netMsg)
    {
        /*
        // Do stuff when connected to the server

        MyNetworkMessage messageContainer = new MyNetworkMessage();
        messageContainer.message = "Hello server!";

        // Say hi to the server when connected
        myClient.Send(1000, messageContainer);
        */
        Debug.Log("CLIENT CONNECTED!");
    }

    // Register the handlers for the different message types
    void RegisterClientHandlers()
    {
        // Unity have different Messages types defined in MsgType
        //client.RegisterHandler(messageID, OnMessageReceived);
        myClient.RegisterHandler(MsgType.Connect, OnClinetConnected);
        //client.RegisterHandler(MsgType.Disconnect, OnDisconnected);
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        int port = 9999;
        string ip = "localhost";

        // The id we use to identify our messages and register the handler
        short TESTID = 1000;

        var config = new ConnectionConfig();

        // Config the Channels we will use
        config.AddChannel(QosType.ReliableFragmented);
        config.AddChannel(QosType.UnreliableFragmented);

        // Create the client ant attach the configuration
        myClient = new NetworkClient();
        myClient.Configure(config, 1);

        // Register the handlers for the different network messages
        RegisterClientHandlers();

        // Connect to the server
        myClient.Connect(ip, port);

        var TESTMSG = new StringMessage("CLIENT MIAO IS HERE!");
        //myClient.Send(TESTID, TESTMSG);
        /*
        void OnDisconnected(NetworkMessage message)
        {
            // Do stuff when disconnected to the server
        }

        
        // Message received from the server
        void OnMessageReceived(NetworkMessage netMessage)
        {
            // You can send any object that inherence from MessageBase
            // The client and server can be on different projects, as long as the MyNetworkMessage or the class you are using have the same implementation on both projects
            // The first thing we do is deserialize the message to our custom type
            var objectMessage = netMessage.ReadMessage<MyNetworkMessage>();

            Debug.Log("Message received: " + objectMessage.message);
        }
        */
    }

    

    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();

    }

    public void SetupServer()
    {
        int port = 9999;
        int maxConnections = 10;

        // The id we use to identify our messages and register the handler
        short messageID = 1000;

        Application.runInBackground = true;
        // Use this for initialization
        // Register handlers for the types of messages we can receive
        //RegisterServerHandlers();

        var config = new ConnectionConfig();
        // There are different types of channels you can use, check the official documentation
        config.AddChannel(QosType.ReliableFragmented);
        config.AddChannel(QosType.UnreliableFragmented);

        var ht = new HostTopology(config, maxConnections);
        if (!NetworkServer.Configure(ht))
        {
            Debug.Log("No server created, error on the configuration definition");
            return;
        }
        else
        {
            // Start listening on the defined port
            if (NetworkServer.Listen(port))
                Debug.Log("Server created, listening on port: " + port);
            else
                Debug.Log("No server created, could not listen to the port: " + port);
        }
        
        /*
        void OnClientConnected(NetworkMessage netMessage)
        {
            // Do stuff when a client connects to this server

            // Send a thank you message to the client that just connected
            MyNetworkMessage messageContainer = new MyNetworkMessage();
            messageContainer.message = "Thanks for joining!";

            // This sends a message to a specific client, using the connectionId
            NetworkServer.SendToClient(netMessage.conn.connectionId, messageID, messageContainer);

            // Send a message to all the clients connected
            messageContainer = new MyNetworkMessage();
            messageContainer.message = "A new player has conencted to the server";

            // Broadcast a message a to everyone connected
            NetworkServer.SendToAll(messageID, messageContainer);
        }

        void OnClientDisconnected(NetworkMessage netMessage)
        {
            // Do stuff when a client dissconnects
        }

        void OnMessageReceived(NetworkMessage netMessage)
        {
            // You can send any object that inherence from MessageBase
            // The client and server can be on different projects, as long as the MyNetworkMessage or the class you are using have the same implementation on both projects
            // The first thing we do is deserialize the message to our custom type
            var objectMessage = netMessage.ReadMessage<MyNetworkMessage>();
            Debug.Log("Message received: " + objectMessage.message);

        }
        */

    }

    /*
    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }
    */

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
}
