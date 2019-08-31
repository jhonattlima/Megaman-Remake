using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleOrNeedle : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<MegamanActor>())
        {
            collision.collider.GetComponent<MegamanActor>().body.velocity = Vector2.zero;
            collision.collider.GetComponent<MegamanHealthController>().die();
        }
    }
}
