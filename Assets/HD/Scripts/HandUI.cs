using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{

    bool StateOfDisplay = false;
    GameObject Menu;
    GameObject DefaultDisplay;


    // Start is called before the first frame update
    void Start()
    {
        //get the two objects for the "wrist watch" interface
        Menu = GameObject.Find("/HandUI/HandFollow/Menu");
        DefaultDisplay = GameObject.Find("/HandUI/HandFollow/DefaultDisplay");     
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //toggle between assembly-step-display and the "calibration- & reset" interface
    public void ToggleInterface()
    {
        Debug.Log("Interface toggle...");
      

        if (StateOfDisplay)
        {
            StateOfDisplay = false;
            Menu.SetActive(false);
            DefaultDisplay.SetActive(true);
        }
        else
        {
            StateOfDisplay = true;
            Menu.SetActive(true);
            DefaultDisplay.SetActive(false);
        }
        

    }




    public void Set_InterfaceRun()
    {           
            Menu.SetActive(false);
            DefaultDisplay.SetActive(true);
    }





}
