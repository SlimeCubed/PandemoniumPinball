using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    public float volume = 1f;
    public AudioClip[] clips;
    private AudioSource source;

    public void TriggerSound()
    {
        if(source == null)
            source = gameObject.AddComponent<AudioSource>();

        source.PlayOneShot(clips[Random.Range(0, clips.Length)], volume);
    }
}
