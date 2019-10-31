using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OLScene : MonoBehaviour
{
    public NetCore net;
    public GameObject infoWindow;
    public GameObject questTitilePre;

    public List<GameObject> questList = new List<GameObject>();

    public ScrollRect scrollRect;
    public void InfoOnClick() {
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
            singleQuestTitle.text = "企鹅世界第一可爱";
            singleQuestContent.text = "喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵" +
                "喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵喵";
        }
        
    }
    public void SingleQuestClose() { SingleQuest.SetActive(false); }



    public GameObject publishWindow;
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
            UpdateQuestList("喵喵喵");
        }
    }
    public void QuestListOff() { questListWindow.SetActive(false); }


    public void UpdateQuestList(string newText)
    {
        foreach(GameObject task in questList)
        {
            Destroy(task);
        }
        AddTextToScrollView(newText);
    }

    public GameObject AddTextToScrollView(string newText)
    {
        if (questTitilePre == null)
        {
            return null;
        }
        GameObject temp = Instantiate(questTitilePre, scrollRect.content, false);
        temp.GetComponentInChildren<Text>().text = newText;
        temp.GetComponent<Button>().onClick.AddListener(SingleQuestOpen);
        questList.Add(temp);

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        scrollRect.verticalNormalizedPosition = 0;

        return temp;

    }
}
