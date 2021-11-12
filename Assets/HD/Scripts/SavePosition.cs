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
public class SavePosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //saves the position,rotation and localScale of object(and child) this script is attached to to "Daten~"
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
            sw.WriteLine(pos.ToString("F4") + " " + dir.ToString("F4") + " " + size.ToString("F4") + "" + sizeChild.ToString("F4"));
        }

    }

}
