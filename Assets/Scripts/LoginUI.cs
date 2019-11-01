using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LoginUI : MonoBehaviour
{
    
    //public Text username;
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

    public ProfileSys profileSys;

    bool isStart;

    public void LoginOnClick()
    {
        //Debug.Log("loginAccount:" + loginAccount.text);
        //Debug.Log("loginPassword:" + loginPassword.text);
        //Player
        if (playerIndicator.isPlayer)
        {
            if(loginAccount.text == "" || loginPassword.text == "")
            {
                //TODO: EMPTY
            }
            else
            {
                inputMessage UNPWMsg = new inputMessage();
                UNPWMsg.addWay("searchUsr");
                UNPWMsg.addArg("name", loginAccount.text) ;
                UNPWMsg.addArg("pwd", loginPassword.text);
                string loginMsg = UNPWMsg.getString();
                Debug.Log(loginMsg);
                netCore.ClientSendLogIn(loginMsg);
            }
        }
        //Server
        else
        {
            inputMessage UNPWMsg = new inputMessage();
            UNPWMsg.addWay("searchUsr");
            UNPWMsg.addArg("name", loginAccount.text);
            UNPWMsg.addArg("pwd", loginPassword.text);
            string loginMsg = UNPWMsg.getString();
            SQLHandler tmp = new SQLHandler();

            string LogInOutPut = tmp.recvMsg(loginMsg);

            outputMessage outputFBMsg = new outputMessage(LogInOutPut);

            if (outputFBMsg.getSuccess())
            {

                //outputFBMsg.lst["result"]["name"];
                //outputFBMsg.lst["result"]["pwd"];        
                Dictionary<string, Dictionary<string, string>> outDic = outputFBMsg.getResut();
                profileSys.exp = Convert.ToInt32(outDic["0"]["exp"]);
                profileSys.gold = Convert.ToInt32(outDic["0"]["coin"]);
                Debug.Log("CLIENT RECEIVED INFO");

                //getAlltsk

                inputMessage tskMessage = new inputMessage();
                tskMessage.addWay("getallTsk");
                string strOpt = tmp.recvMsg(tskMessage.getString());
                outputMessage tskOpt = new outputMessage(strOpt);

                Debug.Log(strOpt);
                
                profileSys.UpdateResult(tskOpt.getResult());
                loginUI.SetActive(false);
            }
            else
            {
                Debug.Log("SOME KIND OF LOG IN ERROR");
            }
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
        //netCore.ClientSendName(signupAccount.text, signupPassword.text);
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
