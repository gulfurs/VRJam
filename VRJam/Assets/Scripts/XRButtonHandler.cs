using UnityEngine;
using UnityEngine.InputSystem;

public class XRButtonHandler : MonoBehaviour
{
    public InputActionReference buttonAction; 
    public VoskResultText getVoiceProcessor;

    void Start() {
        getVoiceProcessor = FindFirstObjectByType<VoskResultText>();
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
        getVoiceProcessor.CheckInput = true;
    }

    private void OnButtonReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Button released");
        getVoiceProcessor.CheckInput = false;
    }
}
