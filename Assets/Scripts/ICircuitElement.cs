using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICircuitElement
{
    public bool Active { get; }
    public float DeactivateAt { get; set; }
}
