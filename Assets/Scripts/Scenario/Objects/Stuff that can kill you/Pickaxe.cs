using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public int damage { get; private set; }
    public bool rotateDirection { get; private set; }
    public Vector2 forceApplied { get; private set; }

    public void Initialize(int damage, bool rotateDirection, Vector2 forceApplied)
    {
        this.damage = damage;
        this.rotateDirection = rotateDirection;
        this.forceApplied = forceApplied;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(forceApplied);
        if (rotateDirection)
        {
            GetComponent<Rigidbody2D>().rotation = 90f;
        } else
        {
            GetComponent<Rigidbody2D>().rotation = -90f;
        }  
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<MegamanActor>())
        {
            collider.GetComponent<MegamanActor>().megamanHealthController.takeDamage(damage);
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
