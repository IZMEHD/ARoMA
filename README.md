# ARoMA: AR-optimized Manufacturing-Assistant for HoloLens 2 in Unity
 


***Summary*** 

This project is the (by)product of a bachelor thesis dealing with the development of an "AR-optimized Manufacturing-Assistant" system for the HoloLens 2.
This system was developed taking into account current findings and results from works that deals with the topic of AR in training,
assembly and maintenance tasks. 
The main purpose of this system is to guide an inexperienced user (e.g. a trainee of some kind) through a manual assembly or maintenance task.
Additionally the system allows the HoloLens 2 sensors to record the user's hand and head movements. These recordings can later be visualized, for example,  
with [this](#recording-and-replay) provided program.


### Table of Contents
**[Feature Overview](#feature-overview)**<br>
**[Prerequisites](#prerequisites)**<br>
**[Setup](#setup)**<br>
**[Assembly Setup (optional)](#assembly-setup)**<br>
**[Recording and Replay (optional)](#recording-and-replay)**<br>
 

## Feature Overview

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width=550 > 
</p>


**Digital Twin:**<br />
In its core-function *ARoMA* uses a digital twin to show the current assembly step and highlights step-relevant parts by animating them in such a way that mounting 
position and direction are shown. Additionally the user can always grab the twin and manipulate its position, orientation and size.
<br />

**Part Amount Indicator:**<br />
It is possible, during the setup process, to designate part-bins, which are then, during assembly,  highlighted and accompanied by numbers to indicate to the user what and how many parts are required 
for a given step.
<br />
**Proccess Indicator:**<br />
During assembly the current assembly-step and the total amount of steps are shown close to the user's right hand. 
<br />
**Voice commands:**<br />
Via the voice command "Next" the subsequent assembly step can be initiated. 
<br />
 


## Prerequisites

 

**Software:**<br />

Unity 2019 or higher (https://unity.com) 

MixedRealityToolkit for Unity (https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/?view=mrtkunity-2021-05)

Visual Studio 2019 or higher (https://visualstudio.microsoft.com)

3D modeling program that can export *.obj*-files. (this project did use TinkerCad (https://www.tinkercad.com))

Microsoft Excel
<br />

**Hardware:**<br />
 	
HoloLens 2 Microsoft 


## Setup

**Step 1: Download the Project**<br />
Download the project ZIP file and unpack it to a desired location (e.g. Desktop).
 
**Step 2: Importing the 3D-Data**<br />
To import the data for the 3D twin, move your obj. files into "ARoMA-main\Assets\HD\3D Objects" 
You will find that there are already some obj. files. These are some 3D models you might want to use later.
This project also provides some 3D files for testing purposes under [ARoMA/Extra Files/Demo/3D Objects](https://github.com/IZMEHD/ARoMA/tree/main/Extra%20Files/Demo/3D%20Objects).  


**Step 3: Start Unity**<br />
To start the project in Unity go to "ARoMA-main\Assets" and click on "Assembly.unity".
After this, Unity will prepare the environment. This step might take a moment.
When Unity opens a dialogue window  "MRTK Project Configurator" opens. Click on "Skip This Step" then on "Next" and after that on "Done". 
To move in the scene go with the mouse courser in the Scene window. Here you can look around while holding the right mouse button. To move, while holding the right mouse button, use *WASD*.
 


**Step 4: Setup the Projekt**

*Prepare the 3D model*<br />
In the project hierachy navigate to "Assets > HD > 3D Objects.
<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/project_hierachy.JPG' width=850 > 
</p>
Mark all 3D objects (hold SHIFT + click on first object in list -> click on last object) 
and click on "Model" in the "Inspector" window. Now set the "Scale Factor" to 0.01 and click on "Apply"
(Depending on the size of your 3D model you might need to adjust the Scale Factor)
To color the 3D parts click on them individually and select in the Inspector window under "Materials"->"Remapped Materials" 
a material of your choice. A small selection of colors is already provided.
In case you need more colors or textures for your parts you might want to refer to [this hello](https://www.youtube.com/watch?v=IFlXvDZezBQ&ab_channel=Unity) and [This](https://www.youtube.com/watch?v=V72pMtqMgFk&t=4s&ab_channel=Zenva).



*Place the 3D model*<br />
Now mark all 3D objects and drop them into the "3D_Model" game object in the scene. 
<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/MarkAll3DFilesAndMoveTo3DModel.gif' width=850 > 
</p>

You can now position and rotate your 3D model onto the build-platform. 

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/RotateAndPlace.gif' width=850 > 
</p>


After positioning all parts you need to add the "AnimationSlave" script to all the 3D parts. 
Every game object under "3D_Model" should have one "AnimationSlave" script attached to it (exept "AssemblyBase" if you want to use it, if not you can delete it).
Save the scene. 



**Step 4: Determine build and animation sequence**<br />
<br />
*Sequence*<br />
Now we want to tell the program at which step the different parts should be displayed and animated.
To do that, we need the "Order Of Activity.xlsx" file. Under "Name" you enter the names of your 3D models of your individual components. 
These names should be the same as in "3D_Model" in your Unity hierarchy.
Now under "Step" you enter a "1" for every step at which the component should be displayed. 

*Animation*<br />
The first time a component appears during an assembly it will be animated according to the two parameters you can enter
under "Direction" and "Distance". 
"Direction" is the direction from where the part moves to its final assembly position.
Valid directions are "Down", "Up", "Back", "Forward", "Left" and "Right"
"Distance" is the distance the part starts the animation from to its final assembly positions( "0.1" is a good start) .
If you don't want a part to be animated set "Direction" to any of the valid directions and "Distance" to "0.0".
In the end your "Order Of Activity.xlsx" file should lock something like this:

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/OrderOfActivityDemo.JPG' width=650 > 
</p>

Now navigate to the "3D_Model" game object in the Hierarchy-window in Unity.
Go to the Inspector-window and now copy the content of cell B1 of the "Order Of Activity.xlsx" 
into the "Order Of Activity" field. 


**Step 5: Define order of component bins**<br />
Open the "Zone Activity.xlsx" file. Here you can enter at what step, how many and from which Box components should be picked.
For example: You need "Item 1" for step-1 two times, for step-5 two times. Also, you need one "Item 2" for step-1 , for step-8 five times.
Then your "Zone Activity.xlsx" should look like this:

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/ZoneActivityDemo.JPG' width=650 > 
</p>

*Note: If you do not intent to use this feature just fill in "1" for the first box for the amount of assembly steps you have.*
You do not need to fill the "Note" fields. They are just for convenience to keep track which box contains what part. <br />

Now navigate to the "AssemblyDirector" game object in the Hierachy-window in Unity.
Go to the Inspector-window and now copy the content of cell B1 of the "Order Of Activity.xlsx" 
into the "Zone Activity" field. 

 
**Step 6: Test in Editor**<br />
At this Point you should be able to test the assembly in Unity. 
For this click on "Play". By clicking "Next" you can go through the assembly process. At all times you should be
able to press the "Restart" button to restart the assembly process. 

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/InEditorView.gif' width=650 > 
</p>

If everything looks good you can now deploy the program to the HoloLens 2. 




**Step 7: Deployment to HoloLens 2 via Wifi**<br />
*Note: This is just one of multiple methods to deploy a program to the HoloLens 2 but only this way was tested.*
First you go to "File" (in Unity), then click on "Build settings..." 
Then make sure your settings look like this:

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/BuildSettings2.JPG' width=650 > 
</p>

Click "Switch Platform" and then "Build". Unity should ask you where to save the build, you can create a folder "Build" in 
your project folder and save it there. 
When Unity is done, navigate to the build folder and click on the ".sln" file. 
Now navigate to your Project folder and open the "Fittslaw.sln". 
In Visual Studio go to "Project" then "Properties". For Configuration, select "Release" and for Platform select "ARM64".
Now under "General" -> "Debugging" enter the IP of your HoloLens 2 under "Machine Name". To get the IP of your HoloLens 2, just say, "What is my IP" while wearing it.
<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/VSConfig1.JPG' width=650 > 
</p>

Now also set "Release" and "ARM64" like this:

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/VSConfig1.JPG' width=650 > 
</p>

At this point make sure the HoloLens 2 is on and connected to the same WiFi as your computer. 
Click on "Remote Machine". Visual Studio should now transfer the program to your HoloLens 2. This might take some time. 
After Visual Studio is done the program should start on the HoloLens 2.


## Assembly Setup
 
**Setup of zones**<br />
After starting the application on the HoloLens you have the option to set up the bin-zones at their respective locations.
To do that say "Interface" and a widow over your right hand will appear. By pressing "Next" you can summon a Zone (starting with "Zone_1) which you can now place.
To place the new zone just grab it with your left hand and move it to its location. You can close the placement-mode by again saying "Interface". 
During and after an assembly you call also call the interface window to restart the assembly process. The Zones do not need to be placed again when you restart an assembly.

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/InterfaceAndSetup.gif' width=650 > 
</p>


**Adding more Zones**<br />
If you need more zones you can add them in the ARoMA program in Unity. In the Hierarchy go to HandUI->HandFollow->ZoneMagazine and copy an existing zone and then change the number to the next highest
value. For example, if you have 18 Zones and want to add two more, just copy Zone_1 two times and change the names of the new objects to "Zone_19" and "Zone_20".
If you need less zones just delete the Zone-objects, starting with the highest values.



## Recording and Replay

ARoMA can record the hand and head movement of the user for all build-steps(except step 0 and the last step(build-complete)). 
If you are not interested you can disable this function in the ARoMA program in Unity. In the Hierarchy go to AssemblyDirector->CursorFocus and uncheck the "World Cursor" Script in the Inspector window. 

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/Demovisualisation.gif' width=650 > 
</p>

To get the recorded data and replay them follow the next steps:


**Step 0: Prerequisites**<br />
To download the data from the HoloLens you need to get Spyder(Anaconda) and the Python scripts from the folder ARoMA/DownloadData


**Step 1: Download Data**<br />
Start Anaconda/Spyder as admin. Next, you enter these commands one at a time:
```
cd *Path to the DownloadData folder*
pip install opencv
import cv2
run recorder_console_app.py --dev_portal_address *ip address of the HoloLens* --workspace_path "*Path to the DownloadData folder*"  --dev_portal_username "*your Username*"--dev_portal_password "*your password*"
```

Here an example, what is can look like:

```
cd C:\Users\JohnSmith\Desktop\ARoMa Git\ARoMA\DownloadData
pip install opencv
import cv2
run recorder_console_app.py --dev_portal_address 192.168.1.1 --workspace_path "C:\Users\JohnSmith\Desktop\ARoMa Git\ARoMA\DownloadData"  --dev_portal_username "JohnSmith"--dev_portal_password "ABCD123456789"
```

When all worked, you have now the option to download the data do the "DownloadData" folder. 



**Step 2: Unpack and start the visualizer**<br />
Unpack the "AssemblyTaskVisualization.zip" file from "ARoMA\Extra Files\AssemblyTaskVisualization".
Now open "AssemblyTaskVisualization\AssemblyTaskVisualization\Assets\AssemblyTaskVisualization.unity" with Unity.



**Step 3: Replay the Recording with the visualizer** <br />
Demo video: <a href="https://youtu.be/rYfoGmanZhE"><img src="https://img.shields.io/badge/-YouTube-red?&style=for-the-badge&logo=youtube&logoColor=white" height=20></a><br />

Now we can load the data from the visualizer. To do that, first cilck on "Select Folder" and navigate to the recordings you previously downloaded. Select the folder of the recording
you want to replay. <br />
Then select the step you want to visualize by entering it in the field next to "Set Page" and the click "Set Page".
Now you can Play/Pause the recording or single-step through the recording by pressing "Step -" or "Step +" while the replay is paused.
  
 
 


**ToDo**


1.Verify instructions
2.How to add Hint Text and Image
3.How to add Hint Video
 

 