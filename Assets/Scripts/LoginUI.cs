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
    bool isStart;
    //public GameObject warningPanel;

    //TODO: MORE PORT NUMBERS
    public void LoginOnClick()
    {
        NetworkManager.singleton.networkPort = 9878;
        NetworkManager.singleton.StartClient();
        SceneManager.LoadScene(2);
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
        NetworkManager.singleton.networkAddress = "localhost";
        NetworkManager.singleton.networkPort = 9877;
        NetworkManager.singleton.StartHost();
        SceneManager.LoadScene(2);
    }

    private void Start()
    {
        isStart = false;
    }
}
