package hricomm.unni.com.blcomlib;
import android.util.Log;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothSocket;
import android.content.Intent;
//import java.util.Set;
import java.util.UUID;
import java.io.IOException;
import java.io.OutputStream;


public class BlueToothConnection
{
    private BluetoothAdapter bluetoothAdapter;
    private BluetoothSocket blSocket;
    private OutputStream outStream = null;
    private static final UUID MY_UUID = UUID.fromString("00001101-0000-1000-8000-00805F9B34FB");

    private BlueToothConnection(BluetoothAdapter _bluetoothAdapter)
    {
        this.bluetoothAdapter = _bluetoothAdapter;
    }

    public static void testFN(String s)
    {
        Log.i("Unity","Test function inside plugin + " + s);
    }

    public static BlueToothConnection getInstance()
    {
        BluetoothAdapter _adapter = BluetoothAdapter.getDefaultAdapter();
        if (_adapter != null)
        {
            return new BlueToothConnection(_adapter);
        }
        return null;
    }

    public void init(String mac)
    {
        Intent turnOn = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
        BluetoothDevice device = bluetoothAdapter.getRemoteDevice(mac);
        try
        {
            blSocket = device.createRfcommSocketToServiceRecord(MY_UUID);
            blSocket.connect();
            outStream = blSocket.getOutputStream();
        }
        catch(IOException e)
        {
            //blSocket.close();
            Log.i("Unity","" + e);
        }
    }

    public void sendMessage(String msg)
    {
        byte[] msgBuffer = msg.getBytes();
        try
        {
            outStream.write(msgBuffer);
        }
        catch(IOException e)
        {
            Log.i("Unity","" + e);
        }
    }

    public void closeConnection()
    {
        try
        {
            blSocket.close();
        }
        catch(IOException e)
        {
            Log.i("Unity","" + e);
        }
    }

    /*public static void initialize(String msg)
    {
        BluetoothSocket blSocket;
        OutputStream outStream = null;
        BluetoothAdapter blueAdapter = BluetoothAdapter.getDefaultAdapter();
        Intent turnOn = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
        // Get a set of currently paired devices and append to 'pairedDevices'
        //Set<BluetoothDevice> pairedDevices = blueAdapter.getBondedDevices();
        //for (BluetoothDevice device : pairedDevices)
        //{
        //    Log.i("Unity","Device " + device.getName() + "address : " + device.getAddress());
        //}
        BluetoothDevice device = blueAdapter.getRemoteDevice("00:18:E4:40:00:06");
        try
        {
            blSocket = device.createRfcommSocketToServiceRecord(MY_UUID);
            blSocket.connect();
            outStream = blSocket.getOutputStream();
            //String msg = "1";
            byte[] msgBuffer = msg.getBytes();
            outStream.write(msgBuffer);
            blSocket.close();
        }
        catch(IOException e)
        {
            //blSocket.close();
            Log.i("Unity","" + e);
        }
    }*/
}
