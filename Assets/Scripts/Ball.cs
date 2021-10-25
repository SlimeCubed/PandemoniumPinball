using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void Start()
    {
        if (BallSelector.selectedSprite != null)
            GetComponent<SpriteRenderer>().sprite = BallSelector.selectedSprite;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * 30f;
        }
    }
}
