using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTracking : MonoBehaviour
{
    public bool saveEyeTracking = true;
    public bool saveHeadTracking = true;
    // Start is called before the first frame update
    void Start()
    {
        //TODO
    }

    // Update is called once per frame
    void Update()
    {
        //TODO
    }
    // save array as a column to csv file
    void SaveArrayToCSV(float[] arrayToSave, string csvPath, string header)
    {
        if (!File.Exists(csvPath))
        {
            var outputFile = File.CreateText(csvPath);
            outputFile.WriteLine(header);
            for (int i = 0; i < arrayToSave.Length; i++)
            {
                outputFile.WriteLine(arrayToSave[i].ToString());
            }
            outputFile.Close();
        }
        else // append to file a new column
        {
            var csv = File.ReadLines(csvPath) // not AllLines
                .Select((line, index) => index == 0
                ? line + ";" + header
                : line + ";" + arrayToSave[index - 1].ToString())
                .ToList(); // we should write into the same file, that´s why we materialize
            File.WriteAllLines(csvPath, csv);
        }
    }
    
    //Same function for int
    void SaveArrayToCSV(int[] arrayToSave, string csvPath, string header)
    {
        if (!File.Exists(csvPath))
        {
            var outputFile = File.CreateText(csvPath);
            outputFile.WriteLine(header);
            for (int i = 0; i < arrayToSave.Length; i++)
            {
                outputFile.WriteLine(arrayToSave[i].ToString());
            }
            outputFile.Close();
        }
        else // append to file a new column
        {
            var csv = File.ReadLines(csvPath) // not AllLines
                .Select((line, index) => index == 0
                ? line + ";" + header
                : line + ";" + arrayToSave[index - 1].ToString())
                .ToList(); // we should write into the same file, that´s why we materialize
            File.WriteAllLines(csvPath, csv);
        }
    }

}
