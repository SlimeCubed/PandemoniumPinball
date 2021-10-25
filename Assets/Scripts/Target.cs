using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, ICircuitElement
{
    public float force;
    public float activeTime = 10f;
    public int score = 10;

    public bool Active => DeactivateAt > Time.fixedTime;
    public float DeactivateAt { get; set; }

    private float spriteOffset;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = collision.rigidbody;
        if (rb == null) return;

        Vector2 dir = transform.up;
        dir.Normalize();

        //rb.velocity -= dir * Vector2.Dot(rb.velocity, dir);
        rb.AddForce(dir * force, ForceMode2D.Impulse);

        spriteOffset = 1f;
        GameController.GiveScore(score);

        DeactivateAt = Time.fixedTime + activeTime;
    }

    public void Update()
    {
        spriteOffset = Mathf.Clamp01(spriteOffset - Time.deltaTime * 4f);

        float maxOffset = force > 0f ? 0.25f : -0.25f;
        transform.GetChild(0).localPosition = new Vector3(0f, 1f + maxOffset * spriteOffset, 0f);
    }
}
