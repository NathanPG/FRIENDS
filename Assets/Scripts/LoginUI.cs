using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginUI : MonoBehaviour
{
    public PlayerIndicator playerIndicator;
    public Text username;
    bool isStart;


    //TODO: MORE PORT NUMBERS
    public void LoginOnClick()
    {
        playerIndicator.isPlayer = true;
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
        playerIndicator.isPlayer = false;
        SceneManager.LoadScene(2);
    }

    private void Start()
    {
        isStart = false;
    }
}
