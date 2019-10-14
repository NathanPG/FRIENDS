using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginUI : NetworkBehaviour
{
    public string Port;
    public string IpAddress;
    public Text username;
    public Player player;
    public NetCore net;
    bool isStart;


    //TODO: MORE PORT NUMBERS
    public void LoginOnClick()
    {
        //net.SetupClient();
        SceneManager.LoadScene(2);
        player.isPlayer = true;
        /*
        if (username.text != "")
        {
            //
        }
        else
        {
            //warningPanel.SetActive(true);
        }
        */
    }

    //THIS IS GOING TO BE A SERVER
    //WE ARE USING IT AS A HOST NOW
    public void AdminOnClick()
    {
        //net.SetupServer();
        SceneManager.LoadScene(2);
        player.isPlayer = false;

    }

    private void Start()
    {
        isStart = false;
    }
}
