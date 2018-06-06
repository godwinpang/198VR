package com.example.polarblereceiver;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

import java.util.StringTokenizer;


public class MyPolarBleReceiver extends BroadcastReceiver {

    public final static String ACTION_GATT_CONNECTED =
            "edu.ucsd.healthware.fw.device.ble.ACTION_GATT_CONNECTED";
    public final static String ACTION_GATT_DISCONNECTED =
            "edu.ucsd.healthware.fw.device.ble.ACTION_GATT_DISCONNECTED";
    public final static String ACTION_GATT_SERVICES_DISCOVERED =
            "edu.ucsd.healthware.fw.device.ble.ACTION_GATT_SERVICES_DISCOVERED";
    public final static String ACTION_DATA_AVAILABLE =
            "edu.ucsd.healthware.fw.device.ble.ACTION_DATA_AVAILABLE";
    public final static String EXTRA_DATA =
            "edu.ucsd.healthware.fw.device.ble.EXTRA_DATA";
    public final static String CLICKER_CMD =
            "edu.ucsd.healthware.fw.device.ble.CLICKER_CMD";
    public final static String UPLOAD_TO_GOOGLE_DOC =
            "edu.ucsd.healthware.fw.device.ble.UPLOAD_TO_GOOGLE_DOC";

    //public final static UUID UUID_HEART_RATE_MEASUREMENT =
    //UUID.fromString(SampleGattAttributes.HEART_RATE_MEASUREMENT);

    public final static String ACTION_HR_DATA_AVAILABLE =
            "edu.ucsd.healthware.fw.device.ble.ACTION_HR_DATA_AVAILABLE";

    public static MyPolarBleReceiver mPolarBleUpdateReceiver;

    public static String heartData;

    public MyPolarBleReceiver(){
        Log.w("CLASS:", "####BleReceiverCreated");
    }

    @Override
    public void onReceive(Context ctx, Intent intent) {
        Log.w(this.getClass().getName(), "####ACTION_GATT_CONNECTED");
        final String action = intent.getAction();
        if (ACTION_GATT_CONNECTED.equals(action)) {
            Log.w(this.getClass().getName(), "####ACTION_GATT_CONNECTED");
        } else if (ACTION_GATT_DISCONNECTED.equals(action)) {
        } else if (ACTION_HR_DATA_AVAILABLE.equals(action)) {
            String data = intent.getStringExtra(EXTRA_DATA);
            StringTokenizer tokens = new StringTokenizer(data, ";");
            int hr = Integer.parseInt(tokens.nextToken());
            int prr = Integer.parseInt(tokens.nextToken());
            int rr = Integer.parseInt(tokens.nextToken());
            heartData = this.getClass().getName() + "####Received HR: " + hr + " RR: " + rr + " pRR: " + prr;
            Log.w(this.getClass().getName(), "####Received HR: " + hr + " RR: " + rr + " pRR: " + prr);
        }
    }


    public static void createReceiver(){
        Log.w("CLASS: ","####ReceiverCreated");

        if (mPolarBleUpdateReceiver == null){
            mPolarBleUpdateReceiver = new MyPolarBleReceiver();
        }
    }
}