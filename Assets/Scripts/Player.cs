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
        if (Input.GetKey(KeyCode.A))
        {
            direction.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction.y = 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            direction.x = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction.x = -1;
        }
        transform.Translate(new Vector3(0, 0, direction.x * Time.deltaTime * movespeed));
    }
    // Start is called before the first frame update
    void Start()
    {
        //If this is a player
        if (isPlayer)
        {

        }
        //If this is a host
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            MOVE();
        }
    }
}
