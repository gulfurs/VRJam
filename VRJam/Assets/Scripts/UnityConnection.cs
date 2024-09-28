using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Sockets;
using System;
using System.IO;

public class UnityConnection : MonoBehaviour
{
private string serverIP = "127.0.0.1";  // Localhost IP
    private int port = 65432;  // Port used by Python server

    void Start()
    {
        ConnectToPythonServer();
    }

    void ConnectToPythonServer()
    {
        try
        {
            // Create a TcpClient to connect to the Python server
            TcpClient client = new TcpClient(serverIP, port);
            NetworkStream stream = client.GetStream();

            // Send a message to the Python server
            string message = "Hello from Unity!";
            byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Debug.Log("Sent message: " + message);

            // Receive the response from the Python server
            data = new byte[256];
            int bytesRead = stream.Read(data, 0, data.Length);
            string response = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead);
            Debug.Log("Received response from Python: " + response);

            // Close the connection
            stream.Close();
            client.Close();
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }
}
