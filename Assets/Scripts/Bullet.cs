using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;

    void OnTriggerEnter(Collider other)
    {
        if(!isMelee && other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
