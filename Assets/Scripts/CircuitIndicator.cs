using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircuitIndicator : MonoBehaviour
{
    public List<MonoBehaviour> triggers = new List<MonoBehaviour>();

    public Sprite onSprite;
    private Sprite offSprite;

    private SpriteRenderer ren;

    void Update()
    {
        if(ren == null)
        {
            ren = GetComponent<SpriteRenderer>();
            offSprite = ren.sprite;
        }

        ren.sprite = triggers.All(t => !(t is ICircuitElement ice) || ice.Active) ? onSprite : offSprite;
    }
}
