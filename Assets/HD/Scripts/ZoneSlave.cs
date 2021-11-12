using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Globalization;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Microsoft.MixedReality.Toolkit.UI;

public class ZoneSlave : MonoBehaviour
{

    //GameObject ZoneMagazine;
    GameObject CalibrationMaster;//when a zone is moved the first time it goes here
    GameObject AssemblyDirector;
    Text MyAmountText;//holds the text which is used to display the amount to pick for a given step
    int MyID;//nr in Zone-name
    bool Calibrated = false;
    Transform ChildZone;

    //Materials to display Zone state
    Material Material_Cal;
    Material Material_Run;

    string ZoneActivity;//string from AssemblyDirector with info on when a zone should be highlighted
    int[] ZoneActivityList;// list with info on when a zone should be highlighted based on ZoneActivity
    int[] ZoneAmountActivityList;//list with info on how many parts to pick based on ZoneActivity
    int CurrentStep = -1;
    bool PosSaved = false;//flag for if the position of zone is already saved
     

    // Start is called before the first frame update
    void Start()
    {

        ChildZone = this.gameObject.transform.GetChild(0);

        //get number out of zone-name
        MyID = int.Parse(Regex.Replace(transform.name, "[^0-9]", ""));

        //access text element to display amounts
        MyAmountText = GameObject.Find(("Zone_" + MyID + "/ZoneBox/Canvas/Text")).GetComponent<Text>();


        
        CalibrationMaster = GameObject.Find("/CalibrationMaster");
        AssemblyDirector = GameObject.Find("/AssemblyDirector");

        //load materials from CalibrationMaster 
        Material_Cal = CalibrationMaster.GetComponent<CalibrationMaster>().get_Material_Cal();
        Material_Run = CalibrationMaster.GetComponent<CalibrationMaster>().get_Material_Run();

        //load ZoneActivity from CalibrationMaster and make MakeZoneActivityList
        ZoneActivity = AssemblyDirector.GetComponent<AssemblyDirector>().get_ZoneActivity();
        MakeZoneActivityList();
         

        if (MyID == 0)//"Zone_0" For debugging 
        {
            for (int i = 0; i < ZoneActivityList.Length; i++)
            {
                Debug.Log("Step " + i + ": " + ZoneActivityList[i]);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        //check calibration-state 
        if (CalibrationMaster.GetComponent<CalibrationMaster>().get_Calibration())
        {
            //while in calibration allow the zones to be moved and highlight them
            ChildZone.GetComponent<ObjectManipulator>().enabled = true;
            ChildZone.GetComponent<MeshRenderer>().material = Material_Cal;
        }
        else
        {
            //if calibration is over deactivate movement of zones 
            ChildZone.GetComponent<ObjectManipulator>().enabled = false;



            //reset flag for save position 
            if (CurrentStep == 0 && PosSaved == true)
            {     
                PosSaved = false;
            }


            //set flag for save position and  save position 
            if (CurrentStep == 1 && PosSaved == false)
            {
                savePosition();
                PosSaved = true;
            }


            //if AssemblyDirector advances assembly
            if (CurrentStep != AssemblyDirector.GetComponent<AssemblyDirector>().get_CurrentStep())
            {
                CurrentStep = AssemblyDirector.GetComponent<AssemblyDirector>().get_CurrentStep();

                //check if zone should be activ for this step
                if (ZoneAmountActivityList[CurrentStep] > 0)
                {   //display amount of parts to be taken from zone
                    MyAmountText.text = ZoneAmountActivityList[CurrentStep].ToString();
                }
                else
                {
                    MyAmountText.text = "";
                }


                //highlight zone if activ
                if (ZoneActivityList[CurrentStep] == 1)
                {
                    ChildZone.GetComponent<MeshRenderer>().material = AssemblyDirector.GetComponent<AssemblyDirector>().get_HighlightZone();
                //    Debug.Log("Hey i am Zone " + MyID + " and i think it is my turn. You should pick " + ZoneAmountActivityList[CurrentStep] + "parts from me!");
                }
                else
                {
                    ChildZone.GetComponent<MeshRenderer>().material = Material_Run;
                }
            }
        }


        //toggle the zone on if it is its turn for calibration
        if (Calibrated == false)
        {
            if (MyID == CalibrationMaster.GetComponent<CalibrationMaster>().get_CurrentCalibrationStep())
            {
                foreach (Transform child in transform)
                    child.gameObject.SetActive(true);

            }
            else
            {
                foreach (Transform child in transform)
                    child.gameObject.SetActive(false);
            }
        }


    }



    //go through ZoneActivity and make list in ZoneAmountActivityList and ZoneActivityList for when a zone is activ and what amounts should be picked
    void MakeZoneActivityList()
    {
        //Debug.Log(ZoneActivity);
        int nrOfSteps = ZoneActivity.Split("."[0]).Length;
        string[] Steps = ZoneActivity.Split("."[0]);

        ZoneActivityList = new int[Steps.Length-1];
        ZoneAmountActivityList = new int[Steps.Length - 1];
        for (int i = 0; i < Steps.Length-1; i++)
        {
            //Debug.Log("This is in Step " + i + " : " +Steps[i]);
            string[] intructionsInStep = (Steps[i].Trim()).Split("_"[0]);
            for (int j = 0; j < intructionsInStep.Length; j++)
            {
                
                string[] subinstructions = (intructionsInStep[j].Trim()).Split("x"[0]);

                if (MyID == int.Parse(subinstructions[0]))
                {
                    ZoneActivityList[i] = 1;
                   // Debug.Log("Zone " + MyID + "  at step " + i + " :" + int.Parse(subinstructions[1]));
                    ZoneAmountActivityList[i] = int.Parse(subinstructions[1]);
                }
                else
                {
                   // ZoneAmountActivityList[i] = 0;
                }
            }

       
       


        }

      //  Debug.Log("there are " + nrOfSteps + " steps");
    }


    //is called when zone is touched for the first time to transfer it from HandUI to CalibrationMaster
    public void ChangeFrameReference()
    {
      //  Debug.Log("Zone" + MyID + " says: I am beeing touched");
        if (Calibrated == false)
        {
            transform.parent = CalibrationMaster.transform;
            Calibrated = true;
        }


    }


    //saves the position, rotation and localScale of object
    public void savePosition()
    {
        var pos = transform.GetChild(0).position;
        var dir = gameObject.transform.GetChild(0).rotation;
        var size = gameObject.transform.localScale;
        var sizeChild = transform.GetChild(0).localScale;
        var boundSize = gameObject.transform.GetChild(0).GetComponent<Collider>().bounds.size;
        string path = "";
        string Obj_name = transform.name;

        if (Application.isEditor)
        {
            path = Application.dataPath + "/Daten~/" + Obj_name + "_MarkerPosition.txt";
        }
        else
        {
            path = Application.persistentDataPath + "/Daten~/" + Obj_name + "_MarkerPosition.txt";
        }

        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(pos.ToString("F4") + " " + dir.ToString("F4") + " " + size.ToString("F4")+ "" + sizeChild.ToString("F4"));
        }

    }

}
