using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MicRecorder : MonoBehaviour
{
    private AudioClip audioClip;
    private string microphone;
    private bool isRecording = false;
    private string filePath = "Assets/AudioRecordings/recording.wav";  // Path to save the recorded audio

     // Reference to the red light GameObject
    public GameObject recordingIndicator;

    void Start()
    {
        // Ensure there's at least one microphone connected
        if (Microphone.devices.Length > 0)
        {
            microphone = Microphone.devices[0];  // Select the first microphone
        }
        else
        {
            Debug.LogError("No microphone found!");
        }

        // Make sure the recording indicator is initially disabled/invisible
        SetRecordingIndicator(false);
    }

    void Update()
    {
        // Press spacebar to start recording (can map this to a VR controller button later)
        if (Input.GetKeyDown(KeyCode.Space) && !isRecording)
        {
            StartRecording();
        }

        // Release spacebar to stop recording
        if (Input.GetKeyUp(KeyCode.Space) && isRecording)
        {
            StopRecording();
        }
    }
    // Start recording audio
    void StartRecording()
    {
        if (microphone != null)
        {
            isRecording = true;
            audioClip = Microphone.Start(microphone, false, 10, 44100);  // Record for a maximum of 10 seconds
            Debug.Log("Recording started...");

            // Enable the visual indicator
            SetRecordingIndicator(true);
        }
    }

    // Stop recording and save the audio
    void StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            Microphone.End(microphone);
            Debug.Log("Recording stopped...");

            // Disable the visual indicator
            SetRecordingIndicator(false);

            // Save the recorded audio to a WAV file
            SaveAudioToFile(audioClip, filePath);
        }
    }

        // Method to toggle the recording indicator on or off
    void SetRecordingIndicator(bool active)
    {
        if (recordingIndicator != null)
        {
            recordingIndicator.SetActive(active);  // Turn the indicator on or off
        }
    }

    // Method to save the recorded audio as a WAV file 
    void SaveAudioToFile(AudioClip clip, string filePath)
    {
        if (!Directory.Exists("Assets/AudioRecordings"))
        {
            Directory.CreateDirectory("Assets/AudioRecordings");
        }

        SavWav.Save(filePath, clip);
        Debug.Log("Audio saved at: " + filePath);
    }
}
