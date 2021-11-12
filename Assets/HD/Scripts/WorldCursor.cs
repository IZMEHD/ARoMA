using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Diagnostics;
using System.IO;
using System;
using System.Text; //StringBuilder

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit;





using UnityEngine.UI;//added by HD
using System.Text.RegularExpressions;//added by HD
using System.Globalization;//added by HD






public class WorldCursor : MonoBehaviour
{
    // Variablen für Zeit
    private Stopwatch stopwatch = new Stopwatch();
    private Stopwatch stopwatch2 = new Stopwatch();
    long elapsedtime2Start;
    // number of eye hits to store
    private int repeats = 5;
    // Zählvariable
    int i = -1;
    int j = 0;
    //Liste für Daten
    List<string> Daten = new List<string>();

    //List for hit or not
    bool[] hits = { false, false };


    //+++++++++Changes to store recording for each page separately++++++++++++++
    public bool is_Recording = true;// Change by HD
    //private string path_RecordingData = Application.dataPath + "/Daten~/";
    int PageName = 0;   //Get Page Name
    int PageName_old = 0;
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



    private MeshRenderer meshRenderer;

    // Hand tracking
    private static readonly int jointCount = Enum.GetNames(typeof(TrackedHandJoint)).Length;
    public TrackedHandJoint ReferenceJoint { get; set; } = TrackedHandJoint.IndexTip;
    public string OutputFileName { get; } = "ArticulatedHandPose";
    private Handedness recordingHand = Handedness.Right;
    private Handedness recordingHandLeft = Handedness.Left;
    MixedRealityPose[] jointPoses = new MixedRealityPose[jointCount];
    MixedRealityPose[] jointPosesLeft = new MixedRealityPose[jointCount];
    private Vector3 offset = Vector3.zero;

    string filePath;
    string filePathLeft;
    string filePathHeadEye;
    string filePathTime;


    //Für Packetweises Schreiben der Daten  by HD
    string[] write_Buffer = new string[100];
    int Buffer_Couner;


    StringBuilder csv = new StringBuilder();
    StringBuilder csvLeft = new StringBuilder();
    StringBuilder csvHeadEye = new StringBuilder();

    // Use this for initialization
    void Start()
    {
        Buffer_Couner = 1;//initial value  by HD

        // Grab the mesh renderer that's on the same object as this script.
        meshRenderer = this.gameObject.GetComponentInChildren<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("AssemblyDirector").GetComponent<AssemblyDirector>().get_CurrentStep() == 0)
        {
            Set_new_Page_Zero();
        }
        else
        {


            // Do a raycast into the world based on the user's
            // head position and orientation.
            var headPosition = Camera.main.transform.position;
            var gazeDirection = Camera.main.transform.forward;
            //Camera.main.transform.rotation

            RaycastHit hitInfo;

            if (Physics.Raycast(headPosition, gazeDirection, out hitInfo))
            {
                //List for hit or not
                hits[0] = hits[1];
                hits[1] = true;

                // If the raycast hits a hologram...
                // Display the cursor mesh.
                meshRenderer.enabled = false;//Chage to "false" by HD

                // Move the cursor to the point where the raycast hit.
                this.transform.position = hitInfo.point;

                // Rotate the cursor to hug the surface of the hologram.
                this.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);

                if ((hits[0] != hits[1]) && (hits[1] == true) && (false))// chage to "... %% false" change by HD 
                {
                    Console.WriteLine("hit detected");
                    // Event is stored
                    OnEvent();
                }
            }
            else
            {
                //List for hit or not
                hits[0] = hits[1];
                hits[1] = false;

                // If the raycast did not hit a hologram, hide the cursor mesh.
                meshRenderer.enabled = false;
            }



            if (j == 0)
            {
                stopwatch2.Start();
                elapsedtime2Start = stopwatch2.ElapsedMilliseconds;
               // UnityEngine.Debug.Log(elapsedtime2Start);
                j++;


            }
            //stopwatch2.Stop();
            var elapsedtime2 = stopwatch2.ElapsedMilliseconds;
            var timedif = elapsedtime2 - elapsedtime2Start;

            if (timedif > 25) //for 60 Hz set to 16.66 (ms)
            {
                //elapsedtime2Start = stopwatch2.ElapsedMilliseconds;
                // UnityEngine.Debug.Log("timedif = " + timedif);
                elapsedtime2Start = elapsedtime2;

                j++;
                //  UnityEngine.Debug.Log("j = " + j);

                for (int k = 0; k < jointCount; ++k)
                {
                    HandJointUtils.TryGetJointPose(ReferenceJoint, recordingHand, out MixedRealityPose joint);
                    HandJointUtils.TryGetJointPose((TrackedHandJoint)k, recordingHand, out jointPoses[k]);

                    //same for left hand
                    HandJointUtils.TryGetJointPose(ReferenceJoint, recordingHandLeft, out MixedRealityPose jointLeft);
                    HandJointUtils.TryGetJointPose((TrackedHandJoint)k, recordingHandLeft, out jointPosesLeft[k]);

                    // UnityEngine.Debug.Log("k" + k);
                }

                ArticulatedHandPose pose = new ArticulatedHandPose();
                pose.ParseFromJointPoses(jointPoses, recordingHand, Quaternion.identity, offset);

                //UnityEngine.Debug.Log("pose =" + jointPoses[1].Position + jointPoses[2].Position);
                //UnityEngine.Debug.Log("pose =" + jointPoses[2].Position);

                //in your loop
                // make for loop here to get all joints
                // set values to long

                //HeadEye motion storage
                var firstHE = Camera.main.transform.position.ToString("N6");
                var secondHE = Camera.main.transform.rotation.ToString("N6");



                // Eye gaze detection
                //Vector3 gazeOrigin = IMixedRealityGazeProvider.GazeOrigin;
                var eyeOrigin = CoreServices.InputSystem?.EyeGazeProvider?.GazeOrigin;
                var eyeDirection = CoreServices.InputSystem?.EyeGazeProvider?.GazeDirection; //euler angles instead of quaternion
                var hitPosition = CoreServices.InputSystem?.EyeGazeProvider?.HitPosition;
                Quaternion eyeRotation = Quaternion.LookRotation((Vector3)eyeDirection, Vector3.up);
                Vector3 eyeOrigin2 = (Vector3)eyeOrigin;
                Vector3 eyeDirection2 = (Vector3)eyeDirection;

                var eyeOriginS = eyeOrigin2.ToString("N6");
                var eyeDirectionS = eyeDirection2.ToString("N6");
                var eyeRotationS = eyeRotation.ToString("N6");

                var newLineHE = string.Format("{0},{1},{2},{3},{4},{5},{6},{7}", firstHE, secondHE, eyeOriginS, eyeRotationS, eyeOrigin, eyeDirectionS, ((Vector3)hitPosition).ToString("N6") , "\n");//eyeDirection durch eyeDirectionS ersetzt, hitPosition druch  ((Vector3)hitPosition).ToString("N6") ersezt ,  by HD
                //csvHeadEye.AppendLine(newLineHE);// HD *1


                //get time-stamp for non-linear behavior//HD
                if (String.IsNullOrEmpty(filePathTime) || PageName_old != PageName)//HD "| PageName_old != PageName"
                {
                    filePathTime = GetPath2("Time");
                    File.AppendAllText(filePathTime, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff" + "\n"));// HD *1                                             
                }
                else
                {
                    File.AppendAllText(filePathTime, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff" + "\n"));// HD *1
                }




                if (String.IsNullOrEmpty(filePathHeadEye) || PageName_old != PageName)//HD "| PageName_old != PageName"
                {
                    // create filepath left
                    filePathHeadEye = GetPath2("HeadEye");
                    //after your loop
                    //File.WriteAllText(filePathHeadEye, csvHeadEye.ToString());// HD *1
                    File.AppendAllText(filePathHeadEye, newLineHE);// HD *1
                                                                   //PageName_old = PageName;
                }
                else
                {
                    //csv.AppendLine(newLine);
                    //after your loop
                    // File.WriteAllText(filePathHeadEye, csvHeadEye.ToString());// HD *1
                    File.AppendAllText(filePathHeadEye, newLineHE);// HD *1
                }





                //loop for hand motion storage
                for (int k = 0; k < jointCount; ++k)
                {
                    var first = jointPoses[k].Position.ToString("N6");
                    var second = jointPoses[k].Rotation.ToString("N6");
                    var newLine = string.Format("{0},{1}", first, second);

                    var firstLeft = jointPosesLeft[k].Position.ToString("N6");
                    var secondLeft = jointPosesLeft[k].Rotation.ToString("N6");
                    var newLineLeft = string.Format("{0},{1}", firstLeft, secondLeft);

                    if (k == 0)
                    {
                        //create new line
                        csv.AppendLine(newLine);
                        csvLeft.AppendLine(newLineLeft);
                    }
                    else
                    {
                        //extend existing line
                        csv.Append(newLine);
                        csvLeft.Append(newLineLeft);
                    }
                }

                if (String.IsNullOrEmpty(filePath) || PageName_old != PageName)//HD "|| PageName_old != PageName "
                {
                    // create filepath
                    filePath = GetPath2("Right");
                    //after your loop
                    File.AppendAllText(filePath, csv.ToString());

                }
                else
                {
                    //csv.AppendLine(newLine);
                    //after your loop
                    File.AppendAllText(filePath, csv.ToString());
                }

                if (String.IsNullOrEmpty(filePathLeft) || PageName_old != PageName)//HD "|| PageName_old != PageName "
                {
                    // create filepath left
                    filePathLeft = GetPath2("Left");
                    //after your loop
                    File.AppendAllText(filePathLeft, csvLeft.ToString());
                    PageName_old = PageName;//HD
                }
                else
                {
                    //csv.AppendLine(newLine);
                    //after your loop
                    File.AppendAllText(filePathLeft, csvLeft.ToString());
                }
            }
            csv.Clear();
            csvLeft.Clear();

        }
    }

    // use each object interaction as an event
    public void OnEvent()
    {
        i++;
        if (i == 0)
        {
            //  UnityEngine.Debug.Log("i = " + i);
            // Position speichern

        }
        if (i > 0 & i <= repeats)
        {
            // Stoppuhr stoppen und Wert abspeichern
            //  UnityEngine.Debug.Log("i = " + i);
            stopwatch.Stop();
            float elapsedtime = stopwatch.ElapsedMilliseconds;
            // UnityEngine.Debug.Log("Zeit: " + elapsedtime + " ms");

            Daten.Add(i.ToString() + "\t" + elapsedtime.ToString());

        }
        if (i >= 0 & i <= repeats)
        {

            // Stopuhr neustarten
            stopwatch.Reset();
            stopwatch.Start();

        }
        if (i >= repeats)
        {
            //  UnityEngine.Debug.Log("Schreibe Daten in Datei");
            WriteDataToFile();
            //  UnityEngine.Debug.Log("Reset");
        }
    }

    public void WriteDataToFile()
    {
        // Hole Dateipfad
        string filepath = GetPath();

        // Erstelle die Datei
        StreamWriter writer = new StreamWriter(filepath);

        // Schreibe Überschriften
        writer.WriteLine("Experimentelle Daten zum Gesetz von Fitts in 3D");
        writer.WriteLine("Nummer der Messung\tZeit in ms");

        // Schreibe die Daten aus der Liste
        foreach (var Datenpunkt in Daten)
        {
            writer.WriteLine(Datenpunkt);
        }
        // Daten aufs Dateisystem schreiben
        writer.Flush();

        // Datei schließen
        writer.Close();
    }

    private string GetPath()
    {
        // Erstelle Dateiname aus Zeitstempel
        string filename = "Experiment_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".dat";

        // Finde Datenordner je nach Architektur
        string directory = "";
        if (Application.isEditor)
        {
            directory = Application.dataPath + "/Daten~/";
        }
        else
        {
            //if running on HL
            directory = Application.persistentDataPath + "/Daten~/";
        }
        /*
#if UNITY_WSA_10_0
        string directory = Application.persistentDataPath + "/Daten~/";
#else
        string directory = Application.dataPath + "/Daten~/";
#endif
*/

        // Überprüfe, ob Ordner existiert
        if (!Directory.Exists(directory))
        {
            //Wenn nicht, erstelle ihn
            Directory.CreateDirectory(directory);
        }

        // Setze Pfad aus Ordner und Dateiname zusammen
        string mypath = directory + filename;

        // Gebe den Pfad aus (Debug)
        //UnityEngine.Debug.Log(mypath);

        // Gebe den Pfad zurück
        return mypath;
    }

    private string GetPath2(string hand)
    {
        // Erstelle Dateiname aus Zeitstempel
        string filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + "_Page_" + PageName + "_" + hand + ".csv";


        // Finde Datenordner je nach Architektur
        string directory = "";
        if (Application.isEditor)
        {
            directory = Application.dataPath + "/Daten~/";
        }
        else
        {
            //if running on HL
            directory = Application.persistentDataPath + "/Daten~/";
        }



        // Überprüfe, ob Ordner existiert
        if (!Directory.Exists(directory))
        {
            //Wenn nicht, erstelle ihn
            Directory.CreateDirectory(directory);
        }

        // Setze Pfad aus Ordner und Dateiname zusammen
        string mypath = directory + filename;

        // Gebe den Pfad aus (Debug)
        //UnityEngine.Debug.Log(mypath);

        // Gebe den Pfad zurück
        return mypath;
    }





    //++++++++++++++Functions added by HD++++++++++++++++++
    public void Start_Recording()
    {
        is_Recording = true;
    }

    public void Stop_Recording()
    {
        is_Recording = false;
    }

    public void Set_new_Page_Zero()//For Getting PageNr for Storage; needs to be called by button or voice command
    {
        PageName = 0;
        csv.Clear();
        csvLeft.Clear();
        csvHeadEye.Clear();
    }

    public void Set_new_Page_up()//For Getting PageNr for Storage; needs to be called by button or voice command
    {
        PageName = PageName + 1;    //Get Page Name
        csv.Clear();
        csvLeft.Clear();
        csvHeadEye.Clear();

    }

    public void Set_new_Page_down()//needs to be called by button or voice command
    {
        PageName = PageName - 1;    //Get Page Name
        csv.Clear();
        csvLeft.Clear();
        csvHeadEye.Clear();
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++

}