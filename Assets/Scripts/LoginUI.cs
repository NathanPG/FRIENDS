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
    public GameObject signup_box;

    public InputField loginAccount;
    public InputField loginPassword;
    public InputField signupAccount;
    public InputField signupPassword;

    public Dictionary<string, string> dbcheck;

    bool isStart;

    public void LoginOnClick()
    {
        playerIndicator.isPlayer = true;
        Debug.Log("loginAccount:" + loginAccount.text);
        Debug.Log("loginPassword:" + loginPassword.text);
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
        //SceneManager.LoadScene(1);
    }

    //Register on click
    public void UserRegister()
    {
        //Register new user to the database
        Debug.Log("signupAccount:" + signupAccount.text);
        Debug.Log("signupPassword:" + signupPassword.text);
        signup_box.SetActive(false);
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

    
    public void OpenSignUp()
    {
        signup_box.SetActive(true);
    }
    public void CloseSignUp()
    {
        signup_box.SetActive(false);
    }

    private void Start()
    {
        isStart = false;
    }
}
