using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZone : MonoBehaviour
{
    public CanvasGroup tutorial;
    private bool anythingInZone;
    private float lingerTime;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.attachedRigidbody.bodyType == RigidbodyType2D.Dynamic)
            anythingInZone = true;
    }

    private void FixedUpdate()
    {
        if (anythingInZone)
            lingerTime += Time.deltaTime;
        else
            lingerTime = 0f;
        anythingInZone = false;
    }

    private void Update()
    {
        tutorial.alpha = Mathf.MoveTowards(tutorial.alpha, Mathf.InverseLerp(1f, 1.5f, lingerTime), Time.deltaTime * 5f);
        tutorial.gameObject.SetActive(tutorial.alpha > 0f);
    }
}
