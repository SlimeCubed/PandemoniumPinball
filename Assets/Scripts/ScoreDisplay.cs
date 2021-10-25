using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TextField = TMPro.TextMeshProUGUI;

public class ScoreDisplay : MonoBehaviour
{
    public TextField scoreLabel;
    public TextField scoreAdderLabel;
    public TextField highScoreLabel;
    public RectTransform highScoreSign;
    public RectTransform downArrow;
    public int highScoreThreshold;
    public float highScoreSignOffset;

    private float addDelay;
    private float addCounter;
    private float drainSpeed;
    private int displayScore;
    private int lastScore;
    private Vector2 highScoreSignVel;
    private MainCamera cam;

    // Update is called once per frame
    void Update()
    {
        if(lastScore != GameController.score)
        {
            lastScore = GameController.score;
            addDelay = 0.75f;
        }

        if(addDelay > 0f)
        {
            addDelay = Mathf.MoveTowards(addDelay, 0f, Time.deltaTime);
            if (addDelay == 0f)
                drainSpeed = Mathf.Max(5f, (GameController.score - displayScore) / 0.75f);
        }
        else if(displayScore != GameController.score)
        {
            addCounter += drainSpeed * Time.deltaTime;
            int intAddCounter = (int)addCounter;
            addCounter -= intAddCounter;

            displayScore = System.Math.Min(displayScore + intAddCounter, GameController.score);
        }

        scoreLabel.text = $"{displayScore}";
        if (displayScore != GameController.score)
        {
            scoreAdderLabel.text = $"+{GameController.score - displayScore}";
            scoreAdderLabel.gameObject.SetActive(true);
        }
        else
            scoreAdderLabel.gameObject.SetActive(false);

        bool highScore = GameController.score > highScoreThreshold;

        cam ??= FindObjectOfType<MainCamera>();

        downArrow.gameObject.SetActive(highScore && cam?.transform.position.y > 10f);
        highScoreSign.gameObject.SetActive(highScore);
        if(highScore)
        {
            highScoreSign.anchoredPosition = Vector2.SmoothDamp(highScoreSign.anchoredPosition, new Vector2(0f, highScoreSignOffset), ref highScoreSignVel, 0.2f);
            highScoreLabel.gameObject.SetActive(Time.time % 1f < 0.5f);

            downArrow.anchoredPosition = new Vector2(0f, Mathf.Sin(Time.time * Mathf.PI * 2f) * 10f);
        }
    }
}
