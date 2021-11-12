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

public class CalibrationMaster : MonoBehaviour
{
    public int CurrentCalibrationStep = 0;//which zone is active 
    bool Calibration = true;// if in Zone setup or not

    //Materials do display calibration state
    public Material Material_Cal;
    public Material Material_Run;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if assembly started stop calibration
        if (GameObject.Find("/AssemblyDirector").GetComponent<AssemblyDirector>().get_CurrentStep() > 0)
        {
            Calibration = false;
        }
    }


    public Material get_Material_Cal()
    {
        return Material_Cal;
    }

    public Material get_Material_Run()
    {
        return Material_Run;
    }
    
    public void ToggleCalibration()
    {
        if (Calibration)
        {
            Calibration = false;
        }
        else
        {
            Calibration = true;
        }
    }

    public bool get_Calibration()
    {
        return Calibration;
    }

    public void CurrentCalibrationStep_INC()
    {
        CurrentCalibrationStep++;
    }

    public void CurrentCalibrationStep_DEC()
    {
        if(CurrentCalibrationStep > 0)
        {
            CurrentCalibrationStep--;
        }
        else
        {
            Debug.Log("Step 0 reached");

        }
    }

    public int get_CurrentCalibrationStep()
    {
        return CurrentCalibrationStep;
    }

}
