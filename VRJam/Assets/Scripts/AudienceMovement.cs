using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMovement : MonoBehaviour
{
    public float baseJumpHeight = 1.5f;
    public float baseJumpDuration = 0.5f;
    public float jumpFrequency = 1f;
    public AnimationCurve jumpCurve;

    private bool isJumping = true;
    private bool shouldStop = false;

    void Start()
    {
        // Initialize jump curve if not set in inspector
        if (jumpCurve == null || jumpCurve.length == 0)
        {
            jumpCurve = new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(0.5f, 1),
                new Keyframe(1, 0)
            );
        }

        Entry();
    }

    public void Entry()
    {
        GameObject[] audienceMembers = GameObject.FindGameObjectsWithTag("Audience");

        if (audienceMembers.Length > 0)
        {
            isJumping = true;
            shouldStop = false;

            foreach (GameObject audienceMember in audienceMembers)
            {
                StartCoroutine(ContinuousJumping(audienceMember));
            }
        }
        else
        {
            Debug.LogWarning("No audience members found with the 'Audience' tag.");
        }
    }

    public void StopJumping()
    {
        shouldStop = true;
    }

    IEnumerator ContinuousJumping(GameObject audienceMember)
    {

        Vector3 basePosition = audienceMember.transform.position;
        while (isJumping && !shouldStop)
        {
            // Randomize jump parameters for more natural variation
            float randomHeightFactor = Random.Range(0.85f, 1.15f);
            float randomDurationFactor = Random.Range(0.8f, 1.2f);
            float randomFrequencyVariation = Random.Range(0.9f, 1.1f);

            yield return StartCoroutine(RealisticJump(
                audienceMember, 
                basePosition,
                baseJumpHeight * randomHeightFactor, 
                baseJumpDuration * randomDurationFactor,
                jumpFrequency * randomFrequencyVariation
            ));
        }

        // Optional: Reset position when stopping
        /*audienceMember.transform.position = new Vector3(
            audienceMember.transform.position.x, 
            audienceMember.transform.position.y - baseJumpHeight, 
            audienceMember.transform.position.z
        );*/
        audienceMember.transform.position = basePosition - new Vector3(0, baseJumpHeight, 0);
    }

    IEnumerator RealisticJump(GameObject audienceMember, Vector3 basePosition, float jumpHeight, float jumpDuration, float frequency)
    {
        //Vector3 startPosition = audienceMember.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            // Use animation curve for smooth, realistic jump arc
            float jumpProgress = elapsedTime / jumpDuration;
            float verticalOffset = jumpCurve.Evaluate(jumpProgress) * jumpHeight;

            audienceMember.transform.position = new Vector3(
                basePosition.x, 
                basePosition.y + verticalOffset, 
                basePosition.z
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Snap back to original position
        audienceMember.transform.position = basePosition;

        // Wait before next jump
        yield return new WaitForSeconds(1f / frequency);
    }
}