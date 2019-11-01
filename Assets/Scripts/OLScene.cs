using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class OLScene : MonoBehaviour
{
    public NetCore net;
    public GameObject infoWindow;
    public GameObject questTitilePre;

    public ProfileSys profileSys;

    public List<GameObject> questList = new List<GameObject>();

    public PlayerIndicator playerIndicator;

    private void Start()
    {
        playerIndicator = GameObject.FindGameObjectWithTag("NET").GetComponent<PlayerIndicator>();
    }

    #region Info_Window
    public Text exp;
    public Text coin;
    public void InfoOnClick() {
        exp.text = "EXP: " + profileSys.exp.ToString();
        coin.text = "Coin: " + profileSys.gold.ToString();
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
    public Text singleQuestTitle;
    public Text singleQuestContent;
    
    public GameObject SingleQuest;
    public void SingleQuestOpen()
    {
        if (SingleQuest.activeInHierarchy)
        {
            SingleQuest.SetActive(false);
        }
        else
        {
            SingleQuest.SetActive(true);
            //CHANGE QUEST CONTENT
            singleQuestTitle.text = this.gameObject.GetComponentInChildren<questINFO>().title;
            singleQuestContent.text = this.gameObject.GetComponentInChildren<questINFO>().content;
        }
        
    }
    public void SingleQuestClose() { SingleQuest.SetActive(false); }
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

    public void pulishOnClick()
    {
        if (playerIndicator.isPlayer)
        {

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
            SQLHandler tmp = new SQLHandler();
            tmp.addTsk(publishMsg.getString());
        }
    }
    #endregion

    #region Task_List
    public ScrollRect scrollRect;
    public GameObject questListWindow;
    public void QuestListOn()
    {
        if (questListWindow.activeInHierarchy)
        {
            questListWindow.SetActive(false);
        }
        else
        {
            questListWindow.SetActive(true);
            
            foreach(KeyValuePair<string, Dictionary<string, string>> itr in profileSys.Task_List)
            {
                UpdateQuestList(itr.Value["QID"], itr.Value["content"], itr.Value["title"], 
                    itr.Value["exp"], itr.Value["coin"], itr.Value["owner"] );
            }
            Debug.Log("UPDATING THE QUESTLIST");
        }
    }

    public void QuestListOff() { questListWindow.SetActive(false); }


    public void UpdateQuestList(string QID, string content, string title,
        string exp, string coin, string owner)
    {
        foreach(GameObject task in questList)
        {
            Destroy(task);
        }
        AddTextToScrollView(QID,content,title,exp,coin,owner);
    }

    public GameObject AddTextToScrollView(string QID, string content, string title, 
        string exp, string coin, string owner)
    {
        if (questTitilePre == null)
        {
            return null;
        }
        GameObject temp = Instantiate(questTitilePre, scrollRect.content, false);
        temp.GetComponentInChildren<Text>().text = title;
        temp.GetComponent<Button>().onClick.AddListener(SingleQuestOpen);
        questINFO QI= temp.GetComponent<questINFO>();
        QI.QID = QID;
        QI.content = content;
        QI.title = title;
        QI.exp = Convert.ToInt32(exp);
        QI.coin = Convert.ToInt32(coin);
        QI.owner = owner;
        questList.Add(temp);

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        scrollRect.verticalNormalizedPosition = 0;

        return temp;
    }
    #endregion
}
