using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSys : MonoBehaviour
{
    public string username;
    public int exp;
    public int gold;
    //                ID
    public Dictionary<string, Dictionary<string, string>> Task_List 
        = new Dictionary<string, Dictionary<string, string>>();

    /*
    public void UpdateProfile(string username, int exp, int gold)
    {

    }
    */

    public void UpdateResult(Dictionary<string, Dictionary<string, string>> sourceDict)
    {
        Debug.Log("UPDATING");
        foreach (KeyValuePair<string, Dictionary<string, string>> itr in sourceDict)
        {
            Debug.Log("GETTING QUEST:" + itr.Value["title"].ToString());
            string qid = "unset id";
            string title = "unset title";
            string content = "unet content";
            string coin = "-1";
            string owner = "unset owner";
            string exp = "-1";
            try
            {
                qid = itr.Value["id"].ToString();
                title = itr.Value["title"].ToString();
                content = itr.Value["content"].ToString();
                coin = itr.Value["coin"].ToString();
                owner = itr.Value["owner"].ToString();
                //exp = itr.Value["exp"].ToString();
            } catch (Exception ex)
            {

            }

            this.Task_List[qid] = new Dictionary<string, string>();
            this.Task_List[qid]["QID"] = qid;
            this.Task_List[qid]["content"] = content;
            this.Task_List[qid]["title"] = title;
            this.Task_List[qid]["exp"] = exp;
            this.Task_List[qid]["coin"] = coin;
            this.Task_List[qid]["owner"] = owner;

        }
    }

}
