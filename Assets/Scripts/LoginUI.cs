using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginUI : MonoBehaviour
{
    public Text username;
    public GameObject player_pre;
    GameObject spawned_player;
    bool isStart;


    //TODO: MORE PORT NUMBERS
    public void LoginOnClick()
    {
        SceneManager.LoadScene(2);
        spawned_player = Instantiate(player_pre);
        spawned_player.GetComponent<Player>().isPlayer = true;
        NetworkServer.Spawn(spawned_player);
        
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
        SceneManager.LoadScene(2);
        spawned_player = Instantiate(player_pre);
        spawned_player.GetComponent<Player>().isPlayer = false;
        NetworkServer.Spawn(spawned_player);
    }

    private void Start()
    {
        isStart = false;
    }
}
