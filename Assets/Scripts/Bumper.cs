using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float force;
    public int score = 5;
    private float spriteBump = 0f;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var rb = collision.rigidbody;
        if (rb == null) return;

        Vector2 dir =  (Vector2)rb.transform.localToWorldMatrix.MultiplyPoint(rb.centerOfMass) - (Vector2)transform.position;
        dir.Normalize();

        //rb.velocity -= dir * Vector2.Dot(rb.velocity, dir);
        rb.AddForce(dir * force, ForceMode2D.Impulse);

        GameController.GiveScore(score);
        SendMessage("TriggerSound");

        spriteBump = 1f;
    }

    public void Update()
    {
        spriteBump = Mathf.Clamp01(spriteBump - Time.deltaTime * 6f);

        float scl = Mathf.Lerp(1f, 1.2f, spriteBump);
        transform.GetChild(0).localScale = new Vector3(scl, scl, scl);
    }
}
