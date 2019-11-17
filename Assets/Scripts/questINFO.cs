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

    public Text QuestTitle;
    public Text QuestContent;
    public Text QuestCoin;
    public Text QuestExp;
    public ProfileSys profileSys;
    public Button acceptButton;

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
    }

    public void AcceptOnClick()
    {
        SQLHandler sql = new SQLHandler();
        //Accept TASK
        inputMessage iptMsg = new inputMessage();
        iptMsg.addWay("takeTsk");
        iptMsg.addArg("id", QID);
        iptMsg.addArg("taker", current_user);

        string strOpt = sql.recvMsg(iptMsg.getString());
        outputMessage tskOpt = new outputMessage(strOpt);
        profileSys.UpdateAccepted(tskOpt.getResult());
    }
}
