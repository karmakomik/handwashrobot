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
    public AudioClip intro, disgust, wash_hands_b4_meals, wash_hands_today_q, appreciation, wash_hands_aftr_toilet,dont_know;
    public AudioClip promise1, promise2, promise3, promise4, promise5, promise6, promise7, promise8;
    public AudioClip steps_intro,step1, step2, step3, step4, step5, step6, step7, step_misc_1, step_misc_2, step_misc_3;
    public AudioClip goodbye_promise, goodbye_final_1, goodbye_final_2;
    public AudioClip song1, song2;
    public AudioClip ente_peru_short, ente_veedu, pinne_parayam, mittayi, kelkunilla, classil_poku, tata, bye, enne_onnum_cheyalle;
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
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("wash_hands_b4_meals_active", false);
    }
    
    IEnumerator wait_wash_hands_today_q_active()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("wash_hands_today_q_active", false);
    }    

    IEnumerator wait_show_disgust_active()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("show_disgust_active", false);
    }

    IEnumerator wait_show_appreciation_active()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("show_appreciation_active", false);
    }

    IEnumerator wait_intro_name_active()
    {
        yield return new WaitForSeconds(10f);
        animator.SetBool("intro_active", false);
    }

    /*IEnumerator wait_()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("", false);
    }*/

    IEnumerator wait_wash_hands_aftr_toilet_active()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("wash_hands_aftr_toilet_active", false);
    }

    IEnumerator wait_dont_know_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("dont_know_active", false);
    }

    IEnumerator wait_goodbye_promise_active()
    {
        yield return new WaitForSeconds(14f);
        animator.SetBool("goodbye_promise_active", false);
    }

    IEnumerator wait_promise1_active()
    {
        yield return new WaitForSeconds(1.4f);
        animator.SetBool("promise1_active", false);
    }

    IEnumerator wait_promise2_active()
    {
        yield return new WaitForSeconds(1.2f);
        animator.SetBool("promise2_active", false);
    }

    IEnumerator wait_promise3_active()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("promise3_active", false);
    }

    IEnumerator wait_promise4_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("promise4_active", false);
    }

    IEnumerator wait_promise5_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("promise5_active", false);
    }

    IEnumerator wait_promise6_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("promise6_active", false);
    }

    IEnumerator wait_promise7_active()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("promise7_active", false);
    }

    IEnumerator wait_promise8_active()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("promise8_active", false);
    }

    IEnumerator wait_goodbye_final_1_active()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("goodbye_final_1_active", false);
    }

    IEnumerator wait_goodbye_final_2_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("goodbye_final_2_active", false);
    }

    IEnumerator wait_steps_intro_active()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("steps_intro_active", false);
    }

    IEnumerator wait_step1_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("step1_active", false);
    }

    IEnumerator wait_step2_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("step2_active", false);
    }

    IEnumerator wait_step3_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("step3_active", false);
    }

    IEnumerator wait_step4_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("step4_active", false);
    }

    IEnumerator wait_step5_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("step5_active", false);
    }

    IEnumerator wait_step6_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("step6_active", false);
    }

    IEnumerator wait_step7_active()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("step7_active", false);
    }

    IEnumerator wait_step_misc_1_active()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("step_misc_1_active", false);
    }

    IEnumerator wait_step_misc_2_active()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("step_misc_2_active", false);
    }

    IEnumerator wait_step_misc_3_active()
    {
        yield return new WaitForSeconds(4f);
        animator.SetBool("step_misc_3_active", false);
    }

    IEnumerator wait_ente_peru_pepe_active()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("ente_peru_pepe_active", false);
    }

    IEnumerator wait_ente_veedu_active()
    {
        yield return new WaitForSeconds(1.3f);
        animator.SetBool("ente_veedu_active", false);
    }

    IEnumerator wait_enne_onnum_cheyalle_active()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("enne_onnum_cheyalle_active", false);
    }

    IEnumerator wait_pinne_parayam_active()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("pinne_parayam_active", false);
    }

    IEnumerator wait_mittayi_active()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("mittayi_active", false);
    }

    IEnumerator wait_onnum_kelkunnilla_active()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool("onnum_kelkunnilla_active", false);
    }

    IEnumerator wait_classilponde_active()
    {
        yield return new WaitForSeconds(3f);
        animator.SetBool("classilponde_active", false);
    }

    IEnumerator wait_song1_active()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("song1_active", false);
    }

    IEnumerator wait_bye_active()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("bye_active", false);
    }

    IEnumerator wait_tata_active()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("tata_active", false);
    }


    public void changeUI(string text)
    {
        if (text.Equals("ReconnectBL"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            initBLConn();
#endif
        }
        else if (text.Equals("my_name_is"))
        {
            audioSource.clip = intro;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "h");
#endif
            animator.SetBool("intro_active", true);
            StartCoroutine(wait_intro_name_active());
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
            audioSource.clip = wash_hands_aftr_toilet;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "e");
#endif
            animator.SetBool("wash_hands_aftr_toilet_active", true);
            StartCoroutine(wait_wash_hands_aftr_toilet_active());
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
            animator.SetBool("show_disgust_active", true);
            StartCoroutine(wait_show_disgust_active());
        }
        else if (text.Equals("show_appreciation"))
        {
            audioSource.clip = appreciation;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "b");
#endif
            animator.SetBool("show_appreciation_active", true);
            StartCoroutine(wait_show_appreciation_active());
        }
        else if (text.Equals("dont_know"))
        {
            audioSource.clip = dont_know;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "f");
#endif
            animator.SetBool("dont_know_active", true);
            StartCoroutine(wait_dont_know_active());
        }
        else if (text.Equals("goodbye_promise"))
        {
            audioSource.clip = goodbye_promise;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "h");
#endif
            animator.SetBool("goodbye_promise_active", true);
            StartCoroutine(wait_goodbye_promise_active());
        }
        else if (text.Equals("promise1"))
        {
            audioSource.clip = promise1;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "j");
#endif
            animator.SetBool("promise1_active", true);
            StartCoroutine(wait_promise1_active());
        }
        else if (text.Equals("promise2"))
        {
            audioSource.clip = promise2;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "k");
#endif
            animator.SetBool("promise2_active", true);
            StartCoroutine(wait_promise2_active());
        }
        else if (text.Equals("promise3"))
        {
            audioSource.clip = promise3;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "j");
#endif
            animator.SetBool("promise3_active", true);
            StartCoroutine(wait_promise3_active());
        }
        else if (text.Equals("promise4"))
        {
            audioSource.clip = promise4;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "k");
#endif
            animator.SetBool("promise4_active", true);
            StartCoroutine(wait_promise4_active());
        }
        else if (text.Equals("promise5"))
        {
            audioSource.clip = promise5;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "j");
#endif
            animator.SetBool("promise5_active", true);
            StartCoroutine(wait_promise5_active());
        }
        else if (text.Equals("promise6"))
        {
            audioSource.clip = promise6;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "k");
#endif
            animator.SetBool("promise6_active", true);
            StartCoroutine(wait_promise6_active());
        }
        else if (text.Equals("promise7"))
        {
            audioSource.clip = promise7;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "j");
#endif
            animator.SetBool("promise7_active", true);
            StartCoroutine(wait_promise7_active());
        }
        else if (text.Equals("promise8"))
        {
            audioSource.clip = promise8;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "k");
#endif
            animator.SetBool("promise8_active", true);
            StartCoroutine(wait_promise8_active());
        }
        else if (text.Equals("goodbye_final_1"))
        {
            audioSource.clip = goodbye_final_1;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "g");
#endif
            animator.SetBool("goodbye_final_1_active", true);
            StartCoroutine(wait_goodbye_final_1_active());
        }
        else if (text.Equals("goodbye_final_2"))
        {
            audioSource.clip = goodbye_final_2;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "g");
#endif
            animator.SetBool("goodbye_final_2_active", true);
            StartCoroutine(wait_goodbye_final_2_active());
        }
        else if (text.Equals("steps_intro"))
        {
            audioSource.clip = steps_intro;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "h");
#endif
            animator.SetBool("steps_intro_active", true);
            StartCoroutine(wait_steps_intro_active());
        }
        else if (text.Equals("step1"))
        {
            audioSource.clip = step1;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "j");
#endif
            animator.SetBool("step1_active", true);
            StartCoroutine(wait_step1_active());
        }
        else if (text.Equals("step2"))
        {
            audioSource.clip = step2;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "k");
#endif
            animator.SetBool("step2_active", true);
            StartCoroutine(wait_step2_active());
        }
        else if (text.Equals("step3"))
        {
            audioSource.clip = step3;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "j");
#endif
            animator.SetBool("step3_active", true);
            StartCoroutine(wait_step3_active());
        }
        else if (text.Equals("step4"))
        {
            audioSource.clip = step4;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "k");
#endif
            animator.SetBool("step4_active", true);
            StartCoroutine(wait_step4_active());
        }
        else if (text.Equals("step5"))
        {
            audioSource.clip = step5;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "j");
#endif
            animator.SetBool("step5_active", true);
            StartCoroutine(wait_step5_active());
        }
        else if (text.Equals("step6"))
        {
            audioSource.clip = step6;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "k");
#endif
            animator.SetBool("step6_active", true);
            StartCoroutine(wait_step6_active());
        }
        else if (text.Equals("step7"))
        {
            audioSource.clip = step7;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "l");
#endif
            animator.SetBool("step7_active", true);
            StartCoroutine(wait_step7_active());
        }
        else if (text.Equals("step_misc_1")) //Clean b/w fingers
        {
            audioSource.clip = step_misc_1;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "l");
#endif
            animator.SetBool("step_misc_1_active", true);
            StartCoroutine(wait_step_misc_1_active());
        }
        else if (text.Equals("step_misc_2")) //Clean back of hand
        {
            audioSource.clip = step_misc_2;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "h");
#endif
            animator.SetBool("step_misc_2_active", true);
            StartCoroutine(wait_step_misc_2_active());
        }
        else if (text.Equals("step_misc_3"))
        {
            audioSource.clip = step_misc_3;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "h");
#endif
            animator.SetBool("step_misc_3_active", true);
            StartCoroutine(wait_step_misc_3_active());
        }
        else if (text.Equals("ente_peru"))
        {
            audioSource.clip = ente_peru_short;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "d");
#endif
            animator.SetBool("ente_peru_pepe_active", true);
            StartCoroutine(wait_ente_peru_pepe_active());
        }
        else if (text.Equals("ente_veedu"))
        {
            audioSource.clip = ente_veedu;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "d");
#endif
            animator.SetBool("ente_veedu_active", true);
            StartCoroutine(wait_ente_veedu_active());
        }
        else if (text.Equals("enne_onnum_cheyalle"))
        {
            audioSource.clip = enne_onnum_cheyalle;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "c");
#endif
            animator.SetBool("enne_onnum_cheyalle_active", true);
            StartCoroutine(wait_enne_onnum_cheyalle_active());
        }
        else if (text.Equals("pinne_parayam"))
        {
            audioSource.clip = pinne_parayam;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "d");
#endif
            animator.SetBool("pinne_parayam_active", true);
            StartCoroutine(wait_pinne_parayam_active());
        }
        else if (text.Equals("mittayi"))
        {
            audioSource.clip = mittayi;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "l");
#endif
            animator.SetBool("mittayi_active", true);
            StartCoroutine(wait_mittayi_active());
        }
        else if (text.Equals("kelkunnilla"))
        {
            audioSource.clip = kelkunilla;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "n");
#endif
            animator.SetBool("onnum_kelkunnilla_active", true);
            StartCoroutine(wait_onnum_kelkunnilla_active());
        }
        else if (text.Equals("classil_poku"))
        {
            audioSource.clip = classil_poku;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "l");
#endif
            animator.SetBool("classilponde_active", true);
            StartCoroutine(wait_classilponde_active());
        }
        else if (text.Equals("song1"))
        {
            audioSource.clip = song1;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            //blCommObj.Call("sendMessage", "o");
#endif
            //animator.SetBool("song1_active", true);
            //StartCoroutine(wait_song1_active());
        }
        else if (text.Equals("tata"))
        {
            audioSource.clip = tata;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "m");
#endif
            animator.SetBool("tata_active", true);
            StartCoroutine(wait_tata_active());
        }
        else if (text.Equals("bye"))
        {
            audioSource.clip = bye;
            audioSource.Play();
#if UNITY_ANDROID && !UNITY_EDITOR
            blCommObj.Call("sendMessage", "m");
#endif
            animator.SetBool("bye_active", true);
            StartCoroutine(wait_bye_active());
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
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                //serverStream.
                //client.rec

                string text = Encoding.UTF8.GetString(data);

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