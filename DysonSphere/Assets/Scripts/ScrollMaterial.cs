using UnityEngine;
using System.Collections;

public class ScrollMaterial : MonoBehaviour
{
    public float range = 5;
    public float speed = 10f;

    public Vector3 pos;
    void Start()
    {
        pos = new Vector2(Random.Range(-range, range), Random.Range(-range, range));
    }
    void Update()
    {
        transform.localPosition = new Vector2((pos.x + speed * Time.time) % (range * 2) - range, pos.y);
    }
}
