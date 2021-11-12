using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;




public class PageChangeEventLog : MonoBehaviour
{

    private string path = "";
    private string storage_path = "";
    private bool running_on_PC = false;
    public Text LogOutput;
    GameObject AssemblyDirector;


    //logs the switching to another step
    public void PageChangeLogTime()
    {
        //Get Page Name
        AssemblyDirector = GameObject.Find("/AssemblyDirector");
        
        

        //Get name of Element that caused the call
        var ControlElementName = gameObject.name;

        //Get Timestamp
        var timey = System.DateTime.Now;
        var TimeNow = timey.ToString();
        if (Application.isEditor)
        {
            TimeNow = System.DateTime.Now.ToString("G", CultureInfo.CreateSpecificCulture("en-us"));
            //Console.WriteLine(TimeNow.ToString("G",CultureInfo.CreateSpecificCulture("en-us")));
        }

        //find direction of command
        string direction = "none";

        if (ControlElementName == "Next" || ControlElementName == "SpeechHandlerNext")
        {
            direction = "Next";
        }
        if (ControlElementName == "Back" || ControlElementName == "SpeechHandlerBack")
        {
            direction = "Back";
        }


        //get page name as string
        int PageNr = AssemblyDirector.GetComponent<AssemblyDirector>().get_CurrentStep();
       


        //get nr of next page as int
        int nextPageNr = PageNr+1;

       
         


        using (StreamWriter sw = File.AppendText(path))
        {

            sw.WriteLine(ControlElementName + "," + PageNr + "," + TimeNow + "," + direction + "," + nextPageNr);

        }

      


        //---




    }//end PageChangeLogTime

    //deletes the old log file
    public void ResetPageChangeLog()
    {

       

        // delete old log
        string content = "NameOfButtonThatCalledThisPage,WhatWasThePreviousPage,TimeNow,Direction_ATM_SameAsNameOfButtonThatCalledThisPage,PageNow\n";
        File.WriteAllText(path, content);

        //create log and log event

        PageChangeLogTime();


    }//end ResetPageChangeLog

    //saves all data in "Data~" to its own folder
    public void moveRunToFolder()
    {
        PageChangeLogTime();

        //Alle Daten aus Daten~ in eigenen Ordner verschieben

        string newFolderPath = "";
        string dataPath = "";
        var TimeNow = System.DateTime.Now;

        //where and under what name to save the log
        var Logname = "/" + TimeNow.Year + "-" + TimeNow.Month + "-" + TimeNow.Day + "_" + TimeNow.Hour + "-" + TimeNow.Minute + "-" + TimeNow.Second;


        if (Application.isEditor)
        {
            dataPath = Application.dataPath + "/Daten~";
            newFolderPath = Application.dataPath + Logname;  //       
        }
        else
        {
            dataPath = Application.persistentDataPath + "/Daten~";
            newFolderPath = Application.persistentDataPath + Logname;//
            //newFolderPath = Environment.SpecialFolder.MyPictures.
        }




        //CreateFolder(Logname);
        Directory.Move(dataPath, newFolderPath);



    }



    //used to get times and display log file in useable form
    //no longer in use
    public void DisplayLog()
    {
        StreamReader inp_stm = new StreamReader(path);

        string buffer = "";
        string[] ElementNameA = new string[255];
        string[] PageNrA = new string[255];
        string[] DateA = new string[255];
        string[] DirA = new string[255];
        string[] NextPageNr = new string[255];
        int counter = 0;
        int maxPageNr = -1;

        //load log data into arrays
        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();

            //split line by "," 
            var LogData = inp_ln.Split(","[0]);
            ElementNameA[counter] = LogData[0];
            PageNrA[counter] = LogData[1];
            DateA[counter] = LogData[2];
            DirA[counter] = LogData[3];
            NextPageNr[counter] = LogData[4];
            if (int.Parse(LogData[1]) > maxPageNr)
            {
                maxPageNr = int.Parse(LogData[1]);
            }

            counter++;

            // buffer = buffer + LogData[0] + "," + LogData[1] + "," + LogData[2] + "," + LogData[3] + "," + LogData[4] + "\n";


        }
        // buffer = buffer + "maxPage:" + maxPageNr;
        //close file
        inp_stm.Close();

        buffer = "The Assembly guide was startet at " + DateA[0] + "\n";


        int[] totalPageCall = new int[255];
        System.TimeSpan[] totalPageTime = new System.TimeSpan[255];


        for (var i = 1; i < counter; i++)
        {
            System.DateTime timeStart = System.DateTime.Parse(DateA[i - 1]);
            System.DateTime timeEnd = System.DateTime.Parse(DateA[i]);
            System.TimeSpan timeOpen = timeEnd - timeStart;
            string thisPageNr = PageNrA[i];
            int thisPageNrInt = int.Parse(thisPageNr);
            totalPageCall[thisPageNrInt] = totalPageCall[thisPageNrInt] + 1;
            totalPageTime[thisPageNrInt] = totalPageTime[thisPageNrInt] + timeOpen;


            buffer = buffer + "Page " + PageNrA[i] + " was open for a time of " + timeOpen + " and was opened by " + ElementNameA[i - 1] + " on Page " + PageNrA[i - 1] + "\n";
        }

        buffer = buffer + "\n";


        //write all page logs into buffer
        for (var i = 1; i <= maxPageNr; i++)
        {
            buffer = buffer + "The Page " + i + " was called " + totalPageCall[i] + " times" + " for a total time of " + totalPageTime[i] + "\n";
        }



        LogOutput.text = buffer;
        inp_stm.Close();


    }//end DisplayLog


    //no longer in use
    public void SaveLog()
    {
        //Get Timestamp
        var TimeNow = System.DateTime.Now;
        var Sec = TimeNow.Second;
        var Min = TimeNow.Minute;
        var Hour = TimeNow.Hour;
        var Day = TimeNow.Day;
        var Month = TimeNow.Month;
        var Year = TimeNow.Year;

        //where and under what name to save the log
        var Logname = storage_path + Hour + "_" + Min + "_" + Sec + "  (" + Day + "." + Month + "." + Year + ").txt";

        System.IO.File.Move(path, @Logname);
        moveRunToFolder();

    }



    // Start is called before the first frame update
    void Start()
    {
        if (Application.isEditor)
        {
            running_on_PC = true;
        }
        else
        {
            running_on_PC = false;
        }


        if (running_on_PC == true)
        {
            path = Application.dataPath + "/Daten~/Page_Log.txt";
            storage_path = Application.dataPath + "/Daten~/Page_Log___";
        }
        else
        {
            //if running on HL
            path = Application.persistentDataPath + "/Daten~/Page_Log.txt";
            storage_path = Application.persistentDataPath + "/Daten~/Page_Log___";
        }

        CreateFolder("Daten~");
        CreateFolder("Messungen");



    }


    //no longer in use
    public void restart_run()
    {

        if (Application.isEditor)
        {
            running_on_PC = true;
        }
        else
        {
            running_on_PC = false;
        }


        if (running_on_PC == true)
        {
            path = Application.dataPath + "/Daten~/Page_Log.txt";
            storage_path = Application.dataPath + "/Daten~/Page_Log___";
        }
        else
        {
            //if running on HL
            path = Application.persistentDataPath + "/Daten~/Page_Log.txt";
            storage_path = Application.persistentDataPath + "/Daten~/Page_Log___";
        }

        CreateFolder("Daten~");
        CreateFolder("Messungen");

    }

    //create Folder if it not exists
    void CreateFolder(string FolderName)
    {

        if (running_on_PC == true)
        {
            string directory = Application.dataPath + "/" + FolderName;
            // Überprüfe, ob Ordner existiert
            if (!Directory.Exists(directory))
            {
                //Wenn nicht, erstelle ihn
                Directory.CreateDirectory(directory);
            }
        }
        else
        {
            string directory = Application.persistentDataPath + "/" + FolderName;
            // Überprüfe, ob Ordner existiert
            if (!Directory.Exists(directory))
            {
                //Wenn nicht, erstelle ihn
                Directory.CreateDirectory(directory);
            }
        }




    }



    // Update is called once per frame
    void Update()
    {

    }
}
