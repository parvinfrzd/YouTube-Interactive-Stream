using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Config 
{
    public string  LiveStreamKey; 
    public string APIKey;
    public int WaitTime; 
    public List<String> Keywords = new List<string>();
}
