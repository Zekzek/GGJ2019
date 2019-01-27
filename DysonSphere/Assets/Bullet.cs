using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public int speed;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 6);
    }

    void Update()
    {
        rb2d.velocity = transform.up * speed;
    }
}
