using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEnergy : MonoBehaviour
{
    public float amountHealed;
    public bool persist = false;
    public float timeTodisappear = 5;

    private void Start()
    {
        if (!persist)
        {
            StartCoroutine(startLifeCycle());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<MegamanActor>())
        {
            SFXPlayer.instance.Play("pickEnergy");
            collision.collider.GetComponent<MegamanHealthController>().gainLife(amountHealed);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        if (!persist)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator startLifeCycle()
    {
        yield return new WaitForSeconds(timeTodisappear);
        StartCoroutine(Shine());
        yield return new WaitForSeconds(timeTodisappear);
        Destroy(gameObject);
    }


    private IEnumerator Shine() // Flashes gutsman when he is invulnerable
    {
        Color temp = GetComponent<SpriteRenderer>().color;
        while (true)
        {
            temp.a = 0;
            GetComponent<SpriteRenderer>().color = temp;
            yield return new WaitForSeconds(.1f);
            temp.a = 1;
            GetComponent<SpriteRenderer>().color = temp;
            yield return new WaitForSeconds(.1f);
        }
    }
}
