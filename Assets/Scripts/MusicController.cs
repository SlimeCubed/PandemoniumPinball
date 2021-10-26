using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public float fadeSpeed = 0.5f;
    private Scene originScene;
    private float maxVolume;
    private float volumeMul;
    private static bool muted;

    void Start()
    {
        originScene = gameObject.scene;
        DontDestroyOnLoad(gameObject);
    }

    private AudioSource source;
    void Update()
    {
        if(Input.GetButtonDown("Mute") && originScene.isLoaded)
        {
            muted = !muted;
        }

        float targetVolume = (TransitionController.InAnimation || !originScene.isLoaded) ? 0f : 1f;
        if (muted) targetVolume = 0f;
        volumeMul = Mathf.MoveTowards(volumeMul, targetVolume, fadeSpeed * Time.deltaTime);

        if(source == null)
        {
            source = GetComponent<AudioSource>();
            maxVolume = source.volume;
        }
        source.volume = volumeMul * maxVolume;

        if(volumeMul == 0f && !originScene.isLoaded)
        {
            Destroy(gameObject);
        }
    }
}
