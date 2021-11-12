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


public class AnimationMaster : MonoBehaviour
{


    GameObject AssemblyDirector;//used to access the Assembly Director object in the Hierarchy
    int CurrentStep = -1; //keeps track of the Current animation Step


    //---Variables for the wrist watch----- 
    bool inteface = true;//Toggles the "wrist watch" interface
    Text OutputTimeAssemblyTotal;//used to access the "wrist watch" step-indicator
    //-------------------------------------


    //----Variables for the animation------ 
    public float AnimationStep = 0.0f ;
    float ElapsedTime = 0f;
    int PauseSteps = 30;//how long the pause for the animation should be
    bool Pausing = false;
    //-------------------------------------

    //order of objects to animate and display (us "Order Of Activity.xlsx" to generate this string and paste it into "Animation Step" via the Inspector window) 
    public string OrderOfActivity = //for slave                    
    "none$Up$0.0$," +               //0
    "Feet$Up$0.0$NutsFeet_Bottom$Up$0.3$Hint4$Up$0.0$," +//1
    "Feet$Up$0.0$NutsFeet_Bottom$Up$0.0$Base$Up$0.2$NutsFeet_Top$Up$0.3$," +//2
    "WireAssembly1$Up$0.0$Hint5$Up$0.0$," +//3
    "WireAssembly1x1$Down$0.0$," +//4
    "WireAssembly2$Up$0.0$," +//5
    "WireAssembly3$Up$0.0$," +//6
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$," +//7
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$," +//8
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.3$," +//9
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.2$Hint1$Up$0.0$," +//10
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.25$Cover$Up$0.35$ScrewSideRight$Right$0.25$ScrewSideLeft$Left$0.25$," +//11
    "BackSideConnectorExtra$Forward$0.10$NutsBackSideConnectorExtra$Forward$0.20$BackSideExtra$StaticBack$0.0$ScrewBackSideConnectorExtra$Back$0.25$," +//12
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.25$Cover$Up$0.35$ScrewSideRight$Right$0.25$ScrewSideLeft$Left$0.25$BackSideConnector$StaticBack$0.3$ScrewBackSideConnector$StaticBack$0.3$BackSide$StaticBack$0.30$Indecator13$Forward$0.115$Hint2$Up$0.0$," +//13
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.25$Cover$Up$0.35$ScrewSideRight$Right$0.25$ScrewSideLeft$Left$0.25$BackSideConnector$Back$0.3$ScrewBackSideConnector$Back$0.3$NutsBackSideConnector$Back$0.30$ScrewFrontBackSide$Back$0.4$BackSide$Back$0.3$," +//14
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.25$Cover$Up$0.35$ScrewSideRight$Right$0.25$ScrewSideLeft$Left$0.25$BackSideConnector$Back$0.15$ScrewBackSideConnector$Back$0.25$NutsBackSideConnector$Back$0.10$ScrewFrontBackSide$Forward$0.2$FrontSide$StaticForward$0.25$BackSide$Back$0.2$Indecator15$Back$0.115$Hint3$Up$0.0$," +//15 
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.25$Cover$Up$0.35$ScrewSideRight$Right$0.25$ScrewSideLeft$Left$0.25$BackSideConnector$Back$0.15$ScrewBackSideConnector$Back$0.25$NutsBackSideConnector$Back$0.10$ScrewFrontBackSide$Forward$0.2$FrontSide$Forward$0.15$ScrewFrontFrontSide$Forward$0.22$BackSide$Back$0.2$," +//16
    "WireAssembly4$Up$0.0$Feet$Up$0.0$NutsFeet_Bottom$Down$0.0$Base$Down$0.2$NutsFeet_Top$Down$0.3$Motor$Back$0.3$ScrewMotor$Forward$0.35$Drum$Forward$0.25$NutDrum$Forward$0.75$WireClip$Up$0.25$Cover$Up$0.35$ScrewSideRight$Right$0.25$ScrewSideLeft$Left$0.25$BackSideConnector$Back$0.15$ScrewBackSideConnector$Back$0.25$NutsBackSideConnector$Back$0.10$ScrewFrontBackSide$Forward$0.2$FrontSide$Forward$0.15$ScrewFrontFrontSide$Up$0.0$Blende$Up$0.2$BackSide$Back$0.2$," +//17
    "RestartPage$Up$0.0,";

    



    // Start is called before the first frame update
    void Start()
    {
        //setup 
        //Debug.Log("There are " + (get_OrderOfActivity().Split(","[0]).Length-2) + " steps and step 0");

        //Find AssemblyDirector and "wrist watch" step-indicator
        AssemblyDirector = GameObject.Find("/AssemblyDirector");
        OutputTimeAssemblyTotal = GameObject.Find("UI_PageInfo").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        //Check if CurrentStep in AssemblyDirector changed. If yes then update animation
        if (CurrentStep != AssemblyDirector.GetComponent<AssemblyDirector>().get_CurrentStep()) {
            CurrentStep = AssemblyDirector.GetComponent<AssemblyDirector>().get_CurrentStep();
         
            UpdateUI();
            AnimationStep = 1;
            PauseSteps = 0;
        }

            UpdateAnimation();
    }

    //used to animate the new parts. AnimationStep is accessed by "AnimationSlave" to determine the position for the animation
    void UpdateAnimation()
    {
        ElapsedTime += Time.deltaTime;

        if (ElapsedTime >= 0.05f)
        {
            ElapsedTime = ElapsedTime % 0.05f;


            //for pausing the animation while the part is at the default position
            if (Pausing)
            {
                PauseSteps--;
                if (PauseSteps <= 0)
                {
                    Pausing = false;
                    AnimationStep = 1;
                }

            }


            
            if (Pausing == false)
            {
                AnimationStep = AnimationStep - 0.025f;
            }
            // of animation looped then pause
            if (AnimationStep < 0f && Pausing == false)
            {
                Pausing = true;
                PauseSteps = 30;
                // Starte pause AnimationStep = 1;
            }

        }
    }

    void UpdateUI()
    {
        if(CurrentStep == 1)
        {
            //switch the wrist watch interface to "step display mode"
            GameObject.Find("HandUI").GetComponent<HandUI>().Set_InterfaceRun();
        }
        //Display current step on wrist watch interface
        OutputTimeAssemblyTotal.text = (CurrentStep + " of " + (get_OrderOfActivity().Split(","[0]).Length - 2));
    }





    //getter methods 

    public float get_AnimationStep()
    {
        return AnimationStep;
    }
    
    public string get_OrderOfActivity()
    {
        return OrderOfActivity;
    }

    public int get_CurrentStep()
    {
        return CurrentStep;
    }

   
}
