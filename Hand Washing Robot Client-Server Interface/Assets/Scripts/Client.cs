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
    //WebCamTexture webCamTexture;
    //Texture2D currTexture;
    //public RawImage rawImage;
    //Arduino Comm
    AndroidJavaClass blCommClass;
    AndroidJavaObject blCommObj;

    Animator animator;

    public GameObject smileUI, disgustUI, confusedUI, sadUI, animationUI;
    public AudioClip laughing, clapping, disgust, wash_hands_b4_meals, wash_hands_today_q;
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
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        animator = animationUI.GetComponent<Animator>();

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

        //webCamTexture = new WebCamTexture(Screen.width / 2, Screen.height / 2); //new WebCamTexture();
        //currTexture = new Texture2D(Screen.width / 2, Screen.height / 2);
        //rawImage.texture = webCamTexture;
        //webCamTexture.Play();
    }

    private void Update()
    {
        if (messageNum > currMsgNum)
        {
            changeUI(currentUI);
            currMsgNum = messageNum;
        }

        //sendVideo();
        //sendVideo("Client to Server test video");
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

    IEnumerator wait_wash_hands_b4_meals_active()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("wash_hands_b4_meals_active", false);
    }
    
    IEnumerator wait_wash_hands_today_q_active()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("wash_hands_today_q_active", false);
    }

    public void changeUI(string text)
    {
        if (text.Equals("my_name_is"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "a");
#endif
        }
        else if (text.Equals("wash_hands_b4_meals"))
        {
            audioSource.clip = wash_hands_b4_meals;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "f");
#endif
            animator.SetBool("wash_hands_b4_meals_active", true);
            StartCoroutine(wait_wash_hands_b4_meals_active());
        }
        else if (text.Equals("wash_hands_aftr_toilet"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "e");
#endif
        }
        else if (text.Equals("wash_hands_today_q"))
        {
            audioSource.clip = wash_hands_today_q;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "d");
#endif
            animator.SetBool("wash_hands_today_q_active", true);
            StartCoroutine(wait_wash_hands_today_q_active());
        }
        else if (text.Equals("show_disgust"))
        {
            audioSource.clip = disgust;
            audioSource.Play();

#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "c");
#endif
            animator.SetBool("ShowDisgust", true);
        }
        else if (text.Equals("show_appreciation"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("dont_know"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("goodbye1"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("step1"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("step2"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR   
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("step3"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("step4"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("step5"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("step6"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "1");
#endif
        }
        else if (text.Equals("step7"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "1");
#endif
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