using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DataReadingManager : MonoBehaviour
{
    public static List<PresetData> ReadCSV(string filePath)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);
        if (csvFile == null)
        {
            Debug.LogError("CSV file not found at path: " + filePath);
            return null;
        }

        List<PresetData> data = new List<PresetData>();
        StringReader reader = new StringReader(csvFile.text);
        string line;
        bool isFirstLine = true;
        while ((line = reader.ReadLine()) != null)
        {
            if (isFirstLine)
            {
                isFirstLine = false;
                continue; // Skip the header line
            }

            string[] values = line.Split(',');
            PresetData presetData = new PresetData
            {
                Index = int.Parse(values[0]),
                RadioVisibility = values[1],
                RadioPosition = int.Parse(values[2]),
                Radius = float.Parse(values[3]),
                Inclination = float.Parse(values[4])
            };
            data.Add(presetData);
        }
        return data;
    }
}

public class PresetData
{
    public int Index { get; set; }
    public string RadioVisibility { get; set; }
    public int RadioPosition { get; set; }
    public float Radius { get; set; }
    public float Inclination { get; set; }
}