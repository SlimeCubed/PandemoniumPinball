using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectArrow : MonoBehaviour
{
    public Vector2 target;

    private RectTransform rt;
    private Vector2 vel;

    private void Update()
    {
        rt ??= GetComponent<RectTransform>();

        if (Vector2.Distance(rt.anchoredPosition, target) > 500f)
        {
            rt.anchoredPosition = target;
            vel = new Vector2();
        }
        else
            rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, target, ref vel, 0.1f);
    }
}
