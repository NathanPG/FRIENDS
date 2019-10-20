using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public bool isPlayer;
    public float movespeed = 1f;
    Vector2 direction = new Vector2();

    public void MOVE()
    {
        Debug.Log("fk my self");
    }
    // Start is called before the first frame update
    void Start()
    {
        //If this is a player
        if (isPlayer)
        {
            Debug.Log("This is a player object");
        }
        //If this is a host
        else
        {
            Debug.Log("This is a server object");
        }
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
