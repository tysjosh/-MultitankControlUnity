using ChartAndGraph;
using System;
using UnityEngine;

public class ConnectionUdp : MonoBehaviour
{
    private UdpConnection connection;
    public string sendIp = "192.168.11.137";
    public int sendPort = 6666;
    public int receivePort = 6540;

    public GameObject Water_Indicator;
    public GameObject Meter_Gauge;
    private Vector3 startPosition;

    double[] simulink_Levels_Array;

    public GraphChart chart;

    private float Timer = 1f;
    private float Timerx = 1f;
    private float x = 2f;
    private float a = 2f;

    float Send_level_Tank_1;
    float Send_level_Tank_2;
    float Send_level_Tank_3;

    float Receive_level_Tank_1;

    [Range(0, 25)]
    public float Water_Level_1;
    [Range(0, 25)]
    public float Water_Level_2;
    [Range(0, 25)]
    public float Water_Level_3;


    void Awake()
    {
        // Set up graph properties using our graph keys
        DebugGUI.SetGraphProperties("smoothFrameRate", "SmoothFPS", 0, 200, 0, new Color(0, 1, 1), false);
        DebugGUI.SetGraphProperties("frameRate", "FPS", 0, 200, 0, new Color(1, 0.5f, 1), false);
    }

    void Start()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 100;

        // the ChartGraph info is obtained via the inspector
        if (chart == null) 
            return;

        // calling StartBatch allows changing the graph data without redrawing the graph for every change
        chart.DataSource.StartBatch();
        chart.DataSource.ClearCategory("Player 1");
        chart.DataSource.ClearCategory("Player 2");

        chart.DataSource.AddPointToCategory("Player 1", 0, 0);
        chart.DataSource.AddPointToCategory("Player 2", x, Receive_level_Tank_1);


        // finally we call EndBatch , this will cause the GraphChart to redraw itself
        chart.DataSource.EndBatch();

        startPosition = Meter_Gauge.transform.position;
        connection = new UdpConnection();
        connection.StartConnection(sendIp, sendPort, receivePort);


    }

    void Update()
    {
        float newWaterLevel = Water_Level_1 / 2.5f;
        Water_Indicator.transform.localPosition = new Vector3(startPosition.x, newWaterLevel, startPosition.z);

        Timer -= Time.deltaTime;
        if (Timer <= 0f)
        {
            Timer = 1f;

            foreach (var receivedData in connection.getData())
            {
                Receive_level_Tank_1 = Convert.ToSingle(receivedData) * 100;
                chart.DataSource.AddPointToCategoryRealtime("Player 2", x, Receive_level_Tank_1);
            }
                
            x++;
        }

        Send_level_Tank_1 = Water_Level_1 ;
        Send_level_Tank_2 = Water_Level_2 ;
        Send_level_Tank_3 = Water_Level_3 ;

        simulink_Levels_Array = new double[]{ Send_level_Tank_1, Send_level_Tank_2, Send_level_Tank_3};
        connection.Send(Send_level_Tank_1);

        Grapher.Log(Send_level_Tank_1, "LevelTank_1", Color.red);
        Grapher.Log(Send_level_Tank_2, "LevelTank_2", Color.green);
        Grapher.Log(Send_level_Tank_3, "LevelTank_3", Color.blue);

        Timerx -= Time.deltaTime;
        if (Timerx <= 0f)
        {
            Timerx = 1f;
            chart.DataSource.AddPointToCategoryRealtime("Player 1", a, Send_level_Tank_1, 1f);
            chart.DataSource.AddPointToCategoryRealtime("Player 2", x, Send_level_Tank_2, 1f);
            chart.DataSource.AddPointToCategoryRealtime("Player 3", x, Send_level_Tank_3, 1f);
            a++;
        }



        // Manual persistent logging
        DebugGUI.LogPersistent("smoothFrameRate", "SmoothFPS: " + (1 / Time.deltaTime).ToString("F3"));
        DebugGUI.LogPersistent("frameRate", "FPS: " + (1 / Time.smoothDeltaTime).ToString("F3"));

        if (Time.smoothDeltaTime != 0)
            DebugGUI.Graph("smoothFrameRate", 1 / Time.smoothDeltaTime);
        if (Time.deltaTime != 0)
            DebugGUI.Graph("frameRate", 1 / Time.deltaTime);

    }

    public void Slider1_Moved(float newValue)
    {
        Water_Level_1 = newValue;
    }

    public void Slider2_Moved(float newValue)
    {
        Water_Level_2 = newValue;
    
    }

    public void Slider3_Moved(float newValue)
    {
        Water_Level_3 = newValue;

    }

    void OnDestroy()
    {
        connection.Stop();
    }
}
