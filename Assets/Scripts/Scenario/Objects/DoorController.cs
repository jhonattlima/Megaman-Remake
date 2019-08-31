using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private CameraController _cam;
    private Animator _animator;
    private BoxCollider2D _collider;
    private bool _isDoorDown = true;
    private bool _used = false;

    public GameObject newCameraPos;
    public GameObject limitLeft;
    public GameObject limitRight;
    public bool isBossDoor = false;
    public bool instantTriggerSpawners = true;

    private void Start()
    {
        _cam = Camera.main.GetComponent<CameraController>();
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MegamanActor>() && !_used)
        {
            StartCoroutine(pass(collision));
            _used = true;
        }
    }

    private void doorIsOpen() // Method called by event
    {
        _isDoorDown = false;
    }

    private void doorIsClosed() // Method called by event
    {
        _isDoorDown = true;
    }

    private IEnumerator pass(Collider2D collision)
    {
        limitRight.SetActive(false);
        MegamanActor.instance.body.velocity = Vector2.zero;
        GameManager.instance.isInputEnabled = false;
        MegamanActor.instance.body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        _animator.SetBool("Open", true); //Open
        SFXPlayer.instance.Play("doorOpen");
        while (_isDoorDown) yield return null; // Wait the door to open
        _cam.slowFollow(newCameraPos.transform.position, instantTriggerSpawners);
        while (MegamanActor.instance.transform.position.x < transform.position.x)
        {
            MegamanActor.instance.body.velocity = new Vector2(2, 0);
            yield return new WaitForEndOfFrame();
        }
        _animator.SetBool("Open", false); //Close
        yield return new WaitForSeconds(0.7f); // Sound at the same time the door closes
        SFXPlayer.instance.Play("doorClose");
        while (!_isDoorDown) yield return null; // Wait the door to close
        GetComponent<BoxCollider2D>().isTrigger = false;
        limitLeft.SetActive(true);
        _cam.blockedRight = false;
        gameObject.layer = LayerMask.NameToLayer(GameManager.instance.LayerNameWall);
        MegamanActor.instance.body.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
        if (isBossDoor)
        {
            MegamanActor.instance.body.velocity = Vector2.zero;
            GameManager.instance.isInputEnabled = false;
            StageActor.instance.stageController.startBossBattle();
        } else
        {
            GameManager.instance.isInputEnabled = true;
        }
    }
}
