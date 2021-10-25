using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallSelector : MonoBehaviour
{
    public static Sprite selectedSprite;

    public List<Sprite> sprites = new List<Sprite>();
    public int defaultSprite;
    public GameObject ballPrefab;
    public SelectArrow selectArrow;

    private List<RectTransform> balls = new List<RectTransform>();
    private float ballAlpha;

    public IEnumerator DoSelection()
    {
        var rt = GetComponent<RectTransform>();

        for (int i = 0; i < sprites.Count; i++)
        {
            var inst = Instantiate(ballPrefab, rt);
            inst.SetActive(true);
            var ballRt = inst.GetComponent<RectTransform>();
            var ballImg = inst.GetComponent<Image>();

            ballImg.sprite = sprites[i];
            ballImg.color = new Color(1f, 1f, 1f, 0f);
            ballRt.anchoredPosition = new Vector2(Mathf.Lerp(rt.offsetMin.x, rt.offsetMax.x, i / (sprites.Count - 1f)), 0f);

            balls.Add(ballRt);
        }

        ballAlpha = 1f;
        yield return new WaitForSeconds(0.25f);
        if (Input.GetButtonDown("Flippers")) yield return null;
        selectArrow.gameObject.SetActive(true);

        int selection = defaultSprite;
        float lastLR = 0f;
        while(!Input.GetButtonDown("Flippers"))
        {
            selectArrow.target = balls[selection].anchoredPosition;

            float lr = Input.GetAxisRaw("Tilt");
            if (Mathf.Abs(lr) < 0.25f) lr = 0f;
            
            if(System.Math.Sign(lr) != System.Math.Sign(lastLR) && lr != 0f)
            {
                selection += lr > 0f ? 1 : -1;
                selection = (selection + sprites.Count) % sprites.Count;
            }
            lastLR = lr;

            yield return null;
        }
        selectArrow.gameObject.SetActive(false);
        selectedSprite = sprites[selection];
        ballAlpha = 0f;

        yield return null;
    }

    private void Update()
    {
        const float speed = 2f;

        foreach(var ball in balls)
        {
            var img = ball.GetComponent<Image>();

            Color c = img.color;
            c.a = Mathf.MoveTowards(c.a, ballAlpha, Time.deltaTime * speed);
            img.color = c;
            ball.eulerAngles = new Vector3(0f, 0f, Time.time * 50f);
        }
    }
}
