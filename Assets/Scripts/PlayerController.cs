using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    public float movespeed = 1f;

    public void MOVE()
    {
        if (Input.GetKey(KeyCode.W))

        {
            this.gameObject.transform.Translate(Vector3.forward * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.S))

        {

            this.gameObject.transform.Translate(Vector3.back * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.A))

        {

            this.gameObject.transform.Translate(Vector3.left * Time.deltaTime);

        }

        if (Input.GetKey(KeyCode.D))

        {

            this.gameObject.transform.Translate(Vector3.right * Time.deltaTime);

        }

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MOVE();
        /*
        if (isLocalPlayer)
        {
            
        }
        */
    }
}
