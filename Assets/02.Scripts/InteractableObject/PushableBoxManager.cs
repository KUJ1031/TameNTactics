using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBoxManager : MonoBehaviour
{
    public static PushableBoxManager Instance { get; private set; }
    internal bool isBoxMoving = false;
    private void Awake()
    {
        //싱글톤화
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
