using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OSCConnect : MonoBehaviour
{
 public int oscPort = 9129;

    private OSCReceiver receiver;

    void Start()
    {
        receiver = gameObject.AddComponent<OSCReceiver>();
        receiver.LocalPort = oscPort;
        receiver.Bind("/lx/", OnReceiveLeftX);
        receiver.Bind("/rx/", OnReceiveRightX);
    }

    private void OnReceiveLeftX(OSCMessage message)
    {
        float leftX = message.Values[0].FloatValue;
        Debug.Log("Left Hand X: " + leftX);
    }

    private void OnReceiveRightX(OSCMessage message)
    {
        float rightX = message.Values[0].FloatValue;
        Debug.Log("Right Hand X: " + rightX);
    }
}
