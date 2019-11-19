using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StartUpUI : MonoBehaviour
{
    public PlayerIndicator playerIndicator;
    public void ClientOnClick()
    {
        playerIndicator.isPlayer = true;
        //NetworkManager.singleton.networkPort = 9999;

        //NetworkManager.singleton.networkAddress = "129.161.55.47";
        SceneManager.LoadScene(1);
    }
    public void HostOnClick()
    {
        playerIndicator.isPlayer = false;
        //NetworkManager.singleton.networkPort = 9999;

        //NetworkManager.singleton.networkAddress = "129.161.55.47";
        SceneManager.LoadScene(1);
    }
}
