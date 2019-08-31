using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicketManFSM : MonoBehaviour
{
    private PicketManState _currentState;
    private int _attackDamage = 2; // Damage caused by pickaxe
    private float _attackForce = 270; //Force applied to pickaxe when throwned
    private float _moveSpeed = 5; // Velocity it moves before landing
    private bool _facingRight = false;
    private float _attackFrequency = 40; // Chance of attacking every attackcheck
    private bool _canAttack = false; // Turns true if is able to attack
    private float _checkTime = 0.5f; // Value in seconds to check if it can attack
    private CircleCollider2D _range;
    private BoxCollider2D _feet;
    private Animator _animator;
    private PicketManHealthTouchController _picketHealthTouchController;
    private Rigidbody2D _body;

    public GameObject throwPoint;
    public Pickaxe pickaxePrefab;

    void Start()
    {
        _range = GetComponent<CircleCollider2D>();
        _feet = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _picketHealthTouchController = GetComponentInChildren<PicketManHealthTouchController>();
        _body = GetComponent<Rigidbody2D>();

        StartCoroutine(checkIfCanAttack());
    }

    void Update()
    {
        stateMachine();
    }

    private void stateMachine()
    {
        if ((MegamanActor.instance.transform.position.x > transform.position.x && !_facingRight) || (MegamanActor.instance.transform.position.x < transform.position.x && _facingRight))
        {
            flip();
        }

        switch (_currentState)
        {
            case PicketManState.Idle:
                {
                    if (!_feet.IsTouchingLayers(GameManager.instance.layerGround))
                    {
                        if(transform.position.x > MegamanActor.instance.transform.position.x)
                        {
                            _body.velocity = new Vector2(-1 * _moveSpeed, _body.velocity.y);
                        } else
                        {
                            _body.velocity = new Vector2(1 * _moveSpeed, _body.velocity.y);
                        }
                        break;
                    } else
                    {
                        _body.constraints = RigidbodyConstraints2D.FreezePosition;
                        if (isMegamanClose()) _currentState = PicketManState.Active;
                    }
                    break;
                }
            case PicketManState.Active:
                {
                    if (_canAttack)
                    {
                        _currentState = PicketManState.Attack;
                    }
                    if (!isMegamanClose()) _currentState = PicketManState.Idle;
                    break;
                }
            case PicketManState.Attack:
                {
                    _picketHealthTouchController.isInvulnerable = false;
                    _animator.SetBool("IsAttacking", true);
                    attack();
                    _canAttack = false;
                    if (isMegamanClose()) _currentState = PicketManState.Active;
                    else _currentState = PicketManState.Idle;
                    break;
                }
        }
    }

    private void attack()
    {
        Instantiate(pickaxePrefab, throwPoint.transform.position, throwPoint.transform.rotation).Initialize(_attackDamage, _facingRight, throwPoint.transform.right * _attackForce);
        SFXPlayer.instance.Play("enemyFires");
    }

    // Method used by animation event
    private void FinishedAttacking()
    {
        _animator.SetBool("IsAttacking", false);
        _picketHealthTouchController.isInvulnerable = true;
    }

    private bool isMegamanClose()
    {
        if (_range.IsTouchingLayers(GameManager.instance.layerMegaman)) return true;
        else return false;
    }

    private IEnumerator checkIfCanAttack()
    {
        while (true)
        {
            if (Random.Range(0.0f, 100.0f) <= _attackFrequency)
            {
                _canAttack = true;
            }
            else
            {
                _canAttack = false;
            }
            yield return new WaitForSeconds(_checkTime);
        }
    }

    private void flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void die() // Called by animation event
    {
        int _score = 1500;
        StageActor.instance.stageController.scoreController.updateScore(_score);
        StageActor.instance.dropController.dropTry(this.transform);
        Destroy(gameObject);
    }

    private void OnBecameInvisible() // This must be here because it is the object that has visible parts
    {
        Destroy(transform.gameObject);
    }
}

/// <summary>
///  States Description: 
///  
///  Active:
///  * 20% chance to animate going up and changing to attack state, becoming vulnerable;
///  * disables if Megaman is far than * X axis position, returning to idle state;
///  
///  Attack:
///  * Stays vulnerable for a second, throw a pickaxe then return to Active state if Megaman is close or Idle state if not;
/// 
/// Idle:
///  * Initial state,moves to megaman direction if not touching the floor;
/// </summary>
public enum PicketManState
{
    Idle,
    Active,
    Attack
}