using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public Transform chassis;
    public Transform ball;
    public Vector3 offset;
    public Texture2D sky;
    public float skyBottom;
    public float skyTop;
    public float tiltMul = 0.1f;

    private Camera cam;
    private float ang;
    private float lastAng;

    public void FixedUpdate()
    {
        lastAng = ang;
        ang = tiltMul * Mathf.DeltaAngle(-90f, Mathf.Atan2(Physics2D.gravity.y, Physics2D.gravity.x) * Mathf.Rad2Deg);
    }

    public void Update()
    {
        float atBall = 1f;
        transform.position = Vector3.Lerp(chassis.position + offset, ball.position + new Vector3(0f, 0f, offset.z), atBall) + (Vector3)GameController.camOffset;

        transform.eulerAngles = new Vector3(0f, 0f, Mathf.LerpAngle(lastAng, ang, (Time.time - Time.fixedTime) / Time.fixedDeltaTime));

        cam ??= GetComponent<Camera>();
        cam.backgroundColor = sky.GetPixelBilinear(Mathf.InverseLerp(skyBottom, skyTop, transform.position.y), 0.5f);
    }
}
