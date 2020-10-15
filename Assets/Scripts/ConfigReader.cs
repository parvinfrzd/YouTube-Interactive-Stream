using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ConfigReader: MonoBehaviour 
{     
    string path; 
    public Config config;
    void Awake()
    {
        path = Application.dataPath + "/Resources/config.json";
        Debug.Log(path);
        var jsonString = File.ReadAllText(path); 
        LoadConfig(); 
    } 

    void LoadConfig()
    {
        string rawJSON = File.ReadAllText(path);
        config = JsonUtility.FromJson<Config>(rawJSON);
        // Debug.Log(rawJSON.Value); 
    }
}



// public class valuesArray 
// {
//     public List<Values> Keywords = new List<Values>();
// }




