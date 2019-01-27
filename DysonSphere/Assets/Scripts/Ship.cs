using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IScannable
{
    public PlanetPresetDB planetDB;
    public SpriteRenderer[] landDetailSpr;
    public SpriteRenderer[] waterSpr;
    public SpriteRenderer[] windSpr;
    public GameObject explosion;
    private List<Resource> resources = new List<Resource>();
    public int Health { get; set; }
    private AIController ai;

    public AIController.RelationshipStatus PlayerRelationShip { get { return ai == null ? AIController.RelationshipStatus.Neutral : ai.PlayerRelationship; } }

    public float TotalResources
    {
        get
        {
            float total = 0;
            foreach (Resource resource in resources)
            {
                total += resource.amount;
            }
            return total;
        }
    }

    private void Start()
    {
        setPreset();
        GetComponent<DistanceJoint2D>().connectedBody = GameObject.FindGameObjectWithTag("center").GetComponent<Rigidbody2D>();
        Health = 100;

        GameState.Instance.AddShip(this);
        resources.Add(new Resource(Resource.Type.Stuffium, 100));
        ai = GetComponent<AIController>();

        if (PlayerShip())
        {
            GameState.Instance.player.Ship = GetComponent<Ship>();
        }
    }

    public void AddRandomResource(float amount)
    {
        int resourceIndex = Random.Range(0, resources.Count);
        resources[resourceIndex].amount += amount;

        if (PlayerShip())
        {
            GameState.Instance.player.OnResourceChange?.Invoke();
        }
    }

    public Resource TakeResource(float amount, Ship damageSource)
    {
        int resourceIndex = Random.Range(0, resources.Count);
        if (resources[resourceIndex].amount < amount)
        {
            amount = resources[resourceIndex].amount;
        }
        resources[resourceIndex].amount -= amount;

        if (PlayerShip())
        {
            GameState.Instance.player.OnResourceChange?.Invoke();
        }

        if (ai != null && damageSource != null)
            ai.TookDamageFrom(damageSource);

        return new Resource(resources[resourceIndex].type, amount);
    }

    public void TakeDamage(int damage, Ship damageSource)
    {
        if (ai != null)
            ai.TookDamageFrom(damageSource);
        Health -= damage;
        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
        GameState.Instance.RemoveShip(this);
        GameState.Instance.CheckWin();
        Destroy(gameObject);
    }

    private void setPreset()
    {
        int i = Random.Range(0, planetDB.planets.Length);
        var landSpr = GetComponent<SpriteRenderer>();

        landSpr.color = planetDB.planets[i].landColor;

        windSpr[1].transform.parent.GetComponent<ScrollMaterial>().speed = planetDB.planets[i].windspeed;
        landDetailSpr[1].transform.parent.GetComponent<ScrollMaterial>().speed = planetDB.planets[i].spinspeed;

        foreach (SpriteRenderer sp in landDetailSpr)
        {
            sp.color = planetDB.planets[i].landDetailColor;
        }

        foreach (SpriteRenderer sp in waterSpr)
        {
            sp.color = planetDB.planets[i].waterColor;
        }

        foreach (SpriteRenderer sp in windSpr)
        {
            sp.color = planetDB.planets[i].cloudColor;
        }

    }

    public void Appreciate(Ship ship)
    {
        if (ai != null)
            ai.Appreciate(ship);
    }

    public void TalkTo(Ship ship)
    {
        //TODO
    }

    public void TalkTo(Planet planet)
    {
        //TODO
    }

    public void ScanMe()
    {
        // Pop up a display that shows resources.
    }

    public bool PlayerShip()
    {
        return ai == null;
    }
}
