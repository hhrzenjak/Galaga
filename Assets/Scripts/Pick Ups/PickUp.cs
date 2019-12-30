using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PickUp : MonoBehaviour
{

    public static CustomEvent<PickUpType> PickUpCollectedEvent = new CustomEvent<PickUpType>();

    public PickUpType PickUpType;

    public Vector2 FallDirection;
    public float FallSpeed;

    private void Awake()
    {
        if (GetComponent<Rigidbody2D>())
            GetComponent<Rigidbody2D>().AddForce(FallDirection.normalized * FallSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            PickUpCollectedEvent.Invoke(PickUpType);
            Destroy(gameObject);
        }

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
