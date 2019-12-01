using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StartUpUI : MonoBehaviour
{
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
}
