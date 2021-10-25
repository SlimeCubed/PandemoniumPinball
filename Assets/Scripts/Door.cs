using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Door : MonoBehaviour
{
    public float angleChange = 45f;
    public float speed = 90f;
    public List<MonoBehaviour> triggers = new List<MonoBehaviour>();

    private Rigidbody2D rb2d;
    private float startAngle;

    void FixedUpdate()
    {
        if (rb2d == null)
        {
            rb2d = GetComponent<Rigidbody2D>();
            startAngle = rb2d.rotation;
        }

        bool active = triggers.All(t => !(t is ICircuitElement ice) || ice.Active);

        if (active)
        {
            float maxDeactivateAt = 0f;
            foreach (var trig in triggers)
            {
                if (trig is ICircuitElement ice)
                    maxDeactivateAt = Mathf.Max(maxDeactivateAt, ice.DeactivateAt);
            }
            foreach (var trig in triggers)
            {
                if (trig is ICircuitElement ice)
                    ice.DeactivateAt = maxDeactivateAt;
            }
        }

        rb2d.MoveRotation(Mathf.Clamp(startAngle + (active ? angleChange : 0f) - rb2d.rotation, -speed * Time.deltaTime, speed * Time.deltaTime) + rb2d.rotation);
    }
}
