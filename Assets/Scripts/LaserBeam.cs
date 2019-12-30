using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LaserBeam : MonoBehaviour
{

    public float Speed;
    public List<string> Targets = new List<string>();

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (Targets.Contains(other.tag))
        {

            if (other.GetComponent<LifeManager>())
                other.GetComponent<LifeManager>().Hit();

            Destroy(gameObject);

        }

    }
//
//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Screen"))
//            Destroy(gameObject);
//    }

    public void Launch(Vector2 direction)
    {

        if (_rigidbody == null)
            return;

        _transform.eulerAngles += new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(direction.normalized * Speed);

    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
