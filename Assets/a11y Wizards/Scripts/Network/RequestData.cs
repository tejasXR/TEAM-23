using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       StartCoroutine(GetRequest("http://0.0.0.0:5000")); 
    }

    IEnumerator GetRequest(string uri) {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError) {
                Debug.Log("Error: " + webRequest.error);
            } else {
                Debug.Log(webRequest.downloadHandler.text);
            }
        }
    }
}
