using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssemblyDirector : MonoBehaviour
{
    public Material HighlightZone;//Material for when a zone is highlighted
    public string ZoneActivity = "0x0.13x4_14x4.13x4_17x1.3x3_4x1.0x0.8x1.15x1.12x4.16x1_9x1.2x1.0x0.20x1_12x4.18x1_6x1_10x4_11x4.0x0.12x4.19x1.12x4.1x1.0x0.";//order and number of parts to pick, start with step 0
    public int CurrentStep = 0;//main source of the current assembly step
    GameObject CursorFocus; //to access the WorldCursor skript

    // Start is called before the first frame update
    void Start()
    {
        //find CursorFocus in hierarchy
        CursorFocus = GameObject.Find("/AssemblyDirector/CursorFocus");
        

        //clear folder at start
        clearfolder();
       
    }

    //resets all values and prepares  for new run
    public void Restart()
    {
        clearfolder();
        CurrentStep = 0;
        CursorFocus.SetActive(false);
    }



    //clears the folder where the data is stored
    void clearfolder()
    {
        //check if app is running in editor or HL2 and get datapath 
        //clears old data and creates folder if not existend
        if (Application.isEditor)
        {
            if (Directory.Exists(Application.dataPath + "/Daten~"))
            {
                Directory.Delete(Application.dataPath + "/Daten~", true);
            }
            Directory.CreateDirectory(Application.dataPath + "/Daten~");
        }
        else
        {
            //if running on HL
            if (Directory.Exists(Application.persistentDataPath + "/Daten~"))
            {
                Directory.Delete(Application.persistentDataPath + "/Daten~", true);
            }
            Directory.CreateDirectory(Application.persistentDataPath + "/Daten~");
        }
       
    }



    // Update is called once per frame
    void Update()
    {

        //(re)-arms the CursorFocus for recording at step 0 (CursorFocus will start recording at step 1 (see WorldCursor skript))
        if (CurrentStep == 0 && (CursorFocus.activeSelf == false))
        {     
            CursorFocus.SetActive(true);
        }

        
    }


     
    public void PageChangeEventLog()
    {
        //get nr of actual assembly steps (step 0 and step after last assembly step not included)
        int ValidSteps = ZoneActivity.Split('.').Length - 3;
        //Debug.Log("There are " + ValidSteps + "valid steps");

        
        if (CurrentStep > (ValidSteps+1)) {
            //Debug.Log("over Max");
        }
        else//if CurrentStep is under ValidSteps+1
        {
            //if assembly is done save run to folder
            if (CurrentStep == (ValidSteps + 1))
            {
                CursorFocus.SetActive(false);
                GetComponent<PageChangeEventLog>().moveRunToFolder();
            }
            else
            {
                if (CurrentStep > (ValidSteps + 1)) {
                    //Debug.Log("over Max");
                }
                else
                {
                    //log time and step
                    GetComponent<PageChangeEventLog>().PageChangeLogTime();
                }
            }
        }
    }




    //used to addvance the assembly
    public void StepNext()
    {

        //while the assembly has not reached its end go to next step and log the time 
        if (CurrentStep < (ZoneActivity.Split("."[0]).Length)-2)
        {

            GetComponent<PageChangeEventLog>().PageChangeLogTime();
            CurrentStep++;
            Debug.Log("AssemblyDirector: " + CurrentStep);
            PageChangeEventLog();


        }
        else//if last step(step after last assembly step) is reached display Log
        {
            int l = (ZoneActivity.Split("."[0]).Length) - 2;
            Debug.Log("Last Step (" + l +  ") reached");
        }

    }



    //not in use atm
    public void StepBack()
    {
        if (CurrentStep > 0)
        {
            CurrentStep--;
           
        }
        else
        {
            Debug.Log("First Step reached");
        }

    }



    public string get_ZoneActivity()
    {
        return ZoneActivity;
    }

    public Material get_HighlightZone()
    {
        return HighlightZone;
    }

    public int get_CurrentStep()
    {
        return CurrentStep;
    }


}
