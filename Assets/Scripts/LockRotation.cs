using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    public Vector3 targetRotation;

    public void Update()
    {
        transform.eulerAngles = targetRotation;
    }
}
