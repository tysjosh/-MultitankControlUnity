![watertank](https://user-images.githubusercontent.com/37803559/127181960-0827cb11-734b-4b78-908b-03183a20df46.png)
# Application of Augmented Reality to Industrial Control Systems using Head-Mounted Display
## Problem Statement
SCADA is the conventional method for locally or remotely monitoring and controlling industrial systems on the shop floor and the HMI helps to visualize data but the entire
process can become rigorious for the control engineer or error-prone as automation data is isolated from the industrial systems. The thesis provides an efficient and modern way of monitoring and visualizing factory telemetry using the Meta
2 Head Mounted Display (HMD). The ability to immersively interact in real-time with the real system while visualizing process data, troubleshooting, and sending
control signals is an advantage over the conventional SCADA. Parameters of the real industrial control system including the tuning of controllers can be changed remotely
by manipulating the controls in the AR application deployed in the Meta 2 Headset.

Most work done applying HMD to industrial automation adopt the Microsoft Hololens as a preferred HMD because of its strong spatial tracking capabilities, but the Meta
2 HMD was actually created to replace the 2D desktop displays, and so provides
a 90-degree FoV while the Microsoft Hololens retain a 30-degree FoV. This makes
the Meta 2 HMD better than the Hololens HMD for this thesis as it will give us a
clear line of sight to the industrial system and its environment while using a unique
neuroscience-driven interface to access and interact with digital contents easily and
naturally providing us with the best immersive AR experience.

The Multitank laboratory model is a laboratory model of a real-life industrial Multitank system used for teaching and researching automatic control in the laboratory. Real-time experiments can be performed on the system design to validate one’s
linear and nonlinear control methods. The objective of the control is to reach and
stabilize the level in the tanks by an adjustment of the pump operation or/and
valves settings. This control problem can be solved by several level control strategies
ranging from PID to adaptive and fuzzy logic controls, but the PID controller is
used in this thesis.

## Aims and Objectives
The aim of the thesis is to design and develop a Meta 2 headset application with it’s
unique virtual object for complementing the control of a real-life laboratory model
of an industrial multi-tank system.

To achieve the design and development of this project, the following were done:
* Review past works that are related to the project.
* Choose suitable components that meet the scope of the project.
* Design the system diagram to integrate various part of the project.
* Create the virtual model
* Develop the required scene
* Develop a closed loop control system to send control signals to the Multitank
system
* Establish communication between the computer hosting Unity3D and the
computer hosting the Simulink
* Integrate the model, software package and the AR device
* Testing and debugging.

## System Design
The system diagram utilizes two
mode of communication which are wireless and wired. The Meta 2 head-mounted
display connects via a USB and HDMI cables to a lab PC while the multitank system
connects to another lab PC via a data acquisition box. Both PC are connected to
the same router, making it possible to send and receive UDP packets from each
other.
![System Diagram](https://user-images.githubusercontent.com/37803559/127182410-911d66b7-3367-4a95-a6fa-47da9d21e4b6.PNG)

The main task of the laboratory computers is to establish a bidirectional data
communication between MATLAB and Unity3D, which in turn provide signal to the
Meta 2 HMD and the Multitank System. The tools and software used are mentioned
below:
* MATLAB/Simulink 2012
* Unity3D Editor version 2018.4 withe Vuforia SDK
* Account in vuforia development portal
* Visual Studio 2019
* Router
* 2 PCs
* Meta 2 and Meta SDK
* Multitank System
* Cables and Connectors

## Multitank System
![download](https://user-images.githubusercontent.com/37803559/127180696-bb071d5b-4cf0-4c2d-bf1f-82e9cf382270.jpg)

## Matlab Simulink Diagram
## Virtual Object Creation
The developed virtual object which includes the water tank, the
metre gauge, the red marker and the water. The water tank comprises of different
cubes representing the different sides of the tank which all have a transparent white
material.
![watertank](https://user-images.githubusercontent.com/37803559/127181996-049bd468-cdf8-42ac-81c4-6fdf5cf3fd77.png)
The Meta 2 Unity SDK is installed into Unity3D by importing the Meta 2 Unity
SDK package downloaded from Meta 2 website. The SDK includes SLAM, hands
interaction tracking, example code, and documentation. Now, we are able to interact
and manipulate 3D holographic content by adding the MetaCameraRig, MetaHands
and MetaInputModule to the Unity3D scene. Figure 22 shows the developed virtual
object which includes the sliders and input field for the setting the setpoints for the
various water tanks.

Figure 23 shows the developed virtual graph. The graph displays how the process
value compares with the set point of the various water tank levels.

Figure 24 shows the integration of all developed virtual objects. The arrangement
was done with the aim to make it easy for a user to interact with the sliders easily,
while also visualizing the data on the graph.
![CaptureControl](https://user-images.githubusercontent.com/37803559/127182045-e14f3589-3b13-4ada-a89a-3af51d978ee7.PNG)
![Captureoo](https://user-images.githubusercontent.com/37803559/127182073-ece5bc10-ff02-42f5-9477-824e4bee58d7.PNG)
![CaptureIntegration](https://user-images.githubusercontent.com/37803559/127182095-adf993dc-cf6c-45c2-830b-1996d92e67dc.PNG)


## Establishing Connection between Unity3D and MATLAB/Simulink
For the two stand-alone applications hosted on two different PCs to communicate
with each other, we need to establish a remote connection between the two hosts.
Since we will be sending and receiving streams of data, the UDP connectionless
protocol provides us a viable solution than TCP. 

To establish a UDP connectionless
protocol, both hosts were connected to the same router(same network), the C#
scripting available for Unity3D was used to create an instance of a UDP client, set
up both sending and receiving ports, set up IP addresses, and assign the number
of bytes we want to send. A receiving and sending UDP packets have already been
installed on the Simulink side to correspond with what the Unity is ending and
receiving.

### c# function for sending UDP packets from Unity3D to Simulink
```c#
  public void Send(double[] val)
  {
    for (int i = 0; i < val.Length; i++)
    {
      IPEndPoint serverEndpoint = new IPEndPoint(IPAddress.Parse(senderIp), senderPort);
      byte[] sendBytes = BitConverter.GetBytes(i);
      udpClient.Send(sendBytes, sendBytes.Length, serverEndpoint);
    } 
  }
```
### c# function for receiving UDP packets in Unity3D from Simulink
```c#
  private void ListenForMessages(UdpClient client)
  {
    IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
    while (threadRunning)
    {
      byte[] receiveBytes = client.Receive(ref remoteIpEndPoint);
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
  }
```


## Results
The pictures below show snapshots taken while a user put on the Meta 2 headset to
utilize the AR application developed and deployed into it. Figures below show
the Meta views while a user is using the developed AR application to send control
signals to the Multitank system.
The list of what the user is able to do is listed below;

* The user is able to see the digital information
* The user is able to interact and give input to the digital UI using the hand
* The user is able to use the sliders to set set-points for the tanks in the Multitank system
* The user is able to see the graphical representation of the set-points and the process values in real-time
