using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginUI : MonoBehaviour
{
    public PlayerIndicator playerIndicator;
    //public Text username;
    public SQLHandler sql;

    public string username;
    public string pwd;
    public GameObject warning;

    public Dictionary<string, string> dbcheck;

    bool isStart;


    //TODO: MORE PORT NUMBERS
    public void LoginOnClick()
    {
        playerIndicator.isPlayer = true;

        /*
        dbcheck = sql.searchUsr(username, pwd);
        
        //SHOW WARNING MESSAGE
        if (dbcheck.ContainsKey("Error"))
        {
            warning.SetActive(true);
            warning.GetComponent<Text>().text = dbcheck["Error"];
        }
        else
        {
            playerIndicator.Username = username;
            SceneManager.LoadScene(1);
        }
        */
        SceneManager.LoadScene(1);
    }

    public void warningClose()
    {
        warning.SetActive(false);
    }

    //THIS IS GOING TO BE A SERVER
    //WE ARE USING IT AS A HOST NOW
    public void AdminOnClick()
    {
        playerIndicator.isPlayer = false;
        SceneManager.LoadScene(1);
    }

    private void Start()
    {
        isStart = false;
    }
}
