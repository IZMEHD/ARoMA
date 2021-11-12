using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using System.IO;

public class DataManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //#################################
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
        //#################################
    }

    // Update is called once per frame
    void Update()
    {

    }
}
