using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is to keep the game object with this script attached when loading
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        
        DontDestroyOnLoad(gameObject);
    }
}
