using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    WebCamTexture webCamTexture;
    public RawImage rawImage;
    //Arduino Comm
    AndroidJavaClass blCommClass;
    AndroidJavaObject blCommObj;


    public GameObject smileUI, disgustUI, confusedUI, sadUI;
    public AudioClip laughing, clapping, disgust, instruction1, instruction2;
    AudioSource audioSource;
    public string currentUI;
    public int messageNum, currMsgNum;

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    //UdpClient client;
    UdpClient client;
    UdpClient clientVideoSend;


    // prefs
    private string IPVideoSend;  // define in init
    public int portVideoSend;  // define in init

    // "connection" things
    IPEndPoint remoteEndPointVideoSend;

    // public
    // public string IP = "127.0.0.1"; default local
    public int port; // define > init

    // infos
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!

    public void initBLConn()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        blCommObj.Call("init", "00:18:E4:40:00:06");
#endif
        //onButton.SetActive(true);
        //offButton.SetActive(true);
    }



    // start from unity3d
    public void Start()
    {
        messageNum = 0;
        currMsgNum = 0;
        currentUI = "smile";
        audioSource = GetComponent<AudioSource>();
#if UNITY_ANDROID && !UNITY_EDITOR        
        blCommClass = new AndroidJavaClass("hricomm.unni.com.blcomlib.BlueToothConnection");
        blCommObj = blCommClass.CallStatic<AndroidJavaObject>("getInstance");
        initBLConn();
#endif
        init();
        initVideoSend();

        webCamTexture = new WebCamTexture(Screen.width / 2, Screen.height / 2); //new WebCamTexture();
        rawImage.texture = webCamTexture;
        //webCamTexture.Play();
    }

    private void Update()
    {
        if (messageNum > currMsgNum)
        {
            changeUI(currentUI);
            currMsgNum = messageNum;
        }

        sendVideo("Client to Server test video");
    }

    // OnGUI
    void OnGUI()
    {
        /*Rect rectObj = new Rect(40, 10, 200, 400);
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        GUI.Box(rectObj, "# UDPReceive\n127.0.0.1 " + port + " #\n"
                + "shell> nc -u 127.0.0.1 : " + port + " \n"
                + "\nLast Packet: \n" + lastReceivedUDPPacket
                + "\n\nAll Messages: \n" + allReceivedUDPPackets
                , style);*/
    }

    void initVideoSend()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        Debug.Log("UDPSend.init()");

        // define
        //IP="127.0.0.1";
        //IP = "192.168.0.102";
        IPVideoSend = MenuHandler.IPAddress; //Local for now
        portVideoSend = 8056;

        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPointVideoSend = new IPEndPoint(IPAddress.Parse(IPVideoSend), portVideoSend);
        clientVideoSend = new UdpClient();

        // status
        //Debug.Log("Sending to " + IP + " : " + port);
        //Debug.Log("Testing: nc -lu " + IP + " : " + port);
    }

    // sendData
    public void sendVideo(string message)
    {
        try
        {
            //if (message != "")
            //{

            // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
            byte[] data = Encoding.UTF8.GetBytes(message);

            // Den message zum Remote-Client senden.
            clientVideoSend.Send(data, data.Length, remoteEndPointVideoSend);
            //}
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }

    // init
    private void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // define port
        port = 8051;

        // status
        print("Sending to 127.0.0.1 : " + port);
        print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");


        // ----------------------------
        // Abhören
        // ----------------------------
        // Lokalen Endpunkt definieren (wo Nachrichten empfangen werden).
        // Einen neuen Thread für den Empfang eingehender Nachrichten erstellen.
        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();

    }

    public void changeUI(string text)
    {
        if (text.Equals("show_smile"))
        {
            smileUI.SetActive(true);
            sadUI.SetActive(false);
            disgustUI.SetActive(false);
            confusedUI.SetActive(false);
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("show_sadness"))
        {
            smileUI.SetActive(false);
            sadUI.SetActive(true);
            disgustUI.SetActive(false);
            confusedUI.SetActive(false);
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "2");
#endif
        }
        else if (text.Equals("show_confusion"))
        {
            smileUI.SetActive(false);
            sadUI.SetActive(false);
            disgustUI.SetActive(false);
            confusedUI.SetActive(true);
        }
        else if (text.Equals("show_disgust"))
        {
            smileUI.SetActive(false);
            sadUI.SetActive(false);
            disgustUI.SetActive(true);
            confusedUI.SetActive(false);
        }
        else if (text.Equals("play_sound_laugh"))
        {
            audioSource.clip = laughing;
            audioSource.Play();
            smileUI.SetActive(true);
            sadUI.SetActive(false);
            disgustUI.SetActive(false);
            confusedUI.SetActive(false);
        }
        else if (text.Equals("play_sound_disgust"))
        {
            audioSource.clip = disgust;
            audioSource.Play();
            smileUI.SetActive(false);
            sadUI.SetActive(false);
            disgustUI.SetActive(true);
            confusedUI.SetActive(false);
        }
        else if (text.Equals("play_sound_applause"))
        {
            audioSource.clip = clapping;
            audioSource.Play();         
            smileUI.SetActive(true);
            sadUI.SetActive(false);
            disgustUI.SetActive(false);
            confusedUI.SetActive(false);
        }
    }

    // receive thread
    private void ReceiveData()
    {
        client = new UdpClient(port);
        //client = new TcpClient(MenuHandler.IPAddress, port);
        //var serverStream = client.GetStream();
        while (true)
        {

            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                //serverStream.
                //client.rec

                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                string text = Encoding.UTF8.GetString(data);

                // Den abgerufenen Text anzeigen.
                Debug.Log(">> " + text);
                ++messageNum;
                currentUI = text;
                // latest UDPpacket
                lastReceivedUDPPacket = text;

                // ....
                allReceivedUDPPackets = allReceivedUDPPackets + text;

            }
            catch (Exception err)
            {
                Debug.Log(err.ToString());
            }
        }
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
        if (receiveThread.IsAlive)
            receiveThread.Abort();

#if UNITY_ANDROID && !UNITY_EDITOR
        blCommObj.Call("closeConnection");
#endif

        client.Close();
    }
}