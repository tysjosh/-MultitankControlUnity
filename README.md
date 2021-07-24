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
