using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public int damage { get; private set; }

    public void Initialize(int damage)
    {
        this.damage = damage;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<MegamanActor>())
        {
            collider.GetComponent<MegamanActor>().megamanHealthController.takeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible() // Destroy the shot if out of the screen
    {
        if(transform.position.y < MegamanActor.instance.transform.position.y)
        {
            Destroy(gameObject);
        }
    }   
}
