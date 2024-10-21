using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceMovement : MonoBehaviour
{
    public GameObject[] Audienceholder;
    public float jumpHeight = 2.0f;
    public float jumpSpeed = 2.0f;

    private AudioManager audioManager;


    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    public void Entry()
    {
        foreach (GameObject Audience in Audienceholder)
        {
            StartCoroutine(Jump(Audience));

            if (audioManager != null)
            {
                audioManager.PlayWalkoutmusic();
                audioManager.PlayWalkoutSound();
            }
        }
    }

    // Coroutine that handles the jumping for a single bean
    IEnumerator Jump(GameObject bean)
    {
        Vector3 startPosition = bean.transform.position;
        Vector3 jumpPosition = new Vector3(startPosition.x, startPosition.y + jumpHeight, startPosition.z);

        // Jump up
        while (bean.transform.position.y < jumpPosition.y)
        {
            bean.transform.position = Vector3.MoveTowards(bean.transform.position, jumpPosition, jumpSpeed * Time.deltaTime);
            yield return null;
        }

        // Jump down
        while (bean.transform.position.y > startPosition.y)
        {
            bean.transform.position = Vector3.MoveTowards(bean.transform.position, startPosition, jumpSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
