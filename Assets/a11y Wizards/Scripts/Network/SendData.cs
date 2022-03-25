using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SendData : MonoBehaviour
{

    private void PackJson(WWWForm data, string jsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error sending Json");
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
        }
    }

    private void PackImage(WWWForm data, Texture2D myTexture)
    {
        if (!myTexture.isReadable) //Texture needs to be readable, otherwise you can't encode it.
        {
            Debug.Log("Made texture readable.");
            return;
        }
        byte[] bytes = myTexture.EncodeToPNG();

        data.AddBinaryData("screenshot", bytes, myTexture.name, "image/jpg");

    }

    IEnumerator EstablishConnection(string uri, WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
        {
            yield return www.SendWebRequest(); //Wait until the server answers.

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                    Debug.Log("Connection error: " + www.error);
            }
            else
            {
                Debug.Log("Connection established: " + www.downloadHandler.text); //Show result as text.
            }
        }
    }
}
