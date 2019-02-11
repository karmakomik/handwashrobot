using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueToothTest : MonoBehaviour
{
    AndroidJavaClass blCommClass;
    AndroidJavaObject blCommObj;
    AndroidJavaObject blDevice;
    AndroidJavaObject blSimpleDevice;


    // Start is called before the first frame update
    void Start()
    {
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
        blCommClass.CallStatic("testFN", "yoyo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
