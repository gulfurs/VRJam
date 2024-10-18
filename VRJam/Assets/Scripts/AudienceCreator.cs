using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceCreator : MonoBehaviour
{
public GameObject audiencePrefab; // The capsule prefab
    
    // Start position (center point of colosseum)
    public Vector3 center = new Vector3(35, 20, 200);

    // Arc parameters
    public float radius = 100f; // Radius of the colosseum arc
    public int numberOfSeats = 10; // Total number of seats in the arc
    public float seatDistance = 4f; // Horizontal distance between each seat
    public float rowHeight = 2.5f;  // Height difference between rows

    public float startHeight = 25f;

    void Start()
    {
        PlaceAudienceInArc();
    }

    void PlaceAudienceInArc()
    {
        // We calculate the total angle the audience members should cover.
        // For example, if we want to cover a half circle (180 degrees).
        float totalArcAngle = 360f;
        float angleStep = totalArcAngle / (numberOfSeats - 1); // Angle between each seat

        // Loop through and place audience members in the arc
        for (int i = 0; i < numberOfSeats; i++)
        {
            // Calculate the angle in degrees for the current seat
            float currentAngle = -totalArcAngle / 2 + i * angleStep; // Starts at -90 degrees and ends at +90
            float radian = Mathf.Deg2Rad * currentAngle;

            // Calculate the X and Z position using the circular arc formula
            float x = center.x + radius * Mathf.Cos(radian);
            float z = center.z + radius * Mathf.Sin(radian);

            // Alternate between the lower and upper row by adding the height every second seat
            float y = (i % 2 == 0) ? startHeight : rowHeight + startHeight;
            //float y = rowHeight + ;

            // Position for the audience member
            Vector3 seatPosition = new Vector3(x, y, z);

            // Instantiate the audience member in the arc, facing the center of the colosseum
            Quaternion rotation = Quaternion.LookRotation(center - seatPosition); // Rotate to face the center
            Instantiate(audiencePrefab, seatPosition, rotation, transform);
        }
    }
}
