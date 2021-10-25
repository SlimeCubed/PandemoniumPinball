using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    public float force = 10f;
    public float damping = 0f;
    public float turnaroundSpeedThreshold = 5f;
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

        if (Vector2.Dot(rb.velocity, transform.up) > -turnaroundSpeedThreshold)
        {
            rb.velocity *= 1f - damping;
            rb.AddForce(transform.up * force, ForceMode2D.Impulse);

            GetComponent<SpriteRenderer>().sprite = onSprite;

            CancelInvoke("DisableSprite");
            Invoke("DisableSprite", 0.5f);
        }
    }

    public void DisableSprite()
    {
        GetComponent<SpriteRenderer>().sprite = offSprite;
    }
}
