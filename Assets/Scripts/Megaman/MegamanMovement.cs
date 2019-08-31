using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanMovement : MonoBehaviour
{
    // Variables
    private CameraController _camera;
    private float _moveInput; // Movement command
    private bool _isBlocked; // Checks if Megaman head is blocked by something
    private float _jumpTimeCounter; //Counts Megaman time in the air

    public bool facingRight; // Checks which side Megaman is looking
    public float moveSpeed = 7; // Megaman move speed
    public float jumpForce = 13; // Megaman jump force
    public float jumpTime = 0.15f ; // Megaman time in the air
    public float checkRadius = 0.4368716f; // Checks feet component radius
    
    void Start()
    {
        _camera = GetComponent<CameraController>();
        facingRight = true;
    }

    void Update()
    {
        if (GameManager.instance.isInputEnabled)
        {
            move();
            jump();
        }
    }

    private void move()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        MegamanActor.instance.body.velocity = new Vector2(_moveInput * moveSpeed, MegamanActor.instance.body.velocity.y);
        if (facingRight == false && _moveInput > 0)
        {
            flip();
        }

        else if (facingRight == true && _moveInput < 0)
        {
            flip();
        }
    }

    private void jump()
    {
        _isBlocked = Physics2D.OverlapCircle(MegamanActor.instance.head.transform.position, checkRadius - 0.4f, GameManager.instance.layerGround);
        if (Input.GetKeyDown(GameManager.instance.jump) && MegamanActor.instance.body.velocity.y == 0 && !_isBlocked) // Jump
        {
            SFXPlayer.instance.Play("megamanJump");
            _jumpTimeCounter = jumpTime;
            MegamanActor.instance.body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if(Input.GetKey(GameManager.instance.jump) && MegamanActor.instance.body.velocity.y > 0.1 && !_isBlocked && _jumpTimeCounter > 0) // Increase jump height
        {
            MegamanActor.instance.body.AddForce(Vector2.up * jumpForce * 0.1f, ForceMode2D.Impulse);
        }
        else if (Input.GetKeyUp(GameManager.instance.jump)) // Forbid increasing jump height
        {
            _jumpTimeCounter = 0;
        }
        if(_jumpTimeCounter > 0)
        {
            _jumpTimeCounter -= Time.deltaTime;
        }  
    }

    private void flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
