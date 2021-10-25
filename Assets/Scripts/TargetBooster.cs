using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBooster : MonoBehaviour
{
    public Vector2 targetOffset;
    public float force = 10f;
    public float damping = 0f;
    public Sprite onSprite;
    private Sprite offSprite;

    public void Awake()
    {
        offSprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var rb = collision.attachedRigidbody;
        if (rb == null) return;

        Vector2 target = transform.TransformPoint(targetOffset);
        Vector2 dir = (target - (Vector2)rb.transform.TransformPoint(rb.centerOfMass)).normalized;

        if (Vector2.Dot(rb.velocity, dir) > 0f)
        {
            rb.velocity *= 1f - damping;
            rb.AddForce(dir * force, ForceMode2D.Impulse);

            GetComponent<SpriteRenderer>().sprite = onSprite;

            CancelInvoke("DisableSprite");
            Invoke("DisableSprite", 0.5f);
        }
    }

    public void DisableSprite()
    {
        GetComponent<SpriteRenderer>().sprite = offSprite;
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 target = transform.TransformPoint(targetOffset);
        Gizmos.DrawLine(target + Vector3.up * 0.25f, target + Vector3.down * 0.25f);
        Gizmos.DrawLine(target + Vector3.left * 0.25f, target + Vector3.right * 0.25f);
    }
}
