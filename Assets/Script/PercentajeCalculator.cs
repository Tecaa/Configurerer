using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Xml.Serialization;
using System.IO;

public static class PercentajeCalculator {
    private static string Filename = "Angles";
    private static Dictionary<Movement, SortedList<float, int>> angulos;


    static PercentajeCalculator()
    {
        Debug.Log(ReadAnglesFile());
    }

    static public float GetPercentage2(float angle, Movement movement)
    {
        if (angulos.ContainsKey(movement))
        {
            SortedList<float, int> angs = angulos[movement];
            KeyValuePair<float, int>? lastPair = null;
            foreach (KeyValuePair<float, int> pair in angs)
            {
                if (!lastPair.HasValue)
                {
                    lastPair = pair;
                    continue;
                }
                if (angle <= Math.Max(pair.Value, lastPair.Value.Value) && angle >= Math.Min(pair.Value, lastPair.Value.Value))
                {
                    return lastPair.Value.Key + (angle - lastPair.Value.Value) * (pair.Key - lastPair.Value.Key) / (pair.Value - lastPair.Value.Value);
                }
                lastPair = pair;
            }
            return 1;
        }
        else
            return angle;
    }

    private static bool ReadAnglesFile()
    {
        try
        {
            TextAsset anglesData = Resources.Load(Filename) as TextAsset;
            angulos = (Dictionary<Movement, SortedList<float, int>>)JsonConvert.DeserializeObject(anglesData.text, typeof(Dictionary<Movement, SortedList<float, int>>));

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        return true;
    }
}

