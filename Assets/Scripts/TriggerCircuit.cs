using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCircuit : MonoBehaviour, ICircuitElement
{
    public float activeTime = 10f;
    public List<MonoBehaviour> rippleTo = new List<MonoBehaviour>();

    public bool Active => DeactivateAt > Time.fixedTime;
    public float DeactivateAt { get; set; }

    public void OnTriggerStay2D(Collider2D collision)
    {
        DeactivateAt = Time.fixedTime + activeTime;

        foreach(var mb in rippleTo)
        {
            if (!(mb is ICircuitElement ice)) continue;
            ice.DeactivateAt = Mathf.Max(ice.DeactivateAt, DeactivateAt);
        }
    }
}
