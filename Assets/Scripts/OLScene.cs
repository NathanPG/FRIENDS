using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Threading;

public class OLScene : MonoBehaviour
{
    public NetCore netcore;
    public ProfileSys profileSys;

    public List<GameObject> questList = new List<GameObject>();
    public List<GameObject> acceptedList = new List<GameObject>();

    public PlayerIndicator playerIndicator;

    public Text singleQuestTitle;
    public Text singleQuestContent;
    public Text singleQuestCoin;
    public Text singleQuestExp;
    public Button acceptButton;
    public Button finishButton;

    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
    }

    #region Info_Window
    public Text exp;
    public Text coin;
    public Text user_name;
    public GameObject infoWindow;

    public void UPDATEINFO(string json)
    {
        outputMessage tskOpt = new outputMessage(json);
        profileSys.exp = Convert.ToInt32(tskOpt.getResult()["0"]["exp"]);
        profileSys.gold = Convert.ToInt32(tskOpt.getResult()["0"]["coin"]);
        profileSys.username = tskOpt.getResult()["0"]["name"].ToString();
        playerIndicator.UserName = profileSys.username;
        exp.text = "EXP: " + profileSys.exp.ToString();
        coin.text = "Coin: " + profileSys.gold.ToString();
        user_name.text = profileSys.username;
    }
    public void InfoOnClick() {
        //CLIENT
        if (playerIndicator.isPlayer)
        {
            //SEND
            inputMessage pMsg = new inputMessage();
            pMsg.addWay("getDetailsUsr");
            pMsg.addArg("name", playerIndicator.UserName);
            netcore.ClientSendMsg(pMsg.getString());
        }
        //HOST
        else
        {
            SQLHandler sql = new SQLHandler();
            inputMessage pMsg = new inputMessage();
            pMsg.addWay("getDetailsUsr");
            pMsg.addArg("name", playerIndicator.UserName);
            string strOpt = sql.recvMsg(pMsg.getString());
            UPDATEINFO(strOpt);
        }
        
        if (infoWindow.activeInHierarchy)
        {
            infoWindow.SetActive(false);
        }
        else
        {
            infoWindow.SetActive(true);
        }
    }

    public void InfoClose() { infoWindow.SetActive(false); }
    #endregion

    #region Quest_Detail
    
    
    public GameObject SingleQuest;
    public GameObject AcceptButtoninDetail;
    public GameObject FinishButtoninDetail;
    public Text Coin;
    public Text Exp;
    public void LobbyQuestOpen()
    {
        if (SingleQuest.activeInHierarchy)
        {
            SingleQuest.SetActive(false);
        }
        else
        {
            SingleQuest.SetActive(true);
            FinishButtoninDetail.SetActive(false);
            AcceptButtoninDetail.SetActive(true);
        }
    }

    public void PersonalQuestOpen()
    {
        if (SingleQuest.activeInHierarchy)
        {
            SingleQuest.SetActive(false);
        }
        else
        {
            SingleQuest.SetActive(true);
            FinishButtoninDetail.SetActive(true);
            AcceptButtoninDetail.SetActive(false);
        }
    }

    public void SingleQuestClose() { SingleQuest.SetActive(false); AcceptButtoninDetail.SetActive(false); FinishButtoninDetail.SetActive(false); }
    #endregion

    #region Publish_Window
    public GameObject publishWindow;
    public InputField questContent;
    public InputField questTitle;
    public Dropdown questCoin;
    public void publishWindowOn() {
        if (publishWindow.activeInHierarchy)
        {
            publishWindow.SetActive(false);
        }
        else
        {
            publishWindow.SetActive(true);
        }
    }
    public void publishWindowOff() { publishWindow.SetActive(false); }

    public void ADDTASK(string json)
    {
        outputMessage opt = new outputMessage(json);

        if (opt.getSuccess())
        {
            Debug.Log("success");
        }
        else
        {
            Debug.Log(json);
        }
    }
    public void pulishOnClick()
    {
        //CLIENT
        if (playerIndicator.isPlayer)
        {
            string owner = playerIndicator.UserName;
            inputMessage publishMsg = new inputMessage();
            publishMsg.addWay("addTsk");
            publishMsg.addArg("title", questTitle.text);
            publishMsg.addArg("content", questContent.text);
            publishMsg.addArg("coin", questCoin.value.ToString());
            publishMsg.addArg("owner", owner);
            netcore.ClientSendMsg(publishMsg.getString());
        }
        //Host
        else
        {
            string owner = playerIndicator.UserName;
            inputMessage publishMsg = new inputMessage();
            publishMsg.addWay("addTsk");
            publishMsg.addArg("title", questTitle.text);
            publishMsg.addArg("content", questContent.text);
            publishMsg.addArg("coin", questCoin.value.ToString());
            publishMsg.addArg("owner", owner);
            SQLHandler sql = new SQLHandler();
            string strOpt = sql.recvMsg(publishMsg.getString());
            ADDTASK(strOpt);
        }
    }
    #endregion

    #region Task_List
    public ScrollRect LobbytRect;
    public ScrollRect AcceptRect;
    public GameObject questListWindow;
    public GameObject questTitilePre;

    public void QuestListOn()
    {
        if (questListWindow.activeInHierarchy)
        {
            questListWindow.SetActive(false);
        }

        else
        {
            //UPDATE TASK
            SQLHandler sql = new SQLHandler();
            inputMessage tskMessage = new inputMessage();
            tskMessage.addWay("getallTsk");
            string strOpt = sql.recvMsg(tskMessage.getString());
            outputMessage tskOpt = new outputMessage(strOpt);
            profileSys.UpdateResult(tskOpt.getResult());


            questListWindow.SetActive(true);
            foreach (GameObject task in questList)
            {
                Destroy(task);
            }
            foreach (KeyValuePair<string, Dictionary<string, string>> itr in profileSys.Task_List)
            {
                UpdateQuestList(itr.Value["QID"], itr.Value["content"], itr.Value["title"], 
                    itr.Value["exp"], itr.Value["coin"], itr.Value["owner"],false);
            }
        }
    }

    public void RefreshOnClick()
    {
        //UPDATE TASK
        SQLHandler sql = new SQLHandler();
        inputMessage tskMessage = new inputMessage();
        tskMessage.addWay("getallTsk");
        string strOpt = sql.recvMsg(tskMessage.getString());
        outputMessage tskOpt = new outputMessage(strOpt);
        profileSys.UpdateResult(tskOpt.getResult());

        questListWindow.SetActive(true);
        foreach (GameObject task in questList)
        {
            Destroy(task);
        }
        if (profileSys.Task_List.Count != 0)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> itr in profileSys.Task_List)
            {
                UpdateQuestList(itr.Value["QID"], itr.Value["content"], itr.Value["title"],
                    itr.Value["exp"], itr.Value["coin"], itr.Value["owner"], false);
            }
        }
        
    }

    public void QuestListOff() { questListWindow.SetActive(false); }


    public GameObject UpdateQuestList(string QID, string content, string title, 
        string exp, string coin, string owner, bool ispersonal)
    {
        if (questTitilePre == null)
        {
            return null;
        }
        GameObject temp;
        if (ispersonal)
        {
            temp = Instantiate(questTitilePre, AcceptRect.content, false);
        }
        else
        {
            temp = Instantiate(questTitilePre, LobbytRect.content, false);
        }
        
        temp.GetComponentInChildren<Text>().text = title;
        if (ispersonal)
        {
            temp.GetComponent<Button>().onClick.RemoveAllListeners();
            temp.GetComponent<Button>().onClick.AddListener(PersonalQuestOpen);
        }
        else
        {
            temp.GetComponent<Button>().onClick.RemoveAllListeners();
            temp.GetComponent<Button>().onClick.AddListener(LobbyQuestOpen);
        }
        temp.GetComponent<questINFO>().QuestTitle = singleQuestTitle;
        temp.GetComponent<questINFO>().QuestContent = singleQuestContent;
        temp.GetComponent<questINFO>().QuestCoin = singleQuestCoin;
        temp.GetComponent<questINFO>().QuestExp = singleQuestExp;
        temp.GetComponent<questINFO>().QID = QID;
        temp.GetComponent<questINFO>().content = content;
        temp.GetComponent<questINFO>().title = title;
        temp.GetComponent<questINFO>().exp = Convert.ToInt32(exp);
        temp.GetComponent<questINFO>().coin = Convert.ToInt32(coin);
        temp.GetComponent<questINFO>().owner = owner;
        temp.GetComponent<questINFO>().current_user = playerIndicator.UserName;
        temp.GetComponent<questINFO>().profileSys = profileSys;
        temp.GetComponent<questINFO>().acceptButton = acceptButton;
        temp.GetComponent<questINFO>().finishButton = finishButton;
        if (ispersonal)
        {
            acceptedList.Add(temp);
            LayoutRebuilder.ForceRebuildLayoutImmediate(AcceptRect.content);
            AcceptRect.verticalNormalizedPosition = 0;
        }
        else
        {
            questList.Add(temp);
            LayoutRebuilder.ForceRebuildLayoutImmediate(LobbytRect.content);
            LobbytRect.verticalNormalizedPosition = 0;
        }

        

        return temp;
    }
    #endregion

    #region Accpeted_List

    public GameObject acceptedWindow;
    public void AcceptedListOff() { acceptedWindow.SetActive(false); }

    public void AcceptedListOn() {
        if (acceptedWindow.activeInHierarchy)
        {
            acceptedWindow.SetActive(false);
        }
        else
        {
            acceptedWindow.SetActive(true);
            AcceptRefresh();
        }
    }

    public void AcceptRefresh()
    {
        //UPDATE TASK
        SQLHandler sql = new SQLHandler();
        inputMessage acceptMessage = new inputMessage();
        acceptMessage.addWay("getAcceptedTsk");
        acceptMessage.addArg("taker", playerIndicator.UserName);
        string strOpt = sql.recvMsg(acceptMessage.getString());
        outputMessage tskOpt = new outputMessage(strOpt);
        profileSys.UpdateAccepted(tskOpt.getResult());

        foreach (GameObject task in acceptedList)
        {
            Destroy(task);
        }
        Debug.Log("ACCEPTED LENGTH" + profileSys.Accepted_List.Count);
        if (profileSys.Accepted_List.Count != 0)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> itr in profileSys.Accepted_List)
            {
                UpdateQuestList(itr.Value["QID"], itr.Value["content"], itr.Value["title"],
                    itr.Value["exp"], itr.Value["coin"], itr.Value["owner"], true);
            }
        } 
    }
    #endregion

}
