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
    public Text SysMsg;

    public Dictionary<string, string> dbcheck;
    public NetCore netCore;

    public ProfileSys profileSys;

    bool isStart;

    /// <summary>
    /// Listener of login button
    /// </summary>
    public void LoginOnClick()
    {
        //CLIENT
        if (playerIndicator.isPlayer)
        {
            if(loginAccount.text == "" || loginPassword.text == "")
            {
                SysMsg.text = "Empty login information";
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
        //HOST
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
                Dictionary<string, Dictionary<string, string>> outDic = outputFBMsg.getResult();
                profileSys.exp = Convert.ToInt32(outDic["0"]["exp"]);
                profileSys.gold = Convert.ToInt32(outDic["0"]["coin"]);
                profileSys.username = outDic["0"]["name"].ToString();
                playerIndicator.UserName = loginAccount.text;
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
                SysMsg.text = "MIAO, Invalid username or password";
                SysMsg.color = Color.red;
            }
        }
    
    }

    /// <summary>
    /// Give registration feedback to clients
    /// </summary>
    /// <param name="json"></param>
    public void REG_USER(string json)
    {
        outputMessage regOpt = new outputMessage(json);
        //Registration compelete, hide login UI
        if (regOpt.getSuccess())
        {
            Debug.Log("REG SUCCESS");
            signup_box.SetActive(false);
            SysMsg.text = "MIAO, registration compelete!";
        }
        //Registration failed, report error
        else
        {
            Debug.Log(regOpt.getErrorMsg());
            SysMsg.color = Color.red;
            SysMsg.text = "MIAO, " + regOpt.getErrorMsg();
        }
    }

    /// <summary>
    /// Listener of sign up button
    /// </summary>
    public void UserRegister()
    {
        //CLIENT
        if (playerIndicator.isPlayer)
        {
            SQLHandler sql = new SQLHandler();
            inputMessage regMsg = new inputMessage();
            regMsg.addWay("addUsr");
            regMsg.addArg("name", signupAccount.text);
            regMsg.addArg("pwd", signupPassword.text);
            netCore.ClientSendMsg(regMsg.getString());
        }
        //HOST
        else
        {
            SQLHandler sql = new SQLHandler();
            inputMessage regMsg = new inputMessage();
            regMsg.addWay("addUsr");
            regMsg.addArg("name", signupAccount.text);
            regMsg.addArg("pwd", signupPassword.text);
            string strOpt = sql.recvMsg(regMsg.getString());
            REG_USER(strOpt);
        }

    }

    /// <summary>
    /// Close or open sign up window
    /// </summary>
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
