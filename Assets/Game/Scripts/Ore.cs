using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    public float lifePoints = 3;
    public GameObject drop;
    public void takeDamage(float damage)
    {
        lifePoints -= damage;
        if (lifePoints <= 0)
        {
            Instantiate(drop, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}