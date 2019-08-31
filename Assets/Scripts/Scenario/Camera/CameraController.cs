using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float _newPosition; // Position where Megaman is to align camera with
    private float _cameraSpeed = 8f; // Speed that camera moves when repositioning
    private bool _follow = true; // If camera is allowed to follow Megaman horizontally
    private bool _isMoving = false; // Check if camera already started the follow routine (in case the object triggers more than once when falling
    private float _shakeDuration = 0.5f; // How long the camera shakes
    private float _shakeMagnitude = 0.5f; // Magnitude which camera shakes

    public bool blockedLeft = false; // If camera can move further to left
    public bool blockedRight = false; // If camera cam move further to right

    public GameObject Spawners;

    void LateUpdate()
    {
        if (_follow)
        {
            allignWithMegamanHorizontal();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Camera enter collided with " + collision.name);
        if (collision.GetComponent<LimitLeft>())
        {
            blockedLeft = true;
        }
        else if (collision.GetComponent<LimitRight>())
        {
            blockedRight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("Camera exit collided with " + collision.name);
        if (collision.GetComponent<LimitLeft>())
        {
            blockedLeft = false;
        }
        else if (collision.GetComponent<LimitRight>())
        {
            blockedRight = false;
        }
    }

    void allignWithMegamanHorizontal() // Align camera with megaman horizontally
    {
        _newPosition = MegamanActor.instance.transform.position.x;
        if((_newPosition > transform.position.x && blockedRight) ||  (_newPosition < transform.position.x && blockedLeft))
        {
            _newPosition = transform.position.x;
        }
        transform.position = new Vector3(_newPosition, transform.position.y, transform.position.z);
    }

    public void shakeCamera() // Start the cameraShake Coroutine
    {
        StartCoroutine(shake());
    }

    public void startFollow() // Used by trigger camera follow objects
    {
        _follow = true;
    }

    public void slowFollow(Vector3 newPos, bool instantTriggerSpawners) // Slow change the camera to new position
    {
        if (!_isMoving)
        {
            _isMoving = true;
            StartCoroutine(sFollow(newPos, instantTriggerSpawners));
        }
    }

    public IEnumerator sFollow(Vector3 newPos, bool instantTriggerSpawners) // Routine to move camera to new position slowly
    {
        Spawners.SetActive(false);
        _follow = false;
        MegamanActor.instance.body.isKinematic = true;
        MegamanActor.instance.body.velocity = Vector2.zero;
        GameManager.instance.isInputEnabled = false;
        while (Vector3.Distance(newPos, transform.position) > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, _cameraSpeed * Time.deltaTime);
            yield return null;
        }
        _isMoving = false;
        MegamanActor.instance.body.isKinematic = false;
        GameManager.instance.isInputEnabled = true;
        if (!instantTriggerSpawners)
        {
            yield return new WaitForSeconds(1f);
        }
        Spawners.SetActive(true);
    }

    private IEnumerator shake() // Shake the camera
    {
        Vector3 _originalPos = transform.localPosition;
        float endTime = Time.time + _shakeDuration;
        float duration = _shakeDuration;
        while (Time.time < endTime)
        {
            transform.localPosition = _originalPos + Random.insideUnitSphere * _shakeMagnitude;

            duration -= Time.deltaTime;

            yield return null;
        }
        transform.localPosition = _originalPos;
    }
}
