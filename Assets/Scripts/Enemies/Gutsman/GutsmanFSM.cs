using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutsmanFSM : MonoBehaviour
{
    private GutsmanState _currentState;
    private int _attackDamage = 4; // Damage caused by stone
    private float _attackForce = 500; // 500 Force applied to stone when throwned
    private float _moveSpeed = 5; // Velocity it moves before landing
    private float _jumpForce = 15; // How high Gustman jumps
    private float _attackFrequency = 40; // Chance of attacking every defineCheck
    private float _checkTime = 2f; // Delay between actions
    public bool _ableToAct = true; // Turns true if is able to do something
    private bool _hasStone = false; // Check if Gutsman has a stone to Throw
    private bool _facingRight = false; // Initial Gutsman sprite is looking to the left
    private bool _grounded; // Check if Gutsman is on the floor
    private BoxCollider2D _feet;
    private Rigidbody2D _rigidBody;
    private Animator _animator;
    private CameraController _cam;
    private Stone _stoneInstantiated;

    public Stone stonePrefab;
    public CircleCollider2D backSensor;
    public CircleCollider2D frontSensor;
    public BoxCollider2D body;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _feet = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _cam = Camera.main.GetComponent<CameraController>();
        _moveSpeed *= _rigidBody.mass; // Applies the same force disconsidering the mass.
        _jumpForce *= _rigidBody.mass; // Applies the same force disconsidering the mass.
        StartCoroutine(defineState());
    }
    private void Update() // Check if Megaman is behind gustman and flip him if it is
    {
        checkFlip();
    }

    private void checkFlip() // Turn over if Megaman is behind
    {
        if ((MegamanActor.instance.transform.position.x > transform.position.x && !_facingRight) || (MegamanActor.instance.transform.position.x < transform.position.x && _facingRight))
        {
            _facingRight = !_facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Stone>()) // Get the stone
        {
            collision.GetComponent<Collider2D>().transform.SetParent(this.transform);
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            _hasStone = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_feet.IsTouchingLayers(GameManager.instance.layerGround))
        {
            _grounded = true;
            _animator.SetBool("IsJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!_feet.IsTouchingLayers(GameManager.instance.layerGround))
        {
            _grounded = false;
            _animator.SetBool("IsJumping", true);  
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        if (_stoneInstantiated)
        {
            Destroy(_stoneInstantiated.gameObject);
        }
    }

    private IEnumerator defineState() // Coroutine to define the state of FSM if Gutsman finished his last action with _checktime delay between actions
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkTime);
            if (_ableToAct)
            {
                float random = Random.Range(0.0f, 100.0f); // Random choose the stateMachine
                if (random <= _attackFrequency)
                {
                    _currentState = GutsmanState.ThrowStone;
                }
                else if (random <= 50)
                {
                    if (!backSensor.IsTouchingLayers(GameManager.instance.layerWall))
                    {
                        _currentState = GutsmanState.JumpBackyard;
                    }
                    else
                    {
                        _currentState = GutsmanState.JumpForward;
                    }
                }
                else
                {
                    if (!frontSensor.IsTouchingLayers(GameManager.instance.layerWall))
                    {
                        _currentState = GutsmanState.JumpForward;
                    }
                    else
                    {
                        _currentState = GutsmanState.JumpBackyard;
                    }
                }
                _ableToAct = false;
                runStateMachine();
            } else
            {
                while (!_ableToAct) yield return null;
            }
        }
    }

    private void runStateMachine() // Finite State machine
    {
        switch (_currentState)
        {
            case GutsmanState.JumpForward:
                {
                    if (_facingRight)
                    {
                        StartCoroutine(jump(Vector2.right));
                    }
                    else
                    {
                        StartCoroutine(jump(Vector2.left));
                    }
                    break;
                }
            case GutsmanState.ThrowStone:
                {
                    StartCoroutine(getStone());
                    break;
                }
            case GutsmanState.JumpBackyard:
                {
                    if (_facingRight)
                    {
                        StartCoroutine(jump(Vector2.left));
                    }
                    else
                    {
                        StartCoroutine(jump(Vector2.right));
                    }
                    break;
                }
        }
    }

    private IEnumerator jump(Vector2 direction) // Coroutine to jump to right or left
    {
        while (!_grounded) yield return null;
        _rigidBody.AddForce(Vector2.up * _jumpForce + direction * _moveSpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        while (!_grounded) yield return null;
        SFXPlayer.instance.Play("gutsmanLanding");
        _cam.shakeCamera();
        MegamanActor.instance.megamanHealthController.paralyze();
        _ableToAct = true;
    }

    private IEnumerator getStone() // Coroutine to jump, get a stone and throw it at Megaman direction
    {
        _rigidBody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        while (!_grounded) yield return null;
        SFXPlayer.instance.Play("gutsmanLanding");
        _cam.shakeCamera();
        MegamanActor.instance.megamanHealthController.paralyze();
        Vector3 stonePos = new Vector3(transform.position.x, transform.position.y + 15, transform.position.z);
        _stoneInstantiated = Instantiate(stonePrefab, stonePos, transform.rotation);
        _stoneInstantiated.Initialize(_attackDamage);
        while (!_hasStone && _stoneInstantiated) yield return null; // Wait until get stone or it is destroyed
        if (_stoneInstantiated) // Check if the stone still exists
        {
            _animator.SetBool("IsAttacking", true);
            yield return new WaitForSeconds(0.2f);
            if (_stoneInstantiated) // Check if the stone still exists
            {
                _stoneInstantiated.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                if (_facingRight)
                {
                    _stoneInstantiated.GetComponent<Rigidbody2D>().AddForce((Vector2.right + Vector2.up * 0.5f) * _attackForce);
                }
                else
                {
                    _stoneInstantiated.GetComponent<Rigidbody2D>().AddForce((Vector2.left + Vector2.up * 0.5f) * _attackForce);
                }
                SFXPlayer.instance.Play("gutsmanRockThrown");
            }
            else
            {
                _hasStone = false;
                _ableToAct = true;
            }
        } else
        {
            _hasStone = false;
            _ableToAct = true;
        }
    }

    private void finishThrowingStone() // Method used by animator
    {
        _animator.SetBool("IsAttacking", false);
        _hasStone = false; // Just in case megaman destroys the stone before exitTrigger
        _ableToAct = true;
    }
}

public enum GutsmanState
{
    JumpForward,
    ThrowStone,
    JumpBackyard
}
