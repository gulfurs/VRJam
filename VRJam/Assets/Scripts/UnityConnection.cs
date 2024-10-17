using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Net.Sockets;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class UnityConnection : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];

    public TextMeshPro transcriptionText;  // Use TextMesh for 3D text

    private string transcription;
    
    public Keywords keywords; // Refrence for keywords script
    private bool PrefabSpawned = false;

    void Start()
    {
        // Connect to Python server
        client = new TcpClient("127.0.0.1", 5000);  // Match with Python's IP and port
        stream = client.GetStream();
        Debug.Log("Connected to Python server.");
    }

    void Update()
    {
        // When spacebar is pressed, start recording
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessageToPython("start");
            Debug.Log("Recording started, waiting for transcription...");
            Task.Run(() => ReceiveTranscriptionAsync());  // Receive transcription asynchronously
            
        }

         if (PrefabSpawned)
        {
        Updatetext();
        PrefabSpawned = false; // Reset flag after Updatetext
        }
    }

    void SendMessageToPython(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
        Debug.Log("Sent message to Python: " + message);
    }

    async Task ReceiveTranscriptionAsync()
    {
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
        transcription = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        Debug.Log("Transcription from Python: " + transcription);  // Log the transcription in Unity
        PrefabSpawned = true;
       
    }

    public void Updatetext(){
        transcriptionText.text = transcription;
        //PrefabSpawned = true;
        if (PrefabSpawned == true){
        keywords.HandleTranscription(transcription);
        }
    }

    void OnApplicationQuit()
    {
        stream.Close();
        client.Close();
    }
}