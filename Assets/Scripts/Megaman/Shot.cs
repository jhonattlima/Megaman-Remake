using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public float speed { get; private set; }
    public int damage { get; private set; }

    public void Initialize(float speed, int damage)
    {
        this.speed = speed;
        this.damage = damage;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
    }

    void OnBecameInvisible() // Destroy the shot if out of the screen
    {
        Destroy(gameObject);
    }
}
