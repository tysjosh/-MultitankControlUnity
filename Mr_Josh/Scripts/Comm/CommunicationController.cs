using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationController : MonoBehaviour, IReceiverObserver
{
    UdpReceiver _UdpReceiver;
    UdpTransmitter _Udpransmitter;

    private void Awake()
    {
        _UdpReceiver = GetComponent<UdpReceiver>();
        _UdpReceiver.SetObserver(this);

        _Udpransmitter = GetComponent<UdpTransmitter>();
    }


    /// <summary>
    /// Send data immediately after receiving it.
    /// </summary>
    /// <param name="val"></param>
    void IReceiverObserver.OnDataReceived(double[] val)
    {
        _Udpransmitter.Send(val);
    }
}
