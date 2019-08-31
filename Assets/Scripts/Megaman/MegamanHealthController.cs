using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MegamanHealthController : MonoBehaviour
{
    private float _initLife = 33;
    private float _currentLife;
    private float _coolDownVulnerable;
    private float _vulnerableDelay = 1.5f;
    private float _lastTouchDamageReceived;

    public Health healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _currentLife = _initLife; 
        healthBar.setLife(_initLife);
    }

    void Update()
    {
        if (_coolDownVulnerable > 0)
        {
            _coolDownVulnerable -= Time.deltaTime;
            if (_coolDownVulnerable <= 0)
            {
                MegamanActor.instance.spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                MegamanActor.instance.megamanAnimation.becomeVulnerable();
            }
        } else if (MegamanActor.instance.body.IsTouchingLayers(GameManager.instance.layerEnemy))
        {
            takeDamage(_lastTouchDamageReceived);
        }
    }

    public void takeDamage(float damage)
    {
        if (_coolDownVulnerable <= 0)
        {
            _coolDownVulnerable = _vulnerableDelay;
            _currentLife -= damage;
            SFXPlayer.instance.Play("megamanHurt");
            if (_currentLife <= 0)
            {
                healthBar.map(0);
                die();
            } else
            {
                healthBar.map(_currentLife);
                MegamanActor.instance.megamanAnimation.takeDamage();
                if (!MegamanActor.instance.body.isKinematic)
                {
                    if (MegamanActor.instance.megamanMovement.facingRight)
                    {
                        MegamanActor.instance.body.velocity = new Vector2(-2, 0);
                    }
                    else
                    {
                        MegamanActor.instance.body.velocity = new Vector2(2, 0);
                    }
                }
            }
        }
    }

    public void die()
    {
        GameManager.instance.isInputEnabled = false;
        MegamanActor.instance.megamanAnimation.die();
        SFXPlayer.instance.Play("megamanDeath");
        StageActor.instance.stageController.restartStage();
    }

    public void touchDamage(float damage)
    {
        takeDamage(damage);
        _lastTouchDamageReceived = damage;
    }

    public void paralyze()
    {
        if (MegamanActor.instance.body.IsTouchingLayers(GameManager.instance.layerGround) && _coolDownVulnerable <= 0 && MegamanActor.instance.body.velocity.y == 0)
        {
            GameManager.instance.isInputEnabled = false;
            MegamanActor.instance.body.velocity = Vector2.zero;
            MegamanActor.instance.megamanAnimation.paralyzeAnimation();
        }
    }

    public void unparalyze() // Called by animation event
    {
        GameManager.instance.isInputEnabled = true;
        MegamanActor.instance.megamanAnimation.unparalyzeAnimation();
    }

    public void gainLife(float lifePoints)
    {
        _currentLife += lifePoints;
        if(_currentLife > _initLife)
        {
            _currentLife = _initLife;
        }
        healthBar.map(_currentLife);
    }
}
