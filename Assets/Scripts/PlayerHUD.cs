using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public GameObject TaskList;
    public GameObject SettingMenu;
    public GameObject ExitBox;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void CloseSettings()
    {
        SettingMenu.SetActive(false);
    }
    public void CloseTaskList()
    {
        TaskList.SetActive(false);
    }
    public void CloseExitBox()
    {
        ExitBox.SetActive(false);
    }
    public void OpenSettings()
    {
        SettingMenu.SetActive(true);
    }
    public void OpenTaskList()
    {
        TaskList.SetActive(true);
    }
    public void OpenExitBox()
    {
        ExitBox.SetActive(true);
    }
}
