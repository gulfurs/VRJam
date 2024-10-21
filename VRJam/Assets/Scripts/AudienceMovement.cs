using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMovement : MonoBehaviour
{
    public float jumpHeight = 2.0f;
    public float jumpSpeed = 2.0f;

    private AudioManager audioManager;
    private bool isJumping = false;

    void Start()
    {
        audioManager = AudioManager.instance;

        if (audioManager == null)
        {
            Debug.LogError("AudioManager instance not found.");
        }
    }

    public void Entry()
    {
        GameObject[] audienceMembers = GameObject.FindGameObjectsWithTag("Audience");

        if (audienceMembers.Length > 0)
        {
            if (audioManager != null && audioManager.walkoutmusic != null)
            {
                audioManager.PlayWalkoutMusic();
                audioManager.PlayWalkoutSound();

                StartCoroutine(HandleJumping(audienceMembers, audioManager.walkoutmusic.length));
            }
        }
        else
        {
            Debug.LogWarning("No audience members found with the 'Audience' tag.");
        }
    }

    IEnumerator HandleJumping(GameObject[] audienceMembers, float musicDuration)
    {
        isJumping = true;

        foreach (GameObject audienceMember in audienceMembers)
        {
            StartCoroutine(Jump(audienceMember));
        }

        yield return new WaitForSeconds(musicDuration);

        isJumping = false;
    }

    IEnumerator Jump(GameObject audienceMember)
    {
        Vector3 startPosition = audienceMember.transform.position;
        Vector3 jumpPosition = new Vector3(startPosition.x, startPosition.y + jumpHeight, startPosition.z);

        while (isJumping)
        {
            while (audienceMember.transform.position.y < jumpPosition.y && isJumping)
            {
                audienceMember.transform.position = Vector3.MoveTowards(audienceMember.transform.position, jumpPosition, jumpSpeed * Time.deltaTime);
                yield return null;
            }

            while (audienceMember.transform.position.y > startPosition.y && isJumping)
            {
                audienceMember.transform.position = Vector3.MoveTowards(audienceMember.transform.position, startPosition, jumpSpeed * Time.deltaTime);
                yield return null;
            }

            yield return null;
        }
    }
}
