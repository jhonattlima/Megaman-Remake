using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public bool _isGoingRight = true;
    private float _speed = 3f;

    void Update()
    {
        if (_isGoingRight)
        {
            transform.Translate(Vector2.right * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.left * _speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<MegamanActor>())
        {
            SFXPlayer.instance.Play("onPlatform");
            collision.collider.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<MegamanActor>())
        {
            SFXPlayer.instance.StopLoopSFX();
            collision.collider.transform.parent = null;
        }
    }
}



