using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanAnimation : MonoBehaviour
{
    private MegamanBuster _buster;
    private CircleCollider2D _frontCollider;
    private Animator _animator;
    private bool _checkAnimation;

    public BoxCollider2D feet;

    void Awake()
    {
        _buster = GetComponentInChildren<MegamanBuster>();
        _frontCollider = GetComponent<CircleCollider2D>();
        _checkAnimation = true;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_checkAnimation)
        {
            checkOnGround();
            checkShooting();
            checkMoving();
        } 
    }

    private void checkShooting()
    {
        if (Input.GetKey(GameManager.instance.buster) && GameManager.instance.isInputEnabled)
        {
            _animator.SetLayerWeight(1, 1);
        }
        else if (_buster.cooldownTimer <= 0)
        {
            _animator.SetLayerWeight(1, 0);
        }
    }

    private void checkOnGround()
    {
        if (MegamanActor.instance.body.velocity.y > 0.1 && Input.GetKey(GameManager.instance.jump))
        {
            _animator.SetBool("IsJumping", true);
        }
        else
        {
            _animator.SetBool("IsJumping", false);
        }

        if (feet.IsTouchingLayers(GameManager.instance.layerGround))
        {
            _animator.SetBool("IsOnGround", true);
            _animator.SetBool("IsFalling", false);
        } else {
            _animator.SetBool("IsOnGround", false);
            _animator.SetBool("IsFalling", true);
        }
    }

    private void checkMoving()
    {
        if (MegamanActor.instance.body.velocity.x != 0 && !_frontCollider.IsTouchingLayers(GameManager.instance.layerGround) && !_frontCollider.IsTouchingLayers(GameManager.instance.layerWall))
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    public void takeDamage()
    {
        GameManager.instance.isInputEnabled = false;
        _checkAnimation = false;
        _animator.SetBool("IsHurt", true);
    }

    public void finishDamage()
    {
        GameManager.instance.isInputEnabled = true;
        _checkAnimation = true;
        _animator.SetBool("IsHurt", false);
        MegamanActor.instance.spriteRenderer.color = new Color(1f, 1f, 1f, .5f);
        _animator.SetBool("IsInvulnerable", true);
    }

    public void becomeVulnerable()
    {
        _animator.SetBool("IsInvulnerable", false);
    }

    public void paralyzeAnimation()
    {
        _animator.SetBool("IsParalyzed", true);
    }

    public void unparalyzeAnimation()
    {
        _animator.SetBool("IsParalyzed", false);
    }

    public void arrive()
    {
        _animator.SetBool("IsArriving", true);
        _checkAnimation = false;
    }

    public void hasArrived()
    {
        _animator.SetBool("IsArriving", false);
        _checkAnimation = true;
    }

    public void die()
    {
        _animator.SetBool("IsDying", true);
    }

    public void victory()
    {
        _animator.SetTrigger("IsVictorious");
    }
}
