using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPolar : MonoBehaviour {

	AndroidJavaClass receiver;
	//AndroidJavaClass polar;
	string heartData = "uninitialized";

	// Use this for initialization
	void Start () {
		Debug.Log ("PolarPluginAboutToBeCalled");
		receiver = new AndroidJavaClass ("com.example.polarblereceiver.MyPolarBleReceiver");
		Debug.Log ("Receiver" + receiver);
		//polar = new AndroidJavaClass ("com.vr198.polarh10.Polar");
		receiver.CallStatic ("createReceiver");
		//polar.CallStatic<BroadcastReceiver>("activatePolar", receiver.GetStatic("mPolarBleUpdateReceiver"));
	}

	// Update is called once per frame
	void Update () {
		heartData = receiver.GetStatic<string> ("heartData");
		Debug.Log (heartData);
	}
}
