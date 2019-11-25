using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ModelTest : NetworkTransform
{
    public float movespeed = 5f;

    [Command]
    public void CmdMOVE()
    {
        if (Input.GetKey(KeyCode.W))

        {
            transform.Translate(2*Vector3.forward * Time.deltaTime);
            GetComponent<Animator>().SetTrigger("forward");
        }

        if (Input.GetKey(KeyCode.S))

        {
            transform.Translate(2*Vector3.back * Time.deltaTime);
            GetComponent<Animator>().SetTrigger("back");
        }

        if (Input.GetKey(KeyCode.A))

        {
            transform.Translate(2*Vector3.left * Time.deltaTime);
            GetComponent<Animator>().SetTrigger("left");
        }

        if (Input.GetKey(KeyCode.D))

        {
            transform.Translate(2*Vector3.right * Time.deltaTime);
            GetComponent<Animator>().SetTrigger("right");
        }
        /*
        if (Input.GetKeyUp(KeyCode.W))
        {
            this.GetComponent<Animator>().SetBool("forward", false);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            this.GetComponent<Animator>().SetBool("back", false);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            this.GetComponent<Animator>().SetBool("left", false);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            this.GetComponent<Animator>().SetBool("right", false);
        }
        */

        
        if (!Input.anyKey)
        {
            GetComponent<Animator>().SetBool("forward", false);
            GetComponent<Animator>().SetBool("back", false);
            GetComponent<Animator>().SetBool("left", false);
            GetComponent<Animator>().SetBool("right", false);
            //this.GetComponent<Animator>().SetBool("idel", true);
        }
        
    }

    private void FixedUpdate()
    {
        
    }

    private void Start()
    {
        GetComponent<Animator>().SetBool("forward", false);
        GetComponent<Animator>().SetBool("back", false);
        GetComponent<Animator>().SetBool("left", false);
        GetComponent<Animator>().SetBool("right", false);
        //this.GetComponent<Animator>().SetBool("idel", true);
    }

    Vector2 rotation = new Vector2(0, 0);
    public float speed = 3;
    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            CmdMOVE();
            rotation.y += Input.GetAxis("Mouse X");
            //rotation.x += -Input.GetAxis("Mouse Y");
            transform.eulerAngles = (Vector2)rotation * speed;
        }
    }
}
