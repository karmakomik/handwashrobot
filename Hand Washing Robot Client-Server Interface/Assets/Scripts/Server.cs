﻿using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    Texture2D tex;
    byte[] data;

    private static int localPort;

    // prefs
    private string IP;  // define in init
    public int port;  // define in init
    public int portVideoReceive;

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // gui
    string strMessage = "";

    // infos
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!


    // start from unity3d
    public void Start()
    {
        init();
        initVideoReceive();
        tex = new Texture2D(0, 0);
        //webCamTexture = new WebCamTexture(Screen.width / 2, Screen.height / 2); //new WebCamTexture();
        //rawImage.texture = webCamTexture;
        //webCamTexture.Play();
    }

    // init
    private void initVideoReceive()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // define port
        portVideoReceive = 8056;

        // status
        print("Sending to 127.0.0.1 : " + portVideoReceive);
        print("Test-Sending to this Port: nc -u 127.0.0.1  " + portVideoReceive + "");


    }

    // OnGUI
    void OnGUI()
    {
        /*Rect rectObj = new Rect(40, 380, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "# UDPSend-Data\n127.0.0.1 " + port + " #\n"
                + "shell> nc -lu 127.0.0.1  " + port + " \n"
                , style);

        // ------------------------
        // send it
        // ------------------------
        strMessage = GUI.TextField(new Rect(40, 420, 140, 20), strMessage);
        if (GUI.Button(new Rect(190, 420, 40, 20), "send"))
        {
            sendString(strMessage + "\n");
        }*/
    }

    // init
    public void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        Debug.Log("UDPSend.init()");

        // define
        //IP="127.0.0.1";
        //IP = "192.168.0.102";
        IP = MenuHandler.IPAddress;
        port = 8051;

        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

        // status
        Debug.Log("Sending to " + IP + " : " + port);
        Debug.Log("Testing: nc -lu " + IP + " : " + port);

    }

    // sendData
    public void sendString(string message)
    {
        try
        {
            //if (message != "")
            //{

            // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Den message zum Remote-Client senden.
            client.Send(data, data.Length, remoteEndPoint);
            //}
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    // endless test
    private void sendEndless(string testStr)
    {
        do
        {
            sendString(testStr);
        }
        while (true);
    }

    // getLatestUDPPacket
    // cleans up the rest
    public string getLatestUDPPacket()
    {
        allReceivedUDPPackets = "";
        return lastReceivedUDPPacket;
    }

    void OnDisable()
    {
        client.Close();
    }

}


