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

    public Text QuestTitle;
    public Text QuestContent;
    public Text QuestCoin;
    public Text QuestExp;

    public void thisOnClick()
    {
        QuestTitle.text = title;
        QuestContent.text = content;
        QuestCoin.text = coin.ToString();
        QuestExp.text = exp.ToString();
    }
}
