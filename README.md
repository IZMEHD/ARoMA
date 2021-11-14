# ARoMA: AR-optimized Manufacturing-Assistant for HoloLens 2 in Unity
 
Demo video: <a href="https://www.youtube.com/watch?v=M1uZh9A-Ros&ab_channel=izmeHD"><img src="https://img.shields.io/badge/-YouTube-red?&style=for-the-badge&logo=youtube&logoColor=white" height=20></a>


<p align="center">
  <a href="https://www.youtube.com/watch?v=M1uZh9A-Ros&ab_channel=izmeHD"><img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/DemoLegoBuildAndTwin.gif' width=450 ></a>  
</p>



***Summary*** 

This project is the (by)product of a bachelor thesis dealing with the development of an "AR-optimized Manufacturing-Assistant" system for the HoloLens 2.
This system was developed taking into account current findings and results from works that deals with the topic of AR in training,
assembly and maintenance tasks. 
The main purpose of this system is to guide a inexperienced user(e.g. a trainee of some kind) through a manuel assembly or maintenance task.
Additionally the system allows the HoloLens 2 sensors to record the user's hand and head movements. These recordings can late be visualized, for example 
with *this* program.




# Feature Overview

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width=550 > 
</p>


**Digital Twin:**
In its core-function *AroMa* uses a digital twin to show the current assembly step and highlights step-relevant parts by animating them in such a way that mounting 
position and direction are shown. Additionally the user can always grab the twin and manipulate its position, orentation and size.


**Part Amount Indicator:**
It is possible, during the setup process, to designate part-bins, which are then, during asasembly,  highlightet and accompanied by numbers to indicate to the user what and how many parts are required 
for a given step.

**Proccess Indicator:**
During assembly the current assembly-step and the total amount of steps are shown close to the users right(per default, but can be changed) hand.

**Voice commands:**
Via the voice command "Next" the subsequent assembly step can be initiated. 

**Display of additional informations:**
Via the voice command "Next" the subsequent assembly step can be initiated. 


 # Prerequisites

 

*Software*

Unity 2019 or higher (https://unity.com) 
MixedRealityToolkit for Unity (https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/?view=mrtkunity-2021-05)

Visual Studio 2019 or higher (https://visualstudio.microsoft.com)
3D modeling programm that can export *.obj*-files. (this project did use TinkerCad (https://www.tinkercad.com))
Microsoft Excell


*Hardware*

HoloLens 2 Microsoft 


# Setup

**Step 1: Download the Project**

Simply download the project ZIP file and unpack it to a disired location(e.g. Desktop)
 
**Step 2: Importing the 3D-Data**

To import the data for the 3D twin move your obj. files into "ARoMA-main\Assets\HD\3D Objects" 
You will find that there are already some obj. files. These are some 3D models you might want to use *later*.


**Step 3: Start Unity**
To start the project in Unity go to "ARoMA-main\Assets" and klick on "Assembly.unity".
After this unity will prepeare the envirement. This step might take a moment.
When unity opens a dialog window  "MRTK Project Configurator" opens. Klick on "Skip This Step" then on "Next" and after that on "Done". 
 


**Step 3: Setup the Projekt**

*Prepare the 3D model*

In the project hieracie navigate to "Assets > HD > 3D Objects.
Mark all 3D objekts(hold SHIFT + klick on first object in list -> klick on last object) 
and klick on "Model" in the "Inspector" window. Now set the "Scale Factor" to 0.01 and klick on "Apply"
(Depending on the size of your 3D model you might need to adjust the Scale Factor)



*Place the 3D model*

Now mark all 3D objekts and drop them into the "3D_Model" game objekt in the scene. 
<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/MarkAll3DFilesAndMoveTo3DModel.gif' width=550 > 
</p>

You can now position and rotate your 3D model onto the build-platform. 
Save the scene. 
<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/RotateAndPlace.gif' width=550 > 
</p>



*Next Chapter:*
<p float="left">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width="100" />
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width="100" /> 
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width="100" />
</p>
 