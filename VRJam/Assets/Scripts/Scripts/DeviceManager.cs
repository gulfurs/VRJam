using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class DeviceManager : MonoBehaviour
{
    private GameObject savedChild;
    [SerializeField] private Vector3 offset = new Vector3(0f, 5f, 0f); // Offset between each instantiated object

    void Start()
    {
        // Find the VoiceProcessor object
        VoiceProcessor voiceProcessor = FindFirstObjectByType<VoiceProcessor>();
        if (voiceProcessor == null)
        {
            Debug.LogError("VoiceProcessor not found in the scene!");
            return;
        }

        // Check if the GameObject has a child
        if (transform.childCount == 0)
        {
            Debug.LogError("No child object found under this GameObject!");
            return;
        }

        // Save the first child
        savedChild = transform.GetChild(0).gameObject;

        // Deactivate the saved child and remove it from the hierarchy
        savedChild.SetActive(false);
        
        int index = 0;
        // Iterate through VoiceProcessor.Devices and instantiate the saved child for each device
        foreach (var device in voiceProcessor.Devices)
        {
            Vector3 newPosition = transform.position + (offset * index);
            GameObject newChild = Instantiate(savedChild, newPosition, Quaternion.identity, transform);
            newChild.name = $"{savedChild.name}_{device}";
            newChild.SetActive(true);

            TextMeshPro textMesh = newChild.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                // Set the text of the TextMesh to the device name
                textMesh.text = device.ToString();
            }
            else
            {
                Debug.LogWarning($"No TextMesh found on child of {newChild.name}");
            }

             // Get the XRSimpleInteractable component on the instantiated child
            XRSimpleInteractable interactable = newChild.GetComponent<XRSimpleInteractable>();
            if (interactable != null)
            {
                // Add the Select event listener to call ChangeDevice with the current index
                interactable.selectEntered.AddListener((args) => voiceProcessor.ChangeDevice(index));
            }
            else
            {
                Debug.LogWarning($"No XRSimpleInteractable found on child of {newChild.name}");
            }


            index++;
        }
    
        // Finally, remove the original saved child (if needed)
        Destroy(savedChild);
    }
}
