using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentPreperation : MonoBehaviour
{
    public static void ShuffleArray(float[] array) // Fisher–Yates shuffle
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int r =  Random.Range(0, i+1);
            // swap array[r] and array[i]
            float temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }
    public static void ShuffleArray(int[] array) // Fisher–Yates shuffle
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int r =  Random.Range(0, i+1);
            // swap array[r] and array[i]
            int temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }
}