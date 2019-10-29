using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LoginUI : MonoBehaviour
{
    
    //public Text username;
    public SQLHandler sql;
    public PlayerIndicator playerIndicator;

    public string username;
    public string pwd;
    public GameObject warning;
    public GameObject signup_box;

    public InputField loginAccount;
    public InputField loginPassword;
    public InputField signupAccount;
    public InputField signupPassword;
    public GameObject loginUI;

    public Dictionary<string, string> dbcheck;
    public NetCore netCore;

    bool isStart;

    public void LoginOnClick()
    {
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

        }
        */

        //playerIndicator.isPlayer代表这是个CLIENT，需要将用户名密码发给HOST,HOST找数据库CHECK用户名密码
        //!playerIndicator.isPlayer代表这是个HOST，可以直接找数据库CHECK用户名密码

        //如果登陆成功，那就关掉登陆界面开始游戏
        if (true)
        {
            loginUI.SetActive(false);
        }
        //如果有任何问题，那就报错
        else
        {

        }
        
    }
    //THIS IS GOING TO BE A SERVER
    //WE ARE USING IT AS A HOST NOW
    public void AdminOnClick()
    {
        loginUI.SetActive(false);
    }

    //Register on click
    public void UserRegister()
    {
        //Register new user to the database
        Debug.Log("signupAccount:" + signupAccount.text);
        Debug.Log("signupPassword:" + signupPassword.text);
        netCore.ClientSendName(signupAccount.text, signupPassword.text);
        //如果没问题那就完成注册，关掉注册窗口
        if (true)
        {
            signup_box.SetActive(false);
        }
        //如果已经被注册之类的，那就弹出error message
        else
        {

        }
        
    }

    public void warningClose()
    {
        warning.SetActive(false);
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
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
        isStart = false;
    }
}
