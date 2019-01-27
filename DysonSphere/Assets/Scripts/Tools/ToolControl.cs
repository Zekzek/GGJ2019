using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ToolControl : MonoBehaviour
{
    protected Ship parentShip;
    public void Start()
    {
        parentShip = GetComponentInParent<Ship>();
    }

    protected void GetInFront(Vector2 size, out Ship nearestShip, out Planet nearestPlanet)
    {
        Vector2 position = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction = transform.TransformDirection(Vector2.up);
        Vector2 point = position + direction * size.y / 2;
        float angle = 90 + Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(point, size, angle);
        GetNearestPlanetOrShip(colliders, out nearestShip, out nearestPlanet);
    }

    private void GetNearestPlanetOrShip(Collider2D[] colliders, out Ship nearestShip, out Planet nearestPlanet)
    {
        nearestShip = null;
        nearestPlanet = null;
        float nearestSqrDistance = float.MaxValue;

        foreach (Collider2D collider in colliders)
        {
            Vector3 toTarget = collider.gameObject.transform.position - transform.position;
            if (toTarget.sqrMagnitude < nearestSqrDistance)
            {
                Ship ship = collider.gameObject.GetComponent<Ship>();
                if (ship != null)
                {
                    nearestShip = ship;
                    nearestPlanet = null;
                    nearestSqrDistance = toTarget.sqrMagnitude;
                }
                Planet planet = collider.gameObject.GetComponent<Planet>();
                if (planet != null)
                {
                    nearestShip = null;
                    nearestPlanet = planet;
                    nearestSqrDistance = toTarget.sqrMagnitude;
                }
            }
        }
    }

    public bool PlayerShip()
    {
        return parentShip.PlayerShip();
    }

    public abstract void DoActivate();
}
