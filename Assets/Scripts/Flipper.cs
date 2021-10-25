using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flipper : MonoBehaviour
{
    public string buttonName = "Flippers";
    public float angleChange = 45f;
    public float flipSpeed = 90f;
    public float returnSpeed = 45f;
    public float acceleration;
    public bool lockFlippers;

    private float angleStart;
    private float speedMul;
    private bool lastFlipPressed;

    private Rigidbody2D RB => rb ??= GetComponent<Rigidbody2D>();
    private Rigidbody2D rb;

    public void Start()
    {
        angleStart = RB.rotation;
    }

    public void FixedUpdate()
    {
        bool flip = lockFlippers ? lastFlipPressed : Input.GetButton(buttonName);
        lastFlipPressed = Input.GetButton(buttonName);

        float target = angleStart + (flip ? angleChange : 0f);
        if (Mathf.Abs(RB.rotation - target) > 0.01f) speedMul = Mathf.Clamp01(speedMul + acceleration * Time.deltaTime);
        else speedMul = 0f;

        float maxVel = speedMul * (flip ? flipSpeed : returnSpeed);

        RB.angularVelocity = Mathf.Clamp((target - RB.rotation) / Time.deltaTime, -maxVel, maxVel);
    }
}
