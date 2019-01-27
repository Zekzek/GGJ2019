using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlanetResource
{
    private const float REPLEN_MULTIPLIER = 1.07f;
    private const float MIN = 100;

    [SerializeField]
    private Resource.Type type;

    [SerializeField]
    private float max;

    [SerializeField]
    private float current;

    [SerializeField]
    private float replenPeriod = 5f;

    private float secondsToNextReplen;

    public float Harvestable { get { return current - MIN; } }

    public PlanetResource(Resource.Type type, float max, float current, float replenPeriod)
    {
        this.type = type;
        this.max = max;
        this.current = current;
        this.secondsToNextReplen = this.replenPeriod = replenPeriod;
    }

    public void Update(float deltaTime)
    {
        if (current < max)
        {
            secondsToNextReplen -= deltaTime;
            if (secondsToNextReplen <= 0)
            {
                secondsToNextReplen = replenPeriod;
                current *= REPLEN_MULTIPLIER;
                if (current > max)
                    current = max;
            }
        }
    }

    public Resource harvest(float amount)
    {
        if (amount > Harvestable)
            amount = Harvestable;
        current -= amount;
        return new Resource(type, amount);
    }
}
