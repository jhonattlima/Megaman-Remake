using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutsmanHealthTouchController : MonoBehaviour
{
    private float _touchDamage = 4;
    private int _life = 1; // Default = 28
    private float _vulnerableDelay = 1f;
    private float _coolDownVulnerable = 0f;
    private bool _toogleShine = false;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    public Health healthBar;
    public Animator explosionAnimator;

    private void Start()
    {
        _spriteRenderer = GetComponentInParent<SpriteRenderer>();
        healthBar.gameObject.SetActive(true);
        healthBar.setLife(_life);  
    }

    private void Update()
    {
        
        if (_coolDownVulnerable > 0)
        {
            _coolDownVulnerable -= Time.deltaTime;
            if(_coolDownVulnerable <= 0)
            {
                _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                _toogleShine = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Shot>())
        {
            if (_coolDownVulnerable > 0)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                int damage = collision.GetComponent<Shot>().damage;
                Destroy(collision.gameObject);
                takeDamage(damage);
            }
        } else if (collision.GetComponent<MegamanActor>())
        {
            MegamanActor.instance.megamanHealthController.touchDamage(_touchDamage);
        }
    }

    private  void takeDamage(int damage)
    {
        if (_coolDownVulnerable <= 0)
        {
            _life -= damage;
            if (_life <= 0)
            {
                healthBar.map(0);
                SFXPlayer.instance.Play("gutsmanDeath");
                GameManager.instance.isInputEnabled = false;
                GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponentInParent<GutsmanFSM>().enabled = false;
                explosionAnimator.SetTrigger("GutsmanDead");
                this.enabled = false;
            }
            else
            {
                _coolDownVulnerable = _vulnerableDelay;
                SFXPlayer.instance.Play("gutsmanDamage");
                healthBar.map(_life);
                StartCoroutine(toogleShine());
            }
        }
    }

    private IEnumerator toogleShine() // Flashes gutsman when he is invulnerable
    {
        _toogleShine = true;
        while (_toogleShine)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }
}
