using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetFSM: MonoBehaviour
{
    private MetState _currentState;
    private float _attackFrequency = 20; // Chance of attacking every second
    private int _attackDamage = 2;
    private float _attackSpeed = 8;
    private bool _canAttack = false; // Turns true if is able to attack
    private float _checkTime = 0.5f; // Value in seconds to check if it can attack
    private bool _facingRight = false;
    private CircleCollider2D _cCollider;
    private MetHealthTouchController _metBody;
    private Animator _animator;

    public EnemyShot enemyShotPrefab;

    void Start()
    {
        _cCollider = GetComponent<CircleCollider2D>();
        _metBody = GetComponentInChildren<MetHealthTouchController>();
        _animator = GetComponent<Animator>();

        StartCoroutine(checkIfCanAttack());
    }

    private void Update()
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
            case MetState.Idle:
                {
                    if (isMegamanClose()) _currentState = MetState.Active;
                    break;
                }
            case MetState.Active:
                {
                    if (_canAttack)
                    {
                        _currentState = MetState.Attack;
                    }
                    if (!isMegamanClose()) _currentState = MetState.Idle;
                    break;
                }
            case MetState.Attack:
                {
                    _canAttack = false;
                    _metBody.isInvulnerable = false;
                    _animator.SetBool("IsAttacking", true);
                    if (isMegamanClose()) _currentState = MetState.Active;
                    else _currentState = MetState.Idle;
                    break;
                }
        }
    }

    private void attack() // Method used by animation event
    {
        int z, p;
        if (!_facingRight)
        {
            z = -135;
            p = -180;
        }
        else
        {
            z = 45;
            p = 0;
        }
        Instantiate(enemyShotPrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + z)).Initialize(_attackSpeed, _attackDamage);
        Instantiate(enemyShotPrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + p)).Initialize(_attackSpeed, _attackDamage);
        Instantiate(enemyShotPrefab, transform.position, Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z - z)).Initialize(_attackSpeed, _attackDamage);
        SFXPlayer.instance.Play("enemyFires");
    }

    private void FinishedAttacking() // Method used by animation event
    {
        _animator.SetBool("IsAttacking", false);
        _metBody.isInvulnerable = true;
    }

    private bool isMegamanClose()
    {
        if (_cCollider.IsTouchingLayers(GameManager.instance.layerMegaman))  return true;
        else return false;
    }

    private IEnumerator checkIfCanAttack() // Routine to check if Met can attack from time to time
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

    private void flip() // Turn over if Megaman is behind
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void die() // Called by animation event
    {
        int _score = 500;
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
///  * Stays vulnerable for a second, throw three bullets, then return to Active state if Megaman is close or Idle state if not;
/// 
/// Idle:
///  * Initial state, goes to active ig Megaman is close;
/// </summary>
public enum MetState
{
    Idle,
    Active,
    Attack
}
