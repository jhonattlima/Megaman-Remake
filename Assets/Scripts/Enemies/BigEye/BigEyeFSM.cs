using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEyeFSM : MonoBehaviour
{
    /// <summary>
    ///   An enemy that basically jumps to megaman trying to apply touch damage.
    ///   Yeah, I know. Most easy AI to program.
    /// </summary>
    private float _moveSpeed = 2f; // Velocity it moves before landing
    private float _jumpForce = 11f; // How high it jumps
    private float _delay = 1.5f; //Delay between actions
    private bool _facingRight = false; // Initial sprite is turned to left
    private bool _grounded = false; // Sensor to check if it is on the ground;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _feet;
    private Animator _animator;

    public GameObject frontSensor;

    void Start()
    {
        _feet = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        StartCoroutine(jump()); // Keeps jumping until we reach the stars, baby! Hell yeah!
    }

    void Update()
    {
        if ((MegamanActor.instance.transform.position.x > transform.position.x && !_facingRight) || (MegamanActor.instance.transform.position.x < transform.position.x && _facingRight)) // Check if it is not facing megaman
        {
            flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_feet.IsTouchingLayers(GameManager.instance.layerGround))
        {
            _grounded = true;
            _animator.SetBool("IsJumping", false);
            _rigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
        }
        if (_rigidBody.IsTouchingLayers(GameManager.instance.layerGround)){
            _rigidBody.velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_feet.IsTouchingLayers(GameManager.instance.layerGround))
        {
            _grounded = false;
            _animator.SetBool("IsJumping", true);
        }
    }

    private void flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private IEnumerator jump()
    {
        Vector2 direction;
        while (true)
        {
            if (_grounded)
            {
                _rigidBody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
                _rigidBody.velocity = (Vector2.up * _jumpForce);
                if (_facingRight && !Physics2D.OverlapCircle(frontSensor.transform.position, 0.4f, GameManager.instance.layerGround))
                {
                    direction = Vector2.right;
                    _rigidBody.velocity += (direction * _moveSpeed);
                }
                if (!_facingRight && !Physics2D.OverlapCircle(frontSensor.transform.position, 0.4f, GameManager.instance.layerGround))
                {
                    direction = Vector2.left;
                    _rigidBody.velocity += (direction * _moveSpeed);
                }
            }
            while (!_grounded) yield return null;
            yield return new WaitForSeconds(_delay);
        }
    }
}