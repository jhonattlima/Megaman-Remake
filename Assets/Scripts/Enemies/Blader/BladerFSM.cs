using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladerFSM : MonoBehaviour
{
    private float _moveSpeed = 4;
    private BladerState _currentState;
    private bool _facingRight = false;
    private CircleCollider2D _cCollider;
    private Rigidbody2D _body;
    private Vector3 _startPosition , _endPosition;
    private float _megamanYPosition;

    void Start()
    {
        _cCollider = GetComponent<CircleCollider2D>();
        _body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if ((MegamanActor.instance.transform.position.x > transform.position.x && !_facingRight) || (MegamanActor.instance.transform.position.x < transform.position.x && _facingRight))
        {
            flip();
        }
        stateMachine();
    }

    private void stateMachine()
    {
        switch (_currentState)
        {
            case BladerState.Follow:
                {
                    if(MegamanActor.instance.transform.position.x > transform.position.x && !isMegamanClose())
                    {
                        _body.velocity = Vector2.right * _moveSpeed;
                    } else if (MegamanActor.instance.transform.position.x < transform.position.x && !isMegamanClose())
                    {
                        _body.velocity = Vector2.left * _moveSpeed;
                    } else if (isMegamanClose())
                    {
                        _body.velocity = Vector2.zero;
                        _startPosition = transform.position;
                        _megamanYPosition = MegamanActor.instance.transform.position.y;
                        float distToMegaman = Mathf.Abs(_startPosition.x - MegamanActor.instance.transform.position.x);
                        float moveDist = Mathf.Max(2 * distToMegaman, 5f);
                        if(transform.position.x < MegamanActor.instance.transform.position.x)
                        {
                            _endPosition = transform.position + moveDist * Vector3.right;
                        }
                        else if(transform.position.x > MegamanActor.instance.transform.position.x)
                        {
                            _endPosition = transform.position + moveDist * Vector3.left;
                        }
                        _currentState = BladerState.Attack;
                    }
                    break;
                }
            case BladerState.Attack:
                {
                    parabolicMovement(_startPosition, _endPosition, _megamanYPosition - _startPosition.y, _moveSpeed);
                    break;
                }
        }
    }

    private bool isMegamanClose()
    {
        if (Mathf.Abs(MegamanActor.instance.transform.position.x - transform.position.x) <= 5) return true;
        else return false;
    }

    public void parabolicMovement(Vector3 startPosition, Vector3 endPosition, float height, float speed)
    {
        float x0 = startPosition.x;
        float x1 = endPosition.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, _moveSpeed * Time.deltaTime);
        if(Mathf.Approximately(nextX, x1))
        {
            transform.position = _endPosition;
            _currentState = BladerState.Follow;
            return;
        }
        float baseY = Mathf.Lerp(startPosition.y, endPosition.y, (nextX - x0) / dist);
        float arc = height * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, startPosition.z);

        transform.position = nextPos;
    }

    private void flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}

/// <summary>
///  States Description: 
///  Follow:
///  * Follows megaman horizontally until find him in range;
///  
///  Attack:
///  * Goes down/up  until  in the same height as megaman, then try to touch him, going back to original position (but inverted in x axis )
/// </summary>
public enum BladerState
{
    Follow,
    Attack
}
