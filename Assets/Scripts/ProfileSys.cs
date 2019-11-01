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
}
