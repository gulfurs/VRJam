using UnityEngine;
using TMPro;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class UnityConnection : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private readonly byte[] buffer = new byte[4096];
    private bool isProcessing;
    
    [Header("Text References")]
    public TextMeshPro transcriptionText;
    public TextMeshPro statusText;
    
    [Header("Status Colors")]
    public Color idleColor = Color.white;
    public Color recordingColor = Color.red;
    public Color processingColor = Color.yellow;
    public Color disconnectedColor = Color.gray;
    
    [Header("Input References")]
    //public InputActionReference leftPrimaryButton; // Reference to XR left hand primary button
    
    [Header("Dependencies")]
    public Keywords keywords;
    
    public string transcription;
    private bool prefabSpawned;
    private bool isConnected;
    private float animationTimer;
    private string[] processingAnimation = new string[] { "Processing.", "Processing..", "Processing..." };
    private int animationFrame;

    private async void Start()
    {
        InitializeStatusText();
        await ConnectToServerAsync();
    }

    private void InitializeStatusText()
    {
        if (statusText != null)
        {
            statusText.text = "Ready";
            statusText.color = idleColor;
        }
    }

    private async Task ConnectToServerAsync()
    {
        UpdateStatus("Connecting...", Color.yellow);
        try
        {
            client = new TcpClient();
            await client.ConnectAsync("127.0.0.1", 5000);
            stream = client.GetStream();
            isConnected = true;
            UpdateStatus("Ready", idleColor);
            Debug.Log("Connected to Python server.");
        }
        catch (SocketException e)
        {
            isConnected = false;
            UpdateStatus("Connection Failed", disconnectedColor);
            Debug.LogError($"Failed to connect: {e.Message}");
        }
    }

    private void Update()
    {
        if (!isConnected)
        {
            UpdateStatus("Disconnected", disconnectedColor);
            return;
        }

        // Handle space key input (kept for debugging/testing)
        if (Input.GetKeyDown(KeyCode.Space) && !isProcessing)
        {
            StartTranscriptionProcess();
        }

        // Handle prefab spawning and text update
        if (prefabSpawned)
        {
            UpdateText();
            prefabSpawned = false;
        }

        // Update processing animation
        if (isProcessing)
        {
            UpdateProcessingAnimation();
        }
    }

    private void UpdateProcessingAnimation()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= 0.5f)
        {
            animationTimer = 0f;
            animationFrame = (animationFrame + 1) % processingAnimation.Length;
            UpdateStatus(processingAnimation[animationFrame], processingColor);
        }
    }

    private void UpdateStatus(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }

    public async void StartTranscriptionProcess()
    {
        if (isProcessing) return;
        
        isProcessing = true;
        animationTimer = 0f;
        animationFrame = 0;
        
        try
        {
            UpdateStatus("Recording...", recordingColor);
            await SendMessageToPythonAsync("start");
            Debug.Log("Recording started, waiting for transcription...");
            await ReceiveTranscriptionAsync();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error during transcription: {e.Message}");
            UpdateStatus("Error occurred", Color.red);
            await ReconnectIfNeededAsync();
        }
        finally
        {
            isProcessing = false;
            UpdateStatus("Ready", idleColor);
        }
    }

    private async Task SendMessageToPythonAsync(string message)
    {
        if (stream == null) return;
        
        byte[] data = Encoding.UTF8.GetBytes(message);
        await stream.WriteAsync(data, 0, data.Length);
        Debug.Log("Sent message to Python: " + message);
    }

    private async Task ReceiveTranscriptionAsync()
    {
        if (stream == null) return;

        try
        {
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                transcription = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                Debug.Log("Transcription from Python: " + transcription);
                prefabSpawned = true;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error receiving transcription: {e.Message}");
            throw;
        }
    }

    private void UpdateText()
    {
        if (transcriptionText != null)
        {
            transcriptionText.text = transcription;
            
            if (keywords != null)
            {
                keywords.HandleTranscription(transcription);
            }
        }
        else
        {
            Debug.LogWarning("TextMeshPro reference is missing!");
        }
    }

    private async Task ReconnectIfNeededAsync()
    {
        if (!isConnected)
        {
            Debug.Log("Attempting to reconnect...");
            await ConnectToServerAsync();
        }
    }

    private void OnApplicationQuit()
    {
        CleanupConnection();
    }

    private void CleanupConnection()
    {
        isConnected = false;
        UpdateStatus("Disconnected", disconnectedColor);
        stream?.Dispose();
        client?.Dispose();
    }
}