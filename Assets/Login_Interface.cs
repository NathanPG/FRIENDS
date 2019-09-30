using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login_Interface : MonoBehaviour
{
    public Text username;
    public GameObject warningPanel;
    public void Login_UI()
    {
        if(username.text!="")
        {
            SceneManager.LoadScene(4);
        }
        else
        {
            warningPanel.SetActive(true);
        }
    }

    public void WarmingDisapper()
    {
        warningPanel.SetActive(false);
    }
}
