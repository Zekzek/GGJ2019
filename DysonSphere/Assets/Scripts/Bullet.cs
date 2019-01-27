using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public int speed;
    public int damage = 25;
    public Ship parentShip;

    public float hitForce;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 6);
    }

    void Update()
    {
        rb2d.velocity = transform.up * speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>())
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();

            if (rb)
            {
                rb.velocity = (rb2d.velocity * hitForce);
                Destroy(gameObject);
            }

            Ship ship = collision.gameObject.GetComponent<Ship>();
            if (ship)
            {
                ship.TakeDamage(damage, parentShip);
            }
        }
    }
}
