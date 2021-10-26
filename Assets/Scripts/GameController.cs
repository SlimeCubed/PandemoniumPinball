using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float tiltDegrees;
    public float tiltTime;

    public static int score;
    public static Vector2 camOffset;
    private static Vector2 camOffsetTarget;
    private static Vector2 camOffsetVel;

    private static GameController instance;
    private Vector2 gravityStart;
    private float tilt;
    private float tiltVel;
    private float nextTimescale = 1f;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        gravityStart = Physics2D.gravity;
    }

    public void FixedUpdate()
    {
        float tiltAxis = Input.GetAxis("Tilt");
        
        tilt = Mathf.SmoothDamp(tilt, tiltAxis * tiltDegrees * Mathf.Deg2Rad, ref tiltVel, tiltTime);

        Physics2D.gravity = new Vector2(Mathf.Cos(tilt) * gravityStart.x - Mathf.Sin(tilt) * gravityStart.y, Mathf.Cos(tilt) * gravityStart.y + Mathf.Sin(tilt) * gravityStart.x);
    }

    public void Update()
    {
        Time.timeScale = nextTimescale;
        nextTimescale = 1f;

        camOffset = Vector2.SmoothDamp(camOffset, camOffsetTarget, ref camOffsetVel, 0.1f);
        camOffsetTarget = new Vector2();
    }

    public static void Slowdown(float amount)
    {
        instance.nextTimescale = Mathf.Max(0.1f, instance.nextTimescale * (1f - amount));
    }

    public static void GiveScore(int score)
    {
        GameController.score += score;
    }

    public static void PushCamera(Vector2 offset)
    {
        camOffsetTarget += offset;
    }
}
