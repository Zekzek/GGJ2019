using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private AudioSource aud;
    public int speed;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.pitch = Random.Range(0.5f, 1.5f);
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 6);
    }

    void Update()
    {
        rb2d.velocity = transform.up * speed;
    }
}
