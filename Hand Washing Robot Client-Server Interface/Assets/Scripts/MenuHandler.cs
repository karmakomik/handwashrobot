﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public static string IPAddress;
    public UnityEngine.UI.InputField ipInput;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setClientIPAndStartServer()
    {
        IPAddress = ipInput.text;
        startServer();
    }

    public void setClientIPAndStartServer(string ip)
    {
        IPAddress = ip;
        startServer();
    }

    public void startClient()
    {
        SceneManager.LoadScene(1);
    }

    public void startServer()
    {
        SceneManager.LoadScene(2);
    }
}

