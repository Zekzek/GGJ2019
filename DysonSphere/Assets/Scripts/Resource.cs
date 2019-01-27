using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public enum Type { Stuffium }
    public Type type;
    public float amount;

    public Resource(Type type, float amount)
    {
        this.type = type;
        this.amount = amount;
    }
}
