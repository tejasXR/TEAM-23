using UnityEngine;
using NativeWebSocket;

public class WebsocketManager : MonoBehaviour
{
    public delegate void MessageRecieved(string message);
    public static event MessageRecieved OnMessageRecieved;
    public static WebsocketManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);  
        }
    }

    private WebSocket websocket;
    // Start is called before the first frame update
    async void Start()
    {
        // Attempts to start new WebSocket connection to 8080:
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");

            // Send Unity version over to server 
            SendWebSocketMessage($"{Application.unityVersion}");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };


        websocket.OnMessage += Websocket_OnMessage;

        await websocket.Connect();
    }

    private void Websocket_OnMessage(byte[] data)
    {
        var message = System.Text.Encoding.UTF8.GetString(data);
        if (OnMessageRecieved != null)
        {
            OnMessageRecieved(message);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async public void SendWebSocketMessage(string message)
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending plain text
            await websocket.SendText(message);
        }
    }

}
