using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class UdpTransmitter : MonoBehaviour
{
    public GameObject Water_Indicator;
    public GameObject Meter_Gauge;
    private Vector3 startPosition;

    [Range(0, 25)]
    public float Water_Level;


    public string IP = "192.168.11.137";
    public int TransmitPort;

    private IPEndPoint _RemoteEndPoint;
    private UdpClient _TransmitClient;

    private void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;

        startPosition = Meter_Gauge.transform.position;
        Initialize();
    }

    private void Update()
    {
        float newWaterLevel = Water_Level / 2.5f;
        Water_Indicator.transform.localPosition = new Vector3(startPosition.x, newWaterLevel, startPosition.z);
        //float simulink_level = Water_Level / 100f;

        //if(target != Application.targetFrameRate)
        //{
        //    Application.targetFrameRate = target;
        //}
        Send(Water_Level);
    }


    /// <summary>
    /// Initialize objects.
    /// </summary>
    private void Initialize()
    {
        _RemoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), TransmitPort);
        _TransmitClient = new UdpClient();

    }

    /// <summary>
    /// Sends a double value to target port and ip.
    /// </summary>
    /// <param name="val"></param>
    public void Send(double val)
    {
        try
        {
            // Convert string message to byte array.  
            byte[] serverMessageAsByteArray = BitConverter.GetBytes(val);

            //Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //_socket.SendTo(serverMessageAsByteArray, _RemoteEndPoint);
            //_TransmitClient.Close();

            _TransmitClient.Send(serverMessageAsByteArray, serverMessageAsByteArray.Length, _RemoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log("<color=red>" + err.Message + "</color>");
        }
    }

    /// <summary>
    /// Sends a double array to target port and ip.
    /// </summary>
    /// <param name="val"></param>
    public void Send(double[] val)
    {
        try
        {
            for (int i = 0; i < val.Length; i++)
                Send(val[i]);
        }
        catch (Exception err)
        {
            Debug.Log("<color=red>" + err.Message + "</color>");
        }
    }

    /// <summary>
    /// Deinitialize everything on quiting the application.Or you might get error in restart.
    /// </summary>
    private void OnApplicationQuit()
    {
        try
        {
            _TransmitClient.Close();
        }
        catch (Exception err)
        {
            Debug.Log("<color=red>" + err.Message + "</color>");
        }
    }
}
