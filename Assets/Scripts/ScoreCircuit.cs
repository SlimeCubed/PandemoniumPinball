using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreCircuit : MonoBehaviour
{
    public int score = 100;
    public float clearDelay = 1f;
    public List<MonoBehaviour> triggers = new List<MonoBehaviour>();
    private float clearTimer;

    void FixedUpdate()
    {
        bool active = triggers.All(t => !(t is ICircuitElement ice) || ice.Active);

        if (active && clearTimer == 0f)
        {
            clearTimer = clearDelay;
        }

        if(clearTimer > 0f)
        {
            clearTimer = Mathf.Max(clearTimer - Time.deltaTime, 0f);
            if(clearTimer == 0f)
            {
                GameController.GiveScore(score);
                foreach (var trig in triggers)
                {
                    if (trig is ICircuitElement ice)
                        ice.DeactivateAt = 0f;
                }
            }
        }
    }
}
