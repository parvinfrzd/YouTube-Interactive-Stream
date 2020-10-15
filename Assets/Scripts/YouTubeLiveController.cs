using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.Networking;
using SimpleJSON;

public enum ActivateFlag {Active, Inactive}; 
public class YouTubeLiveController : MonoBehaviour { 

  public ConfigReader configReader; 
  
  private Config _config; 

  String liveID = null; 
  List<String> SavedEtag = new List<string>(); 
  public ActivateFlag state; 
  public Text mytext; 
  void Awake() 
  {
    _config = configReader.config; 
  }

  void Start() 
  {
    StartCoroutine(GetLiveChatID()); 
    StartCoroutine(PullMessage()); 
  }

  IEnumerator GetLiveChatID() {

    var url = "https://www.googleapis.com/youtube/v3/videos?id=" + _config.LiveStreamKey + "&key=" + _config.APIKey + "&part=liveStreamingDetails";

    var req = UnityWebRequest.Get (url);

    yield return req.SendWebRequest();

    var json = JSON.Parse (req.downloadHandler.text);
    Debug.Log (json.ToString());

    liveID = json["items"][0]["liveStreamingDetails"]["activeLiveChatId"].Value;
    Debug.Log (liveID);
    // yield return liveID; 
  }

  IEnumerator PullMessage() { 

    while (true)
    {
      if(!String.IsNullOrEmpty(liveID)) 
      {
        var url = "https://www.googleapis.com/youtube/v3/liveChat/messages?liveChatId=" + liveID + "&part=snippet,authorDetails&key=" + _config.APIKey;

        var req = UnityWebRequest.Get(url);
        // Debug.Log (liveID);
        yield return req.SendWebRequest();

        var json = JSON.Parse (req.downloadHandler.text);
        var rawMessages = json["items"]; 
        // messages.Add(json["items"]);
        // Debug.Log (json.ToString());
        
        // filtering messages function
      foreach (var message in rawMessages) 
      {
        var etag = message.Value["etag"];
        var displayMessage = message.Value ["snippet"]["displayMessage"].Value;
        var author = message.Value["authorDetails"]["displayName"].Value;

        if(!SavedEtag.Contains(etag)) 
        {
            if(_config.Keywords.Contains(displayMessage))
            {
              Debug.Log(author + " : " + displayMessage); 
              SavedEtag.Add(etag); 
              state = ActivateFlag.Active; 
              mytext.text = author + " : " + displayMessage; 
            }
            else
            {
              state = ActivateFlag.Inactive; 
            }
          
        }     
      }
        yield return new WaitForSeconds(_config.WaitTime);
      }

      else 
      {
        yield return new WaitForSeconds(1);
      }
      
    }
    
  }

}

