using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class UdpConnection : MonoBehaviour
{
    private UdpClient udpClient;
    private readonly Queue<double> incomingQueue =  new Queue<double>();
    Thread receiveThread;
    private bool threadRunning = false;
    private string senderIp;
    private int senderPort;

    public void StartConnection(string sendIp, int sendPort, int receivePort)
    {
        try 
        { 
            udpClient = new UdpClient(receivePort); 
        }
        catch (Exception e)
        {
            Debug.Log("Failed to listen for UDP at port " + receivePort + ": " + e.Message);
            return;
        }

        Debug.Log("Created receiving client at ip  and port " + receivePort);

        this.senderIp = sendIp;
        this.senderPort = sendPort;

        Debug.Log("Set sender at ip " + sendIp + " and port " + sendPort);

        StartReceiveThread();
    }

    private void StartReceiveThread()
    {
        receiveThread = new Thread(() => ListenForMessages(udpClient));
        receiveThread.IsBackground = true;
        threadRunning = true;
        receiveThread.Start();
    }

    private void ListenForMessages(UdpClient client)
    {
        IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

        while (threadRunning)
        {
            try
            {
                byte[] receiveBytes = client.Receive(ref remoteIpEndPoint);// Blocks until a message returns on this socket from a remote host.

                double[] values = new double[receiveBytes.Length / 8];


                for (int i = 0; i < values.Length; i++)
                {
                    
                    values[i] = BitConverter.ToDouble(receiveBytes, 8 * i);

                    lock (incomingQueue)
                    {
                        incomingQueue.Enqueue(values[i]);
                    }
                }

            }
            catch (SocketException e)
            {
                // 10004 thrown when socket is closed
                if (e.ErrorCode != 10004) 
                    Debug.Log("Socket exception while receiving data from udp client: " + e.Message);
            }
            catch (Exception e)
            {
                Debug.Log("Error receiving data from udp client: " + e.Message);
            }
            Thread.Sleep(1);
        }
    }





    public double[] getData()
    {
        double[] pendingMessages = new double[0];
        lock (incomingQueue)
        {
            pendingMessages = new double[incomingQueue.Count];
            int i = 0;
            while (incomingQueue.Count != 0)
            {
                pendingMessages[i] = incomingQueue.Dequeue();
                i++;
            }
        }

        return pendingMessages;
    }

    public void Send(double val)
    {
        IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(senderIp), senderPort);
        byte[] sendBytes = BitConverter.GetBytes(val);
        udpClient.Send(sendBytes, sendBytes.Length, serverEndpoint);
    }

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

    public void Stop()
    {
        threadRunning = false;
        receiveThread.Abort();
        udpClient.Close();
    }

    void OnDestroy()
    {
        // Clean up our logs and graphs when this object is destroyed
        DebugGUI.RemoveGraph("frameRate");
        DebugGUI.RemovePersistent("frameRate");
    }
}


