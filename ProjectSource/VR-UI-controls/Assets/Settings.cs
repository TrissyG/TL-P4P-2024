using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Settings : MonoBehaviour
{
    /*
     * Settings is a singleton which persists data between scenes,
     * such as the volume set in the Spectator UI.
     */
    public static Settings Instance;

    private Dictionary<string, float> floats = new Dictionary<string, float>();
    //private Dictionary<string, int> ints = new Dictionary<string, int>();
    private Dictionary<string, bool> bools = new Dictionary<string, bool>();
    private Dictionary<string, Vector3> vectors = new Dictionary<string, Vector3>();
    private Dictionary<string, string> strings = new Dictionary<string, string>();

    private List<string> blacklistedEntries = new List<string>();

    private const string savedSettingsPath = "SavedSettings";

    // event for when settings are loaded.
    public delegate void OnLoad();
    public static event OnLoad onLoad;


    private void Awake()
    {
        if (Instance != null) {
            // Destroy this instance if another already exists
            // Ensures only one singleton exists at a time
            Destroy(gameObject);
            return;
        }

        // make this instance persistant between scenes
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // create directory to store settings
        if (!Directory.Exists(savedSettingsPath)) {
            Directory.CreateDirectory(savedSettingsPath);
        }
    }

    public bool Load(string filename)
    {
        // Load settings from a file in the saved settings folder.
        // Only overrides settings which are present in the file.
        // Returns false if file could not be found, or read.

        string path = Path.Combine(savedSettingsPath, filename + ".txt");
        if (!File.Exists(path)) {
            return false;
        }

        using (StreamReader reader = new StreamReader(path)) {
            string line;
            while ((line = reader.ReadLine()) != null) {
                string[] tokens = line.Split('=');
                // try parsing for either a vecor, float, int or bool type
                /*if (int.TryParse(tokens[1], out int intvalue)) {
                    SetValue(tokens[0], intvalue);
                } else */
                if (tokens[1].Contains(',')) {
                    string[] axes = tokens[1].Split(',');
                    float x = float.Parse(axes[0].Trim('('));
                    float y = float.Parse(axes[1]);
                    float z = float.Parse(axes[2].Trim(')'));
                    SetValue(tokens[0], new Vector3(x, y, z));
                } else if (float.TryParse(tokens[1], out float floatValue)) {
                    SetValue(tokens[0], floatValue);
                } else if (bool.TryParse(tokens[1], out bool boolValue)) {
                    SetValue(tokens[0], boolValue);
                } else if (tokens[1].Length > 0) {
                    SetValue(tokens[0], tokens[1]);
                } else {
                    // failed
                    Debug.LogError("Failed to parse line of " + path + ", content: " + line);
                    return false;
                }
            }
        }

        // invoke event to indicate new settings have been loaded
        if (onLoad != null) {
            onLoad();
        }

        return true;
    }

    public bool Save(string filename)
    {
        // Saves the current settings to file in the saved settings folder.
        // Overwrites existing file.
        // Returns true if successful.

        string path = Path.Combine(savedSettingsPath, filename + ".txt");
        // overwrite existing files (delete first)
        if (File.Exists(path)) {
            File.Delete(path);
        }
        // convert all settings, except blacklisted to string format
        List<string> lines = new List<string>();
        foreach (KeyValuePair<string, float> entry in floats) {
            if (!blacklistedEntries.Contains(entry.Key)) {
                lines.Add(entry.Key + "=" + entry.Value.ToString("0.0#########"));
            }
        }
        /*foreach (KeyValuePair<string, int> entry in ints) {
            if (!blacklistedEntries.Contains(entry.Key)) {
                lines.Add(entry.Key + "=" + entry.Value.ToString());
            }
        }*/
        foreach (KeyValuePair<string, bool> entry in bools) {
            if (!blacklistedEntries.Contains(entry.Key)) {
                lines.Add(entry.Key + "=" + entry.Value.ToString());
            }
        }

        foreach (KeyValuePair<string, Vector3> entry in vectors) {
            if (!blacklistedEntries.Contains(entry.Key)) {
                lines.Add(entry.Key + "=" + entry.Value.ToString("F6"));
            }
        }

        foreach (KeyValuePair<string, string> entry in strings) {
            if (!blacklistedEntries.Contains(entry.Key)) {
                lines.Add(entry.Key + "=" + entry.Value);
            }
        }
        // sort alphabetically
        lines.Sort();

        // write to file
        using (StreamWriter writer = new StreamWriter(path)) {
            foreach (string line in lines) {
                writer.WriteLine(line);
            }
        }

        // success
        return true;
    }

    public void SetValue(string name, float value)
    {
        // replace entry if it already exists
        if (floats.ContainsKey(name)) {
            floats.Remove(name);
            floats.Add(name, value);
        } else {
            // add new entry
            floats.Add(name, value);
        }
    }

    /*public void SetValue(string name, int value)
    {
        // replace entry if it already exists
        if (ints.ContainsKey(name)) {
            ints.Remove(name);
            ints.Add(name, value);
        } else {
            // add new entry
            ints.Add(name, value);
        }
    }*/

    public void SetValue(string name, bool value)
    {
        // replace entry if it already exists
        if (bools.ContainsKey(name)) {
            bools.Remove(name);
            bools.Add(name, value);
        } else {
            // add new entry
            bools.Add(name, value);
        }
    }

    public void SetValue(string name, Vector3 value)
    {
        // replace entry if it already exists
        if (vectors.ContainsKey(name)) {
            vectors.Remove(name);
            vectors.Add(name, value);
        } else {
            // add new entry
            vectors.Add(name, value);
        }
    }

    public void SetValue(string name, string value)
    {
        // replace entry if it already exists
        if (strings.ContainsKey(name)) {
            strings.Remove(name);
            strings.Add(name, value);
        } else {
            // add new entry
            strings.Add(name, value);
        }
    }

    public void GetValue(string name, out float value)
    {
        bool success = floats.TryGetValue(name, out value);
        if (success) {
            return;
        } else {
            throw new System.Exception("Float \"" + name + "\" not found!");
        }
    }

    /*public void GetValue(string name, out int value)
    {
        bool success = ints.TryGetValue(name, out value);
        if (success) {
            return;
        } else {
            throw new System.Exception("Int \"" + name + "\" not found!");
        }
    }*/

    public void GetValue(string name, out bool value)
    {
        bool success = bools.TryGetValue(name, out value);
        if (success) {
            return;
        } else {
            throw new System.Exception("Bool \"" + name + "\" not found!");
        }
    }

    public void GetValue(string name, out Vector3 value)
    {
        bool success = vectors.TryGetValue(name, out value);
        if (success) {
            return;
        } else {
            throw new System.Exception("Vector \"" + name + "\" not found!");
        }
    }

    public void GetValue(string name, out string value)
    {
        bool success = strings.TryGetValue(name, out value);
        if (success) {
            return;
        } else {
            throw new System.Exception("String \"" + name + "\" not found!");
        }
    }

    public bool Contains(string name)
    {
        return floats.ContainsKey(name) /*|| ints.ContainsKey(name)*/ || bools.ContainsKey(name) || vectors.ContainsKey(name) || strings.ContainsKey(name);
    }

    public List<string> GetSaveFilenames()
    {
        string[] paths = Directory.GetFiles(savedSettingsPath);
        List<string> filenames = new List<string>();
        foreach (string path in paths) {
            string name = path.Split("\\")[1]; // trim leading PATH directory
            name = name.Split(".")[0]; // trim extention (.txt)
            filenames.Add(name);
        }
        return filenames;
    }

    public void BlacklistFromSaving(string name)
    {
        if (!blacklistedEntries.Contains(name)) {
            blacklistedEntries.Add(name);
        }
    }

#if UNITY_EDITOR
    // Debugging tool (Access from Tools>Save or Load current settings... during runtime)
    private class DialogBox : EditorWindow
    {
        private string filename = "";

        [MenuItem("Tools/Save or Load current settings...")]
        public static void ShowWindow()
        {
            GetWindow<DialogBox>("Save/Load current settings");
        }

        private void OnGUI()
        {
            GUILayout.Label("Save/Load Settings from file", EditorStyles.boldLabel);
            filename = EditorGUILayout.TextField("filename:", filename);

            if (GUILayout.Button("Save")) {
                Instance.Save(filename);
            }
            if (GUILayout.Button("Load")) {
                Instance.Load(filename);
            }
            if (GUILayout.Button("Close")) {
                Close();
            }

        }
    }
#endif

}
