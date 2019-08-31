using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPin : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider2D _bCollider;

    void Start()
    {
        _bCollider = GetComponentInParent<BoxCollider2D>();
        _animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<TrailLimit>())
        {
            GetComponentInParent<PlatformController>()._isGoingRight = !GetComponentInParent<PlatformController>()._isGoingRight;
        }
        else if (collision.GetComponent<Trail>())
        {
            if (_bCollider.IsTouchingLayers(GameManager.instance.layerMegaman)){
                if (transform.parent.GetComponentInChildren<MegamanActor>())
                {
                    MegamanActor.instance.transform.parent = null;
                }
                SFXPlayer.instance.StopLoopSFX();
            }
            _bCollider.isTrigger = true;
            _animator.SetBool("isDown", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Trail>())
        {
            if (_bCollider.IsTouchingLayers(GameManager.instance.layerMegaman))
            {
                MegamanActor.instance.transform.parent = transform.parent;
                SFXPlayer.instance.Play("onPlatform");
            }
            _bCollider.isTrigger = false;
            _animator.SetBool("isDown", false);
        }
    }
}
