using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTest : MonoBehaviour
{
    public float movespeed = 5f;

    
    public void MOVE()
    {
        if (Input.GetKey(KeyCode.W))

        {
            this.gameObject.transform.Translate(2*Vector3.forward * Time.deltaTime);
            this.GetComponent<Animator>().SetTrigger("forward");
        }

        if (Input.GetKey(KeyCode.S))

        {
            this.gameObject.transform.Translate(2*Vector3.back * Time.deltaTime);
            this.GetComponent<Animator>().SetTrigger("back");
        }

        if (Input.GetKey(KeyCode.A))

        {
            this.gameObject.transform.Translate(2*Vector3.left * Time.deltaTime);
            this.GetComponent<Animator>().SetTrigger("left");
        }

        if (Input.GetKey(KeyCode.D))

        {
            this.gameObject.transform.Translate(2*Vector3.right * Time.deltaTime);
            this.GetComponent<Animator>().SetTrigger("right");
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
            this.GetComponent<Animator>().SetBool("forward", false);
            this.GetComponent<Animator>().SetBool("back", false);
            this.GetComponent<Animator>().SetBool("left", false);
            this.GetComponent<Animator>().SetBool("right", false);
            //this.GetComponent<Animator>().SetBool("idel", true);
        }
        
    }

    private void FixedUpdate()
    {
        
    }

    private void Start()
    {
        this.GetComponent<Animator>().SetBool("forward", false);
        this.GetComponent<Animator>().SetBool("back", false);
        this.GetComponent<Animator>().SetBool("left", false);
        this.GetComponent<Animator>().SetBool("right", false);
        //this.GetComponent<Animator>().SetBool("idel", true);
    }
    // Update is called once per frame
    void Update()
    {
        MOVE();

    
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // 0 is the groundlayerindex
        if (Physics.Raycast(ray, out hitInfo, 200,0))
        {
            Vector3 target = hitInfo.point;
            target.y = transform.position.y;
            transform.LookAt(target);
        }

        /*
            // 获得鼠标当前位置的X和Y
            float mouseX = Input.GetAxis("Mouse X") * movespeed;
            float mouseY = Input.GetAxis("Mouse Y") * movespeed;

            // 鼠标在Y轴上的移动号转为摄像机的上下运动，即是绕着X轴反向旋转
            Camera.main.transform.localRotation = Camera.main.transform.localRotation * Quaternion.Euler(-mouseY, 0, 0);
            // 鼠标在X轴上的移动转为主角左右的移动，同时带动其子物体摄像机的左右移动
            transform.localRotation = transform.localRotation * Quaternion.Euler(0, mouseX, 0);
            */
    }

}
