using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public RectTransform talkSprite;
    public float charsPerSec;

    public bool Open => isUp || up > 0f;
    public bool Talking => text.maxVisibleCharacters < text.text.Length;

    private bool isUp = false;
    private const float upTime = 0.1f;
    private float up = 0f;
    private float upVel = 0f;

    private float nextCharTimer;
    private RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (up == 1f && Talking)
        {
            nextCharTimer += Time.deltaTime * charsPerSec;
            text.maxVisibleCharacters += (int)nextCharTimer;
            nextCharTimer %= 1f;
        }

        float target = isUp ? 1f : 0f;
        up = Mathf.SmoothDamp(up, target, ref upVel, upTime);
        if (Mathf.Abs(up - target) < 0.01f)
        {
            up = target;
            upVel = 0f;
        }

        rt.pivot = new Vector2(rt.pivot.x, 1f - up);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, Mathf.Lerp(-10f, 50f, up));

        if(Input.GetButtonDown("Flippers"))
        {
            if(Talking)
            {
                text.maxVisibleCharacters = text.text.Length;
                up = 1f;
                upVel = 0f;
            }
            else if(isUp)
            {
                isUp = false;
            }
            else
            {
                up = 0f;
                upVel = 0f;
            }
        }
    }

    public void NewMessage(string message)
    {
        text.text = message.Replace("{score}", GameController.score.ToString());
        
        nextCharTimer = 0f;
        text.maxVisibleCharacters = 0;

        isUp = true;
    }
}
