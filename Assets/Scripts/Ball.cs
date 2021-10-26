using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public List<ImpactSound> impacts = new List<ImpactSound>();

    private void Start()
    {
        if (BallSelector.selectedSprite != null)
            GetComponent<SpriteRenderer>().sprite = BallSelector.selectedSprite;
    }

    private void FixedUpdate()
    {
        /*if (Input.GetKey(KeyCode.Mouse1))
        {
            var rb = GetComponent<Rigidbody2D>();
            rb.velocity = ((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position).normalized * 30f;
        }*/
    }

    private AudioSource source;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ImpactSound snd = GetImpactSound(collision.collider.sharedMaterial);
        float force = Vector2.Dot(collision.relativeVelocity, collision.GetContact(0).normal);
        float vol = Mathf.Lerp(snd.minVol, snd.maxVol, Mathf.InverseLerp(snd.minForce, snd.maxForce, force));
        if(vol > 0f && snd.sounds?.Length > 0)
        {
            source ??= GetComponent<AudioSource>();

            source.PlayOneShot(snd.sounds[Random.Range(0, snd.sounds.Length)], vol);
            source.pitch = Mathf.Lerp(snd.minPitch, snd.maxPitch, Mathf.InverseLerp(snd.minForce, snd.maxForce, force));
        }
    }

    private ImpactSound GetImpactSound(PhysicsMaterial2D mat)
    {
        for(int i = 0; i < impacts.Count; i++)
        {
            if (impacts[i].material == mat) return impacts[i];
        }
        return default;
    }

    [System.Serializable]
    public struct ImpactSound
    {
        public PhysicsMaterial2D material;
        public AudioClip[] sounds;
        public float minForce;
        public float maxForce;
        public float minVol;
        public float maxVol;
        public float minPitch;
        public float maxPitch;
    }
}
