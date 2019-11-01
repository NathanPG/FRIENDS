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
        foreach (KeyValuePair<string, Dictionary<string, string>> itr in sourceDict)
        {
            string qid = itr.Value["id"].ToString();
            string title = itr.Value["title"].ToString();
            string content = itr.Value["content"].ToString();
            string coin = itr.Value["coin"].ToString();
            string owner = itr.Value["owner"].ToString();
            string exp = itr.Value["exp"].ToString();

            this.Task_List[qid] = new Dictionary<string, string>();
            this.Task_List[qid]["QID"] = qid;
            this.Task_List[qid]["content"] = content;
            this.Task_List[qid]["title"] = title;
            this.Task_List[qid]["exp"] = exp;
            this.Task_List[qid]["coin"] = coin;
            this.Task_List[qid]["onwer"] = owner;

        }
    }

}
