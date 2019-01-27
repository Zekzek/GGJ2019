using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitch : MonoBehaviour
{
    public float range;
    private AudioSource aud;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.pitch = Random.Range(-range + 1, range + 1);
    }
}
