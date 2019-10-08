using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class OLScene : NetworkBehaviour
{
    public void DisconnectOnClick()
    {
        if (isClient)
        {
            NetworkManager.singleton.StopClient();
        }
        else
        {
            NetworkManager.singleton.StopHost();
        }
        SceneManager.LoadScene(0);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
