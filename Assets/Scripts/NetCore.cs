using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetCore : MonoBehaviour
{
    public GameObject player_pre;
    GameObject spawned_player;
    NetworkClient myClient;
    public bool isPlayer;

    void OnConnected(NetworkMessage netMsg)
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
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        //client.RegisterHandler(MsgType.Disconnect, OnDisconnected);
    }
    // Create a client and connect to the server port
    public void SetupClient()
    {
        int port = 9999;
        string ip = "localhost";

        // The id we use to identify our messages and register the handler
        short messageID = 1000;

        // The network client
        NetworkClient client;
        var config = new ConnectionConfig();

        // Config the Channels we will use
        config.AddChannel(QosType.ReliableFragmented);
        config.AddChannel(QosType.UnreliableFragmented);

        // Create the client ant attach the configuration
        client = new NetworkClient();
        client.Configure(config, 1);

        // Register the handlers for the different network messages
        RegisterClientHandlers();

        // Connect to the server
        client.Connect(ip, port);
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
    public void OnServerReceive(NetworkMessage netMsg)
    {
        Debug.Log("Server Received MSG!!!!!!!!!!!!!");
    }

    private void RegisterServerHandlers()
    {
        // Unity have different Messages types defined in MsgType
        //NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);
        //NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisconnected);

        // Our message use his own message type.
        //NetworkServer.RegisterHandler(messageID, OnMessageReceived);
        NetworkServer.RegisterHandler(1000, OnServerReceive);
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
        RegisterServerHandlers();

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

    private void Start()
    {
        
        if (GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>().isPlayer)
        {
            SetupClient();
            spawned_player = Instantiate(player_pre);
            spawned_player.GetComponent<Player>().isPlayer = true;
            NetworkServer.Spawn(spawned_player);
        }
        else
        {
            SetupServer();
            spawned_player = Instantiate(player_pre);
            spawned_player.GetComponent<Player>().isPlayer = false;
            NetworkServer.Spawn(spawned_player);
        }
    }
}
