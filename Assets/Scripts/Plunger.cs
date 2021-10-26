using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plunger : MonoBehaviour
{
    public Rigidbody2D plunger;
    public float retractTime = 0.5f;
    public float maxPushSpeed = 100f;
    public float minPushSpeed = 20f;
    public AudioSource releaseSource;
    public AudioSource chargeSource;

    private float offset;
    private float maxOffset;
    private float offsetVel;
    private float? lockPushSpeed;

    public void Start()
    {
        maxOffset = plunger.transform.localPosition.y;
        offset = maxOffset;
    }

    public void FixedUpdate()
    {
        bool back = Input.GetButton("Plunger");
        bool retracting = false;

        if (back && lockPushSpeed == null)
        {
            retracting = offset > 0.1f;
            offset = Mathf.SmoothDamp(offset, 0f, ref offsetVel, retractTime);
        }
        else if (!back && lockPushSpeed == null && Mathf.Abs(offset - maxOffset) >= 0.0001f)
        {
            lockPushSpeed = Mathf.Lerp(maxPushSpeed, minPushSpeed, offset / maxOffset);
            releaseSource.volume = lockPushSpeed.Value / maxPushSpeed * 0.7f;
            releaseSource.Play();
        }

        if(retracting)
        {
            float charge = 1f - offset / maxOffset;

            float pitch = charge;

            chargeSource.pitch = pitch * 0.15f + 0.4f;
            chargeSource.volume = 1f - Mathf.Pow(charge, 2f);

            if (!chargeSource.isPlaying)
                chargeSource.Play();
        }
        else
        {
            chargeSource.Stop();
        }

        if(lockPushSpeed is float lps)
        {
            offsetVel = 0f;
            offset = Mathf.MoveTowards(offset, maxOffset, Time.deltaTime * lps);
            if (Mathf.Abs(offset - maxOffset) < 0.0001f)
                lockPushSpeed = null;
        }
        plunger.MovePosition(transform.TransformPoint(new Vector3(0f, offset, 1f)));
    }
}
