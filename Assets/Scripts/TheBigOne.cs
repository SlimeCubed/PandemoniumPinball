using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBigOne : MonoBehaviour, ICircuitElement
{
    public float force;
    public float activeTime = 10f;
    public int score = 666667;

    public bool Active => DeactivateAt > Time.fixedTime;
    public float DeactivateAt { get; set; }
    private bool givenScore = false;

    private float spriteOffset;
    private Rigidbody2D ball;
    private Collider2D myCollider;
    private Rigidbody2D rb;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = collision.rigidbody;
        if (rb == null || givenScore) return;

        Vector2 dir = transform.up;
        dir.Normalize();

        rb.AddForce(dir * force, ForceMode2D.Impulse);

        givenScore = true;
        GameController.GiveScore(score);

        DeactivateAt = Time.fixedTime + activeTime;
    }

    public void Update()
    {
        if(givenScore)
            spriteOffset = Mathf.Clamp01(spriteOffset + Time.deltaTime * 2f);

        float maxOffset = -1.5f;
        rb ??= GetComponent<Rigidbody2D>();
        rb.MovePosition(transform.parent.TransformPoint(new Vector3(0f, maxOffset * Mathf.Sin(spriteOffset * Mathf.PI / 2f), 0f)));

        // Slowdown
        ball ??= GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody2D>();
        myCollider ??= GetComponent<Collider2D>();

        var dist = ball.Distance(myCollider);
        if (dist.isValid)
        {
            float amount = Mathf.InverseLerp(6f, 1f, dist.distance);
            GameController.Slowdown(amount * 0.85f);
            GameController.PushCamera(new Vector2(0f, amount * 4f));
        }
    }
}
