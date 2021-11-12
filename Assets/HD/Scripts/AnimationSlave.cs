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
 


public class AnimationSlave : MonoBehaviour
{
    CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();// to solve problems with "," and "." separators
    GameObject Master;// to access AnimationMaster
    string MyName;//Name of 3D object this is attached to
    int CurrentStep = -1;//to keep track of the current animation step
    int[] ActivityIndex;//list of when this part is displayed   
    int FirstAppearance = -1;//at what step a part is first appearing & animated 
    int StaticAppearance = -1;//used to shift the animation one step backwards and keeps the part at max animation distance
    Vector3 MyAnimationVector;//The position over which the part moves 
    Vector3 DafaultPosition;//to save the default position
    Vector3 Static_Pos;//same as MyAnimationVector but for the step after the first appearence




    // Start is called before the first frame update
    void Start()
    {
        //get name, DafaultPosition and AnimationMaster
        MyName = transform.name;
        DafaultPosition = transform.localPosition;
        Master = GameObject.Find("3D_Model");

        //get ActivityIndex and fill it
        ActivityIndex = new int[ Master.GetComponent<AnimationMaster>().get_OrderOfActivity().Split(","[0]).Length -1];
        UpdateStateList();
    }


    // Update is called once per frame
    void Update()
    {
        // CurrentStep in Animation Master then update state. 
        if (CurrentStep != Master.GetComponent<AnimationMaster>().get_CurrentStep())
        {
            CurrentStep = Master.GetComponent<AnimationMaster>().get_CurrentStep();
            

            UpdateState();
            transform.localPosition = DafaultPosition;//at beginning of every step move part to default position

        }


        Animation();

    }

   void Animation()
    {
        //only animate if CurrentStep is at FirstAppearance 
        if (CurrentStep == FirstAppearance)
        {
            //animate according to AnimationMaster's AnimationStep
            transform.localPosition = DafaultPosition + (MyAnimationVector * Master.GetComponent<AnimationMaster>().get_AnimationStep());
        }
        if(CurrentStep == StaticAppearance)
        {
            //hold part at max animation distance
            transform.localPosition = Static_Pos;
        }

    
    }


    //go through OrderOfActivity from master and determine at what step a part is displayed
    void UpdateStateList()
    {
        //Reset FirstAppearance
        FirstAppearance = -1;

        //get OrderOfActivity from Master
        string OrderOfActivity = Master.GetComponent<AnimationMaster>().get_OrderOfActivity();


        //go through OrderOfActivity 
        for (int j = 0; j < (Master.GetComponent<AnimationMaster>().get_OrderOfActivity().Split(","[0]).Length - 1); j++)
        {
            //Clean list
            for (int i = 0; i < (OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0]).Length; i++)
            {
                ActivityIndex[j] = 0;
            }


            //Set on-states in list
            for (int i = 0; i < (OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0]).Length; i++)
            {
                if ((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[i] == MyName)//index in line
                {
                    ActivityIndex[j] = 1;
                    if (FirstAppearance == -1)
                    {
                        FirstAppearance = j;//page

                        ci.NumberFormat.CurrencyDecimalSeparator = ".";

                        switch ((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[i+1])
                        {

                            case "Up":
                                MyAnimationVector = new Vector3(0f, 1f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                break;
                            case "Down":
                                MyAnimationVector = new Vector3(0f, -1f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                break;
                            case "Left":
                                MyAnimationVector = new Vector3(-1f, 0f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                break;
                            case "Right":
                                MyAnimationVector = new Vector3(1f, 0f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                break;
                            case "Back":
                                MyAnimationVector = new Vector3(0f, 0f, 1f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                break;
                            case "Forward":
                                MyAnimationVector = new Vector3(0f, 0f, -1f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                break;
                            case "Static":
                                Static_Pos = DafaultPosition + new Vector3(1f, 1f, 1f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                FirstAppearance = -1;
                                break;

                            case "StaticUp":
                                Static_Pos = DafaultPosition + new Vector3(0f, 1f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                FirstAppearance = -1;
                                StaticAppearance = j;
                                break;
                            case "StaticDown":
                                Static_Pos = DafaultPosition + new Vector3(0f, -1f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                FirstAppearance = -1;
                                StaticAppearance = j;
                                break;
                            case "StaticLeft":
                                Static_Pos = DafaultPosition + new Vector3(-1f, 0f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                FirstAppearance = -1;
                                StaticAppearance = j;
                                break;
                            case "StaticRight":
                                Static_Pos = DafaultPosition + new Vector3(1f, 0f, 0f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                FirstAppearance = -1;
                                StaticAppearance = j;
                                break;
                            case "StaticBack":
                                Static_Pos = DafaultPosition + new Vector3(0f, 0f, 1f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                FirstAppearance = -1;
                                StaticAppearance = j;
                                break;
                            case "StaticForward":
                                Static_Pos = DafaultPosition + new Vector3(0f, 0f, -1f) * float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci);
                                FirstAppearance = -1;
                                StaticAppearance = j;
                                break;
                            default:
                                Debug.Log("Incorrect Animation Parameter");
                                MyAnimationVector = new Vector3(0f, 0f, 0f);
                                break;


                        }
                       // Debug.Log(MyName + " has a MyAnimationVector of " + MyAnimationVector.ToString("F4") + " and i read "+ float.Parse(((OrderOfActivity.Split(","[0])[j].Trim()).Split("$"[0])[(i + 2)]), NumberStyles.Any, ci) );
  
                    }


                }
                
            }

        }
    }


    // activate and deactivate the actuall 3D object(Child-Objects) according to the ActivityIndex
    void UpdateState()
    {
        if (ActivityIndex[CurrentStep] == 1)
        {
            foreach (Transform child in transform) child.gameObject.SetActive(true);
        }
        else
        {
            foreach (Transform child in transform) child.gameObject.SetActive(false);
        }
    }
}


       
