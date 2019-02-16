using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueToothTest : MonoBehaviour
{
    AndroidJavaClass blCommClass;
    AndroidJavaObject blCommObj;
    //AndroidJavaObject blDevice;
    //AndroidJavaObject blSimpleDevice;
    public GameObject onButton, offButton;

    //Inspired by https://wingoodharry.wordpress.com/2014/03/16/simple-android-to-arduino-via-bluetooth-part-3/
    // Start is called before the first frame update
    void Start()
    {
        onButton.SetActive(false);
        offButton.SetActive(false);
        //blCommClass = new AndroidJavaClass("com.harrysoft.androidbluetoothserial.BluetoothManager");
        //blCommObj = blCommClass.CallStatic<AndroidJavaObject>("getInstance");
        /*
            List<BluetoothDevice> pairedDevices = bluetoothManager.getPairedDevicesList();
            for (BluetoothDevice device : pairedDevices) {
                Log.d("My Bluetooth App", "Device name: " + device.getName());
                Log.d("My Bluetooth App", "Device MAC Address: " + device.getAddress());
            }
}         
         */
        //blDevice = blCommObj.Call<AndroidJavaObject>("openSerialDevice", "00:18:E4:40:00:06");
        //blSimpleDevice = blDevice.Call<AndroidJavaObject>("toSimpleDeviceInterface");
        //blSimpleDevice.Call("sendMessage", "1");

        blCommClass = new AndroidJavaClass("hricomm.unni.com.blcomlib.BlueToothConnection");
        blCommObj = blCommClass.CallStatic<AndroidJavaObject>("getInstance");       
        //blCommClass.CallStatic("testFN", "yoyo");
        //blCommClass.CallStatic("initialize");
    }

    public void initBLConn()
    {
        blCommObj.Call("init", "00:18:E4:40:00:06");
        onButton.SetActive(true);
        offButton.SetActive(true);
    }

    public void On()
    {
        blCommObj.Call("sendMessage", "1");
        //blCommClass.CallStatic("initialize","1");
    }

    public void Off()
    {
        blCommObj.Call("sendMessage", "2");
        //blCommClass.CallStatic("initialize", "2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnApplicationQuit()
    {
        blCommObj.Call("closeConnection");
    }

    public void close()
    {
        Application.Quit();
    }
}
