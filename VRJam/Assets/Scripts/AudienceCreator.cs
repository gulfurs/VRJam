using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceCreator : MonoBehaviour
{
public GameObject audiencePrefab; // The capsule prefab
    
    // Start position (center point of colosseum)
    public Vector3 center = Vector3.zero;

    // Arc parameters
    public float radius; // Radius of the colosseum arc
    public float radiusInc; 
    public int numberOfSeats; // Total seats in the arc
    public int NrOfRows = 12; // Total rows
    public float seatDistance; // distance between each seat
    public float rowHeight;  // Height between rows
    public float startHeight; // Start height
    public float randomnes = 0.3f; //The random 
    public float tiltRange = 5f;

    void Start()
    {
        NewPlaceAudioenceArc();
    }

    void NewPlaceAudioenceArc(){
        // Outer loop for multiple rows
        for (int row = 0; row < NrOfRows; row++)
        {
            // Calculate the radius for the current row
            float currentRadius = radius + row * radiusInc;

            // Total arc angle (e.g., half circle = 180 degrees)
            float totalArcAngle = 360f;
            float angleStep = totalArcAngle / (numberOfSeats - 1); // Angle between seats

            // Inner loop for seats in the current row
            for (int i = 0; i < numberOfSeats; i++)
            {
                // Calculate the angle for the current seat in degrees
                float currentAngle = -totalArcAngle / 2 + i * angleStep; // Start from -90 to +90
                float radian = Mathf.Deg2Rad * currentAngle;

                // Calculate the X and Z position relative to the arc center
                float x = center.x + currentRadius * Mathf.Cos(radian);
                float z = center.z + currentRadius * Mathf.Sin(radian);

                // Add the random effect for x and z pos
                x += Random.Range(-randomnes, randomnes);
                z += Random.Range(-randomnes, randomnes);

                float randomTiltX = Random.Range(-tiltRange, tiltRange); // Tilt along the X-axis
                float randomTiltZ = Random.Range(-tiltRange, tiltRange); // Tilt along the Z-axis
                Quaternion randomTilt = Quaternion.Euler(randomTiltX, 0f, randomTiltZ);

                // Calculate the Y position for each row
                float y = startHeight + row * rowHeight;

                // Calculate the seat position based on the new arc center and height
                Vector3 seatPosition = new Vector3(x, y, z);

                // Instantiate the audience member, rotating them to face the arc center
                Quaternion rotation = Quaternion.LookRotation(center - seatPosition) * randomTilt;
                Instantiate(audiencePrefab, seatPosition, rotation, transform);
            }
        }
    }
}
