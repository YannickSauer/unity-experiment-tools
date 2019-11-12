using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;
using System.Text;
using ViveSR.anipal.Eye;

public class SaveTracking : MonoBehaviour
{
    public enum EyeTrackingOptions
    {
        none, left, right, combined, all
    }
    [Header("Output Settings")]
    public string trackingFile = "tracking.csv"; // maybe change to automatically get subject dependend name? // application.getpath
    public string delimiter = ";";
    [Header("Eye Tracking Settings")]
    public EyeTrackingOptions eyeTrackingOptions;
    [Header("Object Tracking")]
    public GameObject trackedGameobject;
    public bool savePosition = true;
    public bool saveOrientation = true;
    Queue trackingDataQueue = new Queue();
    static string msgBuffer = "";
    VerboseData eyeData;
    void Start()
    {
        // TODO: check if csv file already exists and change filename if necessary (e.g. _01...)
        WriteHeader();
        InvokeRepeating("Save", 0.0f, 1.0f); // save data to file every second
    }
    void Update()
    {
        QueueTrackingData();
    }
        public static void Msg(string msg)
    {
        msgBuffer = msg;    
    }

    // TODO Use string builder or string.join
    // should be much more effiction: https://stackoverflow.com/questions/21078/most-efficient-way-to-concatenate-strings
    void QueueTrackingData()
    {   
        StringBuilder datasetLine = new StringBuilder(300); // adjust capacity to your needs
        // timestamp: use time at beginning of frame. What makes sense here? Use eye tracker timestamp?
        datasetLine.Append(Time.time.ToString("F5") + delimiter); // convert with 5 digits after decimal point
        // eyetracking data
        switch (eyeTrackingOptions)
        {
            case EyeTrackingOptions.left:
                SRanipal_Eye.GetVerboseData(out eyeData);
                // write left eye origin
                datasetLine.Append(eyeData.left.gaze_origin_mm.ToString("F3") + delimiter);
                // write left eye normalied gaze direction
                datasetLine.Append(eyeData.left.gaze_direction_normalized.ToString("F3") + delimiter);
                break;
            case EyeTrackingOptions.right:
                SRanipal_Eye.GetVerboseData(out eyeData);
                // write right eye origin
                datasetLine.Append(eyeData.right.gaze_origin_mm.ToString("F3") + delimiter);
                // write right eye normalied gaze direction
                datasetLine.Append(eyeData.right.gaze_direction_normalized.ToString("F3") + delimiter);
                break;
            case EyeTrackingOptions.combined:
                SRanipal_Eye.GetVerboseData(out eyeData);
                // write combined eye origin
                datasetLine.Append(eyeData.combined.eye_data.gaze_origin_mm.ToString("F3") + delimiter);
                // write combined eye normalied gaze direction
                datasetLine.Append(eyeData.combined.eye_data.gaze_direction_normalized.ToString("F3") + delimiter);
                break;
            case EyeTrackingOptions.all:
                SRanipal_Eye.GetVerboseData(out eyeData);
                // write left eye origin
                datasetLine.Append(eyeData.left.gaze_origin_mm.ToString("F3") + delimiter);
                // write left eye normalied gaze direction
                datasetLine.Append(eyeData.left.gaze_direction_normalized.ToString("F3") + delimiter);
                // write right eye origin
                datasetLine.Append(eyeData.right.gaze_origin_mm.ToString("F3") + delimiter);
                // write right eye normalied gaze direction
                datasetLine.Append(eyeData.right.gaze_direction_normalized.ToString("F3") + delimiter);
                // write combined eye origin
                datasetLine.Append(eyeData.combined.eye_data.gaze_origin_mm.ToString("F3") + delimiter);
                // write combined eye normalied gaze direction
                datasetLine.Append(eyeData.combined.eye_data.gaze_direction_normalized.ToString("F3") + delimiter);
                break;
        }
        // tracked game object's position
        if (savePosition)
            datasetLine.Append(trackedGameobject.transform.position.ToString("F3") + delimiter);
        // tracked game object's orientation
        if (saveOrientation)
            datasetLine.Append(trackedGameobject.transform.rotation.ToString("F3") + delimiter);
        // buffered message
        if (!String.IsNullOrEmpty(msgBuffer))
        {
            datasetLine.Append(msgBuffer + delimiter);
            msgBuffer = "";
        }
        trackingDataQueue.Enqueue(datasetLine.ToString());
    }

    void Save()
    {
        var thread = new Thread(WriteTrackingData);
        thread.Start();
    }

    void WriteTrackingData()
    {
        Debug.Log("Writing Data");
        StreamWriter sw = new StreamWriter(trackingFile, true); //true for append
        string datasetLine;
        // dequeue trackingDataQueue until empty
        while (trackingDataQueue.Count > 0)
        {
            datasetLine = trackingDataQueue.Dequeue().ToString();
            sw.WriteLine(datasetLine); // write to file
        }
        sw.Close(); // close file
        Debug.Log("End Writing Data");
    }
    void WriteHeader()
    {
        StreamWriter sw = new StreamWriter(trackingFile);
        string header = "Timestamp" + delimiter;
        switch (eyeTrackingOptions)
        {
            case EyeTrackingOptions.left:
                header += "(left_eye_origin.x,left_eye_origin.y,left_eye_origin.z)" + delimiter + "(left_eye_gaze.x,left_eye_gaze.y,left_eye_gaze.z)" + delimiter;
                break;
            case EyeTrackingOptions.right:
                header += "(right_eye_origin.x,right_eye_origin.y,right_eye_origin.z)" + delimiter + "(right_eye_gaze.x,right_eye_gaze.y,right_eye_gaze.z)" + delimiter;
                break;
            case EyeTrackingOptions.combined:
                header += "(combined_eye_origin.x,combined_eye_origin.y,combined_eye_origin.z)" + delimiter + "(combined_eye_gaze.x,combined_eye_gaze.y,combined_eye_gaze.z)" + delimiter;
                break;
            case EyeTrackingOptions.all:
                header += "(left_eye_origin.x,left_eye_origin.y,left_eye_origin.z)" + delimiter + "(left_eye_gaze.x,left_eye_gaze.y,left_eye_gaze.z)" + delimiter;
                header += "(right_eye_origin.x,right_eye_origin.y,right_eye_origin.z)" + delimiter + "(right_eye_gaze.x,right_eye_gaze.y,right_eye_gaze.z)" + delimiter;
                header += "(combined_eye_origin.x,combined_eye_origin.y,combined_eye_origin.z)" + delimiter + "(combined_eye_gaze.x,combined_eye_gaze.y,combined_eye_gaze.z)" + delimiter;
                break;
        }
        if(savePosition)
            header += "(" + trackedGameobject.name + "_position.x" + ",";
            header += trackedGameobject.name + "_position.y" + ",";
            header += trackedGameobject.name + "_position.z" + ")" + delimiter;
        if(saveOrientation)
            header += "(" + trackedGameobject.name + "_rotation.w" + ",";
            header += trackedGameobject.name + "_rotation.x" + ",";
            header += trackedGameobject.name + "_rotation.y" + ",";
            header += trackedGameobject.name + "_rotation.z" + ")" + delimiter;
        header += "messages" + delimiter;
        sw.WriteLine(header);
        sw.Close();
    }
}