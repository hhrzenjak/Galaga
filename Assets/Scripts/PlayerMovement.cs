using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Vector2 ScreenAreaX;
    public Vector2 ScreenAreaY;

    public float Speed;

    public bool _active;

    private Transform _transform;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {

        _active = true;

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();

        if (InputManager.IM)
            InputManager.IM.ControllerInputEvent.AddListener(InputHandler);

    }

    private void Update()
    {

        Vector2 position = _transform.position;

        position.x = Mathf.Clamp(position.x, ScreenAreaX.x, ScreenAreaX.y);
        position.y = Mathf.Clamp(position.y, ScreenAreaY.x, ScreenAreaY.y);

        _transform.position = position;

    }


    private void InputHandler(Vector2 direction)
    {
        if (_active)
            Move(direction);
    }

    private void Move(Vector2 direction)
    {

        if (_active == false)
            return;

        if (_rigidbody == null)
            return;

        _rigidbody.velocity = Vector2.zero;
        _rigidbody.AddForce(direction.normalized * Speed);

    }

    public void Activate()
    {
        _active = true;
    }

    public void Deactivate()
    {

        _active = false;

        if (_rigidbody)
            _rigidbody.velocity = Vector2.zero;

    }

}
