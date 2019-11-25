using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class questINFO : MonoBehaviour
{
    public string QID;
    public string content;
    public string title;
    public int exp;
    public int coin;
    public string owner;
    public string current_user;

    public PlayerIndicator playerIndicator;
    public Text QuestTitle;
    public Text QuestContent;
    public Text QuestCoin;
    public Text QuestExp;
    public ProfileSys profileSys;
    public Button acceptButton;
    public Button finishButton;
    public NetCore netCore;
    public OLScene oLScene;

    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
        netCore = GameObject.FindGameObjectWithTag("NETCORE").GetComponent<NetCore>();
        oLScene = GameObject.FindGameObjectWithTag("CANVAS").GetComponent<OLScene>();
    }

    public void thisOnClick()
    {
        QuestTitle.text = title;
        QuestContent.text = content;
        QuestCoin.text = coin.ToString();
        QuestExp.text = exp.ToString();
        if (acceptButton)
        {
            acceptButton.onClick.RemoveAllListeners();
            acceptButton.onClick.AddListener(AcceptOnClick);
        }
        if (finishButton)
        {
            finishButton.onClick.RemoveAllListeners();
            finishButton.onClick.AddListener(FinishOnClick);
        }
    }

    public void AcceptOnClick()
    {
        SQLHandler sql = new SQLHandler();
        //Accept TASK
        inputMessage iptMsg = new inputMessage();
        iptMsg.addWay("takeTsk");
        iptMsg.addArg("id", QID);
        iptMsg.addArg("taker", current_user);
        //CLIENT
        if (playerIndicator.isPlayer)
        {
            netCore.ClientSendMsg(iptMsg.getString());
            oLScene.infoWindow.SetActive(false);
            oLScene.questListWindow.SetActive(false);
            oLScene.SingleQuestClose();
        }
        //HOST
        else
        {
            string strOpt = sql.recvMsg(iptMsg.getString());
            outputMessage tskOpt = new outputMessage(strOpt);
            profileSys.UpdateAccepted(tskOpt.getResult());
            oLScene.infoWindow.SetActive(false);
            oLScene.questListWindow.SetActive(false);
            oLScene.SingleQuestClose();
        }
        
    }

    public void FinishOnClick()
    {
        SQLHandler sql = new SQLHandler();
        //FINISH TASK
        inputMessage iptMsg = new inputMessage();
        iptMsg.addWay("finishTsk");
        iptMsg.addArg("id", QID);
        iptMsg.addArg("name", current_user);
        //CLIENT
        if (playerIndicator.isPlayer)
        {
            netCore.ClientSendMsg(iptMsg.getString());
            oLScene.infoWindow.SetActive(false);
            oLScene.acceptedWindow.SetActive(false);
            oLScene.SingleQuestClose();
        }
        //HOST
        else
        {
            string strOpt = sql.recvMsg(iptMsg.getString());
            outputMessage tskOpt = new outputMessage(strOpt);
            profileSys.UpdateAccepted(tskOpt.getResult());
            oLScene.infoWindow.SetActive(false);
            oLScene.acceptedWindow.SetActive(false);
            oLScene.SingleQuestClose();
        }
        
    }
}
