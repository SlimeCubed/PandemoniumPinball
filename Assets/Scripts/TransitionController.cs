using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    private static TransitionController instance;
    public static bool InAnimation => instance?.inAnim ?? false;
    public string initScene;
    public RectTransform wipe;
    public float wipeHeight;

    public static void WipeScene(string scene) => instance?.WipeSceneImpl(scene);

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(initScene) && SceneManager.sceneCount == 1)
            SceneManager.LoadScene(initScene);
    }

    private bool inAnim = false;
    private void WipeSceneImpl(string scene)
    {
        if (inAnim) return;
        inAnim = true;
        StartCoroutine(WipeSceneCoroutine(scene));
    }

    private IEnumerator WipeSceneCoroutine(string scene)
    {
        SendMessage("TriggerSound");
        float anim = 0f;
        wipe.gameObject.SetActive(true);
        while(anim < 1f)
        {
            anim = Mathf.Clamp01(anim + Time.deltaTime);
            wipe.sizeDelta = new Vector2(wipe.sizeDelta.x, Mathf.SmoothStep(0f, wipeHeight, anim));
            yield return null;
        }
        SceneManager.LoadScene(scene);
        anim = 0f;
        while (anim < 1f)
        {
            anim = Mathf.Clamp01(anim + Time.deltaTime);
            wipe.sizeDelta = new Vector2(wipe.sizeDelta.x, Mathf.SmoothStep(wipeHeight, 0f, anim));
            yield return null;
        }

        wipe.gameObject.SetActive(false);
        inAnim = false;
    }

    public static IEnumerator FakeWipe() => instance?.FakeWipeImpl();

    public IEnumerator FakeWipeImpl()
    {
        SendMessage("TriggerSound");
        float fakeoutDepth = 0.7f;
        float anim = 0f;
        wipe.gameObject.SetActive(true);
        while (anim < 1f)
        {
            anim = Mathf.Clamp01(anim + Time.deltaTime / fakeoutDepth);
            wipe.sizeDelta = new Vector2(wipe.sizeDelta.x, Mathf.SmoothStep(0f, wipeHeight * fakeoutDepth, anim));
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
        anim = 0f;
        while (anim < 1f)
        {
            anim = Mathf.Clamp01(anim + Time.deltaTime / fakeoutDepth);
            wipe.sizeDelta = new Vector2(wipe.sizeDelta.x, Mathf.SmoothStep(wipeHeight * fakeoutDepth, 0f, anim));
            yield return null;
        }

        wipe.gameObject.SetActive(false);
        inAnim = false;
    }
}
