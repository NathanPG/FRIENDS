using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpUI : MonoBehaviour
{
    public PlayerIndicator playerIndicator;
    public void ClientOnClick()
    {
        playerIndicator.isPlayer = true;
        SceneManager.LoadScene(1);
    }
    public void HostOnClick()
    {
        playerIndicator.isPlayer = false;
        SceneManager.LoadScene(1);
    }
}
