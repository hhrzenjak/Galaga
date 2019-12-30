using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Obstacle : MonoBehaviour
{

    public float Speed;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction)
    {

        if (_rigidbody == null)
            return;

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(direction.normalized * Speed);

    }

}
