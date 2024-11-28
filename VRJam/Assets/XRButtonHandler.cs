using UnityEngine;
using UnityEngine.InputSystem;

public class XRButtonHandler : MonoBehaviour
{
    public InputActionReference buttonAction; // Assign your action in the Inspector
    public VoiceProcessor getVoiceProcessor;

    void Start() {
        getVoiceProcessor = FindFirstObjectByType<VoiceProcessor>();
    }

    private void OnEnable()
    {
        buttonAction.action.performed += OnButtonHeld;
        buttonAction.action.canceled += OnButtonReleased;
        buttonAction.action.Enable();
    }

    private void OnDisable()
    {
        buttonAction.action.performed -= OnButtonHeld;
        buttonAction.action.canceled -= OnButtonReleased;
        buttonAction.action.Disable();
    }

    private void OnButtonHeld(InputAction.CallbackContext context)
    {
        getVoiceProcessor.StartRecord();
    }

    private void OnButtonReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Button released");
        getVoiceProcessor.StopRecording();
    }
}
