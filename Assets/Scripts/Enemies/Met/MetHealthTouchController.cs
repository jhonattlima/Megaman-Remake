using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetHealthTouchController : MonoBehaviour
{
    private float _touchDamage = 1;
    private int _life = 1;
    private bool _isDead = false; // Avoid multiple drop

    public bool isInvulnerable = true;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MegamanActor>())
        {
            MegamanActor.instance.megamanHealthController.touchDamage(_touchDamage);
        } else if (collision.GetComponent<Shot>()){
            if (isInvulnerable)
            {
                Destroy(collision.gameObject);
                SFXPlayer.instance.Play("enemyHitInvulnerable");
            } else
            {
                int damage = collision.GetComponent<Shot>().damage;
                Destroy(collision.gameObject);
                takeDamage(damage); 
            }
        }

        if (GetComponent<BoxCollider2D>().IsTouchingLayers(GameManager.instance.layerGround))
        {
            GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    void takeDamage(int damage)
    {
        _life -= damage;
        if(_life <= 0)
        {
            if (!_isDead)
            {
                _isDead = true;
                SFXPlayer.instance.Play("enemyDying");
                GetComponentInParent<Animator>().SetTrigger("IsDead");
            }
        } else
        {
            SFXPlayer.instance.Play("enemyHit");
        }
    }
}
