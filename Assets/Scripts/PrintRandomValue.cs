using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintRandomValue : MonoBehaviour
{
    public List<WeightedValue> weightedValues;

    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    string randomValue = GetRandomValue(weightedValues);
        //    Debug.Log(randomValue ?? "No entries found");
        //}
    }

    string GetRandomValue(List<WeightedValue> weightedValueList)
    {
        string output = null;

        //Getting a random weight value
        var totalWeight = 0;
        foreach (var entry in weightedValueList)
        {
            totalWeight += entry.weight;
        }
        var rndWeightValue = Random.Range(1, totalWeight + 1);

        //Checking where random weight value falls
        var processedWeight = 0;
        foreach (var entry in weightedValueList)
        {
            processedWeight += entry.weight;
            if (rndWeightValue <= processedWeight)
            {
                output = entry.value;
                break;
            }
        }

        return output;
    }
}