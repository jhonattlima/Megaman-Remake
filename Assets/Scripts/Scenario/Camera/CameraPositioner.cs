using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    private CameraController _cam;

    public GameObject newCameraPos;
    public bool instantTriggerSpawners = false;

    void Start()
    {
        _cam = Camera.main.GetComponent<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<MegamanActor>())
        {
            _cam.slowFollow(newCameraPos.transform.position, instantTriggerSpawners);
            Destroy(this.gameObject);
            Destroy(newCameraPos.gameObject);
        }
    }
}