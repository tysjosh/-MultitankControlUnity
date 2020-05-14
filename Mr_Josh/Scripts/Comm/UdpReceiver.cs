using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UdpReceiver : MonoBehaviour
{
    string RemoteAddress = "192.168.11.137";
    string LocalAddress = "192.168.11.103";

    public int PortLocal = 12000;

    private UdpClient _ReceiveClient;
    private Thread _ReceiveThread;
    byte[] data;

    private IReceiverObserver _Observer;

    [DebugGUIGraph(min: -7, max: 7, r: 0, g: 1, b: 0, autoScale: true)]
    float SinField;



    void Start()
    {
        data = new byte[0];
        Initialize();
    }

    /// <summary>
    /// Initialize objects.
    /// </summary>
    public void Initialize()
    {
        // Receive
        _ReceiveThread = new Thread(new ThreadStart(ReceiveData));
        _ReceiveThread.IsBackground = true;
        _ReceiveThread.Start();

    }

    private void Update()
    {
        // Manual persistent logging
        DebugGUI.LogPersistent("smoothFrameRate", "SmoothFPS: " + (1 / Time.deltaTime).ToString("F3"));
        DebugGUI.LogPersistent("frameRate", "FPS: " + (1 / Time.smoothDeltaTime).ToString("F3"));

        if (Time.smoothDeltaTime != 0)
            DebugGUI.Graph("smoothFrameRate", 1 / Time.smoothDeltaTime);
        if (Time.deltaTime != 0)
            DebugGUI.Graph("frameRate", 1 / Time.deltaTime);
    }


    public void SetObserver(IReceiverObserver observer)
    {
        _Observer = observer;
    }

    /// <summary>
    /// Receive data with pooling.
    /// </summary>
    private void ReceiveData()
    {

        _ReceiveClient = new UdpClient(PortLocal);

        while (true)
        {
            try
            {

                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                data = _ReceiveClient.Receive(ref RemoteIpEndPoint);
                double[] values = new double[data.Length / 8];

                Buffer.BlockCopy(data, 0, values, 0, values.Length * 8);
                Debug.Log(values); 

                if (_Observer != null)
                    _Observer.OnDataReceived(values);
                Debug.Log(">>>>");


            }
            catch (Exception err)
            {
                Debug.Log("<color=red>" + err.Message + "</color>");
                continue;
            }
        }


    }

    

    void Awake()
    {

        // Set up graph properties using our graph keys
        DebugGUI.SetGraphProperties("smoothFrameRate", "SmoothFPS", 0, 200, 1, new Color(0, 1, 1), false);
        DebugGUI.SetGraphProperties("frameRate", "FPS", 0, 200, 1, new Color(1, 0.5f, 1), false);
    }


    /// <summary>
    /// Deinitialize everything on quiting the application.Or you might get error in restart.
    /// </summary>
    private void OnApplicationQuit()
    {
        try
        {
            _ReceiveThread.Abort();
            _ReceiveThread = null;

            if (_ReceiveClient == null)
                _ReceiveClient.Close();
        }
        catch (Exception err)
        {
            Debug.Log("<color=red>" + err.Message + "</color>");
        }
    }

    void OnDestroy()
    {
        // Clean up our logs and graphs when this object is destroyed
        DebugGUI.RemoveGraph("frameRate");
        DebugGUI.RemovePersistent("frameRate");
    }
}
