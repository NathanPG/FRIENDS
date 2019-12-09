using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;

public class StartUpUI : MonoBehaviour
{
    public string server_ip;
    public InputField ip_input;

    public PlayerIndicator playerIndicator;
    /// <summary>
    /// Run the game as a client. (Button listener)
    /// </summary>
    public void ClientOnClick()
    {
        playerIndicator.isPlayer = true;
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// Run the game as a host. (Button listener)
    /// </summary>
    public void HostOnClick()
    {
        playerIndicator.isPlayer = false;
        SceneManager.LoadScene(1);
    }

    public void UpdateIP()
    {
        server_ip = ip_input.text;
        playerIndicator.IP = server_ip;
    }
}
