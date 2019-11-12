using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentPreparation
{
    // randomly permute elements of float array
    public static void RandPermute(float[] array) // Fisher–Yates shuffle
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            // swap array[r] and array[i]
            float temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }
    // randomly permute elements of int array
    public static void RandPermute(int[] array) // Fisher–Yates shuffle
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            // swap array[r] and array[i]
            int temp = array[r];
            array[r] = array[i];
            array[i] = temp;
        }
    }

    // returns a random parmutation of integers from 0 to maxIndex. (random permutation of indices)
    // This can be used in combination with 'PermuteWithInd' to shuffle muliple arrays with the 
    // same permutation (for example arrays for different trial conditions that should keep the same sampling)
    // If you have a condition which alternates between trials, then set alternateCondition to true and the
    // function will permute even with even indices and odd with odd indices
    public static int[] RandIndexPermutation(int maxIndex, bool alternateCondition)
    {
        int[] indices = new int[maxIndex];

        if (alternateCondition)
        {
            int[] tmp_odd;
            int[] tmp_even;
            tmp_odd = new int[maxIndex / 2];
            if (maxIndex % 2 == 0) // if maxIndex is even
            {
                 tmp_even = new int[maxIndex / 2];
            }
            else
            {
                tmp_even = new int[maxIndex / 2 + 1];
            }
            
            for (int i = 0; i < maxIndex / 2; i++)
            {
                tmp_even[i] = 2 * i;
                tmp_odd[i] = 2 * i + 1;
            }
            if (maxIndex % 2 == 1) // if maxIndex is odd
            {
                 tmp_even[maxIndex / 2 + 1] = maxIndex - 1;
            }
            RandPermute(tmp_even);
            RandPermute(tmp_odd);
            for (int i = 0; i < maxIndex / 2; i++)
            {
                indices[2*i] = tmp_even[i];
                indices[2*i + 1] = tmp_odd[i];
            }
            if (maxIndex % 2 == 1) // if maxIndex is odd
            {
                indices[maxIndex - 1] = tmp_even[maxIndex / 2 + 1];
            }
        }
        else
        {
            for (int i = 0; i < maxIndex; i++)
            {
                indices[i] = i;
            }
            RandPermute(indices);
        }
        foreach( var i in indices )
        {
            Debug.Log( i );
        }
        return indices;
    }
    public static int[] RandIndexPermutation(int maxIndex)
    {
        int[] indices = new int[maxIndex];
        for (int i = 0; i < maxIndex; i++)
        {
            indices[i] = i;
        }
        RandPermute(indices);
        return indices;
    }

    public static void PermuteWithInd(float[] array, int[] indices)
    {
        float[] tmp = new float[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            tmp[i] = array[indices[i]];
        }
        Array.Copy(tmp,array,tmp.Length);
    }
    public static void PermuteWithInd(int[] array, int[] indices)
    {
        int[] tmp = new int[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            tmp[i] = array[indices[i]];
        }
        Array.Copy(tmp,array,tmp.Length);
    }

    // returns an array with the elements of 'fillValues' repeated as given by repElement and repArray:
    // First repeat each element of 'fillValues' 'repElement' times
    // Then repeat the result 'repArray' times
    // Example: 
    // fillValues = [1 2 3 4]; repElement = 3; repArray = 2;
    // retuns -> [1 1 1 2 2 2 3 3 3 4 4 4 1 1 1 2 2 2 3 3 3 4 4 4]
    public static float[] FillWithSamples(float[] fillValues, int repElement, int repArray)
    {
        int length = fillValues.Length * repElement * repArray;
        float[] output = new float[length];
        int counter = 0;
        for(int outer = 0; outer < repArray; outer++)
        {
            for(int elem = 0; elem < fillValues.Length; elem++)
            {
                for(int inner = 0; inner < repElement; inner++)
                {
                    output[counter] = fillValues[elem];
                    counter++;
                }
            }
        }
        return output;
    }
    //integer vesion of the function above
    public static int[] FillWithSamples(int[] fillValues, int repElement, int repArray)
    {
        int length = fillValues.Length * repElement * repArray;
        int[] output = new int[length];
        int counter = 0;
        for(int outer = 0; outer < repArray; outer++)
        {
            for(int elem = 0; elem < fillValues.Length; elem++)
            {
                for(int inner = 0; inner < repElement; inner++)
                {
                    output[counter] = fillValues[elem];
                    counter++;
                }
            }
        }
        return output;
    }
}