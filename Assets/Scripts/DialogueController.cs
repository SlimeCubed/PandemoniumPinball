using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class DialogueController : MonoBehaviour
{
    public string startConvo;
    public float initialDelay = 1f;
    public List<DialogueBox> speakerList = new List<DialogueBox>();

    [SerializeField]
    private List<ConvoDesc> convoList = new List<ConvoDesc>();
    private RectTransform talkSprite;
    private Coroutine talkSpriteAnim;

    private void Start()
    {
        StartCoroutine(ConvoLoop());
    }

    IEnumerator ConvoLoop()
    {
        string nextConvo = startConvo;
        yield return new WaitForSeconds(initialDelay);
        if (GetConvo(nextConvo, out var convoAsset))
            yield return DoConvo(convoAsset);
    }

    IEnumerator DoConvo(TextAsset convo)
    {
        var tr = new StringReader(convo.text);
        string line;
        int ln = 0;

        while((line = tr.ReadLine()) != null)
        {
            ln++;

            switch(line.Length > 0 ? line[0] : ' ')
            {
                case '{':
                    if(talkSprite != null) AnimTalkSprite(talkSprite, null, true);
                    break;

                case '}':
                    if (talkSprite != null) AnimTalkSprite(talkSprite, null, false);
                    break;

                case '>':
                    if (!GetSpeaker(line.Substring(1), out DialogueBox tsSource) || tsSource.talkSprite == null)
                    {
                        Debug.LogWarning($"Line {ln} of {convo.name} has an invalid talk sprite name!");
                        continue;
                    }
                    AnimTalkSprite(tsSource.talkSprite, true, false);
                    break;

                case '.':
                    if (float.TryParse(line.Substring(1), out float wait))
                        yield return new WaitForSeconds(wait);
                    break;

                case '@':
                    TransitionController.WipeScene(line.Substring(1));
                    break;

                case '<':
                    if (talkSprite != null) AnimTalkSprite(talkSprite, false, null);
                    break;

                case '!':
                    string[] args = line.Substring(1).Split(' ');
                    yield return Special(args);
                    break;

                default:
                    int colon = line.IndexOf(':');
                    if (colon == -1)
                    {
                        Debug.LogWarning($"Line {ln} of {convo.name} is missing a colon!");
                        continue;
                    }

                    string speakerName = line.Substring(0, colon);
                    string text = line.Substring(colon + 1);

                    if (!GetSpeaker(speakerName, out var speaker))
                    {
                        Debug.LogWarning($"Unknown speaker: {speaker}");
                        continue;
                    }

                    speaker.NewMessage(text);
                    yield return new WaitForDelegate(() => !speaker.Open);
                    break;
            }
        }
        yield break;
    }

    private IEnumerator Special(string[] args)
    {
        switch(args[0])
        {
            case "Fakeout":
                yield return TransitionController.FakeWipe();
                break;
            case "Balls":
                yield return FindObjectOfType<BallSelector>().DoSelection();
                break;
            case "Get":
                yield return DoGetAnim(string.Join(" ", args, 1, args.Length - 1));
                break;
            default: Debug.LogWarning("Unknown special event: " + args[0]); break;
        }
    }

    private IEnumerator DoGetAnim(string text)
    {
        var getBox = transform.parent.Find("Get Box");
        var getText = getBox.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        var getSprite = transform.parent.Find("Get").GetComponent<UnityEngine.UI.Image>();

        getBox.gameObject.SetActive(true);

        float t = 0f;

        getText.text = text;
        getText.maxVisibleCharacters = 0;

        yield return new WaitForSeconds(0.1f);
        while(!Input.GetButtonDown("Flippers") && t < 3f)
        {
            t += Time.deltaTime;
            getText.maxVisibleCharacters = (int)(t * 15f);

            getSprite.gameObject.SetActive(true);
            getSprite.rectTransform.anchoredPosition = new Vector2(t * 125f, t * 125f) + new Vector2(-5f, 5f) * Mathf.Sin(t * 20f);
            getSprite.color = new Color(1f, 1f, 1f, Mathf.InverseLerp(1f, 0.5f, t));

            yield return null;
        }

        getText.maxVisibleCharacters = 0;

        yield return new WaitForSeconds(0.1f);

        getSprite.gameObject.SetActive(false);
        getBox.gameObject.SetActive(false);
    }

    private void AnimTalkSprite(RectTransform rt, bool? active, bool? mini)
    {
        if (talkSpriteAnim != null)
        {
            talkSprite.gameObject.SetActive(false);
            StopCoroutine(talkSpriteAnim);
        }

        talkSprite = rt;
        talkSpriteAnim = StartCoroutine(AnimTalkSpriteCoroutine(rt, active, mini));
    }

    private IEnumerator AnimTalkSpriteCoroutine(RectTransform rt, bool? active, bool? mini)
    {
        UnityEngine.UI.Image img = rt.GetComponent<UnityEngine.UI.Image>();

        Vector2 startPos;
        Vector2 endPos;
        float pivotStart;
        float pivotEnd;
        if(active == null)
        {
            startPos = rt.anchoredPosition;
            endPos = startPos;
            pivotStart = rt.pivot.x;
            pivotEnd = pivotStart;
        }
        else
        {
            startPos = new Vector2(-20f, rt.anchoredPosition.y);
            endPos = new Vector2(80f, rt.anchoredPosition.y);
            pivotStart = 1f;
            pivotEnd = 0f;
            if(!active.Value)
            {
                (startPos, endPos) = (endPos, startPos);
                (pivotStart, pivotEnd) = (pivotEnd, pivotStart);
            }
        }

        Vector3 startScale;
        Vector3 endScale;
        Color startCol;
        Color endCol;
        if(mini == null)
        {
            startScale = rt.localScale;
            endScale = startScale;
            startCol = img.color;
            endCol = startCol;
        }
        else
        {
            startScale = new Vector3(1f, 1f, 1f);
            endScale = new Vector3(0.9f, 0.9f, 1f);
            startCol = new Color(0.5f, 0.5f, 0.5f, 1f / 255f);
            endCol = new Color(0.5f, 0.5f, 0.5f, 0.35f);
            if(!mini.Value)
            {
                (startScale, endScale) = (endScale, startScale);
                (startCol, endCol) = (endCol, startCol);
            }
        }

        rt.gameObject.SetActive(true);
        float anim = 0f;
        float animVel = 0f;
        while(anim < 1f)
        {
            anim = Mathf.SmoothDamp(anim, 1f, ref animVel, 0.1f);
            if (anim > 0.99f) anim = 1f;

            img.color = Color.Lerp(startCol, endCol, anim);
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, anim);
            rt.pivot = new Vector2(Mathf.Lerp(pivotStart, pivotEnd, anim), rt.pivot.y);
            rt.localScale = Vector3.Lerp(startScale, endScale, anim);
            yield return null;
        }
    }

    private bool GetConvo(string name, out TextAsset convo)
    {
        convo = convoList.FirstOrDefault(c => string.Equals(name, c.name, StringComparison.OrdinalIgnoreCase)).text;
        return convo != null;
    }

    private bool GetSpeaker(string name, out DialogueBox speaker)
    {
        speaker = speakerList.FirstOrDefault(s => string.Equals(name, s.name, StringComparison.OrdinalIgnoreCase));
        return speaker != null;
    }

    private class WaitForDelegate : CustomYieldInstruction
    {
        private Func<bool> condition;

        public WaitForDelegate(Func<bool> condition)
        {
            this.condition = condition;
        }

        public override bool keepWaiting => !condition();
    }

    [Serializable]
    private struct ConvoDesc
    {
        public string name;
        public TextAsset text;
    }
}
