using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MegamanHealthController))]
[RequireComponent(typeof(MegamanMovement))]
[RequireComponent(typeof(MegamanAnimation))]
public class MegamanActor : MonoBehaviour
{
    public static MegamanActor instance;
    public MegamanHealthController megamanHealthController { get; private set; }
    public MegamanMovement megamanMovement { get; private set;}
    public MegamanAnimation megamanAnimation { get; private set; }
    public Rigidbody2D body;
    public MegamanHead head;
    public MegamanFeet feet;
    public MegamanBuster buster;
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        megamanHealthController = GetComponent<MegamanHealthController>();
        megamanMovement = GetComponent<MegamanMovement>();
        megamanAnimation = GetComponent<MegamanAnimation>();
        head = GetComponentInChildren<MegamanHead>();
        feet = GetComponentInChildren<MegamanFeet>();
        buster = GetComponentInChildren<MegamanBuster>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();

        if (!megamanHealthController || !megamanMovement || !megamanAnimation || !head || !feet || !buster || !spriteRenderer || !body)
        {
            Debug.LogError("[MegamanActor] Missing components!");
        }
    }
}
