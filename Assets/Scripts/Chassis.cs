using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chassis : MonoBehaviour
{
    private static Chassis instance;
    private readonly LinkedList<RatchetPoint> path = new LinkedList<RatchetPoint>();

    public Transform[] wings = new Transform[0];
    public Transform bodies;
    public float moveSpeed = 1f;
    public AudioClip flippersUp;
    public AudioClip flippersDown;

    private float wingSpeed = 1f;
    private float wingAnim;
    private Flipper[] flippers;
    private readonly List<(Rigidbody2D, Transform)> targets = new List<(Rigidbody2D, Transform)>();

    public void Awake()
    {
        instance = this;

        var rbs = bodies.GetComponentsInChildren<Rigidbody2D>();
        foreach(var rb in rbs)
        {
            var target = transform.Find(rb.gameObject.name);
            if(target != null)
                targets.Add((rb, target));
        }
    }

    public static void AddToPath(RatchetPoint ratchetPoint)
    {
        if (instance == null) return;

        while (instance.path.Contains(ratchetPoint))
        {
            while (instance.path.Last.Value != ratchetPoint)
                instance.path.RemoveLast();
            instance.path.RemoveLast();
        }
        instance?.path.AddLast(ratchetPoint);
    }

    private AudioSource source;
    public void Update()
    {
        wingAnim += wingSpeed * Time.deltaTime;
        foreach(var wing in wings)
        {
            wing.localEulerAngles = new Vector3(0f, 0f, Mathf.Sin(wingAnim * Mathf.PI * 1.75f) * 30f);
        }

        source ??= GetComponent<AudioSource>();
        if (Input.GetButtonDown("Flippers"))
        {
            source.PlayOneShot(flippersUp);
        }
        else if (Input.GetButtonUp("Flippers"))
        {
            source.PlayOneShot(flippersDown);
        }
    }

    private List<Collider2D> colliders = new List<Collider2D>();
    public void FixedUpdate()
    {
        if (flippers == null)
            flippers = bodies.GetComponentsInChildren<Flipper>();

        foreach (var flipper in flippers)
            flipper.lockFlippers = path.Count > 0;

        wingSpeed = 1f;

        if (path.Count == 0) return;

        Vector3 targetPos = path.First.Value.transform.position + new Vector3(0f, path.First.Value.flipperOffset, 0f);

        bool noCollide = targetPos.y < transform.position.y;

        wingSpeed = noCollide ? 0.35f : 1.75f;

        if (Vector3.Distance(transform.position, targetPos) < 0.001f)
        {
            path.RemoveFirst();
            noCollide = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        foreach(var pair in targets)
        {
            for(int i = pair.Item1.GetAttachedColliders(colliders) - 1; i >= 0; i--)
                colliders[i].isTrigger = noCollide;
            pair.Item1.MovePosition(pair.Item2.position);
        }
    }
}
