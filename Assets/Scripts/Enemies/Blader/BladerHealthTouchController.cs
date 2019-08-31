using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladerHealthTouchController : MonoBehaviour
{
    private float _touchDamage = 3;
    private int _life = 1;
    private int _score = 500;
    private bool _isDead = false; // Avoid multiple drop

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MegamanActor>())
        {
            MegamanActor.instance.megamanHealthController.touchDamage(_touchDamage);
        }
        else if (collision.GetComponent<Shot>())
        {
            int damage = collision.GetComponent<Shot>().damage;
            Destroy(collision.gameObject);
            takeDamage(damage);
        }
    }

    void takeDamage(int damage)
    {
        _life -= damage;
        if (_life <= 0)
        {
            if (!_isDead)
            {
                _isDead = true;
                GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                SFXPlayer.instance.Play("enemyDying");
                GetComponent<Animator>().SetTrigger("IsDead");
            }
        } else
        {
            SFXPlayer.instance.Play("enemyHit");
        }
    }

    private void die() // Called by animation event
    {
        StageActor.instance.stageController.scoreController.updateScore(_score);
        StageActor.instance.dropController.dropTry(this.transform);
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
