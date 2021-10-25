using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPoint : MonoBehaviour
{
    public float delay = 1f;
    public Transform spawnAt;
    public int scoreThreshold;
    public string highScoreScene;
    private Coroutine reset;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (reset != null) return;

        reset = StartCoroutine(ResetCoroutine(collision.attachedRigidbody));
    }

    IEnumerator ResetCoroutine(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(delay);

        if (GameController.score <= scoreThreshold)
        {
            rb.transform.position = spawnAt.position;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            reset = null;
        }
        else
        {
            TransitionController.WipeScene(highScoreScene);
        }
    }
}
