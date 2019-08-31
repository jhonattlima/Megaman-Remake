using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegamanBuster : MonoBehaviour
{
    public Shot shotPrefab;
    public float fireDelay = 0.5f; // Default is 0.20f;
    public float cooldownTimer = 0;

    void LateUpdate()
    {
        if (GameManager.instance.isInputEnabled && Input.GetKeyDown(GameManager.instance.buster) && cooldownTimer <= 0)
        {
            shoot();
        } 
        if(cooldownTimer > 0){
            cooldownTimer -= Time.deltaTime;
        }
    }

    void shoot()
    {
        cooldownTimer = fireDelay;
        if (Input.GetAxis("Horizontal") != 0)
        {
            Instantiate(shotPrefab, new Vector3(transform.position.x, (transform.position.y + 0.1f), transform.position.z), transform.rotation).Initialize(20, 1); // Shot a little up because megaman is moving and buster is slightly higher
        }
        else
        {
            Instantiate(shotPrefab, transform.position, transform.rotation).Initialize(20, 1);
        }
        SFXPlayer.instance.Play("megamanShot");
    }
}