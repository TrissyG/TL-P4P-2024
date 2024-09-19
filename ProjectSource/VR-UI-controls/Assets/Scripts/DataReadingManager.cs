using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataReadingManager : MonoBehaviour
{
    public static List<PresetData> ReadCSV(string filePath)
    {
        List<PresetData> presetDataList = new List<PresetData>();

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            // Skip the header line
            sr.ReadLine();
            while ((line = sr.ReadLine()) != null)
            {
                string[] values = line.Split(',');
                PresetData data = new PresetData
                {
                    Index = int.Parse(values[0]),
                    RadioVisibility = values[1],
                    RadioPosition = int.Parse(values[2]),
                    Radius = float.Parse(values[3]),
                    Inclination = float.Parse(values[4])
                };
                presetDataList.Add(data);
            }
        }

        return presetDataList;
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