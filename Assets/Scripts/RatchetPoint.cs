using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RatchetPoint : MonoBehaviour
{
    public float flipperOffset = -3f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
            Chassis.AddToPath(this);
    }

    public void OnDrawGizmos()
    {
        var mat = Gizmos.matrix;

        Gizmos.color = Color.green;
        for (int side = -1; side <= 1; side += 2)
        {
            Gizmos.matrix = Matrix4x4.TRS(transform.position + new Vector3(0f, flipperOffset, 0f), Quaternion.identity, new Vector3(side, 1f, 1f));
            Gizmos.DrawLine(new Vector3(-4.5f, 0.575f, 0f), new Vector3(-2f, -1.85f, 0f));
            Gizmos.DrawLine(new Vector3(-9f, 2f, 0f), new Vector3(-5f, 0.75f, 0f));
        }

        Gizmos.matrix = mat;
    }
}
