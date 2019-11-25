using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tell Unity not to destroy game objects with this
/// script attached while loading
/// </summary>
public class DontDestroyOnLoad : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
