# ARoMA: AR-optimized Manufacturing-Assistant for HoloLens 2 in Unity
 
Demo video: <a href="https://www.youtube.com/watch?v=M1uZh9A-Ros&ab_channel=izmeHD"><img src="https://img.shields.io/badge/-YouTube-red?&style=for-the-badge&logo=youtube&logoColor=white" height=20></a>


<p align="center">
  <a href="https://www.youtube.com/watch?v=M1uZh9A-Ros&ab_channel=izmeHD"><img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/DemoLegoBuildAndTwin.gif' width=450 ></a>  
</p>



***Summary*** 

This project is the (by)product of a bachelor thesis dealing with the development of an "AR-optimized Manufacturing-Assistant" system for the HoloLens 2.
This system was developed taking into account current findings and results from works that deals with the topic of AR in training,
assembly and maintenance tasks. 
The main purpose of this system is to guide an inexperienced user (e.g. a trainee of some kind) through a manual assembly or maintenance task.
Additionally the system allows the HoloLens 2 sensors to record the user's hand and head movements. These recordings can later be visualized, for example 
with *this* program.




# Feature Overview

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width=550 > 
</p>


**Digital Twin:**
In its core-function *AroMa* uses a digital twin to show the current assembly step and highlights step-relevant parts by animating them in such a way that mounting 
position and direction are shown. Additionally the user can always grab the twin and manipulate its position, orientation and size.


**Part Amount Indicator:**
It is possible, during the setup process, to designate part-bins, which are then, during assembly,  highlighted and accompanied by numbers to indicate to the user what and how many parts are required 
for a given step.

**Proccess Indicator:**
During assembly the current assembly-step and the total amount of steps are shown close to the users right hand. Per default, the right hand but it can be changed.

**Voice commands:**
Via the voice command "Next" the subsequent assembly step can be initiated. 

**Display of additional informations:**
Via the voice command "Next" the subsequent assembly step can be initiated. 


 # Prerequisites

 

*Software:*

Unity 2019 or higher (https://unity.com) 

MixedRealityToolkit for Unity (https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/?view=mrtkunity-2021-05)

Visual Studio 2019 or higher (https://visualstudio.microsoft.com)

3D modeling program that can export *.obj*-files. (this project did use TinkerCad (https://www.tinkercad.com))

Microsoft Excel


*Hardware:*
 
	
HoloLens 2 Microsoft 


# Setup

**Step 1: Download the Project**

Download the project ZIP file and unpack it to a desired location (e.g. Desktop).
 
**Step 2: Importing the 3D-Data**

To import the data for the 3D twin, move your obj. files into "ARoMA-main\Assets\HD\3D Objects" 
You will find that there are already some obj. files. These are some 3D models you might want to use *later*.


**Step 3: Start Unity**
To start the project in Unity go to "ARoMA-main\Assets" and klick on "Assembly.unity".
After this, Unity will prepare the environment. This step might take a moment.
When Unity opens a dialogue window  "MRTK Project Configurator" opens. Click on "Skip This Step" then on "Next" and after that on "Done". 
 


**Step 3: Setup the Projekt**

*Prepare the 3D model*

In the project hierachy navigate to "Assets > HD > 3D Objects.
Mark all 3D objects (hold SHIFT + klick on first object in list -> klick on last object) 
and click on "Model" in the "Inspector" window. Now set the "Scale Factor" to 0.01 and click on "Apply"
(Depending on the size of your 3D model you might need to adjust the Scale Factor)



*Place the 3D model*

Now mark all 3D objects and drop them into the "3D_Model" game object in the scene. 
<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/MarkAll3DFilesAndMoveTo3DModel.gif' width=850 > 
</p>

You can now position and rotate your 3D model onto the build-platform. 

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/RotateAndPlace.gif' width=850 > 
</p>


After positioning all parts you need to add the "AnimationSlave" script to all the 3D parts. 
Every game object under "3D_Model" should have one "AnimationSlave" attached to it (exept "AssemblyBase" if you want to use it, if not you can delete it).
Save the scene. 

**Step 4: Determine build and animation sequence**

*Sequence*
Now we want to tell the program at which step the different parts should be displayed and animated.
To do that we need the "Order Of Activity.xlsx" file. Under "Name" you enter the names of your 3D models of your individual components. 
These names should be the the same as in "3D_Model" in your Unity hierachy. 
Now under "Step" you enter a "1" for every step at which the component should be displayed. 

*Animation*
The first time a component appears during an assembly it will be animated acording to the two parameters you can enter
under "Direction" and "Distance". 
"Direction" is the the direction from where the part moves to its final assembly possition.
Valid directions are "Down", "Up", "Back", "Forward", "Left" and "Right"
"Distance" is the distance the part starts the animation from to its final assembly possition( "0.1" is a good start) .
If you don't want a part to be animated set "Direction" to any of the valid directions and "Distance" to "0.0".

In the end your "Order Of Activity.xlsx" file should lock something like this:

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/OrderOfActivityDemo.JPG' width=650 > 
</p>

Now navigate to the "3D_Model" game object in the Hierachy-window in Unity.
Go to the Inspector-window and now copy the content of cell B1 of the "Order Of Activity.xlsx" 
into the "Order Of Activity" field. 


**Step 5: Define order of component bins**

Open the "Zone Activity.xlsx" file. Here you can enter at what step, how many and from which Box components should be picked.

For example: You need "Item 1" for step-1 two times, for step-5 two times. Also you need one "Item 2" for step-1 , for step-8 five times.
Then your "Zone Activity.xlsx" should look like this:

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/ZoneActivityDemo.JPG' width=650 > 
</p>

*Note: If you do not intent to use this feature just fill in "1" for the first box for the amount of assembly steps you have.*
You do not need to fill the "Note" fields. They are just for convenience to keep track which box contains what part. 


Now navigate to the "AssemblyDirector" game object in the Hierachy-window in Unity.
Go to the Inspector-window and now copy the content of cell B1 of the "Order Of Activity.xlsx" 
into the "Zone Activity" field. 

 


**Step 6: Test in Editor**
At this Point you should be able to test the assembly in Unity. 
For this click on "Play". By clicking "Next" you can go through the assembly process. At all times you should be
able to press the "Restart" button to restart the assembly process. 

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/InEditorView.gif' width=650 > 
</p>

If everything looks good you can now deploy the program to the HoloLens 2. 




**Step 7: Deployment to HoloLens 2 via Wifi**
Note: This is just one of multiple methods to deploy a program to the HoloLens 2.   

First you go to "File" (in Unity), then "Build settings..." 
Then make sure your settings look like this:

<p align="center">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/BuildSettings2.JPG' width=650 > 
</p>

Click "Switch Platform" and then "Build". Unity should ask you where to save the build, you can create a folder "Build" in 
your project folder and save it there. 
When Unity is done, navigate to the build folder and click on the ".sln" file. 
Now navigate to your Project folder and open the "Fittslaw.sln". 
In Visual Studio go to "Project" then "Propertys". For Configuration select "Release" and for Platform select "ARM64".
Now under "General" -> "Debugging" enter the IP of your HoloLens 2 under "Machine Name". To get the IP of your HoloLens 2 just say "What is my IP" while wearing it.
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



# Recording & Replay (optional)

ARoMA automitically records the hand and head movement of the user for all build-steps(exept step 0 and last step(build-complete)). 

**Step 0: Prerequisites**
To download the data from the HoloLens you neet to get Spyder(Anaconda) and the Python scripts from the folder ARoMA/DownloadData


**Step 1: Download Data**
Start Anaconda/Spyder as admin. Next you enter these commands one at a time:

cd ***Path to the DownloadData folder***<br />
pip install opencv<br />
import cv2<br />
run recorder_console_app.py --dev_portal_address ***ip address of the HoloLens*** --workspace_path "***Path to the DownloadData folder***"  --dev_portal_username "***your Username***"--dev_portal_password "***your password***"<br />


Here an example what is can look like:


cd C:\Users\JohnSmith\Desktop\ARoMa Git\ARoMA\DownloadData<br />
pip install opencv<br />
import cv2<br />
run recorder_console_app.py --dev_portal_address 192.168.1.1 --workspace_path "C:\Users\JohnSmith\Desktop\ARoMa Git\ARoMA\DownloadData"  --dev_portal_username "JohnSmith"--dev_portal_password "1234556789"<br />


When all worked, you have now the option to download the data do the DownloadData folder. 



**Step 2: Unpack and start the visualizer***
Unpack the the "AssemblyTaskVisualization.zip" file from "ARoMA\Extra Files\AssemblyTaskVisualization".
Now open "AssemblyTaskVisualization\AssemblyTaskVisualization\Assets\AssemblyTaskVisualization.unity" with Unity.



**Step 3: Replay the Recording with the visualizer***
Demo video: <a href="https://www.youtube.com/watch?v=M1uZh9A-Ros&ab_channel=izmeHD"><img src="https://img.shields.io/badge/-YouTube-red?&style=for-the-badge&logo=youtube&logoColor=white" height=20></a>

Now we can load the data from the visualizer. To do that first cilck on "Select Folder" and navigate to the recordings you previously downloaded. Select the folder of the recording
you want to replay. <br />
Then select the step you want to visualize by entering it in the filed next to "Set Page" and the click "Set Page".
Now you can Play/Pause the recording or single-step through the recording by pressing "Step -" or "Step +" while the replay is paused.
  
 
 


**ToDo**
 Hello
1.Recording & Replay / Disable Recording
2.Verify instructions
3.Polishing
4.How to add Hint Text and Image
5.How to add Hint Video
 



*Next Chapter:*
<p float="left">
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width="100" />
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width="100" /> 
  <img src='https://github.com/IZMEHD/ARoMA/blob/main/imgs/AssemblyLego1-4.gif' width="100" />
</p>
 